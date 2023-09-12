using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MovieFInderServer.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)] public int GenreId { get; set; }
        [JsonIgnore]
        public ICollection<SavedMovie>? Movies = new List<SavedMovie>();
    }
}
