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

string? routeTableName = configuration["RouteTableName"];
if (routeTableName is null)
{
    throw new ArgumentException("Configuration property 'RouteTableName' does not exist");
}


string? stopTableName = configuration["StopTableName"];
if (stopTableName is null)
{
    throw new ArgumentException("Configuration property 'StopTableName' does not exist");
}

string? routeStopTableName = configuration["RouteStopTableName"];
if (routeStopTableName is null)
{
    throw new ArgumentException("Configuration property 'RouteStopTableName' does not exist");
}

string? tripTableName = configuration["TripTableName"];
if (tripTableName is null)
{
    throw new ArgumentException("Configuration property 'TripTableName' does not exist");
}

string? tripCheckInTableName = configuration["TripCheckInTableName"];
if (tripCheckInTableName is null)
{
    throw new ArgumentException("Configuration property 'TripCheckInTableName' does not exist");
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
builder.Services.AddScoped<IRouteDao>(sp => new AdoRouteDao(connectionFactory, routeTableName));
builder.Services.AddScoped<IRouteService, RouteService>();

builder.Services.AddScoped<IStopDao>(sp => new AdoStopDao(connectionFactory, stopTableName));
builder.Services.AddScoped<IStopService, StopService>();

builder.Services.AddScoped<IRouteStopDao>(sp => new AdoRouteStopDao(connectionFactory, routeStopTableName, routeTableName, stopTableName));
builder.Services.AddScoped<IRouteStopService, RouteStopService>();

builder.Services.AddScoped<ITripDao>(sp => new AdoTripDao(connectionFactory, tripTableName, routeTableName, stopTableName, routeStopTableName));
builder.Services.AddScoped<ITripService, TripService>();

builder.Services.AddScoped<ITripCheckInDao>(sp => new AdoTripCheckInDao(connectionFactory, tripCheckInTableName));
builder.Services.AddScoped<ITripCheckInService, TripCheckInService>();

builder.Services.AddScoped<IStatisticDao>(sp => new AdoStatisticDao(connectionFactory, tripCheckInTableName, tripTableName, routeStopTableName, routeTableName));


//add keycloak authentication
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        //options.Authority = configuration["Authentication:Keycloak:Authority"];
//        //options.Audience = configuration["Authentication:Keycloak:ClientId"];
//        options.Authority = "http://localhost:8081/realms/NextStopRealm"; 
//        options.Audience = "nextstop-server";
//        options.RequireHttpsMetadata = false;
//        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidIssuer = $"{configuration["Authentication:Keycloak:Authority"]}/protocol/openid-connect",
//            RoleClaimType = "realm_access.roles"
//        };
//    });

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
