using GameOfLifeApi.Models;
using MongoDB.Driver;

namespace GameOfLifeApi.Data
{
    /// <summary>
    /// Context for the MongoDB database
    /// </summary>
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var host = configuration["MongoDbSettings:Host"] ?? "localhost";
            var port = configuration["MongoDbSettings:Port"] ?? "27017";
            var username = configuration["MongoDbSettings:Username"];
            var password = configuration["MongoDbSettings:Password"];
            var databaseName = configuration["MongoDbSettings:DatabaseName"] ?? "gameoflife";

            var connectionString = $"mongodb://{username}:{password}@{host}:{port}";

            var client = new MongoClient(connectionString);

            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<Board> Boards => _database.GetCollection<Board>("Boards");
    }
}
