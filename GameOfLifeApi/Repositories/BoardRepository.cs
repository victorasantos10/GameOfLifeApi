using GameOfLifeApi.Data;
using GameOfLifeApi.Models;
using MongoDB.Driver;

namespace GameOfLifeApi.Repositories
{
    public class BoardRepository : IBoardRepository
    {
        private readonly IMongoCollection<Board> _boards;

        public BoardRepository(MongoDbContext context)
        {
            _boards = context.Boards;
        }

        public async Task AddBoardAsync(Board board)
        {
            await _boards.InsertOneAsync(board);
        }

        public async Task<Board?> GetBoardByIdAsync(Guid id)
        {
            var filter = Builders<Board>.Filter.Eq(b => b.Id, id);
            return await _boards.Find(filter).FirstOrDefaultAsync();
        }

        public async Task UpdateBoardAsync(Board board)
        {
            var filter = Builders<Board>.Filter.Eq(b => b.Id, board.Id);
            await _boards.ReplaceOneAsync(filter, board);
        }
    }
}
