using System.Text.Json;

namespace GameOfLifeApi.Helpers
{
    public static class BoardStateConverter
    {
        public static string Serialize(bool[][] board) => JsonSerializer.Serialize(board);
        public static bool[][]? Deserialize(string json) => JsonSerializer.Deserialize<bool[][]>(json);
    }
}
