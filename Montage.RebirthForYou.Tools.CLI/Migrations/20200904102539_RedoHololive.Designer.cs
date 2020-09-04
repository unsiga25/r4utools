﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Montage.RebirthForYou.Tools.CLI.Entities;

namespace Montage.RebirthForYou.Tools.CLI.Migrations
{
    [DbContext(typeof(CardDatabaseContext))]
    [Migration("20200904102539_RedoHololive")]
    partial class RedoHololive
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3");

            modelBuilder.Entity("Montage.RebirthForYou.Tools.CLI.Entities.ActivityLog", b =>
                {
                    b.Property<int>("LogID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Activity")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDone")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Target")
                        .HasColumnType("TEXT");

                    b.HasKey("LogID");

                    b.ToTable("MigrationLog");

                    b.HasData(
                        new
                        {
                            LogID = 1,
                            Activity = 0,
                            DateAdded = new DateTime(2020, 9, 2, 0, 0, 23, 534, DateTimeKind.Local).AddTicks(9446),
                            IsDone = false,
                            Target = "https://raw.githubusercontent.com/ronelm2000/r4utools/master/Montage.RebirthForYou.Tools.CLI/Sets/gochiusa_bp.r4uset"
                        },
                        new
                        {
                            LogID = 2,
                            Activity = 0,
                            DateAdded = new DateTime(2020, 9, 2, 0, 0, 23, 534, DateTimeKind.Local).AddTicks(9446),
                            IsDone = false,
                            Target = "https://rebirth-for-you.fandom.com/wiki/Trial_Start_Deck_Is_the_Order_a_Rabbit%3F_BLOOM"
                        },
                        new
                        {
                            LogID = 3,
                            Activity = 0,
                            DateAdded = new DateTime(2020, 8, 23, 0, 43, 53, 205, DateTimeKind.Local),
                            IsDone = true,
                            Target = "https://rebirth-for-you.fandom.com/wiki/Trial_Deck_Hololive_Production_(ver._0th_Gen)"
                        },
                        new
                        {
                            LogID = 4,
                            Activity = 0,
                            DateAdded = new DateTime(2020, 8, 23, 0, 43, 53, 206, DateTimeKind.Local),
                            IsDone = true,
                            Target = "https://rebirth-for-you.fandom.com/wiki/Trial_Deck_Hololive_Production_(ver._1st_Gen)"
                        },
                        new
                        {
                            LogID = 5,
                            Activity = 0,
                            DateAdded = new DateTime(2020, 9, 4, 0, 43, 53, 205, DateTimeKind.Local),
                            IsDone = false,
                            Target = "https://rebirth-for-you.fandom.com/wiki/Trial_Deck_Hololive_Production_(ver._0th_Gen)"
                        },
                        new
                        {
                            LogID = 6,
                            Activity = 0,
                            DateAdded = new DateTime(2020, 9, 4, 0, 43, 53, 206, DateTimeKind.Local),
                            IsDone = false,
                            Target = "https://rebirth-for-you.fandom.com/wiki/Trial_Deck_Hololive_Production_(ver._1st_Gen)"
                        });
                });

            modelBuilder.Entity("Montage.RebirthForYou.Tools.CLI.Entities.R4UCard", b =>
                {
                    b.Property<string>("Serial")
                        .HasColumnType("TEXT");

                    b.Property<int?>("ATK")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Color")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Cost")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DEF")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Effect")
                        .HasColumnType("TEXT");

                    b.Property<string>("Flavor")
                        .HasColumnType("TEXT");

                    b.Property<string>("Images")
                        .HasColumnType("TEXT");

                    b.Property<int>("Language")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NonFoilSerial")
                        .HasColumnType("TEXT");

                    b.Property<string>("Rarity")
                        .HasColumnType("TEXT");

                    b.Property<string>("Remarks")
                        .HasColumnType("TEXT");

                    b.Property<int?>("SetReleaseID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Serial")
                        .HasName("Serial");

                    b.HasIndex("NonFoilSerial");

                    b.HasIndex("SetReleaseID");

                    b.ToTable("R4UCards");
                });

            modelBuilder.Entity("R4UReleaseSet", b =>
                {
                    b.Property<int>("ReleaseID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("ReleaseCode")
                        .HasColumnType("TEXT");

                    b.HasKey("ReleaseID");

                    b.ToTable("R4UReleaseSets");
                });

            modelBuilder.Entity("Montage.RebirthForYou.Tools.CLI.Entities.R4UCard", b =>
                {
                    b.HasOne("Montage.RebirthForYou.Tools.CLI.Entities.R4UCard", "NonFoil")
                        .WithMany("Alternates")
                        .HasForeignKey("NonFoilSerial");

                    b.HasOne("R4UReleaseSet", "Set")
                        .WithMany("Cards")
                        .HasForeignKey("SetReleaseID");

                    b.OwnsOne("Montage.RebirthForYou.Tools.CLI.Entities.MultiLanguageString", "Name", b1 =>
                        {
                            b1.Property<string>("R4UCardSerial")
                                .HasColumnType("TEXT");

                            b1.Property<string>("EN")
                                .HasColumnType("TEXT");

                            b1.Property<string>("JP")
                                .HasColumnType("TEXT");

                            b1.HasKey("R4UCardSerial");

                            b1.ToTable("R4UCards");

                            b1.WithOwner()
                                .HasForeignKey("R4UCardSerial");
                        });

                    b.OwnsMany("Montage.RebirthForYou.Tools.CLI.Entities.MultiLanguageString", "Traits", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("INTEGER")
                                .HasAnnotation("Sqlite:Autoincrement", true);

                            b1.Property<string>("EN")
                                .HasColumnType("TEXT");

                            b1.Property<string>("JP")
                                .HasColumnType("TEXT");

                            b1.Property<string>("R4UCardSerial")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.HasKey("Id");

                            b1.HasIndex("R4UCardSerial");

                            b1.ToTable("R4UCards_Traits");

                            b1.WithOwner()
                                .HasForeignKey("R4UCardSerial");
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
