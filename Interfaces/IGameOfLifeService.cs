namespace GameOfLifeApi.Interfaces
{
    public interface IGameOfLifeService
    {
        bool[][] GetNextGeneration(bool[][] board);
        bool AreBoardsEqual(bool[][] board, bool[][] nextBoard);
    }
}
