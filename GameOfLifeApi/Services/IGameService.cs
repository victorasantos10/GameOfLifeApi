using FluentResults;

namespace GameOfLifeApi.Services
{
    public interface IGameService
    {
        Task<Result<Guid>> CreateBoardAsync(bool[][] initialState);
        Task<Result<bool[][]>> RunToNextStateAsync(Guid boardId);
        Task<Result<bool[][]>> AdvanceToFinalStateAsync(Guid boardId, int maxAttempts);
        Task<Result<bool[][]>> AdvanceBoardByStepsAsync(Guid boardId, int steps);

    }
}
