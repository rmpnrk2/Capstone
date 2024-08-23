using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SouthSideK9Camp.Server.Migrations
{
    /// <inheritdoc />
    public partial class InittialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MiddleInitial = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    Sex = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StartingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Slots = table.Column<int>(type: "int", nullable: false),
                    GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WhereWillYouBeStating = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmergencyVet = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmergencyVetNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmergencyContactName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmergencyContactNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmergencyContactEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ClientID = table.Column<int>(type: "int", nullable: false),
                    GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Customers_Clients_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Clients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Occupation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WhereDidYouHereAboutUs = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PuposeOfJoining = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DogClinicAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    WhoTrainYourDog = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RegistrationPaymentURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    ClientID = table.Column<int>(type: "int", nullable: false),
                    GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Members_Clients_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Clients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dogs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Breed = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Sex = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AvatarURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VaccineCardURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Clinic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Rabies = table.Column<bool>(type: "bit", nullable: false),
                    Distemper = table.Column<bool>(type: "bit", nullable: false),
                    HepatitisAdenovirus = table.Column<bool>(type: "bit", nullable: false),
                    Parvovirus = table.Column<bool>(type: "bit", nullable: false),
                    Parainfluenza = table.Column<bool>(type: "bit", nullable: false),
                    ReservationPaymentURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReservationPaymentConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    ClientID = table.Column<int>(type: "int", nullable: false),
                    ReservationID = table.Column<int>(type: "int", nullable: false),
                    GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dogs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Dogs_Clients_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Clients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dogs_Reservations_ReservationID",
                        column: x => x.ReservationID,
                        principalTable: "Reservations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MembershipDues",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProofOfPaymentURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    MemberID = table.Column<int>(type: "int", nullable: false),
                    GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipDues", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MembershipDues_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainingType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TrainingGoal = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Restrictions = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FeedingRoutine = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SleepingRoutine = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ProtectiveOverToys = table.Column<bool>(type: "bit", nullable: false),
                    ProtectiveOverFood = table.Column<bool>(type: "bit", nullable: false),
                    ProtectiveOverTreats = table.Column<bool>(type: "bit", nullable: false),
                    ProtectiveOverSpot = table.Column<bool>(type: "bit", nullable: false),
                    ProtectiveOverPerson = table.Column<bool>(type: "bit", nullable: false),
                    ProtectiveOverOther = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DiscomfortOverPaws = table.Column<bool>(type: "bit", nullable: false),
                    DiscomfortOverTails = table.Column<bool>(type: "bit", nullable: false),
                    DiscomfortOverEars = table.Column<bool>(type: "bit", nullable: false),
                    DiscomfortOverMuzzle = table.Column<bool>(type: "bit", nullable: false),
                    DiscomfortOverHead = table.Column<bool>(type: "bit", nullable: false),
                    DiscomfortOverRump = table.Column<bool>(type: "bit", nullable: false),
                    DiscomfortOverOther = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FearOrAggressionToHuman = table.Column<bool>(type: "bit", nullable: false),
                    FearOrAggressionToDogs = table.Column<bool>(type: "bit", nullable: false),
                    AnythingElseToShare = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ReservationPaymentURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymendConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    DogID = table.Column<int>(type: "int", nullable: false),
                    GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Contracts_Dogs_DogID",
                        column: x => x.DogID,
                        principalTable: "Dogs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CompanyAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CompanyZIPCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CompanyPhone = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccountAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateSent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateDue = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Balance = table.Column<double>(type: "float", nullable: false),
                    IsEmailed = table.Column<bool>(type: "bit", nullable: false),
                    ProofOfPaymentURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    DogID = table.Column<int>(type: "int", nullable: false),
                    GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Invoices_Dogs_DogID",
                        column: x => x.DogID,
                        principalTable: "Dogs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProgressReports",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTraining = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SpanDuration = table.Column<TimeSpan>(type: "time", nullable: false),
                    ScoreFocus = table.Column<int>(type: "int", nullable: false),
                    ScoreObedience = table.Column<int>(type: "int", nullable: false),
                    ScoreProtection = table.Column<int>(type: "int", nullable: false),
                    DogID = table.Column<int>(type: "int", nullable: false),
                    GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgressReports", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProgressReports_Dogs_DogID",
                        column: x => x.DogID,
                        principalTable: "Dogs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MeasurementNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MeasurementUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ammount = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Credit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false),
                    InvoiceID = table.Column<int>(type: "int", nullable: false),
                    GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Items_Invoices_InvoiceID",
                        column: x => x.InvoiceID,
                        principalTable: "Invoices",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_DogID",
                table: "Contracts",
                column: "DogID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ClientID",
                table: "Customers",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_Dogs_ClientID",
                table: "Dogs",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_Dogs_ReservationID",
                table: "Dogs",
                column: "ReservationID");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_DogID",
                table: "Invoices",
                column: "DogID");

            migrationBuilder.CreateIndex(
                name: "IX_Items_InvoiceID",
                table: "Items",
                column: "InvoiceID");

            migrationBuilder.CreateIndex(
                name: "IX_Members_ClientID",
                table: "Members",
                column: "ClientID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MembershipDues_MemberID",
                table: "MembershipDues",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_ProgressReports_DogID",
                table: "ProgressReports",
                column: "DogID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "MembershipDues");

            migrationBuilder.DropTable(
                name: "ProgressReports");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Dogs");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Reservations");
        }
    }
}
