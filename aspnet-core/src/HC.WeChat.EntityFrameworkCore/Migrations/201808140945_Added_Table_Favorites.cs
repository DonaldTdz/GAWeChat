using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace HC.WeChat.Migrations
{
    public partial class Added_Table_Favorites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                { Id = table.Column<Guid>(nullable: false),
                    ShopId = table.Column<Guid>(nullable: false),
                    ShopName = table.Column<string>(maxLength: 200, nullable: true),
                    OpenId = table.Column<string>(nullable: false),
                    NickName = table.Column<string>(maxLength: 200, nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CancelTime = table.Column<DateTime>(nullable: true),
                    IsCancel = table.Column<bool>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: true),
                    CoverPhoto = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.Id);
                });

            migrationBuilder.AddColumn<string>(
                    name: "Tags",
                    table: "Products",
                    maxLength: 500,
                    nullable: true);
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropColumn(
                 name: "Tags",
                 table: "Products");
        }
    }
}
