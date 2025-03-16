using System;
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
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectBudget = table.Column<double>(type: "float", nullable: false),
                    NumberOfEmployees = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProjectDurationMonths = table.Column<int>(type: "int", nullable: false),
                    ROI = table.Column<double>(type: "float", nullable: false),
                    TotalHoursWorkedWeekly = table.Column<int>(type: "int", nullable: false),
                    CompletedTraining = table.Column<int>(type: "int", nullable: false),
                    EmployeesUsingNewTool = table.Column<int>(type: "int", nullable: false),
                    TotalChangeImplementationTime = table.Column<int>(type: "int", nullable: false),
                    TotalPlannedImplementationTime = table.Column<int>(type: "int", nullable: false),
                    TotalProcesses = table.Column<int>(type: "int", nullable: false),
                    CompliantProcesses = table.Column<int>(type: "int", nullable: false),
                    ProjectEvaluationTotalResponses = table.Column<int>(type: "int", nullable: false),
                    ProjectEvaluationPositiveResponses = table.Column<int>(type: "int", nullable: false),
                    ProjectEvaluationSumOfAllScores = table.Column<int>(type: "int", nullable: false)
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
