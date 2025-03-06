using FluentResults;

namespace GameOfLifeApi.Services
{
    public interface IGameService
    {
        Task<Result<Guid>> CreateBoardAsync(bool[][] initialState);
        Task<Result<bool[][]>> GetNextStateAsync(Guid boardId);
        Task<Result<bool[][]>> GetFinalStateAsync(Guid boardId, int maxAttempts);
        Task<Result<bool[][]>> GetStateAfterStepsAsync(Guid boardId, int steps);

    }
}
