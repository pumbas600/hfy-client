﻿// <auto-generated />
using System;
using HfyClientApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HfyClientApi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HfyClientApi.Models.Chapter", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Downvotes")
                        .HasColumnType("integer");

                    b.Property<DateTime>("EditedAtUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FirstChapterId")
                        .HasColumnType("text");

                    b.Property<bool>("IsNsfw")
                        .HasColumnType("boolean");

                    b.Property<string>("NextChapterId")
                        .HasColumnType("text");

                    b.Property<string>("PreviousChapterId")
                        .HasColumnType("text");

                    b.Property<DateTime>("ProcessedAtUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Subreddit")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TextHtml")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Upvotes")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CreatedAtUtc");

                    b.HasIndex("FirstChapterId");

                    b.ToTable("Chapters");
                });
#pragma warning restore 612, 618
        }
    }
}
