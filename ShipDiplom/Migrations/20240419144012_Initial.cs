using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShipDiplom.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Piers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Length = table.Column<float>(type: "real", nullable: false),
                    Width = table.Column<float>(type: "real", nullable: false),
                    TrackingNumber = table.Column<string>(type: "text", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Piers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ships",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ShipType = table.Column<string>(type: "text", nullable: false),
                    SystemId = table.Column<string>(type: "text", nullable: false),
                    Length = table.Column<float>(type: "real", nullable: false),
                    Width = table.Column<float>(type: "real", nullable: false),
                    OwnerId = table.Column<string>(type: "text", nullable: false),
                    PierId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ships_Piers_PierId",
                        column: x => x.PierId,
                        principalTable: "Piers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ships_PierId",
                table: "Ships",
                column: "PierId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ships");

            migrationBuilder.DropTable(
                name: "Piers");
        }
    }
}
