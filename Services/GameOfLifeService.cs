using GameOfLifeApi.Interfaces;

namespace GameOfLifeApi.Services
{
    public class GameOfLifeService : IGameOfLifeService
    {  

        /// <summary>
        /// Create the next board based on the current board.
        /// </summary>
        /// <param name="board">the current board</param>
        /// <returns>the next board</returns>
        public bool[][] GetNextGeneration(bool[][] board)
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
        public bool AreBoardsEqual(bool[][] board, bool[][] nextBoard)
        {
            int rows = board.Length;
            int cols = board[0].Length;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (board[i][j] != nextBoard[i][j])
                        return false;
                }
            }
            return true;
        }
    }
}
