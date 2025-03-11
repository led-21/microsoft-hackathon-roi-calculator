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
                name: "CostBenefitMetrics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NetBenefit = table.Column<double>(type: "float", nullable: false),
                    CostOfInvestment = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostBenefitMetrics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeMetrics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalEmployees = table.Column<int>(type: "int", nullable: false),
                    TotalHoursWorkedWeekly = table.Column<int>(type: "int", nullable: false),
                    EmployeesUsingNewTool = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeMetrics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImplementationMetrics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalChangeImplementationTime = table.Column<int>(type: "int", nullable: false),
                    TotalPlannedImplementationTime = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImplementationMetrics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcessMetrics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalProcesses = table.Column<int>(type: "int", nullable: false),
                    CompliantProcesses = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessMetrics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResponseMetrics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SumOfAllScores = table.Column<int>(type: "int", nullable: false),
                    TotalResponses = table.Column<int>(type: "int", nullable: false),
                    PositiveResponses = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponseMetrics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingMetrics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnrolledTraining = table.Column<int>(type: "int", nullable: false),
                    CompletedTraining = table.Column<int>(type: "int", nullable: false),
                    PreTrainingScore = table.Column<int>(type: "int", nullable: false),
                    PostTrainingScore = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingMetrics", x => x.Id);
                });

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
                    CostBenefitMetricsId = table.Column<int>(type: "int", nullable: true),
                    EmployeeMetricsId = table.Column<int>(type: "int", nullable: true),
                    ImplementationMetricsId = table.Column<int>(type: "int", nullable: true),
                    TrainingMetricsId = table.Column<int>(type: "int", nullable: true),
                    ResponseMetricsId = table.Column<int>(type: "int", nullable: true),
                    ProcessMetricsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectROIs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectROIs_CostBenefitMetrics_CostBenefitMetricsId",
                        column: x => x.CostBenefitMetricsId,
                        principalTable: "CostBenefitMetrics",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjectROIs_EmployeeMetrics_EmployeeMetricsId",
                        column: x => x.EmployeeMetricsId,
                        principalTable: "EmployeeMetrics",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjectROIs_ImplementationMetrics_ImplementationMetricsId",
                        column: x => x.ImplementationMetricsId,
                        principalTable: "ImplementationMetrics",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjectROIs_ProcessMetrics_ProcessMetricsId",
                        column: x => x.ProcessMetricsId,
                        principalTable: "ProcessMetrics",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjectROIs_ResponseMetrics_ResponseMetricsId",
                        column: x => x.ResponseMetricsId,
                        principalTable: "ResponseMetrics",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjectROIs_TrainingMetrics_TrainingMetricsId",
                        column: x => x.TrainingMetricsId,
                        principalTable: "TrainingMetrics",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectROIs_CostBenefitMetricsId",
                table: "ProjectROIs",
                column: "CostBenefitMetricsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectROIs_EmployeeMetricsId",
                table: "ProjectROIs",
                column: "EmployeeMetricsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectROIs_ImplementationMetricsId",
                table: "ProjectROIs",
                column: "ImplementationMetricsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectROIs_ProcessMetricsId",
                table: "ProjectROIs",
                column: "ProcessMetricsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectROIs_ResponseMetricsId",
                table: "ProjectROIs",
                column: "ResponseMetricsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectROIs_TrainingMetricsId",
                table: "ProjectROIs",
                column: "TrainingMetricsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectROIs");

            migrationBuilder.DropTable(
                name: "CostBenefitMetrics");

            migrationBuilder.DropTable(
                name: "EmployeeMetrics");

            migrationBuilder.DropTable(
                name: "ImplementationMetrics");

            migrationBuilder.DropTable(
                name: "ProcessMetrics");

            migrationBuilder.DropTable(
                name: "ResponseMetrics");

            migrationBuilder.DropTable(
                name: "TrainingMetrics");
        }
    }
}
