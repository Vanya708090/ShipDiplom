using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShipDiplom.Migrations
{
    public partial class UpdateFieldToShip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShipType",
                table: "Ships",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Ships",
                newName: "ShipType");
        }
    }
}
