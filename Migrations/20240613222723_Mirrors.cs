using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReflectiveMirror.Migrations
{
    /// <inheritdoc />
    public partial class Mirrors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Brand",
                table: "Mirror",
                newName: "Type");

            migrationBuilder.AddColumn<string>(
                name: "Shape",
                table: "Mirror",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Shape",
                table: "Mirror");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Mirror",
                newName: "Brand");
        }
    }
}
