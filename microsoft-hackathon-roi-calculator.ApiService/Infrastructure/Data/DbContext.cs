using Microsoft.EntityFrameworkCore;
using microsoft_hackathon_roi_calculator.Core.Models;

namespace microsoft_hackathon_roi_calculator.ApiService.Infrastructure.Data
{
    public class CalculatorDbContext : DbContext
    {
        public CalculatorDbContext(DbContextOptions<CalculatorDbContext> options) : base(options)
        {
        }
        public DbSet<ProjectROI> ProjectROIs { get; set; }
    }
}
