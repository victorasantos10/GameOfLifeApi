using FluentResults;

namespace GameOfLifeApi.Services
{
    public interface IGameService
    {
        /// <summary>
        /// Creates a new board
        /// </summary>
        /// <param name="initialState"">initial state for the board</param>
        /// <returns>The board ID</returns>
        Task<Result<Guid>> CreateBoardAsync(bool[][] initialState);

        /// <summary>
        /// Runs a board to the next state
        /// </summary>
        /// <param name="boardId">The Id of the board</param>
        /// <returns>An object containing both ASCII and boolean 2d arrays with the next state</returns>
        Task<Result<bool[][]>> RunToNextStateAsync(Guid boardId);

        /// <summary>
        /// Advance a board to its final state
        /// </summary>
        /// <param name="boardId" example="21022942-c23c-4a63-8bae-d28a40ac4f7b">The ID of the board</param>
        /// <param name="maxAttempts" example="1000">An optional parameter with the number of max attempts (default = 1000)</param>
        /// <returns>An object containing both ASCII and boolean 2d arrays with the final state of the board, or an error if there's no finite state.</returns>
        Task<Result<bool[][]>> AdvanceToFinalStateAsync(Guid boardId, int maxAttempts);

        /// <summary>
        /// Advance a board in a specified amount of steps
        /// </summary>
        /// <param name="boardId">The ID of the board</param>
        /// <param name="steps">Amount of steps to advance</param>
        /// <returns>An object containing both ASCII and boolean 2d arrays with the state after <paramref name="steps"/> number of iterations</returns>
        Task<Result<bool[][]>> AdvanceBoardByStepsAsync(Guid boardId, int steps);

    }
}
