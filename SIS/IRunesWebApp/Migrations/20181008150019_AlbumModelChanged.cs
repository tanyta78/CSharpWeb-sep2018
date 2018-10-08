using Microsoft.EntityFrameworkCore.Migrations;

namespace IRunesWebApp.Migrations
{
    public partial class AlbumModelChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Albums");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Albums",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
