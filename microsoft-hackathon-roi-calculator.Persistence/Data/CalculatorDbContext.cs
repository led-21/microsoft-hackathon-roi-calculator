using Microsoft.EntityFrameworkCore;
using microsoft_hackathon_roi_calculator.Domain.Models;

namespace microsoft_hackathon_roi_calculator.Persistence.Data
{
    public class CalculatorDbContext : DbContext
    {
        public CalculatorDbContext(DbContextOptions<CalculatorDbContext> options) : base(options)
        {
           
        }

        public DbSet<ProjectROI> ProjectROIs { get; set; }
    }
}
