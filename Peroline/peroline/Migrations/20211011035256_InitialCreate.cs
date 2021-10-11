using Microsoft.EntityFrameworkCore.Migrations;

namespace peroline.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ErrorMemo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerId = table.Column<string>(type: "TEXT", nullable: true),
                    ErrorType = table.Column<string>(type: "TEXT", nullable: true),
                    RetriedByBotId = table.Column<string>(type: "TEXT", nullable: true),
                    HasBeenRetried = table.Column<bool>(type: "INTEGER", nullable: false),
                    NumOfRetry = table.Column<int>(type: "INTEGER", nullable: false),
                    RecoveryFor = table.Column<string>(type: "TEXT", nullable: true),
                    DataForRetry = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorMemo", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ErrorMemo");
        }
    }
}
