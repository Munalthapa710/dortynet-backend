using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeApi.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF OBJECT_ID(N'[dbo].[Departments]', N'U') IS NULL
                BEGIN
                    CREATE TABLE [dbo].[Departments] (
                        [Id] int NOT NULL IDENTITY,
                        [Name] nvarchar(max) NOT NULL,
                        [Description] nvarchar(max) NOT NULL,
                        CONSTRAINT [PK_Departments] PRIMARY KEY ([Id])
                    );
                END

                IF OBJECT_ID(N'[dbo].[Departments]', N'U') IS NOT NULL
                   AND COL_LENGTH(N'[dbo].[Departments]', 'Description') IS NOT NULL
                BEGIN
                    UPDATE [dbo].[Departments]
                    SET [Description] = N''
                    WHERE [Description] IS NULL;

                    ALTER TABLE [dbo].[Departments]
                    ALTER COLUMN [Description] nvarchar(max) NOT NULL;
                END
                """);

            migrationBuilder.Sql("""
                IF OBJECT_ID(N'[dbo].[Employees]', N'U') IS NULL
                BEGIN
                    CREATE TABLE [dbo].[Employees] (
                        [Id] int NOT NULL IDENTITY,
                        [Name] nvarchar(max) NOT NULL,
                        [Email] nvarchar(max) NOT NULL,
                        [Salary] decimal(18,2) NOT NULL,
                        [DepartmentId] int NOT NULL,
                        CONSTRAINT [PK_Employees] PRIMARY KEY ([Id])
                    );
                END
                """);

            migrationBuilder.Sql("""
                IF OBJECT_ID(N'[dbo].[Employees]', N'U') IS NOT NULL
                   AND COL_LENGTH(N'[dbo].[Employees]', 'DepartmentId') IS NULL
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM [dbo].[Departments])
                    BEGIN
                        INSERT INTO [dbo].[Departments] ([Name], [Description])
                        VALUES (N'Unassigned', N'Imported from existing employee records');
                    END

                    ALTER TABLE [dbo].[Employees] ADD [DepartmentId] int NULL;
                END
                """);

            migrationBuilder.Sql("""
                IF OBJECT_ID(N'[dbo].[Employees]', N'U') IS NOT NULL
                   AND COL_LENGTH(N'[dbo].[Employees]', 'DepartmentId') IS NOT NULL
                   AND EXISTS (
                       SELECT 1
                       FROM sys.columns
                       WHERE [name] = N'DepartmentId'
                         AND [object_id] = OBJECT_ID(N'[dbo].[Employees]')
                         AND [is_nullable] = 1
                   )
                BEGIN
                    DECLARE @DefaultDepartmentId int = (
                        SELECT TOP (1) [Id]
                        FROM [dbo].[Departments]
                        ORDER BY [Id]
                    );

                    UPDATE [dbo].[Employees]
                    SET [DepartmentId] = @DefaultDepartmentId
                    WHERE [DepartmentId] IS NULL;

                    ALTER TABLE [dbo].[Employees] ALTER COLUMN [DepartmentId] int NOT NULL;
                END
                """);

            migrationBuilder.Sql("""
                IF OBJECT_ID(N'[dbo].[AssignedTasks]', N'U') IS NULL
                BEGIN
                    CREATE TABLE [dbo].[AssignedTasks] (
                        [Id] int NOT NULL IDENTITY,
                        [EmployeeId] int NOT NULL,
                        [Title] nvarchar(max) NOT NULL,
                        [Description] nvarchar(max) NOT NULL,
                        [DueDate] datetime2 NULL,
                        [Status] nvarchar(max) NOT NULL,
                        [AssignedOn] datetime2 NOT NULL,
                        CONSTRAINT [PK_AssignedTasks] PRIMARY KEY ([Id])
                    );
                END
                """);

            migrationBuilder.Sql("""
                IF OBJECT_ID(N'[dbo].[Employees]', N'U') IS NOT NULL
                   AND OBJECT_ID(N'[dbo].[Departments]', N'U') IS NOT NULL
                   AND COL_LENGTH(N'[dbo].[Employees]', 'DepartmentId') IS NOT NULL
                   AND NOT EXISTS (
                       SELECT 1
                       FROM sys.indexes
                       WHERE [name] = N'IX_Employees_DepartmentId'
                         AND [object_id] = OBJECT_ID(N'[dbo].[Employees]')
                   )
                BEGIN
                    CREATE INDEX [IX_Employees_DepartmentId] ON [dbo].[Employees] ([DepartmentId]);
                END
                """);

            migrationBuilder.Sql("""
                IF OBJECT_ID(N'[dbo].[Employees]', N'U') IS NOT NULL
                   AND OBJECT_ID(N'[dbo].[Departments]', N'U') IS NOT NULL
                   AND COL_LENGTH(N'[dbo].[Employees]', 'DepartmentId') IS NOT NULL
                   AND NOT EXISTS (
                       SELECT 1
                       FROM sys.foreign_keys
                       WHERE [name] = N'FK_Employees_Departments_DepartmentId'
                         AND [parent_object_id] = OBJECT_ID(N'[dbo].[Employees]')
                   )
                BEGIN
                    ALTER TABLE [dbo].[Employees] WITH CHECK ADD CONSTRAINT [FK_Employees_Departments_DepartmentId]
                        FOREIGN KEY ([DepartmentId]) REFERENCES [dbo].[Departments] ([Id]) ON DELETE CASCADE;
                END
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignedTasks");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
