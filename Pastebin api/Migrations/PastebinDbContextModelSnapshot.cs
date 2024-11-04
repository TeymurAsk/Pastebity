﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Pastebin_api.Data;

#nullable disable

namespace Pastebin_api.Migrations
{
    [DbContext(typeof(PastebinDbContext))]
    partial class PastebinDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Pastebin_api.Data.TextBlock", b =>
                {
                    b.Property<string>("Link")
                        .HasMaxLength(80)
                        .HasColumnType("character varying(80)");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Link");

                    b.ToTable("TextBlocks");
                });
#pragma warning restore 612, 618
        }
    }
}
