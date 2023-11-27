using ReceivableInfoApi.Common.Services;
using ReceivableInfoApi.DataAccess;
using ReceivableInfoApi.DataAccess.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<DataContext>();

builder.Services.AddScoped<IReceivableCRUDService, ReceivableCRUDService>();
builder.Services.AddScoped<IReceivableStatisticsService, ReceivableStatisticsService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

namespace ReceivableInfoApi.WebApi
{
    public partial class Program
    {
    }
}