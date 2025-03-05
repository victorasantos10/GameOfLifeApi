using FluentResults;
using GameOfLifeApi.Services;
using Microsoft.AspNetCore.Mvc;

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

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Value);
        }

        [HttpGet("{id}/next")]
        public async Task<IActionResult> GetNextState(Guid id)
        {
            Result<bool[][]> result = await _gameService.GetNextStateAsync(id);
            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Value);
        }

        [HttpGet("{id}/advance/{steps:int}")]
        public async Task<IActionResult> GetStateAfterSteps(Guid id, int steps)
        {
            Result<bool[][]> result = await _gameService.GetStateAfterStepsAsync(id, steps);
            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Value);
        }

        [HttpGet("{id}/final")]
        public async Task<IActionResult> GetFinalState(Guid id, [FromQuery] int maxAttempts = 1000)
        {
           Result<bool[][]> result = await _gameService.GetFinalStateAsync(id, maxAttempts);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Value);
        }
    }
}
