using System.Diagnostics;
using CityInfo.API;
using CityInfo.API.DbContext;
using CityInfo.API.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Build.Locator;
using Microsoft.EntityFrameworkCore;
using Serilog;
using AutoMapper;


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/cityinfo.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
// builder.Logging.ClearProviders();
// builder.Logging.AddConsole();
// Replaced by serilog.Builder 

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers(options =>
{
    // options.InputFormatters // TODO: look into this for better support
    options.ReturnHttpNotAcceptable = true; // This makes it so that we don't return 200 for
    // any format type since we are returning json'
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

// Mail Service
#if DEBUG
builder.Services.AddTransient<IMailService, LocalMailService>();
#endif
builder.Services.AddTransient<IMailService, CloudMailService>();

builder.Services.AddSingleton<CitiesDataStore>();
builder.Services.AddDbContext<CityInfoContext>(
    dbContextOptions => dbContextOptions
        .UseSqlite(builder.Configuration["ConnectionStrings:CityInfoDBConnectionString"]));

builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.UseAuthorization();

app.Run();