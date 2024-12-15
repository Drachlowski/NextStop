using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NextStop.Common.Database;
using NextStop.Server.Services;

IConfiguration configuration = ConfigurationUtil.GetConfiguration();
string? holidayTableName = configuration["HolidayTableName"];
if (holidayTableName is null)
{
    throw new ArgumentException("Configuration property 'HolidayTableName' does not exist");
}

IConnectionFactory connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "NextStopDbConnection", "ProviderName");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddScoped<IHolidayService, HolidayService(connectionFactory, holidayTableName) >();
builder.Services.AddScoped<IHolidayService>(sp =>
    new HolidayService(connectionFactory, holidayTableName));

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

app.Run();
