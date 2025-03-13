﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace microsoft_hackathon_roi_calculator.MigrationService.Migrations
{
    [DbContext(typeof(CalculatorDbContext))]
    [Migration("20250313190844_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("microsoft_hackathon_roi_calculator.Core.Models.ProjectROI", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("NumberOfEmployees")
                        .HasColumnType("int");

                    b.Property<double>("ProjectBudget")
                        .HasColumnType("float");

                    b.Property<int>("ProjectDurationMonths")
                        .HasColumnType("int");

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("ROI")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("ProjectROIs");
                });
#pragma warning restore 612, 618
        }
    }
}
