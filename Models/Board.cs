using System.ComponentModel.DataAnnotations;

namespace GameOfLifeApi.Models
{
    public class Board
    {
        /// <summary>
        /// Id field for the board
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// JSON-serialized representation of the board state (bool[][]) 
        /// </summary>
        [Required]
        public string State { get; set; }

        /// <summary>
        /// Date and time for the board creation
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// Date and time for the last board update
        /// </summary>
        public DateTime LastUpdated { get; set; }
    }
}
