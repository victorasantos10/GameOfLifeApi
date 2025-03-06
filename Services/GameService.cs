using FluentResults;
using GameOfLifeApi.Helpers;
using GameOfLifeApi.Models;
using GameOfLifeApi.Repositories;

namespace GameOfLifeApi.Services
{
    public class GameService : IGameService
    {  
        private readonly IBoardRepository _boardRepository;

        public GameService(IBoardRepository boardRepository)
        {
            _boardRepository = boardRepository;
        }

        /// <summary>
        /// Creates the board on the database, using an initial state
        /// </summary>
        /// <param name="initialState"></param>
        /// <returns></returns>
        public async Task<Result<Guid>> CreateBoardAsync(bool[][] initialState)
        {
            if (initialState == null || initialState.Length == 0 || initialState[0].Length == 0)
            {
                return Result.Fail("Board is empty");
            }

            int cols = initialState[0].Length;

            foreach (var row in initialState)
            {
                if(row.Length != cols)
                {
                    return Result.Fail("Board must be rectangular");
                }
            }

            var board = new Board
            {
                Id = Guid.NewGuid(),
                State = BoardStateConverter.Serialize(initialState),
                LastUpdated = DateTime.UtcNow
            };

            await _boardRepository.AddBoardAsync(board);
            return Result.Ok(board.Id);
        }

        /// <summary>
        /// Gets the next state of the board.
        /// </summary>
        /// <param name="boardId">the id of the board</param>
        /// <returns>the board state</returns>
        public async Task<Result<bool[][]>> GetNextStateAsync(Guid boardId)
        {
            var board = await _boardRepository.GetBoardByIdAsync(boardId);

            if (board == null)
                return Result.Fail("Board not found.");

            bool[][]? currentState = BoardStateConverter.Deserialize(board.State);

            if(currentState == null)
            {
                return Result.Fail("Board not found.");
            }

            bool[][] nextState = GetNextGeneration(currentState);

            board.State = BoardStateConverter.Serialize(nextState);
            board.LastUpdated = DateTime.UtcNow;
            await _boardRepository.UpdateBoardAsync(board);

            return Result.Ok(nextState);
        }

        /// <summary>
        /// Gets the final state of the board.
        /// </summary>
        /// <param name="boardId">the id of the board</param>
        /// <param name="maxAttempts">limit of attempts</param>
        /// <returns>the board state</returns>
        public async Task<Result<bool[][]>> GetFinalStateAsync(Guid boardId, int maxAttempts = 1000)
        {
            var board = await _boardRepository.GetBoardByIdAsync(boardId);
            if (board == null)
                return Result.Fail("Board not found.");

            var currentState = BoardStateConverter.Deserialize(board.State);

            if (currentState == null)
            {
                return Result.Fail("Board not found.");
            }

            int attempts = 0;
            while (attempts < maxAttempts)
            {
                var nextState = GetNextGeneration(currentState);

                if (AreBoardsEqual(currentState, nextState))
                {
                    board.State = BoardStateConverter.Serialize(nextState);
                    board.LastUpdated = DateTime.UtcNow;
                    await _boardRepository.UpdateBoardAsync(board);
                    return Result.Ok(nextState);
                }
                currentState = nextState;
                attempts++;
            }

            return Result.Fail($"Final state not reached after {maxAttempts} attempts.");
        }

        /// <summary>
        /// Gets the board state after a specified amount of steps
        /// </summary>
        /// <param name="boardId">the id of the board</param>
        /// <param name="steps">the amount of steps to advance</param>
        /// <returns>the board state</returns>
        public async Task<Result<bool[][]>> GetStateAfterStepsAsync(Guid boardId, int steps)
        {
            if (steps < 1)
                return Result.Fail("Steps must be at least 1.");

            var board = await _boardRepository.GetBoardByIdAsync(boardId);
            if (board == null)
                return Result.Fail("Board not found.");

            var currentState = BoardStateConverter.Deserialize(board.State);

            if (currentState == null)
            {
                return Result.Fail("Board not found.");
            }

            bool[][] newState = currentState;
            for (int i = 0; i < steps; i++)
            {
                newState = GetNextGeneration(newState);
            }

            board.State = BoardStateConverter.Serialize(newState);
            board.LastUpdated = DateTime.UtcNow;
            await _boardRepository.UpdateBoardAsync(board);
            return Result.Ok(newState);
        }

