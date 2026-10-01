import './App.css';
import {  HashRouter as Router, Routes, Route, useLocation } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';

import Home from './Home';
import Signup from './Signup';
import Login from './Login';
import Forgot from './Forgot';
import Dashboard from './Dashboard';
import Header from './Header';
import RoomManagement from './RoomManagement';
import StudyRoom from './StudyRoom';
import Resources from './Resources';
import PrivateRoute from './PrivateRoute.jsx';

function AppWrapper() {
  const location = useLocation();
  const showHeader = ['/home', '/', '/login'].includes(location.pathname);

  return (
    <>
      {showHeader && <Header />}
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/home" element={<Home />} />
        <Route path="/signup" element={<Signup />} />
        <Route path="/login" element={<Login />} />
        <Route path="/forgot" element={<Forgot />} />

        {/* ✅ Protected Routes */}
        <Route path="/dashboard" element={<PrivateRoute><Dashboard /></PrivateRoute>} />
        <Route path="/resources" element={<PrivateRoute><Resources /></PrivateRoute>} />
        <Route path="/roommanagement" element={<PrivateRoute><RoomManagement /></PrivateRoute>} />
        <Route path="/study-room/:id" element={<PrivateRoute><StudyRoom /></PrivateRoute>} />
      </Routes>
    </>
  );
}

function App() {
  return (
    <Router>
      <AppWrapper />
    </Router>
  );
}

export default App;
