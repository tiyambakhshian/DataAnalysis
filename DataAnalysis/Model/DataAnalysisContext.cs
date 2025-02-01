using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAnalysis.Model
{
    public partial class DataAnalysisContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DataAnalysisContext(DbContextOptions<DataAnalysisContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<Product> Products { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}