using HC.WeChat.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace HC.WeChat.Migrations
{
    [DbContext(typeof(WeChatDbContext))]
    [Migration("201907261640_Added_Table_Questions")]
    public partial class Added_Table_Questions
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity("GYSWP.AnswerRecords.AnswerRecord", b => {
                b.Property<Guid>("Id")
.ValueGeneratedOnAdd(); b.Property<Guid>("QuestionnaireId").IsRequired(); b.Property<string>("Values"); b.Property<string>("Remark").HasMaxLength(500); b.Property<string>("OpenId").IsRequired().HasMaxLength(50); b.Property<DateTime?>("CreationTime"); b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("AnswerRecords");
            }); modelBuilder.Entity("GYSWP.DemandDetails.DemandDetail", b => {
                b.Property<Guid>("Id")
.ValueGeneratedOnAdd(); b.Property<Guid>("DemandForecastId").IsRequired(); b.Property<string>("Name").IsRequired(); b.Property<int?>("Type"); b.Property<decimal?>("WholesalePrice"); b.Property<decimal?>("SuggestPrice"); b.Property<bool?>("IsAlien"); b.Property<int?>("LastMonthNum"); b.Property<decimal?>("YearOnYear"); b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("DemandDetails");
            }); modelBuilder.Entity("GYSWP.DemandForecasts.DemandForecast", b => {
                b.Property<Guid>("Id")
.ValueGeneratedOnAdd(); b.Property<string>("Title"); b.Property<DateTime?>("Month"); b.Property<DateTime?>("CreationTime"); b.Property<long?>("CreatorUserId"); b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("DemandForecasts");
            }); modelBuilder.Entity("GYSWP.ForecastRecords.ForecastRecord", b => {
                b.Property<Guid>("Id")
.ValueGeneratedOnAdd(); b.Property<Guid>("DemandDetailId").IsRequired(); b.Property<int>("PredictiveValue").IsRequired(); b.Property<string>("OpenId").IsRequired().HasMaxLength(50); b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("ForecastRecords");
            }); modelBuilder.Entity("GYSWP.Questionnaires.Questionnaire", b => {
                b.Property<Guid>("Id")
.ValueGeneratedOnAdd(); b.Property<int>("Type").IsRequired(); b.Property<bool>("IsMultiple").IsRequired(); b.Property<string>("No").IsRequired(); b.Property<string>("Question").IsRequired().HasMaxLength(500); b.Property<DateTime?>("CreationTime"); b.Property<long?>("CreatorUserId"); b.Property<DateTime?>("LastModificationTime"); b.Property<long?>("LastModifierUserId"); b.Property<DateTime?>("DeletionTime"); b.Property<long?>("DeleterUserId"); b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("Questionnaires");
            });
        }
    }
}
