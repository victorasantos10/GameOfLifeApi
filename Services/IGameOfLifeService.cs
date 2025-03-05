namespace GameOfLifeApi.Services
{
    public interface IGameOfLifeService
    {
        bool[][] GetNextGeneration(bool[][] board);
        bool AreBoardsEqual(bool[][] board, bool[][] nextBoard);
    }
}
