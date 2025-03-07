using FluentResults;
using GameOfLifeApi.Handlers;
using GameOfLifeApi.Helpers;
using GameOfLifeApi.Models.DTO;
using GameOfLifeApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace GameOfLifeApi.Controllers
{
    [ApiController]
    [Route("/api/boards")]
    [ProducesResponseType(typeof(ApiResponseDTO), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiResponseDTO), (int)HttpStatusCode.BadRequest)]
    public class BoardsController : ControllerBase
    {
        private readonly IGameService _gameService;

        public BoardsController(IGameService gameOfLifeService)
        {
            _gameService = gameOfLifeService;
        }

        /// <summary>
        /// Creates a new board
        /// </summary>
        /// <param name="board" example="[[false,false,false],[true,true,true],[false,false,false]]">Payload containing the Blinker pattern</param>
        /// <returns>The board ID</returns>
        [HttpPost]
        public async Task<IActionResult> CreateBoard([FromBody] bool[][] board)
        {
            Result<Guid> result = await _gameService.CreateBoardAsync(board);
            return ApiResultHandler.HandleResult(result);
        }

        /// <summary>
        /// Runs a board to the next state
        /// </summary>
        /// <param name="id" example="21022942-c23c-4a63-8bae-d28a40ac4f7b">The ID of the board</param>
        /// <returns>An object containing both ASCII and boolean 2d arrays with the next state</returns>
        [HttpPut("{id}/next")]
        public async Task<IActionResult> RunToNextState(Guid id)
        {
            Result<bool[][]> result = await _gameService.RunToNextStateAsync(id);

            return ApiResultHandler.HandleResult(result);
        }

        /// <summary>
        /// Advance a board in a specified amount of steps
        /// </summary>
        /// <param name="id" example="21022942-c23c-4a63-8bae-d28a40ac4f7b">The ID of the board</param>
        /// <param name="steps" example="2">Amount of steps to advance</param>
        /// <returns>An object containing both ASCII and boolean 2d arrays with the state after <paramref name="steps"/> number of iterations</returns>
        [HttpPut("{id}/advance/{steps:int}")]
        public async Task<IActionResult> AdvanceBoardBySteps(Guid id, [Range(1, int.MaxValue)] int steps)
        {
            Result<bool[][]> result = await _gameService.AdvanceBoardByStepsAsync(id, steps);
            return ApiResultHandler.HandleResult(result);

        }

        /// <summary>
        /// Advance a board to its final state
        /// </summary>
        /// <param name="id" example="21022942-c23c-4a63-8bae-d28a40ac4f7b">The ID of the board</param>
        /// <param name="maxAttempts" example="1000">An optional parameter with the number of max attempts (default = 1000)</param>
        /// <returns>An object containing both ASCII and boolean 2d arrays with the final state of the board, or an error if there's no finite state.</returns>
        [HttpPut("{id}/final")]
        public async Task<IActionResult> AdvanceToFinalState(Guid id, [FromQuery, Range(1, int.MaxValue)] int maxAttempts = 1000)
        {
           Result<bool[][]> result = await _gameService.AdvanceToFinalStateAsync(id, maxAttempts);
           return ApiResultHandler.HandleResult(result);
        }
    }
}
