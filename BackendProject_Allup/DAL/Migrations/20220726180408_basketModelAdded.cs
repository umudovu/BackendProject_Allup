using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendProject_Allup.DAL.Migrations
{
    public partial class basketModelAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketItem_AspNetUsers_AppUserId",
                table: "BasketItem");

            migrationBuilder.DropIndex(
                name: "IX_BasketItem_AppUserId",
                table: "BasketItem");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "BasketItem");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BasketItem");

            migrationBuilder.AddColumn<int>(
                name: "BasketId",
                table: "BasketItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "BasketItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Basket",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Basket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Basket_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BasketItem_BasketId",
                table: "BasketItem",
                column: "BasketId");

            migrationBuilder.CreateIndex(
                name: "IX_Basket_UserId",
                table: "Basket",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItem_Basket_BasketId",
                table: "BasketItem",
                column: "BasketId",
                principalTable: "Basket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketItem_Basket_BasketId",
                table: "BasketItem");

            migrationBuilder.DropTable(
                name: "Basket");

            migrationBuilder.DropIndex(
                name: "IX_BasketItem_BasketId",
                table: "BasketItem");

            migrationBuilder.DropColumn(
                name: "BasketId",
                table: "BasketItem");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "BasketItem");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "BasketItem",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "BasketItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BasketItem_AppUserId",
                table: "BasketItem",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItem_AspNetUsers_AppUserId",
                table: "BasketItem",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
