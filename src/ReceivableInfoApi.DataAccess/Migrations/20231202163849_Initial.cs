using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReceivableInfoApi.DataAccess.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Receivables",
                columns: table => new
                {
                    Reference = table.Column<string>(type: "text", nullable: false),
                    CurrencyCode = table.Column<string>(type: "text", nullable: false),
                    IssueDate = table.Column<string>(type: "text", nullable: false),
                    OpeningValue = table.Column<decimal>(type: "numeric", nullable: false),
                    PaidValue = table.Column<decimal>(type: "numeric", nullable: false),
                    DueDate = table.Column<string>(type: "text", nullable: false),
                    ClosedDate = table.Column<string>(type: "text", nullable: true),
                    Cancelled = table.Column<bool>(type: "boolean", nullable: true),
                    DebtorName = table.Column<string>(type: "text", nullable: false),
                    DebtorReference = table.Column<string>(type: "text", nullable: false),
                    DebtorAddress1 = table.Column<string>(type: "text", nullable: true),
                    DebtorAddress2 = table.Column<string>(type: "text", nullable: true),
                    DebtorTown = table.Column<string>(type: "text", nullable: true),
                    DebtorState = table.Column<string>(type: "text", nullable: true),
                    DebtorZip = table.Column<string>(type: "text", nullable: true),
                    DebtorCountryCode = table.Column<string>(type: "text", nullable: false),
                    DebtorRegistrationNumber = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receivables", x => x.Reference);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Receivables");
        }
    }
}
