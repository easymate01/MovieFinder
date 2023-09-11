using System.ComponentModel.DataAnnotations;

namespace MovieFInderServer.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        public int GenreId { get; set; }
        public int MovieId { get; set; }
        public ICollection<SavedMovie>? Movies { get; set; }
    }
}
