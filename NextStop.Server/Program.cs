using NextStop.Common.Database;
using NextStop.Dal.Ado;
using NextStop.Dal.Interface;
using NextStop.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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

// Register Route services
builder.Services.AddScoped<IRouteDao>(sp => new AdoRouteDao(connectionFactory));
builder.Services.AddScoped<IRouteService, RouteService>();

builder.Services.AddScoped<IStopDao>(sp => new AdoStopDao(connectionFactory));
builder.Services.AddScoped<IStopService, StopService>();

builder.Services.AddScoped<IRouteStopDao>(sp => new AdoRouteStopDao(connectionFactory));
builder.Services.AddScoped<IRouteStopService, RouteStopService>();

//add keycloak authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = configuration["Authentication:Keycloak:Authority"];
        options.Audience = configuration["Authentication:Keycloak:ClientId"];
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = $"{configuration["Authentication:Keycloak:Authority"]}/protocol/openid-connect",
        };
    });

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
