using Microsoft.EntityFrameworkCore.Migrations;

namespace ExoAuthEtRoleSolution.Data.Migrations
{
    public partial class AvecVetements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vetement",
                columns: table => new
                {
                    VetementId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vetement", x => x.VetementId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vetement");
        }
    }
}
