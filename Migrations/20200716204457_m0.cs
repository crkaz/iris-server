using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace iris_server.Migrations
{
    public partial class m0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ApiKey = table.Column<string>(nullable: false),
                    Role = table.Column<string>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ApiKey);
                });

            migrationBuilder.CreateTable(
                name: "Carers",
                columns: table => new
                {
                    Email = table.Column<string>(nullable: false),
                    UserApiKey = table.Column<string>(nullable: true),
                    AssignedPatientIds = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carers", x => x.Email);
                    table.ForeignKey(
                        name: "FK_Carers_Users_UserApiKey",
                        column: x => x.UserApiKey,
                        principalTable: "Users",
                        principalColumn: "ApiKey",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DbLogs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    What = table.Column<string>(nullable: true),
                    When = table.Column<DateTime>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    UserApiKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DbLogs_Users_UserApiKey",
                        column: x => x.UserApiKey,
                        principalTable: "Users",
                        principalColumn: "ApiKey",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserApiKey = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    JsonPatientInfo = table.Column<string>(nullable: true),
                    JsonConfig = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patients_Users_UserApiKey",
                        column: x => x.UserApiKey,
                        principalTable: "Users",
                        principalColumn: "ApiKey",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActivityLogs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Caption = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    JsonDescription = table.Column<string>(nullable: true),
                    PatientId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityLogs_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Calendars",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CarerId = table.Column<string>(nullable: true),
                    Start = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: false),
                    Repeat = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    JsonReminders = table.Column<string>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    PatientId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Calendars_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CarerId = table.Column<string>(nullable: true),
                    Sent = table.Column<DateTime>(nullable: false),
                    Read = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    PatientId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Stickies",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    Scale = table.Column<int>(nullable: false),
                    PatientId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stickies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stickies_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_PatientId",
                table: "ActivityLogs",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Calendars_PatientId",
                table: "Calendars",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Carers_UserApiKey",
                table: "Carers",
                column: "UserApiKey");

            migrationBuilder.CreateIndex(
                name: "IX_DbLogs_UserApiKey",
                table: "DbLogs",
                column: "UserApiKey");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_PatientId",
                table: "Messages",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_UserApiKey",
                table: "Patients",
                column: "UserApiKey");

            migrationBuilder.CreateIndex(
                name: "IX_Stickies_PatientId",
                table: "Stickies",
                column: "PatientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityLogs");

            migrationBuilder.DropTable(
                name: "Calendars");

            migrationBuilder.DropTable(
                name: "Carers");

            migrationBuilder.DropTable(
                name: "DbLogs");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Stickies");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
