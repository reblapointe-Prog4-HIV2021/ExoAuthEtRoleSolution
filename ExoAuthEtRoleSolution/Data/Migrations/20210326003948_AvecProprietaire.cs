using Microsoft.EntityFrameworkCore.Migrations;

namespace ExoAuthEtRoleSolution.Data.Migrations
{
    public partial class AvecProprietaire : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProprietaireId",
                table: "Vetement",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProprietaireId",
                table: "Vetement");
        }
    }
}
