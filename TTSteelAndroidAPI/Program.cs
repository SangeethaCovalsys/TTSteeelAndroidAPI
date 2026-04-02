using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using TTSteelAndroidAPI.Data;
using TTSteelAndroidAPI.Interface;
using TTSteelWebAPI.Service;

var builder = WebApplication.CreateBuilder(args);
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// -------------------------------
// Register your custom services
// -------------------------------
builder.Services.AddSingleton<DbConnectionContext>();

builder.Services.AddHttpClient<ISapService, SapService>(client =>
{
    var baseUrl = builder.Configuration["SapSettings:BaseUrl"];
    if (!string.IsNullOrEmpty(baseUrl))
        client.BaseAddress = new Uri(baseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// -------------------------------
// CORS Configuration
// -------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()   // For development only; restrict in production
              .AllowAnyMethod()
              .AllowAnyHeader();
    });

    // Example of a restrictive policy (for production):
    // options.AddPolicy("Restricted", policy =>
    // {
    //     policy.WithOrigins("https://yourfrontend.com", "http://localhost:3000")
    //           .AllowAnyMethod()
    //           .AllowAnyHeader()
    //           .AllowCredentials();
    // });
});
if (builder.Environment.IsDevelopment())
{
    var httpHandler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
    };

    builder.Services.AddHttpClient<ISapService, SapService>(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["SapSettings:BaseUrl"]);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    })
    .ConfigurePrimaryHttpMessageHandler(() => httpHandler);
}
else
{
    // Production: use default validation (requires valid certificate)
    builder.Services.AddHttpClient<ISapService, SapService>(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["SapSettings:BaseUrl"]);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    });
}
builder.Services.AddMemoryCache();

//Log files handle
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
//builder.Logging.AddFile(builder.Configuration.GetSection("Logging:File"));
builder.Services.AddAutoMapper(typeof(Program));
//builder.Services.AddAutoMapper(typeof(StockPostProfile));
Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Error() // Log only errors
            .WriteTo.Console()
            .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day) // Log to a file
            .CreateLogger();
var dbType = builder.Configuration["DbType"]; // e.g., "HANA"

builder.Services.AddLogging(builder =>
{
    builder.AddSerilog();
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("*")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();
app.UseCors("AllowFrontend");

app.UseSwagger();
app.UseSwaggerUI();




// Use CORS middleware (must be placed after UseRouting and before UseAuthorization)
 // or "Restricted" if you switch to a restrictive policy

app.UseAuthorization();

app.MapControllers();

app.Run();