CREATE OR ALTER PROCEDURE [dbo].[DeleteEmployee]
    @Id int
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [dbo].[Employees]
    SET [IsDeleted] = 1,
        [IsActive] = 0,
        [DeletedOn] = SYSUTCDATETIME()
    WHERE [Id] = @Id
      AND [IsDeleted] = 0;

    SELECT @@ROWCOUNT;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[GetEmployeesPaged]
    @Page int,
    @Limit int,
    @Query nvarchar(255) = N'',
    @DepartmentId int = NULL,
    @Status nvarchar(50) = N''
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset int = CASE WHEN @Page <= 1 THEN 0 ELSE (@Page - 1) * @Limit END;

    SELECT
        e.[Id],
        e.[Name],
        e.[Email],
        e.[PhoneNumber],
        e.[ProfileImagePath],
        e.[Salary],
        e.[DepartmentId],
        d.[Name] AS [DepartmentName],
        e.[ClientId],
        c.[ClientName],
        e.[Role],
        e.[Status],
        COUNT(1) OVER() AS [RowTotal]
    FROM [dbo].[Employees] e
    LEFT JOIN [dbo].[Departments] d ON d.[Id] = e.[DepartmentId] AND d.[IsDeleted] = 0
    LEFT JOIN [dbo].[Clients] c ON c.[Id] = e.[ClientId] AND c.[IsDeleted] = 0
    WHERE e.[IsDeleted] = 0
      AND (@Query = N'' OR e.[Name] LIKE '%' + @Query + '%' OR e.[Email] LIKE '%' + @Query + '%' OR e.[PhoneNumber] LIKE '%' + @Query + '%')
      AND (@DepartmentId IS NULL OR e.[DepartmentId] = @DepartmentId)
      AND (@Status = N'' OR e.[Status] = @Status)
    ORDER BY e.[Name]
    OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY;
END;
GO
