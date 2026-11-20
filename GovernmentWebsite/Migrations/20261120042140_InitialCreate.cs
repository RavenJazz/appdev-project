using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GovernmentWebsite.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FormSubmissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReferenceCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    FormType = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    ApplicantFullName = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    ContactNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    SubmittedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    FieldsJson = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormSubmissions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormSubmissions_Email",
                table: "FormSubmissions",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_FormSubmissions_FormType",
                table: "FormSubmissions",
                column: "FormType");

            migrationBuilder.CreateIndex(
                name: "IX_FormSubmissions_ReferenceCode",
                table: "FormSubmissions",
                column: "ReferenceCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormSubmissions_SubmittedAt",
                table: "FormSubmissions",
                column: "SubmittedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormSubmissions");
        }
    }
}
