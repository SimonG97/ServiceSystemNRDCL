﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ServiceSystemNRDCL.Data;

namespace ServiceSystemNRDCL.Migrations
{
    [DbContext(typeof(CustomerContext))]
    [Migration("20210224053333_CutomerDB")]
    partial class CutomerDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("ServiceSystemNRDCL.Data.Customers", b =>
                {
                    b.Property<long>("CustomerCID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasMaxLength(11)
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("Phone")
                        .HasColumnType("bigint")
                        .HasMaxLength(8);

                    b.HasKey("CustomerCID");

                    b.ToTable("Customers");
                });
#pragma warning restore 612, 618
        }
    }
}
