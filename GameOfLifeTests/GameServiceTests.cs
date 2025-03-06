using GameOfLifeApi.Helpers;
using GameOfLifeApi.Models;
using GameOfLifeApi.Repositories;
using GameOfLifeApi.Services;
using Moq;

namespace GameOfLifeTests
{
    public class GameServiceTests
    {
        private readonly Mock<IBoardRepository> _boardRepositoryMock;
        private readonly GameService _service;

        public GameServiceTests()
        {
            _boardRepositoryMock = new Mock<IBoardRepository>();
            _service = new GameService(_boardRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateBoardAsync_ValidBoard_ReturnsNewGuid()
        {
            bool[][] boardState = new bool[][]
            {
                new bool[] { true, false, true },
                new bool[] { false, true, false },
                new bool[] { true, false, true }
            };

            _boardRepositoryMock.Setup(r => r.AddBoardAsync(It.IsAny<Board>()))
                .Returns(Task.CompletedTask);

            var boardId = await _service.CreateBoardAsync(boardState);

            Assert.NotEqual(Guid.Empty, boardId);
            _boardRepositoryMock.Verify(r => r.AddBoardAsync(It.Is<Board>(b => b.Id == boardId.Value)), Times.Once);
        }

        [Fact]
        public async Task GetNextStateAsync_ValidBoard_ReturnsNextState()
        {
            bool[][] boardState = new bool[][]
            {
                new bool[] { false, false },
                new bool[] { false, false }
            };

            var board = new Board
            {
                Id = Guid.NewGuid(),
                State = BoardStateConverter.Serialize(boardState),
                CreatedAt = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow
            };

            _boardRepositoryMock.Setup(r => r.GetBoardByIdAsync(board.Id))
                .ReturnsAsync(board);
            _boardRepositoryMock.Setup(r => r.UpdateBoardAsync(It.IsAny<Board>()))
                .Returns(Task.CompletedTask);

            var nextState = await _service.RunToNextStateAsync(board.Id);

            Assert.Equal(boardState.Length, nextState.Value.Length);
            for (int i = 0; i < boardState.Length; i++)
            {
                Assert.Equal(boardState[i], nextState.Value[i]);
            }
        }

        [Fact]
        public async Task GetNextStateAsync_InvalidBoardId_ReturnsFailedResult()
        {
            Guid invalidId = Guid.NewGuid();
            _boardRepositoryMock.Setup(r => r.GetBoardByIdAsync(invalidId))
                .ReturnsAsync((Board?)null);

            var result = await _service.RunToNextStateAsync(invalidId);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task GetStateAfterStepsAsync_InvalidSteps_ReturnsFailedResult()
        {
            Guid boardId = Guid.NewGuid();

            var result = await _service.AdvanceBoardByStepsAsync(boardId, 0);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task GetStateAfterStepsAsync_ValidBoard_ReturnsStateAfterSteps()
        {
            bool[][] boardState = new bool[][]
            {
                new bool[] { false, false, false },
                new bool[] { false, false, false },
                new bool[] { false, false, false }
            };

            var board = new Board
            {
                Id = Guid.NewGuid(),
                State = BoardStateConverter.Serialize(boardState),
                CreatedAt = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow
            };

            _boardRepositoryMock.Setup(r => r.GetBoardByIdAsync(board.Id))
                .ReturnsAsync(board);
            _boardRepositoryMock.Setup(r => r.UpdateBoardAsync(It.IsAny<Board>()))
                .Returns(Task.CompletedTask);

            var stateAfterSteps = await _service.AdvanceBoardByStepsAsync(board.Id, 5);

            Assert.Equal(boardState.Length, stateAfterSteps.Value.Length);
            for (int i = 0; i < boardState.Length; i++)
            {
                Assert.Equal(boardState[i], stateAfterSteps.Value[i]);
            }
        }

        [Fact]
        public async Task GetFinalStateAsync_StableBoard_ReturnsFinalState()
        {
            bool[][] boardState = new bool[][]
            {
                new bool[] { false, false, false },
                new bool[] { false, false, false },
                new bool[] { false, false, false }
            };

            var board = new Board
            {
                Id = Guid.NewGuid(),
                State = BoardStateConverter.Serialize(boardState),
                CreatedAt = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow
            };

            _boardRepositoryMock.Setup(r => r.GetBoardByIdAsync(board.Id))
                .ReturnsAsync(board);
            _boardRepositoryMock.Setup(r => r.UpdateBoardAsync(It.IsAny<Board>()))
                .Returns(Task.CompletedTask);

            var finalState = await _service.AdvanceToFinalStateAsync(board.Id);

            Assert.Equal(boardState.Length, finalState.Value.Length);
            for (int i = 0; i < boardState.Length; i++)
            {
                Assert.Equal(boardState[i], finalState.Value[i]);
            }
        }

        [Fact]
        public async Task GetFinalStateAsync_Blinker_ReturnsErrorDueToInfiniteLoop()
        {
            bool[][] boardState = new bool[][]
            {
                new bool[] { false, false, false },
                new bool[] { true,  true,  true},
                new bool[] { false, false, false }
            };

            var board = new Board
            {
                Id = Guid.NewGuid(),
                State = BoardStateConverter.Serialize(boardState),
                CreatedAt = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow
            };

            _boardRepositoryMock.Setup(r => r.GetBoardByIdAsync(board.Id))
                .ReturnsAsync(board);
            _boardRepositoryMock.Setup(r => r.UpdateBoardAsync(It.IsAny<Board>()))
                .Returns(Task.CompletedTask);

            var result = await _service.AdvanceToFinalStateAsync(board.Id);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task GetFinalStateAsync_BoardNotFound_ReturnsFailedResult()
        {
            Guid boardId = Guid.NewGuid();
            _boardRepositoryMock.Setup(r => r.GetBoardByIdAsync(boardId))
                .ReturnsAsync((Board?)null);

            var result = await _service.AdvanceToFinalStateAsync(boardId);
            Assert.False(result.IsSuccess);
        }
    }
};