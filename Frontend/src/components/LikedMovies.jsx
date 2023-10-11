import React, { useState } from "react";
import "./likedmovies.scss";
import GenreSelector from "./GenresSelector";
import API_BASE_URL from "../config";

function Likes({ movieDatas }) {
  const [selectedGenre, setSelectedGenre] = useState("All");

  async function handleDeleteFunction(id) {
    fetch(`${API_BASE_URL}/SaveMovie/api/delete/${id}`, {
      method: "DELETE",
      headers: { "Content-type": "application/json" },
    })
      .then((response) => response.json())
      .then((data) => console.log(data))
      .catch((error) => console.log(error));
  }

  function sortMovies(movies) {
    if (selectedGenre === "All") {
      return movies;
    }

    const filteredMovies = movies.filter((movie) => {
      return movie.genres.includes(selectedGenre);
    });

    console.log(filteredMovies); // Log the filtered array
    return filteredMovies;
  }

  function uniqueGenres(movies) {
    const allGenres = movies
      .flatMap((movie) => (movie.genres ? movie.genres.split(", ") : []))
      .filter(Boolean);

    const genresSet = new Set(allGenres);
    return ["All", ...Array.from(genresSet)];
  }
  const sortedMovies = sortMovies([...movieDatas]);
  return (
    <div>
      {movieDatas.length != 0 && (
        <GenreSelector
          genres={uniqueGenres(sortedMovies)}
          isLikedGenres={true}
          onChange={setSelectedGenre}
        />
      )}

      {sortedMovies.map((movie, index) => (
        <div key={movie.movieId} className="movie_card" id="bright">
          <div className="info_section">
            <div className="movie_header">
              <img
                className="locandina"
                src={`https://image.tmdb.org/t/p/w500${movie.imageUrl}`}
              />
              <h1>{movie.title}</h1>
            </div>
            <div className="movie_desc">
              <p className="text">{movie.owerview}</p>
            </div>
            <div>
              <a
                onClick={() => handleDeleteFunction(movie.id)}
                className="delete"
              >
                DELETE
              </a>
            </div>
          </div>
          <div
            className="blur_back bright_back"
            style={{
              backgroundImage: `url(https://image.tmdb.org/t/p/w500${movie.imageUrl})`,
            }}
          ></div>
        </div>
      ))}

      {movieDatas.length == 0 && (
        <div className="noMoviesMessage">No movies liked yet</div>
      )}
    </div>
  );
}

export default Likes;
