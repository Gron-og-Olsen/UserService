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

    // Retrieve the MongoDB connection string from the environment variable in docker-compose.yml
    var connectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING");
    var databaseName = Environment.GetEnvironmentVariable("UserDatabaseName");
    var collectionName = Environment.GetEnvironmentVariable("UserCollectionName");

    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("MongoDB connection string is not set in the environment variables.");
    }

    var mongoSettings = builder.Configuration.GetSection("MongoDB");
    
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
