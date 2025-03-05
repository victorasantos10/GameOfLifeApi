using GameOfLifeApi.Models;
using MongoDB.Driver;

namespace GameOfLifeApi.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDb")
                                   ?? "mongodb://localhost:27017";
            var client = new MongoClient(connectionString);

            var databaseName = configuration["MongoDbSettings:DatabaseName"] ?? "gameoflife";
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<Board> Boards => _database.GetCollection<Board>("Boards");
    }
}
