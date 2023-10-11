import "./App.css";
import Card from "./components/Card";
import Likes from "./components/LikedMovies";
import { useState, useEffect } from "react";
import GenreSelector from "./components/GenresSelector";
import Navbar from "./components/Navbar";

import API_BASE_URL from "./config";
const API_KEY = "0ce5ed7e6d3963f8ee3842e031d5ec7f";

function App() {
  const [swiper, setSwiper] = useState(true);
  const [isActive2, setIsActive2] = useState(false);
  const [movieDatas, setMovieDatas] = useState([]);
  const [savedMovies, setSavedMovies] = useState([]);
  const [selectedGenre, setSelectedGenre] = useState("");
  const [genres, setGenres] = useState([]);

  // Fetch Genres
  async function fetchGenres() {
    const response = await fetch(
      `https://api.themoviedb.org/3/genre/movie/list?api_key=${API_KEY}&language=en-US`
    );
    const data = await response.json();
    setGenres([...data.genres]);
  }

  useEffect(() => {
    fetchGenres();
  }, []);

  //------FETCH THE DATA FROM -----
  useEffect(() => {
    async function fetchData() {
      const genreUrl = selectedGenre ? `&with_genres=${selectedGenre}` : "";

      const response = await fetch(
        `https://api.themoviedb.org/3/discover/movie?api_key=${API_KEY}&language=en-US&sort_by=popularity.desc&include_adult=false&include_video=false&page=1${genreUrl}`
      );
      const data = await response.json();
      setMovieDatas(data.results);
    }

    fetchData();
  }, [selectedGenre]);

  useEffect(() => {
    async function fetchLikes() {
      const response = await fetch(`${API_BASE_URL}/SaveMovie/api/movies`);
      const newData = await response.json();
      setSavedMovies(newData);
    }

    fetchLikes();
  }, [savedMovies]);

  const toggleActive1 = () => {
    setSwiper(!swiper);
    setIsActive2(false);
  };

  const toggleActive2 = () => {
    setSwiper(false);
    setIsActive2(!isActive2);
  };

  const handleGenreChange = (selectedGenreId) => {
    setSelectedGenre(selectedGenreId);
  };

  return (
    <div className="main-component">
      {swiper && (
        <div className="selector">
          <GenreSelector
            genres={genres}
            onChange={handleGenreChange}
            isLikedGenres={false}
          />
        </div>
      )}
      <div className="card-section">
        {swiper && <Card movieDatas={movieDatas} />}
      </div>
      <div className="liked-movies-section">
        {isActive2 && <Likes movieDatas={savedMovies} />}
      </div>

      <section className="navbar-section">
        <Navbar
          swiper={swiper}
          toggleActive1={toggleActive1}
          toggleActive2={toggleActive2}
          isActive2={isActive2}
          savedMovies={savedMovies}
        />
      </section>
    </div>
  );
}

export default App;
