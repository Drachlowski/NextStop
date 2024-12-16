using NextStop.Common.Database;
using NextStop.Dal.Ado;
using NextStop.Dal.Interface;
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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Holiday services
builder.Services.AddScoped<IHolidayService>(sp => new HolidayService(connectionFactory, holidayTableName));

// Register Stop services
builder.Services.AddScoped<IStopService, StopService>();
builder.Services.AddScoped<IStopDao>(sp => new AdoStopDao(connectionFactory));

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
