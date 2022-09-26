using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniTrade.Persistence.Migrations
{
    public partial class _mig3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StorageType",
                table: "Files",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StorageType",
                table: "Files");
        }
    }
}
