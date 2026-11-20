using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GovernmentWebsite.Migrations
{
    /// <inheritdoc />
    public partial class AddReviewAndAuditFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReviewNotes",
                table: "FormSubmissions",
                type: "TEXT",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "FormSubmissions",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReviewNotes",
                table: "FormSubmissions");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "FormSubmissions");
        }
    }
}
