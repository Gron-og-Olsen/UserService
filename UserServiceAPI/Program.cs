using MongoDB.Driver;
using Models;
using Microsoft.AspNetCore.Identity;
using NLog;
using NLog.Web;



try
{
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings()
.GetCurrentClassLogger();
logger.Debug("init main");

var builder = WebApplication.CreateBuilder(args);

// Register PasswordHasher for User
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// Configure MongoDB settings
var mongoSettings = builder.Configuration.GetSection("MongoDB");
var connectionString = mongoSettings["ConnectionString"];
var databaseName = mongoSettings["DatabaseName"];
var collectionName = mongoSettings["CollectionName"];

// Register MongoDB services
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(connectionString));
builder.Services.AddSingleton(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    var database = client.GetDatabase(databaseName);
    return database.GetCollection<User>(collectionName);
});

builder.Services.AddControllers();
builder.Logging.ClearProviders();
builder.Host.UseNLog();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();

}
catch (Exception ex)
{
    var logger = NLog.LogManager.GetCurrentClassLogger();
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}
