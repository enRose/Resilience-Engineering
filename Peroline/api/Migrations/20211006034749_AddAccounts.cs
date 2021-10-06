using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class AddAccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Content");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonalLoan",
                table: "PersonalLoan");

            migrationBuilder.RenameTable(
                name: "PersonalLoan",
                newName: "PersonalLoans");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "PersonalLoans",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "AppId",
                table: "PersonalLoans",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonalLoans",
                table: "PersonalLoans",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    PersonalLoanId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Account_PersonalLoans_PersonalLoanId",
                        column: x => x.PersonalLoanId,
                        principalTable: "PersonalLoans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "App",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Data = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonalLoans_AppId",
                table: "PersonalLoans",
                column: "AppId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_PersonalLoanId",
                table: "Account",
                column: "PersonalLoanId");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalLoans_App_AppId",
                table: "PersonalLoans",
                column: "AppId",
                principalTable: "App",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonalLoans_App_AppId",
                table: "PersonalLoans");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "App");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonalLoans",
                table: "PersonalLoans");

            migrationBuilder.DropIndex(
                name: "IX_PersonalLoans_AppId",
                table: "PersonalLoans");

            migrationBuilder.DropColumn(
                name: "AppId",
                table: "PersonalLoans");

            migrationBuilder.RenameTable(
                name: "PersonalLoans",
                newName: "PersonalLoan");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "PersonalLoan",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonalLoan",
                table: "PersonalLoan",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Content",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    PersonalLoanId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Content", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Content_PersonalLoan_PersonalLoanId",
                        column: x => x.PersonalLoanId,
                        principalTable: "PersonalLoan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Content_PersonalLoanId",
                table: "Content",
                column: "PersonalLoanId");
        }
    }
}
