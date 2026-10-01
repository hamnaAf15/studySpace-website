import React, { useState } from 'react';
import './Login.css';
import { FaGithub, FaGoogle } from 'react-icons/fa';
import { Link, useNavigate } from 'react-router-dom';
import axios from 'axios';

const Login = () => {
  const navigate = useNavigate();

  const [formData, setFormData] = useState({
    email: '',
    password: ''
  });

  const handleLogin = async () => {
    try {
      const response = await axios.post('https://localhost:7183/api/UsersApi/login', formData);
      localStorage.setItem('user', JSON.stringify(response.data));
      navigate('/dashboard');
    } catch (error) {
      console.error(error);
      alert('Login failed: ' + (error.response?.data?.message || error.message));
    }
  };

  const handleClear = () => {
    setFormData({ email: '', password: '' });
  };

  return (
    <div className="login-wrapper">
      <div className="loginform-container">
        <h1 className="login">Login</h1>

        <div className="loginform-row">
          <label>Email</label>
          <input
            type="email"
            placeholder="jane@example.com"
            value={formData.email}
            onChange={(e) => setFormData({ ...formData, email: e.target.value })}
          />
        </div>

        <div className="loginform-row">
          <label>Password</label>
          <input
            type="password"
            placeholder="********"
            value={formData.password}
            onChange={(e) => setFormData({ ...formData, password: e.target.value })}
          />
        </div>

        <div className="loginform-buttons">
          <button className="btn btn-signup" onClick={handleLogin}>Log in</button>
          <button className="btn btn-clear" onClick={handleClear}>Clear</button>
        </div>

        {/* ✅ Forgot Password Button */}
        <div className="text-center mt-3 mb-3">
          <Link to="/forgot" className="btn btn-outline-secondary btn-sm">
            Forgot your password?
          </Link>
        </div>

        <hr className="divider" />

        <button className="btn loginbtn loginbtn-social">
          <FaGithub style={{ marginRight: '8px' }} />
          Login with Github
        </button>

        <button className="btn loginbtn loginbtn-social">
          <FaGoogle style={{ marginRight: '8px' }} />
          Login with Google
        </button>

        <div className="signup-section mt-4">
          <p className="signup-text">
            Don't have an account?{' '}
            <Link to="/signup" className="btn loginbtn">Sign Up</Link>
          </p>
        </div>
      </div>
    </div>
  );
};

export default Login;
