import React, { useState } from 'react';
import './Login.css'; // Reuse login styles
import { Link } from 'react-router-dom';
import axios from 'axios';

const Forgot = () => {
  const [email, setEmail] = useState('');
  const [status, setStatus] = useState('');

  const handleRecover = async () => {
    if (!email.trim()) {
      setStatus('Please enter your email.');
      return;
    }

    try {
      await axios.post(`https://localhost:7183/api/UsersApi/forgot-password?email=${email}`);
      setStatus('Password reset link has been sent to your email (if it exists).');
    } catch (error) {
      console.error('Error during forgot password:', error);
      setStatus('Error sending recovery email.');
    }
  };

  const handleClear = () => {
    setEmail('');
    setStatus('');
  };

  return (
    <div className="login-wrapper forgot-wrapper">
      <div className="loginform-container">
        <h1 className="login">Forgot Password</h1>

        <div className="loginform-row">
          <label>Email</label>
          <input
            type="email"
            placeholder="Enter your email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />
        </div>

        <div className="loginform-buttons">
          <button className="btn btn-signup" onClick={handleRecover}>Recover Password</button>
          <button className="btn btn-clear" onClick={handleClear}>Clear</button>
        </div>

        {status && <p className="text-center mt-3 text-info">{status}</p>}

        <div className="signup-section">
          <p className="signup-text">
            <Link to="/login" className="btn loginbtn">Back to Login</Link>
          </p>
          <p className="signup-text">
            <Link to="/signup" className="btn loginbtn">Sign Up</Link>
          </p>
        </div>
      </div>
    </div>
  );
};

export default Forgot;
