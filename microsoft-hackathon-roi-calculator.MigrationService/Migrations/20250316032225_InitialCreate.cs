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
                name: "roi",
                columns: table => new
                {
                    project_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    project_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    project_budget = table.Column<double>(type: "float", nullable: false),
                    number_of_employees = table.Column<int>(type: "int", nullable: false),
                    start_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    project_duration_months = table.Column<int>(type: "int", nullable: false),
                    roi = table.Column<double>(type: "float", nullable: false),
                    total_hours_worked_weekly = table.Column<int>(type: "int", nullable: false),
                    completed_training = table.Column<int>(type: "int", nullable: false),
                    employees_using_new_tool = table.Column<int>(type: "int", nullable: false),
                    total_change_implementation_time = table.Column<int>(type: "int", nullable: false),
                    total_planned_implementation_time = table.Column<int>(type: "int", nullable: false),
                    total_processes = table.Column<int>(type: "int", nullable: false),
                    compliant_processes = table.Column<int>(type: "int", nullable: false),
                    project_evaluation_total_responses = table.Column<int>(type: "int", nullable: false),
                    project_evaluation_positive_responses = table.Column<int>(type: "int", nullable: false),
                    project_evaluation_sum_of_all_scores = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roi", x => x.project_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "roi");
        }
    }
}
