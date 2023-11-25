using ReceivableInfoApi.Common.Services;
using ReceivableInfoApi.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<DataContext>();

builder.Services.AddScoped<IReceivableCRUDService, ReceivableCRUDService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();