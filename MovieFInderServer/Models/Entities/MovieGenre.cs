namespace MovieFInderServer.Models.Entities
{
    public class MovieGenre
    {
        public int MovieId { get; set; }
        public SavedMovie Movie { get; set; }

        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
