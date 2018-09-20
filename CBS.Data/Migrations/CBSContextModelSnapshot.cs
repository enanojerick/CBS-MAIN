﻿// <auto-generated />
using CBS.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace CBS.Data.Migrations
{
    [DbContext(typeof(CBSContext))]
    partial class CBSContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CBS.Data.Entities.CBS_Application", b =>
                {
                    b.Property<int?>("AppID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClientID");

                    b.Property<string>("ClientName");

                    b.Property<string>("CorsOrigins");

                    b.Property<int?>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<bool?>("IsActive");

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("PostLogoutRedirectUris");

                    b.Property<string>("RedirectURIs");

                    b.Property<string>("ReturnURL");

                    b.Property<string>("Sha256");

                    b.Property<string>("URL");

                    b.HasKey("AppID");

                    b.ToTable("CBS_Application");
                });

            modelBuilder.Entity("CBS.Data.Entities.CBS_Department", b =>
                {
                    b.Property<int?>("DepartmentID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("DepartmentName");

                    b.Property<string>("FilePath");

                    b.Property<bool?>("IsActive");

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<int?>("ParentID");

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("PhoneNumber2");

                    b.HasKey("DepartmentID");

                    b.ToTable("CBS_Department");
                });

            modelBuilder.Entity("CBS.Data.Entities.CBS_DepartmentPlatforms", b =>
                {
                    b.Property<int?>("PlatformID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<int?>("DepartmentID");

                    b.Property<string>("Icon");

                    b.Property<bool?>("IsActive");

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name");

                    b.Property<int?>("PlatformTypeID");

                    b.Property<string>("URL");

                    b.HasKey("PlatformID");

                    b.HasIndex("DepartmentID");

                    b.HasIndex("PlatformTypeID");

                    b.ToTable("CBS_DepartmentPlatforms");
                });

            modelBuilder.Entity("CBS.Data.Entities.CBS_DepartmentPlatformsType", b =>
                {
                    b.Property<int?>("PlatformTypeID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<bool?>("IsActive");

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name");

                    b.HasKey("PlatformTypeID");

                    b.ToTable("CBS_DepartmentPlatformsType");
                });

            modelBuilder.Entity("CBS.Data.Entities.CBS_RolesExtention", b =>
                {
                    b.Property<int?>("RoleExtId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("Description");

                    b.Property<bool?>("IsActive");

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Role");

                    b.Property<string>("RoleId");

                    b.HasKey("RoleExtId");

                    b.ToTable("CBS_RolesExtention");
                });

            modelBuilder.Entity("CBS.Data.Entities.CBS_UserPlatform", b =>
                {
                    b.Property<int?>("PlatformID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("Icon");

                    b.Property<bool?>("IsActive");

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name");

                    b.Property<string>("URL");

                    b.Property<int?>("UserId");

                    b.HasKey("PlatformID");

                    b.HasIndex("UserId");

                    b.ToTable("CBS_UserPlatform");
                });

            modelBuilder.Entity("CBS.Data.Entities.CBS_Users", b =>
                {
                    b.Property<int?>("UserID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<int?>("DepartmentID");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("IdentityID");

                    b.Property<bool?>("IsActive");

                    b.Property<bool?>("IsFirstLogin");

                    b.Property<string>("LastName");

                    b.Property<string>("Mobile");

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("PlatformSorting");

                    b.Property<string>("PreferedName");

                    b.Property<string>("RegisterPassword");

                    b.HasKey("UserID");

                    b.HasIndex("DepartmentID");

                    b.ToTable("CBS_Users");
                });

            modelBuilder.Entity("CBS.Data.Entities.CBS_UsersApps", b =>
                {
                    b.Property<int?>("UsersAppID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AppID");

                    b.Property<int?>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<bool?>("IsActive");

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<int?>("UserID");

                    b.HasKey("UsersAppID");

                    b.HasIndex("AppID");

                    b.HasIndex("UserID");

                    b.ToTable("CBS_UsersApps");
                });

            modelBuilder.Entity("CBS.Data.Entities.CBS_DepartmentPlatforms", b =>
                {
                    b.HasOne("CBS.Data.Entities.CBS_Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentID");

                    b.HasOne("CBS.Data.Entities.CBS_DepartmentPlatformsType", "DepartmentPlatformsType")
                        .WithMany()
                        .HasForeignKey("PlatformTypeID");
                });

            modelBuilder.Entity("CBS.Data.Entities.CBS_UserPlatform", b =>
                {
                    b.HasOne("CBS.Data.Entities.CBS_Users", "Users")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("CBS.Data.Entities.CBS_Users", b =>
                {
                    b.HasOne("CBS.Data.Entities.CBS_Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentID");
                });

            modelBuilder.Entity("CBS.Data.Entities.CBS_UsersApps", b =>
                {
                    b.HasOne("CBS.Data.Entities.CBS_Application", "Application")
                        .WithMany()
                        .HasForeignKey("AppID");

                    b.HasOne("CBS.Data.Entities.CBS_Users", "Users")
                        .WithMany()
                        .HasForeignKey("UserID");
                });
#pragma warning restore 612, 618
        }
    }
}
