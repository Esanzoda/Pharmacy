using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmasy.Migrations
{
    /// <inheritdoc />
    public partial class IntialCreate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderPrice",
                table: "Products",
                newName: "Price");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Products",
                newName: "OrderPrice");
        }
    }
}
