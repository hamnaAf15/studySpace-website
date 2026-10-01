import React from 'react';
import { Link, useLocation, useNavigate } from 'react-router-dom';

const Header = () => {
  const location = useLocation();
  const currentPath = location.pathname;
  const navigate = useNavigate();

  const handleAboutClick = (e) => {
    e.preventDefault();
    if (currentPath === '/home') {
      const aboutSection = document.getElementById('about');
      if (aboutSection) {
        aboutSection.scrollIntoView({ behavior: 'smooth' });
      }
    } else {
      navigate('/home#about');
    }
  };

  return (
    <header id="header" className="header d-flex align-items-center fixed-top">
      <div className="header-container container-fluid container-xl position-relative d-flex align-items-center justify-content-between">
        <a href="index.html" className="logo d-flex align-items-center me-auto me-xl-0">
          <h1 className="sitename">study<span className="accent-text">Space</span></h1>
        </a>

        <nav id="navmenu" className="navmenu">
          <ul>
            <li>
              <Link to="/home" className={currentPath === '/home' ? 'active' : ''}>
                Home
              </Link>
            </li>
            <li>
              <a href="#about" onClick={handleAboutClick}>
                About
              </a>
            </li>
            <li>
              <Link to="/dashboard" className={currentPath === '/dashboard' ? 'active' : ''}>
                Dashboard
              </Link>
            </li>
            {/* ❌ Removed Rooms and Resources */}
          </ul>
          <i className="mobile-nav-toggle d-xl-none bi bi-list"></i>
        </nav>

        <Link to="/signup" className="btn-getstarted">Get Started</Link>
      </div>
    </header>
  );
};

export default Header;
