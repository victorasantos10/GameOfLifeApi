using Microsoft.AspNetCore.Mvc;

namespace GameOfLifeApi.Controllers
{
    [ApiController]
    [Route("/api/boards")]
    public class BoardsController : ControllerBase
    {
        private readonly ILogger<BoardsController> _logger;

        public BoardsController(ILogger<BoardsController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IEnumerable<object> Get()
        {
            return null;
        }
    }
}
