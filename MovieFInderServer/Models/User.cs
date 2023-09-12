using System.Text.Json.Serialization;

namespace MovieFInderServer.Models
{
    public class User
    {
        public User()
        {
            LikedMovies = new List<SavedMovie>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        [JsonIgnore] public ICollection<SavedMovie> LikedMovies = new List<SavedMovie>();
    }
}
