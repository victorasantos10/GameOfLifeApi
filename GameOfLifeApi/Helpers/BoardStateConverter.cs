using System.Text.Json;

namespace GameOfLifeApi.Helpers
{
    /// <summary>
    /// Helper class to convert a 2D boolean array to a JSON string and vice versa
    /// </summary>
    public static class BoardStateConverter
    {
        public static string Serialize(bool[][] board) => JsonSerializer.Serialize(board);
        public static bool[][]? Deserialize(string json) => JsonSerializer.Deserialize<bool[][]>(json);
    }
}
