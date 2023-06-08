using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcVideoGames.Models;

namespace MvcVideoGames.Data
{
    public class MvcVideoGamesContext : DbContext
    {
        public MvcVideoGamesContext (DbContextOptions<MvcVideoGamesContext> options)
            : base(options)
        {
        }

        public DbSet<MvcVideoGames.Models.VideoGames> VideoGames { get; set; } = default!;
    }
}
