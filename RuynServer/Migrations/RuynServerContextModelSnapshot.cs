﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RuynServer.Data;

#nullable disable

namespace RuynServer.Migrations
{
    [DbContext(typeof(RuynServerContext))]
    partial class RuynServerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("RuynServer.Models.LevelData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TINYTEXT");

                    b.Property<int>("DownloadCount")
                        .HasColumnType("INT");

                    b.Property<byte[]>("FileData")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<string>("FileDataHash")
                        .HasColumnType("TEXT");

                    b.Property<int>("LevelCount")
                        .HasColumnType("TINYINT");

                    b.Property<string>("LevelPackName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TINYTEXT");

                    b.Property<DateTime>("UploadDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FileDataHash")
                        .IsUnique();

                    b.HasIndex("LevelPackName")
                        .IsUnique();

                    b.ToTable("LevelData");
                });
#pragma warning restore 612, 618
        }
    }
}
