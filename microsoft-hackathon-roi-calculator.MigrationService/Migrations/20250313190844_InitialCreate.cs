using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace microsoft_hackathon_roi_calculator.MigrationService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectROIs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectBudget = table.Column<double>(type: "float", nullable: false),
                    NumberOfEmployees = table.Column<int>(type: "int", nullable: false),
                    ProjectDurationMonths = table.Column<int>(type: "int", nullable: false),
                    ROI = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectROIs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectROIs");
        }
    }
}
