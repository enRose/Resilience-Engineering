using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class RemovedUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Content_Users_PersonalLoanId",
                table: "Content");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "PersonalLoan");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "PersonalLoan",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonalLoan",
                table: "PersonalLoan",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Content_PersonalLoan_PersonalLoanId",
                table: "Content",
                column: "PersonalLoanId",
                principalTable: "PersonalLoan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Content_PersonalLoan_PersonalLoanId",
                table: "Content");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonalLoan",
                table: "PersonalLoan");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PersonalLoan");

            migrationBuilder.RenameTable(
                name: "PersonalLoan",
                newName: "Users");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Content_Users_PersonalLoanId",
                table: "Content",
                column: "PersonalLoanId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
