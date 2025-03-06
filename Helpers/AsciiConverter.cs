﻿using System.Text;

namespace GameOfLifeApi.Helpers
{
    public static class AsciiConverter
    {
        public static string ParseToAscii(bool[][] board, char live = 'O', char dead = '.')
        {
            var sb = new StringBuilder();
            foreach (var row in board)
            {
                foreach (var cell in row)
                {
                    sb.Append(cell ? live : dead);
                    sb.Append(" ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
