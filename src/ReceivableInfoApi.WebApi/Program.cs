using System.Reflection;
using ReceivableInfoApi.Common.Services;
using ReceivableInfoApi.DataAccess;
using ReceivableInfoApi.DataAccess.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<DataContext>();

builder.Services.AddScoped<IReceivableCRUDService, ReceivableCRUDService>();
builder.Services.AddScoped<IReceivableStatisticsService, ReceivableStatisticsService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, 
        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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