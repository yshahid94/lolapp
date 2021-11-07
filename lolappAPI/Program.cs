using lolappAPI.Repository;
using lolappAPI.Repository.Interfaces;
using lolappAPI.Types;
using lolappAPI.Types.Interfaces;
using Microsoft.Extensions.Options;

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

app.Run();
