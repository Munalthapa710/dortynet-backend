using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeApi.Migrations
{
    /// <inheritdoc />
    public partial class AddInternStoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE OR ALTER PROCEDURE [dbo].[GetAllInterns]
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT Id, Name, Description
                    FROM [dbo].[Interns]
                    ORDER BY Id DESC;
                END
                """);

            migrationBuilder.Sql("""
                CREATE OR ALTER PROCEDURE [dbo].[GetInternById]
                    @Id INT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT Id, Name, Description
                    FROM [dbo].[Interns]
                    WHERE Id = @Id;
                END
                """);

            migrationBuilder.Sql("""
                CREATE OR ALTER PROCEDURE [dbo].[CreateIntern]
                    @Name NVARCHAR(MAX),
                    @Description NVARCHAR(MAX)
                AS
                BEGIN
                    SET NOCOUNT ON;

                    INSERT INTO [dbo].[Interns] (Name, Description)
                    VALUES (@Name, @Description);

                    SELECT Id, Name, Description
                    FROM [dbo].[Interns]
                    WHERE Id = CONVERT(INT, SCOPE_IDENTITY());
                END
                """);

            migrationBuilder.Sql("""
                CREATE OR ALTER PROCEDURE [dbo].[UpdateIntern]
                    @Id INT,
                    @Name NVARCHAR(MAX),
                    @Description NVARCHAR(MAX)
                AS
                BEGIN
                    SET NOCOUNT ON;

                    UPDATE [dbo].[Interns]
                    SET Name = @Name,
                        Description = @Description
                    WHERE Id = @Id;

                    SELECT Id, Name, Description
                    FROM [dbo].[Interns]
                    WHERE Id = @Id;
                END
                """);

            migrationBuilder.Sql("""
                CREATE OR ALTER PROCEDURE [dbo].[DeleteIntern]
                    @Id INT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DELETE FROM [dbo].[Interns]
                    WHERE Id = @Id;

                    SELECT @@ROWCOUNT;
                END
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[DeleteIntern]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[UpdateIntern]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[CreateIntern]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetInternById]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetAllInterns]");
        }
    }
}
