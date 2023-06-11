using System.ComponentModel.DataAnnotations;

namespace MvcVideoGames.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }
        public string ReviewVideoGame { get; set; }
        public int VideoGamesId { get; set; }
        public virtual VideoGames VideoGame { get; set; }

    }
}
