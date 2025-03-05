using GameOfLifeApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GameOfLifeApi.Data
{

    public class GameOfLifeContext : DbContext
    {
        public GameOfLifeContext(DbContextOptions<GameOfLifeContext> options)
            : base(options)
        {
        }

        public DbSet<Board> Boards { get; set; }
    }
}
