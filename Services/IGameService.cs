using FluentResults;

namespace GameOfLifeApi.Services
{
    public interface IGameService
    {
        Task<Result<Guid>> CreateBoardAsync(bool[][] initialState);
        Task<Result<string>> GetNextStateAsync(Guid boardId);
        Task<Result<string>> GetFinalStateAsync(Guid boardId, int maxAttempts);
        Task<Result<string>> GetStateAfterStepsAsync(Guid boardId, int steps);

    }
}
