using FluentResults;
using GameOfLifeApi.Handlers;
using GameOfLifeApi.Helpers;
using GameOfLifeApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GameOfLifeApi.Controllers
{
    [ApiController]
    [Route("/api/boards")]
    public class BoardsController : ControllerBase
    {
        private readonly IGameService _gameService;

        public BoardsController(IGameService gameOfLifeService)
        {
            _gameService = gameOfLifeService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBoard([FromBody] bool[][] board)
        {
            Result<Guid> result = await _gameService.CreateBoardAsync(board);
            return ApiResultHandler.HandleResult(result);
        }

        [HttpPut("{id}/next")]
        public async Task<IActionResult> RunToNextState(Guid id)
        {
            Result<bool[][]> result = await _gameService.RunToNextStateAsync(id);

            return ApiResultHandler.HandleResult(result);
        }

        [HttpPut("{id}/advance/{steps:int}")]
        public async Task<IActionResult> AdvanceBoardBySteps(Guid id, [Range(1, int.MaxValue)] int steps)
        {
            Result<bool[][]> result = await _gameService.AdvanceBoardByStepsAsync(id, steps);
            return ApiResultHandler.HandleResult(result);

        }

        [HttpPut("{id}/final")]
        public async Task<IActionResult> AdvanceToFinalState(Guid id, [FromQuery, Range(1, int.MaxValue)] int maxAttempts = 1000)
        {
           Result<bool[][]> result = await _gameService.AdvanceToFinalStateAsync(id, maxAttempts);
           return ApiResultHandler.HandleResult(result);
        }
    }
}
