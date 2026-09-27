namespace EmployeeApi.Service.Files;

public interface IFileStorageService
{
    Task<string?> SaveBase64FileAsync(string? base64File, string folderName);
}
