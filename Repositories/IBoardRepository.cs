using GameOfLifeApi.Models;

namespace GameOfLifeApi.Repositories
{
    public interface IBoardRepository
    {
        Task AddBoardAsync(Board board);
        Task<Board?> GetBoardByIdAsync(Guid id);
        Task UpdateBoardAsync(Board board);
    }
}
