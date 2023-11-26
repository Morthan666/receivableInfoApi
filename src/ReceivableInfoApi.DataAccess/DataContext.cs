using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ReceivableInfoApi.Common.Model;

namespace ReceivableInfoApi.DataAccess;

public class DataContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public DataContext(IConfiguration configuration) => Configuration = configuration;

    protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseInMemoryDatabase("InMemoryDb");

    public DbSet<Receivable> Receivables { get; set; }
}