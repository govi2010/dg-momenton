using Microsoft.EntityFrameworkCore.Migrations;

namespace DG.Momenton.Infrastructure.Migrations
{
    #region SetupEmployee

    /// <summary>
    /// This used to create/migrate the database via EF Code-First
    /// This class is auto-generated via running "Add-Migration SetupEmployee" command on Package-Manager-Console
    /// </summary>
    public partial class SetupEmployee : Migration
    {
        #region Up

        /// <summary>
        /// Create the table
        /// </summary>
        /// <param name="migrationBuilder">builder instance</param>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ManagerId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });
        }

        #endregion
        #region Down

        /// <summary>
        /// Drop the table
        /// </summary>
        /// <param name="migrationBuilder">builder instance</param>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");
        }

        #endregion
    }

    #endregion
}
