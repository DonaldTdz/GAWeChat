using HC.WeChat.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Text;


namespace HC.WeChat.Migrations
{
    [DbContext(typeof(WeChatDbContext))]
    [Migration("201807021554_Added_Table_Exhibition")]
    public partial class Added_Table_Exhibition
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Abp.Application.Editions.Edition", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<long?>("DeleterUserId");

                b.Property<DateTime?>("DeletionTime");

                b.Property<string>("DisplayName")
                    .IsRequired()
                    .HasMaxLength(64);

                b.Property<bool>("IsDeleted");

                b.Property<DateTime?>("LastModificationTime");

                b.Property<long?>("LastModifierUserId");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(32);

                b.HasKey("Id");

                b.ToTable("AbpEditions");
            });

            modelBuilder.Entity("Abp.Application.Features.FeatureSetting", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<string>("Discriminator")
                    .IsRequired();

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(128);

                b.Property<int?>("TenantId");

                b.Property<string>("Value")
                    .IsRequired()
                    .HasMaxLength(2000);

                b.HasKey("Id");

                b.ToTable("AbpFeatures");

                b.HasDiscriminator<string>("Discriminator").HasValue("FeatureSetting");
            });

            modelBuilder.Entity("Abp.Auditing.AuditLog", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("BrowserInfo")
                    .HasMaxLength(256);

                b.Property<string>("ClientIpAddress")
                    .HasMaxLength(64);

                b.Property<string>("ClientName")
                    .HasMaxLength(128);

                b.Property<string>("CustomData")
                    .HasMaxLength(2000);

                b.Property<string>("Exception")
                    .HasMaxLength(2000);

                b.Property<int>("ExecutionDuration");

                b.Property<DateTime>("ExecutionTime");

                b.Property<int?>("ImpersonatorTenantId");

                b.Property<long?>("ImpersonatorUserId");

                b.Property<string>("MethodName")
                    .HasMaxLength(256);

                b.Property<string>("Parameters")
                    .HasMaxLength(1024);

                b.Property<string>("ServiceName")
                    .HasMaxLength(256);

                b.Property<int?>("TenantId");

                b.Property<long?>("UserId");

                b.HasKey("Id");

                b.HasIndex("TenantId", "ExecutionDuration");

                b.HasIndex("TenantId", "ExecutionTime");

                b.HasIndex("TenantId", "UserId");

                b.ToTable("AbpAuditLogs");
            });

            modelBuilder.Entity("Abp.Authorization.PermissionSetting", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<string>("Discriminator")
                    .IsRequired();

                b.Property<bool>("IsGranted");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(128);

                b.Property<int?>("TenantId");

                b.HasKey("Id");

                b.HasIndex("TenantId", "Name");

                b.ToTable("AbpPermissions");

                b.HasDiscriminator<string>("Discriminator").HasValue("PermissionSetting");
            });

            modelBuilder.Entity("Abp.Authorization.Roles.RoleClaim", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("ClaimType")
                    .HasMaxLength(256);

                b.Property<string>("ClaimValue");

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<int>("RoleId");

                b.Property<int?>("TenantId");

                b.HasKey("Id");

                b.HasIndex("RoleId");

                b.HasIndex("TenantId", "ClaimType");

                b.ToTable("AbpRoleClaims");
            });

            modelBuilder.Entity("Abp.Authorization.Users.UserAccount", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<long?>("DeleterUserId");

                b.Property<DateTime?>("DeletionTime");

                b.Property<string>("EmailAddress")
                    .HasMaxLength(256);

                b.Property<bool>("IsDeleted");

                b.Property<DateTime?>("LastLoginTime");

                b.Property<DateTime?>("LastModificationTime");

                b.Property<long?>("LastModifierUserId");

                b.Property<int?>("TenantId");

                b.Property<long>("UserId");

                b.Property<long?>("UserLinkId");

                b.Property<string>("UserName")
                    .HasMaxLength(32);

                b.HasKey("Id");

                b.HasIndex("EmailAddress");

                b.HasIndex("UserName");

                b.HasIndex("TenantId", "EmailAddress");

                b.HasIndex("TenantId", "UserId");

                b.HasIndex("TenantId", "UserName");

                b.ToTable("AbpUserAccounts");
            });

            modelBuilder.Entity("Abp.Authorization.Users.UserClaim", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("ClaimType")
                    .HasMaxLength(256);

                b.Property<string>("ClaimValue");

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<int?>("TenantId");

                b.Property<long>("UserId");

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.HasIndex("TenantId", "ClaimType");

                b.ToTable("AbpUserClaims");
            });

            modelBuilder.Entity("Abp.Authorization.Users.UserLogin", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("LoginProvider")
                    .IsRequired()
                    .HasMaxLength(128);

                b.Property<string>("ProviderKey")
                    .IsRequired()
                    .HasMaxLength(256);

                b.Property<int?>("TenantId");

                b.Property<long>("UserId");

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.HasIndex("TenantId", "UserId");

                b.HasIndex("TenantId", "LoginProvider", "ProviderKey");

                b.ToTable("AbpUserLogins");
            });

            modelBuilder.Entity("Abp.Authorization.Users.UserLoginAttempt", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("BrowserInfo")
                    .HasMaxLength(256);

                b.Property<string>("ClientIpAddress")
                    .HasMaxLength(64);

                b.Property<string>("ClientName")
                    .HasMaxLength(128);

                b.Property<DateTime>("CreationTime");

                b.Property<byte>("Result");

                b.Property<string>("TenancyName")
                    .HasMaxLength(64);

                b.Property<int?>("TenantId");

                b.Property<long?>("UserId");

                b.Property<string>("UserNameOrEmailAddress")
                    .HasMaxLength(255);

                b.HasKey("Id");

                b.HasIndex("UserId", "TenantId");

                b.HasIndex("TenancyName", "UserNameOrEmailAddress", "Result");

                b.ToTable("AbpUserLoginAttempts");
            });

            modelBuilder.Entity("Abp.Authorization.Users.UserOrganizationUnit", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<bool>("IsDeleted");

                b.Property<long>("OrganizationUnitId");

                b.Property<int?>("TenantId");

                b.Property<long>("UserId");

                b.HasKey("Id");

                b.HasIndex("TenantId", "OrganizationUnitId");

                b.HasIndex("TenantId", "UserId");

                b.ToTable("AbpUserOrganizationUnits");
            });

            modelBuilder.Entity("Abp.Authorization.Users.UserRole", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<int>("RoleId");

                b.Property<int?>("TenantId");

                b.Property<long>("UserId");

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.HasIndex("TenantId", "RoleId");

                b.HasIndex("TenantId", "UserId");

                b.ToTable("AbpUserRoles");
            });

            modelBuilder.Entity("Abp.Authorization.Users.UserToken", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("LoginProvider");

                b.Property<string>("Name");

                b.Property<int?>("TenantId");

                b.Property<long>("UserId");

                b.Property<string>("Value");

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.HasIndex("TenantId", "UserId");

                b.ToTable("AbpUserTokens");
            });

            modelBuilder.Entity("Abp.BackgroundJobs.BackgroundJobInfo", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<bool>("IsAbandoned");

                b.Property<string>("JobArgs")
                    .IsRequired()
                    .HasMaxLength(1048576);

                b.Property<string>("JobType")
                    .IsRequired()
                    .HasMaxLength(512);

                b.Property<DateTime?>("LastTryTime");

                b.Property<DateTime>("NextTryTime");

                b.Property<byte>("Priority");

                b.Property<short>("TryCount");

                b.HasKey("Id");

                b.HasIndex("IsAbandoned", "NextTryTime");

                b.ToTable("AbpBackgroundJobs");
            });

            modelBuilder.Entity("Abp.Configuration.Setting", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<DateTime?>("LastModificationTime");

                b.Property<long?>("LastModifierUserId");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(256);

                b.Property<int?>("TenantId");

                b.Property<long?>("UserId");

                b.Property<string>("Value")
                    .HasMaxLength(2000);

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.HasIndex("TenantId", "Name");

                b.ToTable("AbpSettings");
            });

            modelBuilder.Entity("Abp.EntityHistory.EntityChange", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<DateTime>("ChangeTime");

                b.Property<byte>("ChangeType");

                b.Property<long>("EntityChangeSetId");

                b.Property<string>("EntityId")
                    .HasMaxLength(48);

                b.Property<string>("EntityTypeFullName")
                    .HasMaxLength(192);

                b.Property<int?>("TenantId");

                b.HasKey("Id");

                b.HasIndex("EntityChangeSetId");

                b.HasIndex("EntityTypeFullName", "EntityId");

                b.ToTable("AbpEntityChanges");
            });

            modelBuilder.Entity("Abp.EntityHistory.EntityChangeSet", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("BrowserInfo")
                    .HasMaxLength(256);

                b.Property<string>("ClientIpAddress")
                    .HasMaxLength(64);

                b.Property<string>("ClientName")
                    .HasMaxLength(128);

                b.Property<DateTime>("CreationTime");

                b.Property<string>("ExtensionData");

                b.Property<int?>("ImpersonatorTenantId");

                b.Property<long?>("ImpersonatorUserId");

                b.Property<string>("Reason")
                    .HasMaxLength(256);

                b.Property<int?>("TenantId");

                b.Property<long?>("UserId");

                b.HasKey("Id");

                b.HasIndex("TenantId", "CreationTime");

                b.HasIndex("TenantId", "Reason");

                b.HasIndex("TenantId", "UserId");

                b.ToTable("AbpEntityChangeSets");
            });

            modelBuilder.Entity("Abp.EntityHistory.EntityPropertyChange", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<long>("EntityChangeId");

                b.Property<string>("NewValue")
                    .HasMaxLength(512);

                b.Property<string>("OriginalValue")
                    .HasMaxLength(512);

                b.Property<string>("PropertyName")
                    .HasMaxLength(96);

                b.Property<string>("PropertyTypeFullName")
                    .HasMaxLength(192);

                b.Property<int?>("TenantId");

                b.HasKey("Id");

                b.HasIndex("EntityChangeId");

                b.ToTable("AbpEntityPropertyChanges");
            });

            modelBuilder.Entity("Abp.Localization.ApplicationLanguage", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<long?>("DeleterUserId");

                b.Property<DateTime?>("DeletionTime");

                b.Property<string>("DisplayName")
                    .IsRequired()
                    .HasMaxLength(64);

                b.Property<string>("Icon")
                    .HasMaxLength(128);

                b.Property<bool>("IsDeleted");

                b.Property<bool>("IsDisabled");

                b.Property<DateTime?>("LastModificationTime");

                b.Property<long?>("LastModifierUserId");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(10);

                b.Property<int?>("TenantId");

                b.HasKey("Id");

                b.HasIndex("TenantId", "Name");

                b.ToTable("AbpLanguages");
            });

            modelBuilder.Entity("Abp.Localization.ApplicationLanguageText", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<string>("Key")
                    .IsRequired()
                    .HasMaxLength(256);

                b.Property<string>("LanguageName")
                    .IsRequired()
                    .HasMaxLength(10);

                b.Property<DateTime?>("LastModificationTime");

                b.Property<long?>("LastModifierUserId");

                b.Property<string>("Source")
                    .IsRequired()
                    .HasMaxLength(128);

                b.Property<int?>("TenantId");

                b.Property<string>("Value")
                    .IsRequired()
                    .HasMaxLength(67108864);

                b.HasKey("Id");

                b.HasIndex("TenantId", "Source", "LanguageName", "Key");

                b.ToTable("AbpLanguageTexts");
            });

            modelBuilder.Entity("Abp.Notifications.NotificationInfo", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<string>("Data")
                    .HasMaxLength(1048576);

                b.Property<string>("DataTypeName")
                    .HasMaxLength(512);

                b.Property<string>("EntityId")
                    .HasMaxLength(96);

                b.Property<string>("EntityTypeAssemblyQualifiedName")
                    .HasMaxLength(512);

                b.Property<string>("EntityTypeName")
                    .HasMaxLength(250);

                b.Property<string>("ExcludedUserIds")
                    .HasMaxLength(131072);

                b.Property<string>("NotificationName")
                    .IsRequired()
                    .HasMaxLength(96);

                b.Property<byte>("Severity");

                b.Property<string>("TenantIds")
                    .HasMaxLength(131072);

                b.Property<string>("UserIds")
                    .HasMaxLength(131072);

                b.HasKey("Id");

                b.ToTable("AbpNotifications");
            });

            modelBuilder.Entity("Abp.Notifications.NotificationSubscriptionInfo", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<string>("EntityId")
                    .HasMaxLength(96);

                b.Property<string>("EntityTypeAssemblyQualifiedName")
                    .HasMaxLength(512);

                b.Property<string>("EntityTypeName")
                    .HasMaxLength(250);

                b.Property<string>("NotificationName")
                    .HasMaxLength(96);

                b.Property<int?>("TenantId");

                b.Property<long>("UserId");

                b.HasKey("Id");

                b.HasIndex("NotificationName", "EntityTypeName", "EntityId", "UserId");

                b.HasIndex("TenantId", "NotificationName", "EntityTypeName", "EntityId", "UserId");

                b.ToTable("AbpNotificationSubscriptions");
            });

            modelBuilder.Entity("Abp.Notifications.TenantNotificationInfo", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<string>("Data")
                    .HasMaxLength(1048576);

                b.Property<string>("DataTypeName")
                    .HasMaxLength(512);

                b.Property<string>("EntityId")
                    .HasMaxLength(96);

                b.Property<string>("EntityTypeAssemblyQualifiedName")
                    .HasMaxLength(512);

                b.Property<string>("EntityTypeName")
                    .HasMaxLength(250);

                b.Property<string>("NotificationName")
                    .IsRequired()
                    .HasMaxLength(96);

                b.Property<byte>("Severity");

                b.Property<int?>("TenantId");

                b.HasKey("Id");

                b.HasIndex("TenantId");

                b.ToTable("AbpTenantNotifications");
            });

            modelBuilder.Entity("Abp.Notifications.UserNotificationInfo", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<DateTime>("CreationTime");

                b.Property<int>("State");

                b.Property<int?>("TenantId");

                b.Property<Guid>("TenantNotificationId");

                b.Property<long>("UserId");

                b.HasKey("Id");

                b.HasIndex("UserId", "State", "CreationTime");

                b.ToTable("AbpUserNotifications");
            });

            modelBuilder.Entity("Abp.Organizations.OrganizationUnit", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("Code")
                    .IsRequired()
                    .HasMaxLength(95);

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<long?>("DeleterUserId");

                b.Property<DateTime?>("DeletionTime");

                b.Property<string>("DisplayName")
                    .IsRequired()
                    .HasMaxLength(128);

                b.Property<bool>("IsDeleted");

                b.Property<DateTime?>("LastModificationTime");

                b.Property<long?>("LastModifierUserId");

                b.Property<long?>("ParentId");

                b.Property<int?>("TenantId");

                b.HasKey("Id");

                b.HasIndex("ParentId");

                b.HasIndex("TenantId", "Code");

                b.ToTable("AbpOrganizationUnits");
            });

            modelBuilder.Entity("HC.WeChat.Authorization.Roles.Role", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken();

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<long?>("DeleterUserId");

                b.Property<DateTime?>("DeletionTime");

                b.Property<string>("Description")
                    .HasMaxLength(5000);

                b.Property<string>("DisplayName")
                    .IsRequired()
                    .HasMaxLength(64);

                b.Property<bool>("IsDefault");

                b.Property<bool>("IsDeleted");

                b.Property<bool>("IsStatic");

                b.Property<DateTime?>("LastModificationTime");

                b.Property<long?>("LastModifierUserId");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(32);

                b.Property<string>("NormalizedName")
                    .IsRequired()
                    .HasMaxLength(32);

                b.Property<int?>("TenantId");

                b.HasKey("Id");

                b.HasIndex("CreatorUserId");

                b.HasIndex("DeleterUserId");

                b.HasIndex("LastModifierUserId");

                b.HasIndex("TenantId", "NormalizedName");

                b.ToTable("AbpRoles");
            });

            modelBuilder.Entity("HC.WeChat.Authorization.Users.User", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<int>("AccessFailedCount");

                b.Property<string>("AuthenticationSource")
                    .HasMaxLength(64);

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken();

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<long?>("DeleterUserId");

                b.Property<DateTime?>("DeletionTime");

                b.Property<string>("EmailAddress")
                    .IsRequired()
                    .HasMaxLength(256);

                b.Property<string>("EmailConfirmationCode")
                    .HasMaxLength(328);

                b.Property<bool>("IsActive");

                b.Property<bool>("IsDeleted");

                b.Property<bool>("IsEmailConfirmed");

                b.Property<bool>("IsLockoutEnabled");

                b.Property<bool>("IsPhoneNumberConfirmed");

                b.Property<bool>("IsTwoFactorEnabled");

                b.Property<DateTime?>("LastLoginTime");

                b.Property<DateTime?>("LastModificationTime");

                b.Property<long?>("LastModifierUserId");

                b.Property<DateTime?>("LockoutEndDateUtc");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(32);

                b.Property<string>("NormalizedEmailAddress")
                    .IsRequired()
                    .HasMaxLength(256);

                b.Property<string>("NormalizedUserName")
                    .IsRequired()
                    .HasMaxLength(32);

                b.Property<string>("Password")
                    .IsRequired()
                    .HasMaxLength(128);

                b.Property<string>("PasswordResetCode")
                    .HasMaxLength(328);

                b.Property<string>("PhoneNumber");

                b.Property<string>("SecurityStamp");

                b.Property<string>("Surname")
                    .IsRequired()
                    .HasMaxLength(32);

                b.Property<int?>("TenantId");

                b.Property<string>("UserName")
                    .IsRequired()
                    .HasMaxLength(32);

                b.HasKey("Id");

                b.HasIndex("CreatorUserId");

                b.HasIndex("DeleterUserId");

                b.HasIndex("LastModifierUserId");

                b.HasIndex("TenantId", "NormalizedEmailAddress");

                b.HasIndex("TenantId", "NormalizedUserName");

                b.ToTable("AbpUsers");
            });

            modelBuilder.Entity("HC.WeChat.MultiTenancy.Tenant", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("ConnectionString")
                    .HasMaxLength(1024);

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<long?>("DeleterUserId");

                b.Property<DateTime?>("DeletionTime");

                b.Property<int?>("EditionId");

                b.Property<bool>("IsActive");

                b.Property<bool>("IsDeleted");

                b.Property<DateTime?>("LastModificationTime");

                b.Property<long?>("LastModifierUserId");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(128);

                b.Property<string>("TenancyName")
                    .IsRequired()
                    .HasMaxLength(64);

                b.HasKey("Id");

                b.HasIndex("CreatorUserId");

                b.HasIndex("DeleterUserId");

                b.HasIndex("EditionId");

                b.HasIndex("LastModifierUserId");

                b.HasIndex("TenancyName");

                b.ToTable("AbpTenants");
            });

            modelBuilder.Entity("Abp.Application.Features.EditionFeatureSetting", b =>
            {
                b.HasBaseType("Abp.Application.Features.FeatureSetting");

                b.Property<int>("EditionId");

                b.HasIndex("EditionId", "Name");

                b.ToTable("AbpFeatures");

                b.HasDiscriminator().HasValue("EditionFeatureSetting");
            });

            modelBuilder.Entity("Abp.MultiTenancy.TenantFeatureSetting", b =>
            {
                b.HasBaseType("Abp.Application.Features.FeatureSetting");


                b.HasIndex("TenantId", "Name");

                b.ToTable("AbpFeatures");

                b.HasDiscriminator().HasValue("TenantFeatureSetting");
            });

            modelBuilder.Entity("Abp.Authorization.Roles.RolePermissionSetting", b =>
            {
                b.HasBaseType("Abp.Authorization.PermissionSetting");

                b.Property<int>("RoleId");

                b.HasIndex("RoleId");

                b.ToTable("AbpPermissions");

                b.HasDiscriminator().HasValue("RolePermissionSetting");
            });

            modelBuilder.Entity("Abp.Authorization.Users.UserPermissionSetting", b =>
            {
                b.HasBaseType("Abp.Authorization.PermissionSetting");

                b.Property<long>("UserId");

                b.HasIndex("UserId");

                b.ToTable("AbpPermissions");

                b.HasDiscriminator().HasValue("UserPermissionSetting");
            });

            modelBuilder.Entity("Abp.Authorization.Roles.RoleClaim", b =>
            {
                b.HasOne("HC.WeChat.Authorization.Roles.Role")
                    .WithMany("Claims")
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Abp.Authorization.Users.UserClaim", b =>
            {
                b.HasOne("HC.WeChat.Authorization.Users.User")
                    .WithMany("Claims")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Abp.Authorization.Users.UserLogin", b =>
            {
                b.HasOne("HC.WeChat.Authorization.Users.User")
                    .WithMany("Logins")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Abp.Authorization.Users.UserRole", b =>
            {
                b.HasOne("HC.WeChat.Authorization.Users.User")
                    .WithMany("Roles")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Abp.Authorization.Users.UserToken", b =>
            {
                b.HasOne("HC.WeChat.Authorization.Users.User")
                    .WithMany("Tokens")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Abp.Configuration.Setting", b =>
            {
                b.HasOne("HC.WeChat.Authorization.Users.User")
                    .WithMany("Settings")
                    .HasForeignKey("UserId");
            });

            modelBuilder.Entity("Abp.EntityHistory.EntityChange", b =>
            {
                b.HasOne("Abp.EntityHistory.EntityChangeSet")
                    .WithMany("EntityChanges")
                    .HasForeignKey("EntityChangeSetId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Abp.EntityHistory.EntityPropertyChange", b =>
            {
                b.HasOne("Abp.EntityHistory.EntityChange")
                    .WithMany("PropertyChanges")
                    .HasForeignKey("EntityChangeId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Abp.Organizations.OrganizationUnit", b =>
            {
                b.HasOne("Abp.Organizations.OrganizationUnit", "Parent")
                    .WithMany("Children")
                    .HasForeignKey("ParentId");
            });

            modelBuilder.Entity("HC.WeChat.Authorization.Roles.Role", b =>
            {
                b.HasOne("HC.WeChat.Authorization.Users.User", "CreatorUser")
                    .WithMany()
                    .HasForeignKey("CreatorUserId");

                b.HasOne("HC.WeChat.Authorization.Users.User", "DeleterUser")
                    .WithMany()
                    .HasForeignKey("DeleterUserId");

                b.HasOne("HC.WeChat.Authorization.Users.User", "LastModifierUser")
                    .WithMany()
                    .HasForeignKey("LastModifierUserId");
            });

            modelBuilder.Entity("HC.WeChat.Authorization.Users.User", b =>
            {
                b.HasOne("HC.WeChat.Authorization.Users.User", "CreatorUser")
                    .WithMany()
                    .HasForeignKey("CreatorUserId");

                b.HasOne("HC.WeChat.Authorization.Users.User", "DeleterUser")
                    .WithMany()
                    .HasForeignKey("DeleterUserId");

                b.HasOne("HC.WeChat.Authorization.Users.User", "LastModifierUser")
                    .WithMany()
                    .HasForeignKey("LastModifierUserId");
            });

            modelBuilder.Entity("HC.WeChat.MultiTenancy.Tenant", b =>
            {
                b.HasOne("HC.WeChat.Authorization.Users.User", "CreatorUser")
                    .WithMany()
                    .HasForeignKey("CreatorUserId");

                b.HasOne("HC.WeChat.Authorization.Users.User", "DeleterUser")
                    .WithMany()
                    .HasForeignKey("DeleterUserId");

                b.HasOne("Abp.Application.Editions.Edition", "Edition")
                    .WithMany()
                    .HasForeignKey("EditionId");

                b.HasOne("HC.WeChat.Authorization.Users.User", "LastModifierUser")
                    .WithMany()
                    .HasForeignKey("LastModifierUserId");
            });

            modelBuilder.Entity("Abp.Application.Features.EditionFeatureSetting", b =>
            {
                b.HasOne("Abp.Application.Editions.Edition", "Edition")
                    .WithMany()
                    .HasForeignKey("EditionId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Abp.Authorization.Roles.RolePermissionSetting", b =>
            {
                b.HasOne("HC.WeChat.Authorization.Roles.Role")
                    .WithMany("Permissions")
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Abp.Authorization.Users.UserPermissionSetting", b =>
            {
                b.HasOne("HC.WeChat.Authorization.Users.User")
                    .WithMany("Permissions")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
#pragma warning restore 612, 618

            modelBuilder.Entity("HC.WeChat.WechatAppConfigs.WechatAppConfig", b =>
            {
                b.Property<int>("Id").ValueGeneratedOnAdd();
                b.Property<string>("Name").IsRequired().HasMaxLength(50);
                b.Property<string>("AppOrgId").IsRequired().HasMaxLength(250);
                b.Property<int>("AppType").IsRequired();
                b.Property<string>("AppId").IsRequired().HasMaxLength(250);
                b.Property<string>("AppSecret").IsRequired().HasMaxLength(250);
                b.Property<string>("EncodingAESKey").HasMaxLength(500);
                b.Property<string>("QRCodeUrl ").HasMaxLength(250);
                b.Property<string>("Token").IsRequired().HasMaxLength(250);
                b.Property<string>("AccessToken").HasMaxLength(255);
                b.Property<int?>("ExpiresIn");
                b.Property<DateTime?>("NextGettime");
                b.Property<int>("TenantId").IsRequired();
                b.Property<DateTime>("CreationTime").IsRequired();
                b.Property<long?>("CreatorUserId");
                b.Property<DateTime?>("LastModificationTime");
                b.Property<long>("LastModifierUserId"); b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("WechatAppConfigs");
            });

            modelBuilder.Entity("HC.WeChat.WechatMessages.WechatMessage", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<string>("KeyWord").IsRequired().HasMaxLength(50);
                b.Property<int>("MatchMode").IsRequired();
                b.Property<int>("MsgType").IsRequired();
                b.Property<string>("Content").IsRequired();
                b.Property<int>("TenantId").IsRequired();
                b.Property<DateTime>("CreationTime").IsRequired();
                b.Property<long?>("CreatorUserId");
                b.Property<DateTime?>("LastModificationTime");
                b.Property<long>("LastModifierUserId"); b.HasKey("Id");
                b.Property<string>("Title");
                b.Property<string>("Desc");
                b.Property<string>("PicLink");
                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("WechatMessages");
            });

            modelBuilder.Entity("HC.WeChat.WechatSubscribes.WechatSubscribe", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<int>("MsgType").IsRequired();
                b.Property<string>("Content").IsRequired();
                b.Property<int>("TenantId").IsRequired();
                b.Property<DateTime>("CreationTime").IsRequired();
                b.Property<long?>("CreatorUserId");
                b.Property<DateTime?>("LastModificationTime");
                b.Property<long>("LastModifierUserId"); b.HasKey("Id");
                b.Property<string>("Title");
                b.Property<string>("Desc");
                b.Property<string>("PicLink");
                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("WechatSubscribes");
            });

            modelBuilder.Entity("HC.WeChat.Activities.Activity", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<string>("Name").IsRequired().HasMaxLength(200);
                b.Property<DateTime>("BeginTime").IsRequired();
                b.Property<DateTime>("EndTime").IsRequired();
                b.Property<int>("ActivityType").IsRequired();
                b.Property<string>("Content").IsRequired();
                b.Property<int?>("MUnfinished");
                b.Property<int?>("RUnfinished");
                b.Property<int?>("TenantId");
                b.Property<bool>("IsDeleted").IsRequired();
                b.Property<DateTime>("CreationTime").IsRequired();
                b.Property<long?>("CreatorUserId");
                b.Property<DateTime?>("LastModificationTime");
                b.Property<long?>("LastModifierUserId");
                b.Property<DateTime?>("DeletionTime");
                b.Property<long?>("DeleterUserId");
                b.Property<DateTime?>("PublishTime");
                b.Property<int>("Status").IsRequired();

                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("Activities");
            });

            modelBuilder.Entity("HC.WeChat.ActivityForms.ActivityForm", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<string>("FormCode").IsRequired().HasMaxLength(50);
                b.Property<Guid>("ActivityId").IsRequired();
                b.Property<Guid?>("RetailerId");
                b.Property<Guid?>("ActivityGoodsId");
                b.Property<string>("GoodsSpecification").HasMaxLength(200);
                b.Property<int>("Num").IsRequired();
                b.Property<string>("Reason").IsRequired();
                b.Property<int>("Status").IsRequired();
                b.Property<DateTime>("CreationTime").IsRequired();
                b.Property<string>("ActivityName").HasMaxLength(200);
                b.Property<string>("RetailerName").HasMaxLength(50);
                b.Property<string>("ManagerName").HasMaxLength(50);
                b.Property<Guid?>("ManagerId");
                b.Property<Guid?>("CreationId");
                b.Property<string>("CreationUser").HasMaxLength(50);

                b.HasKey("Id");

                b.HasIndex("FormCode").IsUnique();

                b.ToTable("ActivityForms");
            });

            modelBuilder.Entity("HC.WeChat.ActivityBanquets.ActivityBanquet", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<Guid>("ActivityFormId").IsRequired();
                b.Property<string>("Area").IsRequired();
                b.Property<string>("Responsible").HasMaxLength(50);
                b.Property<string>("Executor").HasMaxLength(50);
                b.Property<DateTime>("BanquetTime").IsRequired();
                b.Property<string>("Position").IsRequired().HasMaxLength(500);
                b.Property<int>("Num").IsRequired();
                b.Property<string>("Desc").IsRequired().HasMaxLength(500);
                b.Property<string>("PhotoUrl");
                b.Property<DateTime>("CreationTime").IsRequired();
                b.Property<string>("UserName").IsRequired().HasMaxLength(50);

                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("ActivityBanquets");
            });

            modelBuilder.Entity("HC.WeChat.ActivityDeliveryInfos.ActivityDeliveryInfo", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<Guid>("ActivityFormId").IsRequired();
                b.Property<string>("UserName").HasMaxLength(50);
                b.Property<string>("Phone").HasMaxLength(20);
                b.Property<string>("Address").HasMaxLength(500);
                b.Property<int?>("Type");
                b.Property<string>("ExpressCompany").HasMaxLength(200);
                b.Property<string>("ExpressNo").HasMaxLength(200);
                b.Property<string>("Remark").HasMaxLength(500);
                b.Property<string>("DeliveryRemark").HasMaxLength(500);
                b.Property<DateTime?>("SendTime");
                b.Property<DateTime>("CreationTime").IsRequired();
                b.Property<bool>("IsSend");

                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("ActivityDeliveryInfos");
            });

            modelBuilder.Entity("HC.WeChat.ActivityFormLogs.ActivityFormLog", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<Guid>("ActivityFormId").IsRequired();
                b.Property<int>("Status").IsRequired();
                b.Property<string>("StatusName").IsRequired();
                b.Property<string>("Opinion").HasMaxLength(200);
                b.Property<int>("UserType").IsRequired();
                b.Property<Guid?>("UserId");
                b.Property<string>("UserName").HasMaxLength(50);
                b.Property<DateTime>("ActionTime").IsRequired();

                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("ActivityFormLogs");
            });

            modelBuilder.Entity("HC.WeChat.ActivityGoodses.ActivityGoods", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<string>("Specification").IsRequired().HasMaxLength(200);
                b.Property<Guid>("ActivityId").IsRequired();
                b.Property<int>("MinNum").IsRequired();
                b.Property<int>("MaxNum").IsRequired();
                b.Property<string>("DiscountDesc");
                b.Property<bool>("IsDeleted").IsRequired();
                b.Property<DateTime>("CreationTime").IsRequired();
                b.Property<long?>("CreatorUserId");
                b.Property<DateTime?>("LastModificationTime");
                b.Property<long?>("LastModifierUserId");
                b.Property<DateTime?>("DeletionTime");
                b.Property<long?>("DeleterUserId");

                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("ActivityGoodses");
            });

            modelBuilder.Entity("HC.WeChat.Employees.Employee", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<string>("Code").IsRequired().HasMaxLength(50);
                b.Property<string>("Name").IsRequired().HasMaxLength(50);
                b.Property<int>("Position").IsRequired();
                b.Property<string>("Phone").HasMaxLength(20);
                b.Property<string>("Company").HasMaxLength(200);
                b.Property<string>("Department").HasMaxLength(200);
                b.Property<bool>("IsAction").IsRequired();
                b.Property<int?>("TenantId");
                b.Property<bool>("IsDeleted").IsRequired();
                b.Property<DateTime>("CreationTime").IsRequired();
                b.Property<long?>("CreatorUserId");
                b.Property<DateTime?>("LastModificationTime");
                b.Property<long?>("LastModifierUserId");
                b.Property<DateTime?>("DeletionTime");
                b.Property<long?>("DeleterUserId");
                b.Property<string>("VerificationCode").HasMaxLength(50);
                b.Property<string>("InnerCode").HasMaxLength(50);

                b.HasKey("Id");

                b.HasIndex("Code");

                b.ToTable("Employees");
            });

            modelBuilder.Entity("HC.WeChat.Retailers.Retailer", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<string>("Code").IsRequired().HasMaxLength(50);
                b.Property<string>("Name").IsRequired().HasMaxLength(50);
                b.Property<string>("BusinessAddress").HasMaxLength(500);
                b.Property<string>("ArchivalLevel").HasMaxLength(100);
                b.Property<string>("OrderCycle").HasMaxLength(100);
                b.Property<string>("StoreType").HasMaxLength(100);
                b.Property<string>("Telephone").HasMaxLength(100);
                b.Property<bool>("IsAction").IsRequired();
                b.Property<string>("BranchCompany").HasMaxLength(200);
                b.Property<string>("Department").HasMaxLength(100);
                b.Property<Guid?>("EmployeeId");
                b.Property<string>("Manager").HasMaxLength(50);
                b.Property<int?>("OrderMode");
                b.Property<int?>("TerminalType");
                b.Property<string>("BusinessType").HasMaxLength(100);
                b.Property<int?>("Scale");
                b.Property<int?>("MarketType");
                b.Property<string>("DeliveryLine").HasMaxLength(500);
                b.Property<int?>("TenantId");
                b.Property<bool>("IsDeleted").IsRequired();
                b.Property<DateTime>("CreationTime").IsRequired();
                b.Property<long?>("CreatorUserId");
                b.Property<DateTime?>("LastModificationTime");
                b.Property<long?>("LastModifierUserId");
                b.Property<DateTime?>("DeletionTime");
                b.Property<long?>("DeleterUserId");
                b.Property<string>("LicenseKey").HasMaxLength(50);
                b.Property<string>("VerificationCode").HasMaxLength(50);
                b.Property<string>("CustId").HasMaxLength(30);
                b.Property<string>("DisLineCode").HasMaxLength(50);
                b.Property<string>("Category").HasMaxLength(6);
                b.Property<string>("DepartmentId").HasMaxLength(50);
                b.Property<string>("SlsmanId").HasMaxLength(50);
                b.Property<string>("SlsmanName").HasMaxLength(100);
                b.Property<string>("Area").HasMaxLength(100);
                b.HasKey("Id");

                b.HasIndex("Code");

                b.ToTable("Retailers");
            });

            modelBuilder.Entity("HC.WeChat.WeChatUsers.WeChatUser", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<string>("NickName").IsRequired().HasMaxLength(50);
                b.Property<string>("OpenId").IsRequired().HasMaxLength(50);
                b.Property<int>("UserType").IsRequired();
                b.Property<Guid?>("UserId");
                b.Property<string>("UserName").IsRequired().HasMaxLength(50);
                b.Property<int>("BindStatus").IsRequired();
                b.Property<DateTime?>("BindTime");
                b.Property<int?>("TenantId");
                b.Property<DateTime?>("UnBindTime");
                b.Property<string>("HeadImgUrl").HasMaxLength(500);
                b.Property<string>("Phone").HasMaxLength(20);
                b.Property<string>("MemberBarCode").HasMaxLength(30);
                b.Property<int?>("IntegralTotal");
                b.Property<bool?>("IsShopkeeper");
                b.Property<int>("UserType").IsRequired();
                b.Property<int?>("Status");
                b.Property<DateTime?>("AttentionTime");
                b.Property<DateTime?>("UnfollowTime");
                b.Property<string>("Ticket").HasMaxLength(200);
                b.Property<int?>("SourceType");
                b.Property<string>("SourceId").HasMaxLength(100);

                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("WeChatUsers");
            });

            modelBuilder.Entity("HC.WeChat.ActivityForms.ActivityForm", b =>
            {
                b.HasOne("HC.WeChat.Activities.Activity")
                    .WithMany("Forms")
                    .HasForeignKey("ActivityId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("HC.WeChat.ActivityGoodses.ActivityGoods", b =>
            {
                b.HasOne("HC.WeChat.Activities.Activity")
                    .WithMany("Goodses")
                    .HasForeignKey("ActivityId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("HC.WeChat.ActivityBanquets.ActivityBanquet", b =>
            {
                b.HasOne("HC.WeChat.ActivityForms.ActivityForm")
                    .WithMany("Banquet")
                    .HasForeignKey("ActivityFormId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("HC.WeChat.ActivityDeliveryInfos.ActivityDeliveryInfo", b =>
            {
                b.HasOne("HC.WeChat.ActivityForms.ActivityForm")
                    .WithMany("DeliveryInfo")
                    .HasForeignKey("ActivityFormId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("HC.WeChat.ActivityFormLogs.ActivityFormLog", b =>
            {
                b.HasOne("HC.WeChat.ActivityForms.ActivityForm")
                    .WithMany("ApprovalLogs")
                    .HasForeignKey("ActivityFormId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            //add 2018-4-5
            modelBuilder.Entity("HC.WeChat.Advises.Advise", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<string>("Title").IsRequired().HasMaxLength(500);
                b.Property<string>("UserTypeName").IsRequired().HasMaxLength(50);
                b.Property<string>("OpenId").HasMaxLength(50);
                b.Property<string>("Phone").HasMaxLength(20);
                b.Property<string>("Content").HasMaxLength(500);
                b.Property<string>("PhotoUrl").HasMaxLength(2000);
                b.Property<int?>("TenantId");
                b.Property<DateTime>("CreationTime").IsRequired();
                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("Advises");
            });

            modelBuilder.Entity("HC.WeChat.UserQuestions.UserQuestion", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<string>("Name").IsRequired().HasMaxLength(500);
                b.Property<string>("UserName").HasMaxLength(50);
                b.Property<string>("Phone").HasMaxLength(20);
                b.Property<string>("Address").HasMaxLength(500);
                b.Property<string>("OpenId").HasMaxLength(50);
                b.Property<int?>("TenantId");
                b.Property<DateTime>("CreationTime").IsRequired();

                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("UserQuestions");
            });

            modelBuilder.Entity("HC.WeChat.UserAnswers.UserAnswer", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<Guid>("UserQuestionId").IsRequired();
                b.Property<int?>("AnswerSqe");
                b.Property<string>("Content").HasMaxLength(500);
                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("UserAnswers");
            });

            modelBuilder.Entity("HC.WeChat.UserAnswers.UserAnswer", b =>
            {
                b.HasOne("HC.WeChat.UserQuestions.UserQuestion")
                    .WithMany("UserAnswer")
                    .HasForeignKey("UserQuestionId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            //2018-4-23
            modelBuilder.Entity("HC.WeChat.Articles.Article", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<string>("Title").IsRequired().HasMaxLength(200);
                b.Property<string>("Author").IsRequired().HasMaxLength(50);
                b.Property<int>("Type");
                b.Property<string>("CoverPhoto").IsRequired().HasMaxLength(500);
                b.Property<string>("Content");
                b.Property<int?>("ReadTotal");
                b.Property<int?>("GoodTotal");
                b.Property<int?>("TenantId");
                b.Property<bool>("IsDeleted").IsRequired();
                b.Property<DateTime>("CreationTime").IsRequired();
                b.Property<long?>("CreatorUserId");
                b.Property<DateTime?>("LastModificationTime");
                b.Property<long?>("LastModifierUserId");
                b.Property<DateTime?>("DeletionTime");
                b.Property<long?>("DeleterUserId");
                b.Property<DateTime?>("PushTime");

                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("Articles");
            });

            modelBuilder.Entity("HC.WeChat.IntegralDetails.IntegralDetail", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<string>("OpenId").HasMaxLength(50);
                b.Property<int?>("InitialIntegral");
                b.Property<int?>("Integral");
                b.Property<int?>("FinalIntegral");
                b.Property<int?>("Type");
                b.Property<string>("Desc").HasMaxLength(500);
                b.Property<string>("RefId").HasMaxLength(500);
                b.Property<DateTime>("CreationTime").IsRequired();
                b.Property<int?>("TenantId");

                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("IntegralDetails");
            });

            modelBuilder.Entity("HC.WeChat.Manuscripts.Manuscript", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<int?>("Type");
                b.Property<string>("Title").IsRequired().HasMaxLength(200);
                b.Property<string>("Content");
                b.Property<string>("UserName").HasMaxLength(50);
                b.Property<string>("Phone").HasMaxLength(20);
                b.Property<string>("OpenId").HasMaxLength(50);
                b.Property<int?>("Status");
                b.Property<int?>("TenantId");
                b.Property<DateTime>("CreationTime").IsRequired();
                b.Property<DateTime>("DealWithTime");

                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("Manuscripts");
            });

            modelBuilder.Entity("HC.WeChat.MemberConfigs.MemberConfig", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<int?>("Type");
                b.Property<int?>("TenantId");
                b.Property<int?>("Code");
                b.Property<string>("Value");
                b.Property<DateTime>("CreationTime").IsRequired();

                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("MemberConfigs");
            });

            modelBuilder.Entity("HC.WeChat.Products.Product", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<string>("Specification").IsRequired().HasMaxLength(200);
                b.Property<int?>("Type");
                b.Property<decimal?>("Price");
                b.Property<bool?>("IsRare");
                b.Property<string>("PackageCode").HasMaxLength(50);
                b.Property<string>("BarCode").HasMaxLength(50);
                b.Property<int?>("SearchCount");
                b.Property<bool?>("IsAction");
                b.Property<DateTime>("CreationTime").IsRequired();
                b.Property<int?>("TenantId");
                b.Property<long?>("CreatorUserId");
                b.Property<string>("PhotoUrl").HasMaxLength(500);
                b.Property<string>("Desc").HasMaxLength(200);
                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("Products");
            });

            modelBuilder.Entity("HC.WeChat.PurchaseRecords.PurchaseRecord", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<Guid?>("ProductId");
                b.Property<string>("Specification").HasMaxLength(200);
                b.Property<int?>("Quantity");
                b.Property<Guid?>("ShopId");
                b.Property<string>("ShopName").HasMaxLength(200);
                b.Property<string>("OpenId").IsRequired().HasMaxLength(50);
                b.Property<int?>("TenantId");
                b.Property<int?>("Integral");
                b.Property<string>("Remark").HasMaxLength(500);
                b.Property<DateTime>("CreationTime").IsRequired();
                b.Property<string>("OperatorOpenId").HasMaxLength(50);
                b.Property<string>("OperatorName").HasMaxLength(50);

                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("PurchaseRecords");
            });

            modelBuilder.Entity("HC.WeChat.Shops.Shop", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<string>("Name").IsRequired().HasMaxLength(200);
                b.Property<string>("Address").HasMaxLength(200);
                b.Property<string>("Desc").HasMaxLength(500);
                b.Property<Guid?>("RetailerId");
                b.Property<string>("CoverPhoto").HasMaxLength(500);
                b.Property<int?>("SaleTotal");
                b.Property<int?>("ReadTotal");
                b.Property<string>("Evaluation").HasMaxLength(100);
                b.Property<double?>("Longitude").HasColumnType("decimal(11,8)");
                b.Property<double?>("Latitude").HasColumnType("decimal(11,8)");
                b.Property<double?>("QqLongitude").HasColumnType("decimal(11,8)");
                b.Property<double?>("QqLatitude").HasColumnType("decimal(11,8)");
                b.Property<int?>("Status");
                b.Property<DateTime?>("AuditTime");
                b.Property<DateTime>("CreationTime").IsRequired();
                b.Property<int?>("TenantId");
                b.Property<string>("Tel").HasMaxLength(20);
                b.Property<string>("Reason").HasMaxLength(500);
                b.Property<int?>("SingleTotal");
                b.Property<string>("Ticket").HasMaxLength(200);
                b.Property<string>("WechatUrl").HasMaxLength(150);
                b.Property<string>("QRUrl").HasMaxLength(500);
                b.Property<int?>("FansNum");

                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("Shops");
            });

            modelBuilder.Entity("HC.WeChat.ShopEvaluations.ShopEvaluation", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<Guid?>("PurchaseRecordId");
                b.Property<Guid>("ShopId").IsRequired();
                b.Property<string>("OpenId").IsRequired().HasMaxLength(50);
                b.Property<int?>("Evaluation");
                b.Property<bool?>("IsCorrectQuantity");
                b.Property<string>("Content").HasMaxLength(500);
                b.Property<DateTime>("CreationTime").IsRequired();
                b.Property<int?>("TenantId");

                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("ShopEvaluations");
            });

            modelBuilder.Entity("HC.WeChat.ShopProducts.ShopProduct", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<Guid>("ProductId").IsRequired();
                b.Property<Guid>("ShopId").IsRequired();
                b.Property<string>("Specification").HasMaxLength(200);

                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("ShopProducts");
            });

            modelBuilder.Entity("HC.WeChat.StatisticalDetails.StatisticalDetail", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<string>("OpenId").IsRequired().HasMaxLength(50);
                b.Property<Guid>("ArticleId").IsRequired();
                b.Property<int>("Type").IsRequired();
                b.Property<DateTime>("CreationTime").IsRequired();
                b.Property<int?>("TenantId");

                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("StatisticalDetails");
            });
            modelBuilder.Entity("HC.WeChat.WeChatGroups.WeChatGroup", b =>
            {
                b.Property<int>("Id").ValueGeneratedOnAdd();
                b.Property<int>("TypeCode").IsRequired();
                b.Property<string>("TypeName");
                b.Property<int>("TagId").IsRequired();
                b.Property<string>("TagName").IsRequired().HasMaxLength(50);
                b.Property<int?>("TenantId");
                b.Property<DateTime>("CreationTime").IsRequired();
                b.Property<long>("CreatorUserId");
                b.Property<DateTime>("LastModificationTime");
                b.Property<long>("LastModifierUserId");

                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("WeChatGroups");
            });

            modelBuilder.Entity("HC.WeChat.LevelLogs.LevelLog", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<string>("LevelData").IsRequired().HasMaxLength(50);
                b.Property<DateTime?>("ChangeTime");

                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("LevelLogs");
            });

            modelBuilder.Entity("HC.WeChat.LuckyDraws.LuckyDraw", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<string>("Name").IsRequired().HasMaxLength(200);
                b.Property<DateTime?>("BeginTime");
                b.Property<DateTime?>("EndTime");
                b.Property<int>("Type").IsRequired();
                b.Property<string>("Content");
                b.Property<string>("Desc");
                b.Property<int?>("Consume");
                b.Property<int?>("Frequency");
                b.Property<int?>("TenantId");
                b.Property<DateTime>("CreationTime").IsRequired();
                b.Property<long?>("CreatorUserId");
                b.Property<DateTime?>("LastModificationTime");
                b.Property<long?>("LastModifierUserId");
                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("LuckyDraws");
            }); modelBuilder.Entity("HC.WeChat.Prizes.Prize", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<string>("Name").IsRequired().HasMaxLength(50);
                b.Property<int>("Value").IsRequired();
                b.Property<Guid>("LuckyDrawId").IsRequired();
                b.Property<int>("Type").IsRequired();
                b.Property<string>("Color").HasMaxLength(50);
                b.Property<int>("GetWay").IsRequired();
                b.Property<int?>("Num");
                b.Property<int?>("UserMaxNum");
                b.Property<int?>("WinningNum");
                b.Property<decimal?>("WinningRate");
                b.Property<int?>("Seq");
                b.Property<int?>("ExpiryDay");
                b.Property<int?>("LimitedMode");
                b.Property<int?>("LimitedNum");
                b.Property<bool>("IsDeleted").IsRequired();
                b.Property<DateTime>("CreationTime").IsRequired();
                b.Property<long?>("CreatorUserId");
                b.Property<DateTime?>("LastModificationTime");
                b.Property<long?>("LastModifierUserId");
                b.Property<DateTime?>("DeletionTime");
                b.Property<long?>("DeleterUserId");
                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("Prizes");
            }); modelBuilder.Entity("HC.WeChat.UserAddresss.UserAddress", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<Guid?>("UserId");
                b.Property<string>("Name").IsRequired().HasMaxLength(50);
                b.Property<string>("Phone").IsRequired().HasMaxLength(20);
                b.Property<string>("Address").IsRequired().HasMaxLength(500);
                b.Property<bool?>("IsDefault");
                b.Property<string>("Remark").HasMaxLength(500);
                b.Property<int?>("TenantId");
                b.Property<bool>("IsDeleted").IsRequired();
                b.Property<DateTime>("CreationTime").IsRequired();
                b.Property<long?>("CreatorUserId");
                b.Property<DateTime?>("LastModificationTime");
                b.Property<long?>("LastModifierUserId");
                b.Property<DateTime?>("DeletionTime");
                b.Property<long?>("DeleterUserId");
                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("UserAddresss");
            }); modelBuilder.Entity("HC.WeChat.WinningRecords.WinningRecord", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<Guid?>("PrizeId");
                b.Property<Guid?>("UserId");
                b.Property<Guid?>("AddressId");
                b.Property<DateTime?>("WinningTime");
                b.Property<int?>("Num");
                b.Property<DateTime>("ExpiryTime").IsRequired();
                b.Property<int?>("Status");
                b.Property<DateTime?>("ApplyTime");
                b.Property<DateTime?>("CompleteTime");
                b.Property<string>("ExpressCompany").HasMaxLength(200);
                b.Property<string>("ExpressNo").HasMaxLength(50);
                b.Property<DateTime>("CreationTime").IsRequired();
                b.Property<long?>("CreatorUserId");
                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("WinningRecords");
            });
            modelBuilder.Entity("HC.WeChat.QrCodeLogs.QrCodeLog", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<DateTime?>("AttentionTime").IsRequired();
                b.Property<string>("OpenId").HasMaxLength(50);
                b.Property<string>("SourceId").HasMaxLength(100);
                b.Property<int?>("SourceType");
                b.Property<string>("Ticket").HasMaxLength(200);
                b.Property<int?>("TenantId");
                b.Property<DateTime>("CreationTime").IsRequired();
                b.Property<long?>("CreatorUserId");
                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("QrCodeLogs");
            });
            modelBuilder.Entity("HC.WeChat.Exhibitions.Exhibition", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<DateTime?>("BeginTime");
                b.Property<DateTime?>("EndTime"); b.Property<string>("Content");
                b.Property<string>("Desc"); b.Property<int>("TopTotal").IsRequired();
                b.Property<int?>("Frequency"); b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("Exhibitions");
            }); modelBuilder.Entity("HC.WeChat.ExhibitionShops.ExhibitionShop", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd(); b.Property<DateTime>("CreateTime").IsRequired();
                b.Property<Guid?>("RetailerId");
                b.Property<string>("ShopName").IsRequired().HasMaxLength(200);
                b.Property<string>("ShopAddress").HasMaxLength(500);
                b.Property<string>("PicPath").IsRequired();
                b.Property<int?>("Votes");
                b.Property<int>("Status").IsRequired();
                b.Property<Guid>("ShopId").IsRequired();
                b.Property<DateTime?>("AuditTime");
                b.Property<int>("NoId");
                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("ExhibitionShops");
            }); modelBuilder.Entity("HC.WeChat.VoteLogs.VoteLog", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<DateTime>("CreateTime").IsRequired();
                b.Property<string>("OpenId").IsRequired().HasMaxLength(50);
                b.Property<string>("UserName").HasMaxLength(50);
                b.Property<Guid>("ExhibitionId").IsRequired();
                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("VoteLogs");
            });
        }
    }
}
