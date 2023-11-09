﻿// <auto-generated />
using System;
using GameQueue.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GameQueue.Host.Migrations
{
    [DbContext(typeof(GameQueueContext))]
    [Migration("20231109114043_User-Role")]
    partial class UserRole
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GameQueue.Core.Entities.Map", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CoverImageUrl")
                        .HasColumnType("text")
                        .HasColumnName("cover_image_url");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<int>("Height")
                        .HasColumnType("integer")
                        .HasColumnName("height");

                    b.Property<int>("MaxPlayersCount")
                        .HasColumnType("integer")
                        .HasColumnName("max_players_count");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<string>("Status")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("Pending")
                        .HasColumnName("status");

                    b.Property<int>("Width")
                        .HasColumnType("integer")
                        .HasColumnName("width");

                    b.HasKey("Id")
                        .HasName("pk_maps");

                    b.ToTable("maps", (string)null);
                });

            modelBuilder.Entity("GameQueue.Core.Entities.RequestToMap", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("MapId")
                        .HasColumnType("integer")
                        .HasColumnName("map_id");

                    b.Property<int>("SearchMapsRequestId")
                        .HasColumnType("integer")
                        .HasColumnName("search_maps_request_id");

                    b.HasKey("Id")
                        .HasName("pk_request_to_maps");

                    b.HasAlternateKey("SearchMapsRequestId", "MapId")
                        .HasName("ak_request_to_maps_search_maps_request_id_map_id");

                    b.HasIndex("MapId")
                        .HasDatabaseName("ix_request_to_maps_map_id");

                    b.ToTable("request_to_maps", (string)null);
                });

            modelBuilder.Entity("GameQueue.Core.Entities.SearchMapsRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset?>("ComposeDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("compose_date");

                    b.Property<DateTimeOffset>("CreationDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("creation_date");

                    b.Property<int>("CreatorUserId")
                        .HasColumnType("integer")
                        .HasColumnName("creator_user_id");

                    b.Property<DateTimeOffset?>("DoneDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("done_date");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.HasKey("Id")
                        .HasName("pk_search_maps_requests");

                    b.HasIndex("CreatorUserId")
                        .HasDatabaseName("ix_search_maps_requests_creator_user_id");

                    b.ToTable("search_maps_requests", (string)null);
                });

            modelBuilder.Entity("GameQueue.Core.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("hashed_password");

                    b.Property<int>("Level")
                        .HasColumnType("integer")
                        .HasColumnName("level");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<int>("Role")
                        .HasColumnType("integer")
                        .HasColumnName("role");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("GameQueue.Core.Entities.RequestToMap", b =>
                {
                    b.HasOne("GameQueue.Core.Entities.Map", "Map")
                        .WithMany("RequestsToMap")
                        .HasForeignKey("MapId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_request_to_maps_maps_map_id");

                    b.HasOne("GameQueue.Core.Entities.SearchMapsRequest", "SearchMapsRequest")
                        .WithMany("RequestsToMap")
                        .HasForeignKey("SearchMapsRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_request_to_maps_search_maps_requests_search_maps_request_id");

                    b.Navigation("Map");

                    b.Navigation("SearchMapsRequest");
                });

            modelBuilder.Entity("GameQueue.Core.Entities.SearchMapsRequest", b =>
                {
                    b.HasOne("GameQueue.Core.Entities.User", "CreatorUser")
                        .WithMany("SearchMapsRequests")
                        .HasForeignKey("CreatorUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_search_maps_requests_users_creator_user_id");

                    b.Navigation("CreatorUser");
                });

            modelBuilder.Entity("GameQueue.Core.Entities.Map", b =>
                {
                    b.Navigation("RequestsToMap");
                });

            modelBuilder.Entity("GameQueue.Core.Entities.SearchMapsRequest", b =>
                {
                    b.Navigation("RequestsToMap");
                });

            modelBuilder.Entity("GameQueue.Core.Entities.User", b =>
                {
                    b.Navigation("SearchMapsRequests");
                });
#pragma warning restore 612, 618
        }
    }
}
