using lolappAPI.Repository;
using lolappAPI.Repository.Interfaces;
using lolappAPI.Types;
using lolappAPI.Types.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

var appSettingsFile = "appsettings.json";

if (builder.Environment.IsDevelopment())
{
    appSettingsFile = "appsettings.Development.json";
}

var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile(appSettingsFile, optional: true)
        .AddCommandLine(args)
        .Build();

// Add services to the container.
builder.Services.Configure<MongoDbSettings>(config.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<IMongoDbSettings>(serviceProvider =>
    serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);

//builder.Services.AddScoped(typeof(IDataAccessRepository<>), typeof(DataAccessRepository<>));
builder.Services.AddScoped(typeof(ISummonerRepository), typeof(SummonerRepository));
builder.Services.AddScoped(typeof(ILeagueRepository), typeof(LeagueRepository));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.ConfigureSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo()));
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("v1/swagger.json", "MyAPI V1");
    });
}
//app.UseStaticFiles();

app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            );

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
