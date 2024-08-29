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

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("EditedAtUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsNsfw")
                        .HasColumnType("boolean");

                    b.Property<string>("NextChapterId")
                        .HasColumnType("text");

                    b.Property<string>("PreviousChapterId")
                        .HasColumnType("text");

                    b.Property<DateTime>("ProcessedAtUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("StoryId")
                        .HasColumnType("integer");

                    b.Property<string>("TextHtml")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("StoryId");

                    b.ToTable("Chapters");
                });

            modelBuilder.Entity("HfyClientApi.Models.Story", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstChapterId")
                        .HasColumnType("text");

                    b.Property<string>("Subreddit")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("FirstChapterId");

                    b.ToTable("Stories");
                });

            modelBuilder.Entity("HfyClientApi.Models.Chapter", b =>
                {
                    b.HasOne("HfyClientApi.Models.Story", "Story")
                        .WithMany("Chapters")
                        .HasForeignKey("StoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Story");
                });

            modelBuilder.Entity("HfyClientApi.Models.Story", b =>
                {
                    b.HasOne("HfyClientApi.Models.Chapter", "FirstChapter")
                        .WithMany()
                        .HasForeignKey("FirstChapterId");

                    b.Navigation("FirstChapter");
                });

            modelBuilder.Entity("HfyClientApi.Models.Story", b =>
                {
                    b.Navigation("Chapters");
                });
#pragma warning restore 612, 618
        }
    }
}
