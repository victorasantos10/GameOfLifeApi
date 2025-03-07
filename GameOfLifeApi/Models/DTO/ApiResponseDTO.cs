namespace GameOfLifeApi.Models.DTO
{
    /// <summary>
    /// DTO for the API response
    /// </summary>
    public class ApiResponseDTO
    {
        /// <summary>
        /// Data to be returned
        /// </summary>
        public object? data { get; set; }

        /// <summary>
        /// ASCII representation of the data (if applicable)
        /// </summary>
        public string? asciiData { get; set; }

        /// <summary>
        /// Message describing the request state.
        /// </summary>
        public string? message { get; set; }
    }
}
