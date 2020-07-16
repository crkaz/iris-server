﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using iris_server.Models;

namespace iris_server.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20200715235751_m0")]
    partial class m0
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("iris_server.Models.ActivityLog", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Caption");

                    b.Property<DateTime>("DateTime");

                    b.Property<string>("JsonDescription");

                    b.Property<string>("PatientId");

                    b.HasKey("Id");

                    b.HasIndex("PatientId");

                    b.ToTable("ActivityLogs");
                });

            modelBuilder.Entity("iris_server.Models.CalendarEntry", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CarerId");

                    b.Property<string>("Description");

                    b.Property<DateTime>("End");

                    b.Property<string>("JsonReminders");

                    b.Property<string>("PatientId");

                    b.Property<int>("Repeat");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<DateTime>("Start");

                    b.HasKey("Id");

                    b.HasIndex("CarerId");

                    b.HasIndex("PatientId");

                    b.ToTable("Calendars");
                });

            modelBuilder.Entity("iris_server.Models.Carer", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("UserApiKey");

                    b.HasKey("Id");

                    b.HasIndex("UserApiKey");

                    b.ToTable("Carers");
                });

            modelBuilder.Entity("iris_server.Models.DbLog", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("UserApiKey");

                    b.Property<string>("What");

                    b.Property<DateTime>("When");

                    b.HasKey("Id");

                    b.HasIndex("UserApiKey");

                    b.ToTable("DbLogs");
                });

            modelBuilder.Entity("iris_server.Models.Patient", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CarerId");

                    b.Property<string>("JsonConfig");

                    b.Property<string>("JsonPatientInfo");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("UserApiKey");

                    b.HasKey("Id");

                    b.HasIndex("CarerId");

                    b.HasIndex("UserApiKey");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("iris_server.Models.PatientMessage", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CarerId");

                    b.Property<string>("Message");

                    b.Property<string>("PatientId");

                    b.Property<DateTime?>("Read");

                    b.Property<DateTime>("Sent");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("CarerId");

                    b.HasIndex("PatientId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("iris_server.Models.StickyNote", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<string>("PatientId");

                    b.Property<int>("Scale");

                    b.HasKey("Id");

                    b.HasIndex("PatientId");

                    b.ToTable("Stickies");
                });

            modelBuilder.Entity("iris_server.Models.User", b =>
                {
                    b.Property<string>("ApiKey")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Role");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("ApiKey");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("iris_server.Models.ActivityLog", b =>
                {
                    b.HasOne("iris_server.Models.Patient")
                        .WithMany("ActivityLogs")
                        .HasForeignKey("PatientId");
                });

            modelBuilder.Entity("iris_server.Models.CalendarEntry", b =>
                {
                    b.HasOne("iris_server.Models.Carer")
                        .WithMany("CreatedAppointments")
                        .HasForeignKey("CarerId");

                    b.HasOne("iris_server.Models.Patient")
                        .WithMany("CalendarEntries")
                        .HasForeignKey("PatientId");
                });

            modelBuilder.Entity("iris_server.Models.Carer", b =>
                {
                    b.HasOne("iris_server.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserApiKey");
                });

            modelBuilder.Entity("iris_server.Models.DbLog", b =>
                {
                    b.HasOne("iris_server.Models.User")
                        .WithMany("DbLogs")
                        .HasForeignKey("UserApiKey");
                });

            modelBuilder.Entity("iris_server.Models.Patient", b =>
                {
                    b.HasOne("iris_server.Models.Carer")
                        .WithMany("AssignedPatients")
                        .HasForeignKey("CarerId");

                    b.HasOne("iris_server.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserApiKey");
                });

            modelBuilder.Entity("iris_server.Models.PatientMessage", b =>
                {
                    b.HasOne("iris_server.Models.Carer")
                        .WithMany("SentMessages")
                        .HasForeignKey("CarerId");

                    b.HasOne("iris_server.Models.Patient")
                        .WithMany("Messages")
                        .HasForeignKey("PatientId");
                });

            modelBuilder.Entity("iris_server.Models.StickyNote", b =>
                {
                    b.HasOne("iris_server.Models.Patient")
                        .WithMany("Stickies")
                        .HasForeignKey("PatientId");
                });
#pragma warning restore 612, 618
        }
    }
}
