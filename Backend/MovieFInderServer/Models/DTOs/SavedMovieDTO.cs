namespace MovieFInderServer.Models.DTOs
{
    public class SavedMovieDTO
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public List<int> GenreIds { get; set; }

        public string Overview { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
