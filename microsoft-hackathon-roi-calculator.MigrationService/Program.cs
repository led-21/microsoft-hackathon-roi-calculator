using microsoft_hackathon_roi_calculator.MigrationService;
using Microsoft.EntityFrameworkCore;
using microsoft_hackathon_roi_calculator.Domain.Models;


var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.AddSqlServerDbContext<CalculatorDbContext>("roidb");

var host = builder.Build();
host.Run();

public class CalculatorDbContext : DbContext
{
    public CalculatorDbContext(DbContextOptions<CalculatorDbContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProjectROI>(entity =>
        {
            entity.ToTable("roi");
            entity.Property(p => p.Id).HasColumnName("project_id");
            entity.Property(p => p.ProjectName).HasColumnName("project_name");
            entity.Property(p => p.Description).HasColumnName("description");
            entity.Property(p => p.ProjectBudget).HasColumnName("project_budget");
            entity.Property(p => p.NumberOfEmployees).HasColumnName("number_of_employees");
            entity.Property(p => p.StartDate).HasColumnName("start_date");
            entity.Property(p => p.ProjectDurationMonths).HasColumnName("project_duration_months");
            entity.Property(p => p.ROI).HasColumnName("roi");
            entity.Property(p => p.TotalHoursWorkedWeekly).HasColumnName("total_hours_worked_weekly");
            entity.Property(p => p.CompletedTraining).HasColumnName("completed_training");
            entity.Property(p => p.EmployeesUsingNewTool).HasColumnName("employees_using_new_tool");
            entity.Property(p => p.TotalChangeImplementationTime).HasColumnName("total_change_implementation_time");
            entity.Property(p => p.TotalPlannedImplementationTime).HasColumnName("total_planned_implementation_time");
            entity.Property(p => p.TotalProcesses).HasColumnName("total_processes");
            entity.Property(p => p.CompliantProcesses).HasColumnName("compliant_processes");
            entity.Property(p => p.ProjectEvaluationTotalResponses).HasColumnName("project_evaluation_total_responses");
            entity.Property(p => p.ProjectEvaluationPositiveResponses).HasColumnName("project_evaluation_positive_responses");
            entity.Property(p => p.ProjectEvaluationSumOfAllScores).HasColumnName("project_evaluation_sum_of_all_scores");
        });
    }
    public DbSet<ProjectROI> ProjectROIs { get; set; }

}