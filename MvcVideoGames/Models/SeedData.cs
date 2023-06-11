using Microsoft.EntityFrameworkCore;
using MvcVideoGames.Data;

namespace MvcVideoGames.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MvcVideoGamesContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<MvcVideoGamesContext>>()))
            {
              
                if (context.VideoGames.Any())
                {
                    return;   
                }

                context.VideoGames.AddRange(
                    new VideoGames
                    {
                        Title = "Red Dead Redemption 2",
                        ReleaseDate = DateTime.Parse("2018-2-12"),
                        Genre = "Action",
                        Price = 60M
                    },

                    new VideoGames
                    {
                        Title = "Outlast",
                        ReleaseDate = DateTime.Parse("2013-3-13"),
                        Genre = "Horror",
                        Price = 50M
                    },

                    new VideoGames
                    {
                        Title = "Resident Evil Village",
                        ReleaseDate = DateTime.Parse("2021-2-23"),
                        Genre = "Horror",
                        Price = 40M
                    },

                    new VideoGames
                    {
                        Title = "FIFA 22",
                        ReleaseDate = DateTime.Parse("2021-10-15"),
                        Genre = "Sports",
                        Price = 30M
                    }
                );
                context.SaveChanges();
            }
        }
    }
}