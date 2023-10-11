const Navbar = ({
  swiper,
  toggleActive1,
  toggleActive2,
  isActive2,
  savedMovies,
}) => {
  return (
    <div className="navbar">
      <div
        className={`nav ${swiper ? "active-nav" : ""}`}
        onClick={toggleActive1}
      >
        <svg
          xmlns="http://www.w3.org/2000/svg"
          width="28"
          height="28"
          fill="currentColor"
          className="bi bi-collection-fill"
          viewBox="0 0 16 16"
        >
          <path d="M0 13a1.5 1.5 0 0 0 1.5 1.5h13A1.5 1.5 0 0 0 16 13V6a1.5 1.5 0 0 0-1.5-1.5h-13A1.5 1.5 0 0 0 0 6v7zM2 3a.5.5 0 0 0 .5.5h11a.5.5 0 0 0 0-1h-11A.5.5 0 0 0 2 3zm2-2a.5.5 0 0 0 .5.5h7a.5.5 0 0 0 0-1h-7A.5.5 0 0 0 4 1z" />
        </svg>
      </div>

      <div
        className={`nav ${isActive2 ? "active-nav" : ""}`}
        onClick={toggleActive2}
      >
        <svg
          xmlns="http://www.w3.org/2000/svg"
          width="35"
          height="35"
          fill="black"
          className="bi bi-heart-fill"
          viewBox="0 0 16 16"
        >
          <path
            fillRule="evenodd"
            d="M8 1.314C12.438-3.248 23.534 4.735 8 15-7.534 4.736 3.562-3.248 8 1.314z"
          />
        </svg>{" "}
        {savedMovies.length}
      </div>
    </div>
  );
};

export default Navbar;
