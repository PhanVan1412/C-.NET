using Npgsql;
using OrdersManagement.Common.Helper;
using OrdersManagement.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


ConfigHelper.configModel = app.Configuration.Get<AppSettingsModel>() ?? new AppSettingsModel();

foreach (var item in app.Configuration.GetSection("ConnectionStrings").GetChildren())
{
    if (item.Value != null)
    {
        var dataSource = NpgsqlDataSource.Create(item.Value);
        PostgreSQLService.Instance.AddConnection(item.Key, item.Value, dataSource);
    }
}

app.Run();
