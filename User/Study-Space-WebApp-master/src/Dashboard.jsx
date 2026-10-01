import React, { useEffect, useState } from 'react';
import axios from 'axios';
import './Dashboard.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap-icons/font/bootstrap-icons.css';
import { Link, useNavigate, useLocation } from 'react-router-dom';

const Dashboard = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const currentPath = location.pathname;

  const [user, setUser] = useState(null);
  const [roomsCreated, setRoomsCreated] = useState(0);
  const [roomsJoined, setRoomsJoined] = useState(0);
  const [lastLogin, setLastLogin] = useState('—');

  useEffect(() => {
    const storedUser = JSON.parse(localStorage.getItem('user'));
    if (!storedUser) {
      navigate('/login'); // Redirect if not logged in
      return;
    }

    setUser(storedUser);
    const userId = storedUser.userId;

    axios.get(`https://localhost:7183/api/UsersApi/${userId}`)
      .then((res) => setUser(res.data))
      .catch((err) => console.error('User fetch error:', err));

    axios.get('https://localhost:7183/api/StudyRoomsApi')
      .then((res) => {
        const createdCount = res.data.filter(room => room.createdBy === userId).length;
        setRoomsCreated(createdCount);
      })
      .catch((err) => console.error('Rooms fetch error:', err));

    axios.get('https://localhost:7183/api/RoomMembersApi')
      .then((res) => {
        const joinedCount = res.data.filter(member => member.userId === userId).length;
        setRoomsJoined(joinedCount);
      })
      .catch((err) => console.error('Room members fetch error:', err));

    setLastLogin('2 days ago');
  }, [navigate]);

  const handleLogout = () => {
    localStorage.removeItem('user');
    navigate('/login');
  };

  if (!user) return <div className="p-5">Loading...</div>;

  return (
    <div className="dashboard-container d-flex">
      {/* Sidebar */}
      <aside className="sidebar bg-dark text-white p-3">
        <h4 className="mb-4">My Dashboard</h4>
        <ul className="nav flex-column gap-2">
          <li>
            <Link to="/profile" className={`nav-link text-white ${currentPath === '/profile' ? 'active' : ''}`}>
              <i className="bi bi-person me-2"></i>Profile
            </Link>
          </li>
          <li>
            <Link to="/settings" className={`nav-link text-white ${currentPath === '/settings' ? 'active' : ''}`}>
              <i className="bi bi-gear me-2"></i>Settings
            </Link>
          </li>
          <li>
            <Link to="/roommanagement" className={`nav-link text-white ${currentPath === '/roommanagement' ? 'active' : ''}`}>
              <i className="bi bi-door-open me-2"></i>Rooms
            </Link>
          </li>
          <li>
            <Link to="/resources" className={`nav-link text-white ${currentPath === '/resources' ? 'active' : ''}`}>
              <i className="bi bi-folder2-open me-2"></i>Resources
            </Link>
          </li>
          <li onClick={handleLogout} style={{ cursor: 'pointer' }}>
            <i className="bi bi-box-arrow-right me-2"></i>Logout
          </li>
        </ul>
      </aside>

      {/* Main Content */}
      <main className="main-content flex-grow-1 p-4">
        <div className="pt-4">
          <h2 className="text-primary">Welcome, {user.fullName?.split(' ')[0] || 'User'}!</h2>
          <p className="text-muted">Here's your profile overview and dashboard.</p>
        </div>

        {/* Profile Card */}
        <div className="card profile-card p-4 my-4 shadow-sm">
          <div className="d-flex align-items-center">
            <img
              src={`https://ui-avatars.com/api/?name=${encodeURIComponent(user.fullName || 'User')}&background=2d465e&color=fff`}
              alt="Profile"
              className="profile-image me-4"
            />
            <div>
              <h4>{user.fullName}</h4>
              <p className="text-muted">{user.email}</p>
              <button className="btn btn-outline-primary mt-2">
                <i className="bi bi-pencil me-2"></i>Edit Profile
              </button>
            </div>
          </div>
        </div>

        {/* Stats */}
        <div className="row g-4">
          <div className="col-md-6 col-lg-4">
            <div className="card stat-card bg-primary text-white p-3 shadow-sm">
              <h5><i className="bi bi-door-open me-2"></i>Rooms Created</h5>
              <h3>{roomsCreated}</h3>
            </div>
          </div>
          <div className="col-md-6 col-lg-4">
            <div className="card stat-card bg-success text-white p-3 shadow-sm">
              <h5><i className="bi bi-box-arrow-in-right me-2"></i>Rooms Joined</h5>
              <h3>{roomsJoined}</h3>
            </div>
          </div>
          <div className="col-md-6 col-lg-4">
            <div className="card stat-card bg-warning text-dark p-3 shadow-sm">
              <h5><i className="bi bi-clock-history me-2"></i>Last Login</h5>
              <h3>{lastLogin}</h3>
            </div>
          </div>
        </div>
      </main>
    </div>
  );
};

export default Dashboard;
