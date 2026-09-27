namespace EmployeeApi.Service.Files;

public class LocalFileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _environment;

    public LocalFileStorageService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string?> SaveBase64FileAsync(string? base64File, string folderName)
    {
        if (string.IsNullOrWhiteSpace(base64File))
        {
            return null;
        }

        var content = base64File;
        var extension = ".jpg";

        if (base64File.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
        {
            var commaIndex = base64File.IndexOf(',');
            if (commaIndex < 0)
            {
                throw new InvalidOperationException("Invalid base64 file format.");
            }

            var metadata = base64File[..commaIndex];
            content = base64File[(commaIndex + 1)..];
            extension = GetExtension(metadata);
        }

        var bytes = Convert.FromBase64String(content);
        var safeFolderName = folderName.Replace('\\', '/').Trim('/');
        var uploadRoot = Path.Combine(_environment.ContentRootPath, "wwwroot", "uploads", safeFolderName);
        Directory.CreateDirectory(uploadRoot);

        var fileName = $"{Guid.NewGuid():N}{extension}";
        var filePath = Path.Combine(uploadRoot, fileName);
        await File.WriteAllBytesAsync(filePath, bytes);

        return $"/uploads/{safeFolderName}/{fileName}";
    }

    private static string GetExtension(string metadata)
    {
        if (metadata.Contains("image/png", StringComparison.OrdinalIgnoreCase))
        {
            return ".png";
        }

        if (metadata.Contains("image/webp", StringComparison.OrdinalIgnoreCase))
        {
            return ".webp";
        }

        if (metadata.Contains("image/gif", StringComparison.OrdinalIgnoreCase))
        {
            return ".gif";
        }

        if (metadata.Contains("application/pdf", StringComparison.OrdinalIgnoreCase))
        {
            return ".pdf";
        }

        return ".jpg";
    }
}
