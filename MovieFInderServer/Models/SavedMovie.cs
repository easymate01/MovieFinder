using System.ComponentModel.DataAnnotations;

namespace MovieFInderServer.Models
{
    public class SavedMovie
    {
        [Key]
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string Title { get; set; }

        public string ImageUrl { get; set; }
        public ICollection<Genre>? Genres = new List<Genre>();

        public string Owerview { get; set; }

        public DateTime ReleaseDate { get; set; }

        public ICollection<User> LikedByUsers = new List<User>();
    }
}
