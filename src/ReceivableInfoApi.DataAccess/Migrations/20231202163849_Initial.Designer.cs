﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ReceivableInfoApi.DataAccess;

#nullable disable

namespace ReceivableInfoApi.DataAccess.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20231202163849_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.22")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ReceivableInfoApi.Common.Model.Receivable", b =>
                {
                    b.Property<string>("Reference")
                        .HasColumnType("text");

                    b.Property<bool?>("Cancelled")
                        .HasColumnType("boolean");

                    b.Property<string>("ClosedDate")
                        .HasColumnType("text");

                    b.Property<string>("CurrencyCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DebtorAddress1")
                        .HasColumnType("text");

                    b.Property<string>("DebtorAddress2")
                        .HasColumnType("text");

                    b.Property<string>("DebtorCountryCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DebtorName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DebtorReference")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DebtorRegistrationNumber")
                        .HasColumnType("text");

                    b.Property<string>("DebtorState")
                        .HasColumnType("text");

                    b.Property<string>("DebtorTown")
                        .HasColumnType("text");

                    b.Property<string>("DebtorZip")
                        .HasColumnType("text");

                    b.Property<string>("DueDate")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("IssueDate")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("OpeningValue")
                        .HasColumnType("numeric");

                    b.Property<decimal>("PaidValue")
                        .HasColumnType("numeric");

                    b.HasKey("Reference");

                    b.ToTable("Receivables");
                });
#pragma warning restore 612, 618
        }
    }
}
