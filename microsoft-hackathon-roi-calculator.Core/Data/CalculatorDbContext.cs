using microsoft_hackathon_roi_calculator.Core.Models;
using microsoft_hackathon_roi_calculator.Core.Models.Metrics;
using Microsoft.EntityFrameworkCore;

namespace microsoft_hackathon_roi_calculator.Core.Data
{
    public class CalculatorDbContext : DbContext
    {
        public CalculatorDbContext(DbContextOptions<CalculatorDbContext> options) : base(options)
        {
           
        }

        public DbSet<ProjectROI> ProjectROIs { get; set; }
    }
}
