using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace GameOfLifeApi.Models
{
    public class Board
    {
        /// <summary>
        /// Id field for the board
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        /// <summary>
        /// JSON-serialized representation of the board state (bool[][]) 
        /// </summary>
        [BsonElement("state")]
        public string State { get; set; }

        /// <summary>
        /// Date and time for the board creation
        /// </summary>
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time for the last board update
        /// </summary>
        [BsonElement("lastUpdated")]
        public DateTime LastUpdated { get; set; }
    }
}
