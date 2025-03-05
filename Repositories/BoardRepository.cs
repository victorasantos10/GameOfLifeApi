using GameOfLifeApi.Data;
using GameOfLifeApi.Models;

namespace GameOfLifeApi.Repositories
{
    public class BoardRepository : IBoardRepository
    {
        private readonly GameOfLifeContext _context;
        public BoardRepository(GameOfLifeContext context)
        {
            _context = context;
        }

        public async Task AddBoardAsync(Board board)
        {
            _context.Boards.Add(board);
            await _context.SaveChangesAsync();
        }

        public async Task<Board?> GetBoardByIdAsync(Guid id)
        {
            return await _context.Boards.FindAsync(id);
        }

        public async Task UpdateBoardAsync(Board board)
        {
            _context.Boards.Update(board);
            await _context.SaveChangesAsync();
        }
    }
}
