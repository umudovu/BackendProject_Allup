using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendProject_Allup.DAL.Migrations
{
    public partial class miq3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrackingNo",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrackingNo",
                table: "Orders");
        }
    }
}
