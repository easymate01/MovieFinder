using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieFInderServer.Models
{
    public class Genre
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int GenreId { get; set; }
        public int MovieId { get; set; }
        public ICollection<SavedMovie>? Movies { get; set; }
    }
}
