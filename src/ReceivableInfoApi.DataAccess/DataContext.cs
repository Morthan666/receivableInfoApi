using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ReceivableInfoApi.Common.Model;

namespace ReceivableInfoApi.DataAccess;

public class DataContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public DataContext(IConfiguration configuration) => Configuration = configuration;
    public DataContext() {}
    
    public DbSet<Receivable> Receivables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var connectionString = Configuration.GetConnectionString("LocalDb");
        
        if (string.IsNullOrWhiteSpace(connectionString))
            options.UseInMemoryDatabase("InMemoryDb");
        else
            options.UseNpgsql(connectionString);
    }
}