using MovieFInderServer.Models.Entities;

namespace MovieFInderServer.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public ICollection<MovieGenre>? Movies { get; set; }
    }
}
