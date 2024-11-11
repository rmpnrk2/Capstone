﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SouthSideK9Camp.Server.Data;

#nullable disable

namespace SouthSideK9Camp.Server.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20241111023949_AddidDeleteBool")]
    partial class AddidDeleteBool
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SouthSideK9Camp.Shared.Client", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("datetime2");

                    b.Property<string>("Contact")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("GUID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("MiddleInitial")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("Sex")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("ID");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("SouthSideK9Camp.Shared.Contract", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("AnythingElseToShare")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<bool>("DiscomfortOverEars")
                        .HasColumnType("bit");

                    b.Property<bool>("DiscomfortOverHead")
                        .HasColumnType("bit");

                    b.Property<bool>("DiscomfortOverMuzzle")
                        .HasColumnType("bit");

                    b.Property<string>("DiscomfortOverOther")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("DiscomfortOverPaws")
                        .HasColumnType("bit");

                    b.Property<bool>("DiscomfortOverRump")
                        .HasColumnType("bit");

                    b.Property<bool>("DiscomfortOverTails")
                        .HasColumnType("bit");

                    b.Property<int>("DogID")
                        .HasColumnType("int");

                    b.Property<bool>("FearOrAggressionToDogs")
                        .HasColumnType("bit");

                    b.Property<bool>("FearOrAggressionToHuman")
                        .HasColumnType("bit");

                    b.Property<string>("FeedingRoutine")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("GUID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("PaymendConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("ProtectiveOverFood")
                        .HasColumnType("bit");

                    b.Property<string>("ProtectiveOverOther")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ProtectiveOverPerson")
                        .HasColumnType("bit");

                    b.Property<bool>("ProtectiveOverSpot")
                        .HasColumnType("bit");

                    b.Property<bool>("ProtectiveOverToys")
                        .HasColumnType("bit");

                    b.Property<bool>("ProtectiveOverTreats")
                        .HasColumnType("bit");

                    b.Property<string>("ReservationPaymentURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Restrictions")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SleepingRoutine")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrainingGoal")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrainingType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("DogID")
                        .IsUnique();

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("SouthSideK9Camp.Shared.Customer", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("ClientID")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("EmergencyContactEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmergencyContactName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmergencyContactNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmergencyVet")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmergencyVetNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("GUID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("WhereWillYouBeStating")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("ClientID")
                        .IsUnique();

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("SouthSideK9Camp.Shared.Dog", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("AvatarURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("datetime2");

                    b.Property<string>("Breed")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ClientID")
                        .HasColumnType("int");

                    b.Property<string>("Clinic")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Distemper")
                        .HasColumnType("bit");

                    b.Property<Guid>("GUID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("HepatitisAdenovirus")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Parainfluenza")
                        .HasColumnType("bit");

                    b.Property<bool>("Parvovirus")
                        .HasColumnType("bit");

                    b.Property<bool>("Rabies")
                        .HasColumnType("bit");

                    b.Property<int>("ReservationID")
                        .HasColumnType("int");

                    b.Property<bool>("ReservationPaymentConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("ReservationPaymentURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sex")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VaccineCardURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("ClientID");

                    b.HasIndex("ReservationID");

                    b.ToTable("Dogs");
                });

            modelBuilder.Entity("SouthSideK9Camp.Shared.Invoice", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("AccountAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AccountName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Balance")
                        .HasColumnType("float");

                    b.Property<string>("CompanyAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyPhone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyZIPCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateDue")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateSent")
                        .HasColumnType("datetime2");

                    b.Property<int>("DogID")
                        .HasColumnType("int");

                    b.Property<Guid>("GUID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsEmailed")
                        .HasColumnType("bit");

                    b.Property<bool>("PaymentConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("ProofOfPaymentURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isDefault")
                        .HasColumnType("bit");

                    b.HasKey("ID");

                    b.HasIndex("DogID");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("SouthSideK9Camp.Shared.Item", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Credit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("GUID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("InvoiceID")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<double>("Quantity")
                        .HasColumnType("float");

                    b.Property<double>("Total")
                        .HasColumnType("float");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isModel")
                        .HasColumnType("bit");

                    b.HasKey("ID");

                    b.HasIndex("InvoiceID");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("SouthSideK9Camp.Shared.Log", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("GUID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Severity")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("SouthSideK9Camp.Shared.Member", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("ClientID")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("DogClinicAddress")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<Guid>("GUID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Occupation")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PurposeOfJoining")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("RegistrationConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("RegistrationPaymentURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WhereDidYouHereAboutUs")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("WhoTrainYourDog")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID");

                    b.HasIndex("ClientID")
                        .IsUnique();

                    b.ToTable("Members");
                });

            modelBuilder.Entity("SouthSideK9Camp.Shared.MembershipDue", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateTimeDue")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("GUID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("MemberID")
                        .HasColumnType("int");

                    b.Property<bool>("PaymentConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("ProofOfPaymentURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("MemberID");

                    b.ToTable("MembershipDues");
                });

            modelBuilder.Entity("SouthSideK9Camp.Shared.ProgressReport", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateTraining")
                        .HasColumnType("datetime2");

                    b.Property<int>("DogID")
                        .HasColumnType("int");

                    b.Property<Guid>("GUID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ImageURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ScoreFocus")
                        .HasColumnType("int");

                    b.Property<int>("ScoreObedience")
                        .HasColumnType("int");

                    b.Property<int>("ScoreProtection")
                        .HasColumnType("int");

                    b.Property<TimeSpan?>("SpanDuration")
                        .HasColumnType("time");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("DogID");

                    b.ToTable("ProgressReports");
                });

            modelBuilder.Entity("SouthSideK9Camp.Shared.ReasonForRejection", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("GUID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Reasons");
                });

            modelBuilder.Entity("SouthSideK9Camp.Shared.Reservation", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("EndingDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("GUID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Slots")
                        .HasColumnType("int");

                    b.Property<DateTime?>("StartingDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("SouthSideK9Camp.Shared.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Contact")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("GUID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SouthSideK9Camp.Shared.Contract", b =>
                {
                    b.HasOne("SouthSideK9Camp.Shared.Dog", "Dog")
                        .WithOne("Contract")
                        .HasForeignKey("SouthSideK9Camp.Shared.Contract", "DogID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dog");
                });

            modelBuilder.Entity("SouthSideK9Camp.Shared.Customer", b =>
                {
                    b.HasOne("SouthSideK9Camp.Shared.Client", "Client")
                        .WithOne("Customer")
                        .HasForeignKey("SouthSideK9Camp.Shared.Customer", "ClientID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("SouthSideK9Camp.Shared.Dog", b =>
                {
                    b.HasOne("SouthSideK9Camp.Shared.Client", "Client")
                        .WithMany("Dogs")
                        .HasForeignKey("ClientID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SouthSideK9Camp.Shared.Reservation", "Reservation")
                        .WithMany("Dogs")
                        .HasForeignKey("ReservationID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Reservation");
                });

            modelBuilder.Entity("SouthSideK9Camp.Shared.Invoice", b =>
                {
                    b.HasOne("SouthSideK9Camp.Shared.Dog", "Dog")
                        .WithMany("Invoices")
                        .HasForeignKey("DogID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dog");
                });

            modelBuilder.Entity("SouthSideK9Camp.Shared.Item", b =>
                {
                    b.HasOne("SouthSideK9Camp.Shared.Invoice", "Invoice")
                        .WithMany("Items")
                        .HasForeignKey("InvoiceID");

                    b.Navigation("Invoice");
                });

            modelBuilder.Entity("SouthSideK9Camp.Shared.Member", b =>
                {
                    b.HasOne("SouthSideK9Camp.Shared.Client", "Client")
                        .WithOne("Member")
                        .HasForeignKey("SouthSideK9Camp.Shared.Member", "ClientID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("SouthSideK9Camp.Shared.MembershipDue", b =>
                {
                    b.HasOne("SouthSideK9Camp.Shared.Member", "Member")
                        .WithMany("MembershipDues")
                        .HasForeignKey("MemberID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Member");
                });

            modelBuilder.Entity("SouthSideK9Camp.Shared.ProgressReport", b =>
                {
                    b.HasOne("SouthSideK9Camp.Shared.Dog", "Dog")
                        .WithMany("ProgressReports")
                        .HasForeignKey("DogID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dog");
                });

            modelBuilder.Entity("SouthSideK9Camp.Shared.Client", b =>
                {
                    b.Navigation("Customer");

                    b.Navigation("Dogs");

                    b.Navigation("Member");
                });

            modelBuilder.Entity("SouthSideK9Camp.Shared.Dog", b =>
                {
                    b.Navigation("Contract")
                        .IsRequired();

                    b.Navigation("Invoices");

                    b.Navigation("ProgressReports");
                });

            modelBuilder.Entity("SouthSideK9Camp.Shared.Invoice", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("SouthSideK9Camp.Shared.Member", b =>
                {
                    b.Navigation("MembershipDues");
                });

            modelBuilder.Entity("SouthSideK9Camp.Shared.Reservation", b =>
                {
                    b.Navigation("Dogs");
                });
#pragma warning restore 612, 618
        }
    }
}
