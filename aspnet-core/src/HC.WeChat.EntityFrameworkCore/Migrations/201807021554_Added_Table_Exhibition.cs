using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
namespace HC.WeChat.Migrations
{
    public partial class Added_Table_Exhibition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exhibitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BeginTime = table.Column<DateTime>(nullable: true),
                    EndTime = table.Column<DateTime>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Desc = table.Column<string>(nullable: true),
                    TopTotal = table.Column<int>(nullable: false),
                    Frequency = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exhibitions", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "ExhibitionShops",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    RetailerId = table.Column<Guid>(nullable: true),
                    ShopName = table.Column<string>(maxLength: 200, nullable: false),
                    ShopAddress = table.Column<string>(maxLength: 500, nullable: true),
                    PicPath = table.Column<string>(nullable: false),
                    Votes = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ShopId = table.Column<Guid>(nullable: false),
                    AuditTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExhibitionShops", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "VoteLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    OpenId = table.Column<string>(maxLength: 50, nullable: false),
                    UserName = table.Column<string>(maxLength: 50, nullable: true),
                    ExhibitionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoteLogs", x => x.Id);
                });
            migrationBuilder.AddColumn<string>(
                    name: "Area",
                    table: "Retailers",
                    nullable: true);

            migrationBuilder.AddColumn<int>(
                     name: "NoId",
                     table: "ExhibitionShops",
                     nullable: true);

            migrationBuilder.AddColumn<string>(
                     name: "Desc",
                     table: "Products",
                     nullable: true);
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Exhibitions");
            migrationBuilder.DropTable(
                 name: "ExhibitionShops");
            migrationBuilder.DropTable(
                 name: "VoteLogs");
            migrationBuilder.DropColumn(
                 name: "Area",
                 table: "Retailers");

            migrationBuilder.DropColumn(
                name: "NoId",
                table: "ExhibitionShops");

            migrationBuilder.DropColumn(
           name: "Desc",
           table: "Products");
        }
    }
}
