import React, { useEffect } from 'react';
import './Home.css';

import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap-icons/font/bootstrap-icons.css';
import 'swiper/css';
import 'swiper/css/navigation';
import 'swiper/css/pagination';

import Swiper from 'swiper';
import AOS from 'aos';
import GLightbox from 'glightbox';
import PureCounter from '@srexi/purecounterjs';
import { Link } from 'react-router-dom';



const Home = () => {

    useEffect(() => {
    // Handle scroll class toggle
    const toggleScrolled = () => {
      const body = document.querySelector('body');
      const header = document.querySelector('#header');
      if (!header) return;
      if (
        !header.classList.contains('scroll-up-sticky') &&
        !header.classList.contains('sticky-top') &&
        !header.classList.contains('fixed-top')
      ) return;
      window.scrollY > 100 ? body.classList.add('scrolled') : body.classList.remove('scrolled');
    };

    const toggleScrollTop = () => {
      const scrollTop = document.querySelector('.scroll-top');
      if (scrollTop) {
        window.scrollY > 100
          ? scrollTop.classList.add('active')
          : scrollTop.classList.remove('active');
      }
    };

    const mobileNavToggleBtn = document.querySelector('.mobile-nav-toggle');

    const mobileNavToggle = () => {
      document.body.classList.toggle('mobile-nav-active');
      mobileNavToggleBtn.classList.toggle('bi-list');
      mobileNavToggleBtn.classList.toggle('bi-x');
    };

    if (mobileNavToggleBtn) {
      mobileNavToggleBtn.addEventListener('click', mobileNavToggle);
    }

    // Close mobile nav on nav link click
    document.querySelectorAll('#navmenu a').forEach(link => {
      link.addEventListener('click', () => {
        if (document.body.classList.contains('mobile-nav-active')) {
          mobileNavToggle();
        }
      });
    });

    // Toggle dropdowns
    document.querySelectorAll('.navmenu .toggle-dropdown').forEach(dropdown => {
      dropdown.addEventListener('click', e => {
        e.preventDefault();
        dropdown.parentNode.classList.toggle('active');
        dropdown.parentNode.nextElementSibling?.classList.toggle('dropdown-active');
        e.stopImmediatePropagation();
      });
    });

    // FAQ toggle
    document.querySelectorAll('.faq-item h3, .faq-item .faq-toggle').forEach(el => {
      el.addEventListener('click', () => {
        el.parentNode.classList.toggle('faq-active');
      });
    });

    // ScrollSpy
    const navmenulinks = document.querySelectorAll('.navmenu a');
    const navmenuScrollspy = () => {
      const position = window.scrollY + 200;
      navmenulinks.forEach(link => {
        if (!link.hash) return;
        const section = document.querySelector(link.hash);
        if (!section) return;
        if (position >= section.offsetTop && position <= section.offsetTop + section.offsetHeight) {
          document.querySelectorAll('.navmenu a.active').forEach(active => active.classList.remove('active'));
          link.classList.add('active');
        } else {
          link.classList.remove('active');
        }
      });
    };

    // Scroll-to-hash fix on page load
    if (window.location.hash && document.querySelector(window.location.hash)) {
      setTimeout(() => {
        const section = document.querySelector(window.location.hash);
        const scrollMarginTop = getComputedStyle(section).scrollMarginTop;
        window.scrollTo({
          top: section.offsetTop - parseInt(scrollMarginTop),
          behavior: 'smooth',
        });
      }, 100);
    }

    // Swiper init
    const initSwiper = () => {
      document.querySelectorAll('.init-swiper').forEach(el => {
        const configElement = el.querySelector('.swiper-config');
        if (configElement) {
          const config = JSON.parse(configElement.innerHTML.trim());
          new Swiper(el, config);
        }
      });
    };

    // Init external libs
    AOS.init({
      duration: 600,
      easing: 'ease-in-out',
      once: true,
      mirror: false,
    });

    GLightbox({ selector: '.glightbox' });
    new PureCounter();
    initSwiper();

    // Listeners
    window.addEventListener('scroll', toggleScrolled);
    window.addEventListener('scroll', toggleScrollTop);
    window.addEventListener('scroll', navmenuScrollspy);
    window.addEventListener('load', () => {
      toggleScrolled();
      toggleScrollTop();
      navmenuScrollspy();
    });

    return () => {
      window.removeEventListener('scroll', toggleScrolled);
      window.removeEventListener('scroll', toggleScrollTop);
      window.removeEventListener('scroll', navmenuScrollspy);
    };
  }, []);

  return (
    <div className="index-page" style={{
    minHeight: '100vh',
    width: '210vh',
    display: 'flex',
    flexDirection: 'column',
    margin: 0,
    padding: 0,
  }}>
      <main className="main" style={{
    flexGrow: 1,
    display: 'flex',
    flexDirection: 'column',
    width: '100%',
  }}>
        {/* Hero Section */}
        <section id="hero" className="hero section" style={{
    flex: 1,
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
    minHeight: '100vh',
  }}>
          <div className="container" data-aos="fade-up" data-aos-delay="100">
            <div className="row align-items-center">
              <div className="col-lg-6">
                <div className="hero-content" data-aos="fade-up" data-aos-delay="200">
                  <div className="company-badge mb-4">
                    <i className="bi bi-gear-fill me-2"></i>
                    Empowering Your Academic Journey
                  </div>

                  <h1 className="mb-4">
                    Connect, Learn <br />
                    <span className="accent-text">Succeed</span>
                  </h1>

                  <p className="mb-4 mb-md-5">
                    Collaborate in real-time, share notes, chat live, and stream lectures — all within one seamless virtual study space.
                  </p>

                  <div className="hero-buttons">
                     <Link to="/signup" className="btn btn-primary me-0 me-sm-2 mx-1">Get Started</Link>
                   
                  </div>
                </div>
              </div>

              <div className="col-lg-6">
                <div className="hero-image" data-aos="zoom-out" data-aos-delay="300">
                  <img src="src/assets/homepage.png" alt="Loading" className="img-fluid" />
                  
                </div>
              </div>
            </div>

            <div className="row stats-row gy-4 mt-5" data-aos="fade-up" data-aos-delay="500">
              {[
                { icon: "people", title: "Collaborative Rooms", subtitle: "Create or join study rooms for focused sessions" },
                { icon: "chat-dots", title: "Live Chat", subtitle: "Discuss topics and get instant help" },
                { icon: "camera-video", title: "Lecture Streaming", subtitle: "Watch or host lectures together" },
                { icon: "file-earmark-text", title: "Notes & resources", subtitle: "Share PDFs, notes, and to-do tasks" },
              ].map((stat, i) => (
                <div key={i} className="col-lg-3 col-md-6">
                  <div className="stat-item">
                    <div className="stat-icon"><i className={`bi bi-${stat.icon}`}></i></div>
                    <div className="stat-content">
                      <h4>{stat.title}</h4>
                      <p className="mb-0">{stat.subtitle}</p>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          </div>
        </section>

        {/* About Section */}
        <section id="about" className="about section">
          <div className="container" data-aos="fade-up" data-aos-delay="100">
            <div className="row gy-4 align-items-center justify-content-between">
              <div className="col-xl-5" data-aos="fade-up" data-aos-delay="200">
                <span className="about-meta">ABOUT US</span>
                <h2 className="about-title">Empowering Students Through Collaboration</h2>
                <p className="about-description">
                 Our mission is to revolutionize the way students connect and learn by providing a seamless collaborative study environment with real-time chat and shared resources.
                </p>

                <div className="row feature-list-wrapper">
                  {[
                    ["Create or join focused study rooms", "Engage in real-time group discussions", "Share notes and study materials easily"],
                    ["Boost productivity with timer tools", "Interact with a supportive community", "Accessible on desktop and mobile"]
                  ].map((group, i) => (
                    <div key={i} className="col-md-6">
                      <ul className="feature-list">
                        {group.map((item, j) => (
                          <li key={j}><i className="bi bi-check-circle-fill"></i> {item}</li>
                        ))}
                      </ul>
                    </div>
                  ))}
                </div>

                <div className="info-wrapper">
                  <div className="row gy-4">
                    <div className="col-lg-5">
                      
                    </div>
                    
                  </div>
                </div>
              </div>

              <div className="col-xl-6" data-aos="fade-up" data-aos-delay="300">
                <div className="image-wrapper">
                  <div className="images position-relative" data-aos="zoom-out" data-aos-delay="400">
                    <img src="src/assets/illustration-1.webp" alt="Business Meeting" className="img-fluid main-image rounded-4" />
                    <img src="src/assets/about.png" alt="Team Discussion" className="img-fluid small-image rounded-4" />
                  </div>
                  <div className="experience-badge floating">
                    <h3>10+ <span>Features</span></h3>
                    <p>Built for modern, connected learning</p>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </section>
      </main>

      <footer id="footer" className="footer">
      <div className="container footer-top">
        <div className="row gy-4">
          <div className="col-lg-4 col-md-6 footer-about">
            <a href="index.html" className="logo d-flex align-items-center">
              <span className="sitename">studySpace</span>
            </a>
            <div className="footer-contact pt-3">
              
              <p>
                <strong>Email:</strong> <span>info@email.com</span>
              </p>
            </div>
            <div className="social-links d-flex mt-4">
              <a href="#"><i className="bi bi-twitter-x"></i></a>
              <a href="#"><i className="bi bi-facebook"></i></a>
              <a href="#"><i className="bi bi-instagram"></i></a>
              <a href="#"><i className="bi bi-linkedin"></i></a>
            </div>
          </div>

          <div className="col-lg-2 col-md-3 footer-links">
            <h4>Useful Links</h4>
            <ul>
              <li><a href="#">Home</a></li>
              <li><a href="#">About us</a></li>
              <li><a href="#">Dashboard</a></li>
              <li><a href="#">Join a Session</a></li>
              <li><a href="#">Privacy policy</a></li>
            </ul>
          </div>

          <div className="col-lg-2 col-md-3 footer-links">
            <h4>Our Services</h4>
            <ul>
              <li><a href="#">Real Time Chat</a></li>
              <li><a href="#">Study groups</a></li>
              <li><a href="#">Progress Tracking</a></li>
              <li><a href="#">Collaborative Notes</a></li>
              <li><a href="#">Video Streaming</a></li>
            </ul>
          </div>

          <div className="col-lg-2 col-md-3 footer-links">
            <h4>Resources</h4>
            <ul>
              <li><a href="#">Help Center</a></li>
              <li><a href="#">User Guides</a></li>
              <li><a href="#">FAQs</a></li>
              <li><a href="#">Knowledge Base</a></li>
              <li><a href="#">Tutorials</a></li>
            </ul>
          </div>

          <div className="col-lg-2 col-md-3 footer-links">
            <h4>Explore</h4>
            <ul>
              <li><a href="#">Blog</a></li>
              <li><a href="#">Events</a></li>
              <li><a href="#">Careers</a></li>
              <li><a href="#">Partners</a></li>
              <li><a href="#">Feedback</a></li>
            </ul>
          </div>
        </div>
      </div>

      <div className="container copyright text-center mt-4">
        <p>© <span>Copyright</span> <strong className="px-1 sitename">studySpace</strong> <span>All Rights Reserved</span></p>
        <div className="credits">
          Designed by <a href="#">AMMH</a> 
        </div>
      </div>
    </footer>
    </div>
  );
   
 
};

export default Home;
