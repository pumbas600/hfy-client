﻿// <auto-generated />
using System;
using HfyClientApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HfyClientApi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240912113854_Subreddit-Model")]
    partial class SubredditModel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "fuzzystrmatch");
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

                    b.Property<string>("SearchableTitle")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Subreddit")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("SyncedAtUtc")
                        .HasColumnType("timestamp with time zone");

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

                    b.HasIndex("Subreddit");

                    b.ToTable("Chapters");
                });

            modelBuilder.Entity("HfyClientApi.Models.StoryMetadata", b =>
                {
                    b.Property<string>("FirstChapterId")
                        .HasColumnType("text");

                    b.Property<string>("CoverArtUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("FirstChapterId");

                    b.ToTable("StoryMetadata");
                });

            modelBuilder.Entity("HfyClientApi.Models.Subreddit", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("IconUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Name");

                    b.ToTable("Subreddits");
                });
#pragma warning restore 612, 618
        }
    }
}