        /// <summary>
        /// Create the next board based on the current board.
        /// </summary>
        /// <param name="board">the current board</param>
        /// <returns>the next board</returns>
        private bool[][] GetNextGeneration(bool[][] board)
        {
            int rows = board.Length;
            int cols = board[0].Length;
            bool[][] nextBoard = new bool[rows][]; //Creating the next board 2d array with the same amount of rows of the current one.
            for (int i = 0; i < rows; i++)
            {
                nextBoard[i] = new bool[cols]; //Creating the next board 2d array with the same amount of cols of the current one.
                for (int j = 0; j < cols; j++)
                {
                    int liveNeighbors = CountLiveNeighbors(board, i, j);
                    
                    //Applying rules:
                    if (board[i][j]) // Any live cell...
                    {
                        /*
                         * ... with fewer than two live neighbors *dies*, as if by underpopulation (Rule nº 1).
                         * ... with two or three live neighbors lives on to the next generation (Rule nº2).
                         * ... with more than three live neighbors *dies*, as if by overpopulation (Rule nº 3)
                         * That being said, in order to simplify the result, just assign the second rule and it is enough to check.
                        */
                        bool alive = liveNeighbors == 2 || liveNeighbors == 3;
                        nextBoard[i][j] = alive;
                    }
                    else // Any dead cell 
                    {
                        //... with exactly three live neighbors becomes a live cell, as if by reproduction (Rule nº4).
                        bool alive = liveNeighbors == 3;
                        nextBoard[i][j] = alive;
                    }
                }
            }
            return nextBoard;
        }

        /// <summary>
        /// Count how many neighbors around position x = <paramref name="row"/> and y = <paramref name="col"/> are alive
        /// </summary>
        /// <param name="board">The board itself</param>
        /// <param name="row">the row to be checked</param>
        /// <param name="col">the col to be checked</param>
        /// <returns>Amount of live neighbors</returns>
        private int CountLiveNeighbors(bool[][] board, int row, int col)
        {
            int rows = board.Length;
            int cols = board[0].Length;
            int liveNeighbors = 0;

            //Iterating through the board.
            /* 
             * For clarity, imagine the board as follows:
             * (row -1, col -1) (row -1, col  0) (row -1, col +1)
             * (row  0, col -1) (    (skip)    ) (row  0, col +1)
             * (row +1, col -1) (row +1, col  0) (row +1, col +1)
            */
            for (int r = row - 1; r <= row + 1; r++)
            {
                for (int c = col - 1; c <= col + 1; c++)
                {
                    //Skipping the cell itself
                    if (r == row && c == col)
                        continue;

                    if 
                     (
                            r >= 0 && r < rows  // ensuring that we are inside the limits of the rows
                         && c >= 0 && c < cols  // ensuring that we are inside the limits of the cols
                         && board[r][c]         // checking if the neighbor being checked is alive
                     )
                    {
                         liveNeighbors++;
                    }
                }
            }
            return liveNeighbors;
        }

        /// <summary>
        /// Checking if the current board is the same as the next board
        /// </summary>
        /// <param name="board">current board</param>
        /// <param name="nextBoard">next board</param>
        /// <returns>true if they are equal, otherwise false</returns>
        private bool AreBoardsEqual(bool[][] board, bool[][] nextBoard)
        {
            int rows = board.Length;
            int cols = board[0].Length;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (board[i][j] != nextBoard[i][j]) //if there are any different elements, it means they aren't equal.
                        return false;
                }
            }
            return true;
        }
    }
}
