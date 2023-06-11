namespace MvcVideoGames.Models
{
    public class VideoGameReviews
    {
        public IEnumerable<Review> Reviews { get; set; }
        public VideoGames VideoGame { get; set; }
        public Review Review { get; set; }

    }
}
