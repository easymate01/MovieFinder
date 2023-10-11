import React, {
  useState,
  useMemo,
  useRef,
  useCallback,
  useEffect,
} from "react";
import TinderCard from "react-tinder-card";
import API_BASE_URL from "../config";

const API_KEY = "0ce5ed7e6d3963f8ee3842e031d5ec7f";

function Card({ movieDatas }) {
  const [currentIndex, setCurrentIndex] = useState(movieDatas.length);
  const [lastDirection, setLastDirection] = useState("");
  const [likedMovies, setLikedMovies] = useState([]);
  const [genres, setGenres] = useState([]);
  const [getOverview, setGetOverview] = useState(false);

  useEffect(() => {
    async function fetchData() {
      const response = await fetch(
        `https://api.themoviedb.org/3/genre/movie/list?api_key=${API_KEY}&language=en-US"Ő`
      );
      const newData = await response.json();
      setGenres(newData.genres);
    }

    fetchData();
  }, []);

  const currentIndexRef = useRef(currentIndex);

  const childRefs = useMemo(
    () =>
      Array(movieDatas.length)
        .fill(0)
        .map((i) => React.createRef()),
    []
  );

  const updateCurrentIndex = (val) => {
    setCurrentIndex(val);
    currentIndexRef.current = val;
  };

  // set last direction and decrease current index
  const swiped = (direction, nameToDelete, index) => {
    setLastDirection(direction);
    updateCurrentIndex(index - 1);
    setGetOverview(false);
  };
  const outOfFrame = useCallback(
    (name, idx, movieId, direction, url, overview, genres, release_date) => {
      if (direction === "right") {
        setLikedMovies((prevLikedMovies) => {
          if (prevLikedMovies.some((m) => m.movieId === movieId)) {
            return prevLikedMovies;
          }
          return [
            ...prevLikedMovies,
            { movieId, name, url, overview, genres, release_date },
          ];
        });
      }

      currentIndexRef.current >= idx && childRefs[idx].current.restoreCard();
    },
    [lastDirection, childRefs]
  );

  const saveMovies = (likedMovies) => {
    if (likedMovies.length === 0) {
      console.log("likedMovies is empty");
      return;
    }
    likedMovies.forEach((movie) => {
      const movieId = movie.movieId;
      const title = movie.name;
      const imageUrl = movie.url;
      const overview = movie.overview;
      const releaseDate = movie.release_date;

      // Get genre IDs as an array from the comma-separated string
      const genreIds = movie.genres
        .split(",")
        .map((genreName) => {
          const genre = genres.find((g) => g.name === genreName.trim());
          return genre ? genre.id : null;
        })
        .filter(Boolean);

      const newLikes = {
        movieId,
        title,
        imageUrl,
        genreIds,
        overview,
        releaseDate,
      };
      console.log(genres);

      fetch(`${API_BASE_URL}/SaveMovie/api/savemovie`, {
        method: "POST",
        headers: { "Content-type": "application/json" },
        body: JSON.stringify(newLikes),
      })
        .then((response) => response.json())
        .then((data) => console.log(data))
        .catch((error) => console.log(error));
    });
  };

  //Ez helyett kéne fetchelni databasebe.
  useEffect(() => {
    saveMovies(likedMovies);
  }, [likedMovies]);

  function getGenreNames(genreIds) {
    const genreNames = [];
    genreIds.forEach((id) => {
      const genre = genres.find((genre) => genre.id === id);
      if (genre) {
        genreNames.push(genre.name);
      }
    });
    return genreNames.join(", ");
  }

  const handleEdit = () => {
    console.log("click");
    setGetOverview(!getOverview);
  };

  return (
    <div className="cardContainer" onDoubleClick={handleEdit}>
      {movieDatas && movieDatas.length > 0 ? (
        movieDatas.map((character, index) => (
          <TinderCard
            ref={childRefs[index]}
            className="swipe"
            key={character.title}
            onSwipe={(dir) => swiped(dir, character.title, index)}
            onCardLeftScreen={(dir) =>
              outOfFrame(
                character.title,
                index,
                character.id,
                dir,
                character.poster_path,
                character.overview,
                getGenreNames(character.genre_ids),
                character.release_date
              )
            }
          >
            <div
              style={{
                backgroundImage: `url(https://image.tmdb.org/t/p/w500${character.poster_path})`,
              }}
              className="card"
            >
              <div className="wrapper">
                {getOverview ? (
                  <div className="overview">
                    <div>
                      {character.overview}
                      <span className="genre">
                        {getGenreNames(character.genre_ids)}
                      </span>
                    </div>
                  </div>
                ) : null}
              </div>
              {!getOverview ? (
                <>
                  <h3>{character.title}</h3>
                  <h4>{character.overview}</h4>
                  <p>{getGenreNames(character.genre_ids)}</p>
                </>
              ) : null}
            </div>
          </TinderCard>
        ))
      ) : (
        <div>Loading...</div>
      )}
    </div>
  );
}

export default Card;
