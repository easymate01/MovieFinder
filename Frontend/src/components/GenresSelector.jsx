import React, { useState, useEffect, useRef } from "react";
import "./likedmovies.scss";

const GenreSelector = ({ genres, onChange, isLikedGenres }) => {
  const [activeGenreId, setActiveGenreId] = useState(null);
  const [scrollPosition, setScrollPosition] = useState(0);
  const [scrollWidth, setScrollWidth] = useState(0);
  const tabsListRef = useRef(null);

  useEffect(() => {
    setScrollWidth(tabsListRef.current?.scrollWidth || 0);
  }, [genres]);

  const handleGenreClick = (id) => {
    onChange(id);
    setActiveGenreId(id);
  };

  const handleScroll = (event) => {
    const scrollLeft = event.target.scrollLeft || 0;
    setScrollPosition(scrollLeft);
  };

  const scrollLeft = () => {
    tabsListRef.current.scrollLeft -= 200;
  };

  const scrollRight = () => {
    tabsListRef.current.scrollLeft += 200;
  };
  return (
    <div className="scrollable-tabs-container">
      <div
        className={`left-arrow ${scrollPosition > 0 ? "active" : ""}`}
        onClick={scrollLeft}
      >
        <svg
          xmlns="http://www.w3.org/2000/svg"
          fill="none"
          viewBox="0 0 24 24"
          strokeWidth="1.5"
          stroke="currentColor"
          className="w-6 h-6"
        >
          <path
            strokeLinecap="round"
            strokeLinejoin="round"
            d="M15.75 19.5L8.25 12l7.5-7.5"
          />
        </svg>
      </div>

      <ul
        ref={tabsListRef}
        onScroll={handleScroll}
        className="scrollable-tabs-list"
      >
        {Array.isArray(genres) && // Check if genres is an array
          genres.map((genre) => (
            <li
              key={isLikedGenres ? genre + "-" : genre.id || genre}
              className={
                activeGenreId === (isLikedGenres ? genre : genre.id)
                  ? "active"
                  : ""
              }
              onClick={() => handleGenreClick(isLikedGenres ? genre : genre.id)}
            >
              <span>{typeof genre === "object" ? genre.name : genre}</span>
            </li>
          ))}
      </ul>

      <div
        className={`right-arrow ${
          scrollPosition < scrollWidth - tabsListRef.current?.offsetWidth
            ? "active"
            : ""
        }`}
        onClick={scrollRight}
      >
        <svg
          xmlns="http://www.w3.org/2000/svg"
          fill="none"
          viewBox="0 0 24 24"
          strokeWidth="1.5"
          stroke="white"
          className="w-6 h-6"
        >
          <path d="M8.25 4.5l7.5 7.5-7.5 7.5" />
        </svg>
      </div>
    </div>
  );
};

export default GenreSelector;
