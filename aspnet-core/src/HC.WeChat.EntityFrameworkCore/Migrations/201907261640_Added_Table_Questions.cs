using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace HC.WeChat.Migrations
{
    public partial class Added_Table_Questions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnswerRecords",
                columns: table => new
                { Id = table.Column<Guid>(nullable: false), QuestionnaireId = table.Column<Guid>(nullable: false), Values = table.Column<string>(nullable: true), Remark = table.Column<string>(maxLength: 500, nullable: true), OpenId = table.Column<string>(maxLength: 50, nullable: false), CreationTime = table.Column<DateTime>(nullable: true) },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerRecords", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "DemandDetails",
                columns: table => new
                { Id = table.Column<Guid>(nullable: false), DemandForecastId = table.Column<Guid>(nullable: false), Name = table.Column<string>(nullable: false), Type = table.Column<int>(nullable: true), WholesalePrice = table.Column<decimal>(nullable: true), SuggestPrice = table.Column<decimal>(nullable: true), IsAlien = table.Column<bool>(nullable: true), LastMonthNum = table.Column<int>(nullable: true), YearOnYear = table.Column<decimal>(nullable: true) },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandDetails", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "DemandForecasts",
                columns: table => new
                { Id = table.Column<Guid>(nullable: false), Title = table.Column<string>(nullable: true), Month = table.Column<DateTime>(nullable: true), CreationTime = table.Column<DateTime>(nullable: true), CreatorUserId = table.Column<long>(nullable: true) },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandForecasts", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "ForecastRecords",
                columns: table => new
                { Id = table.Column<Guid>(nullable: false), DemandDetailId = table.Column<Guid>(nullable: false), PredictiveValue = table.Column<int>(nullable: false), OpenId = table.Column<string>(maxLength: 50, nullable: false) },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForecastRecords", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "Questionnaires",
                columns: table => new
                { Id = table.Column<Guid>(nullable: false), Type = table.Column<int>(nullable: false), IsMultiple = table.Column<bool>(nullable: false), No = table.Column<string>(nullable: false), Question = table.Column<string>(maxLength: 500, nullable: false), CreationTime = table.Column<DateTime>(nullable: true), CreatorUserId = table.Column<long>(nullable: true), LastModificationTime = table.Column<DateTime>(nullable: true), LastModifierUserId = table.Column<long>(nullable: true), DeletionTime = table.Column<DateTime>(nullable: true), DeleterUserId = table.Column<long>(nullable: true) },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questionnaires", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerRecords");
            migrationBuilder.DropTable(
               name: "DemandDetails");
            migrationBuilder.DropTable(
                name: "DemandForecasts");
            migrationBuilder.DropTable(
               name: "ForecastRecords");
            migrationBuilder.DropTable(
               name: "Questionnaires");
        }
    }
}
