import React, { useState } from 'react';
import './Signup.css';
import { FaGithub, FaGoogle } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import axios from 'axios';

function Signup() {
  const [formData, setFormData] = useState({
    fullName: '',
    email: '',
    password: '',
    confirmPassword: ''
  });

  const handleSignup = async () => {
  if (formData.password !== formData.confirmPassword) {
    alert("Passwords do not match!");
    return;
  }

  if (!formData.fullName || !formData.email || !formData.password) {
    alert("All fields are required.");
    return;
  }

  try {
    const response = await axios.post('https://localhost:7183/api/UsersApi', {
      fullName: formData.fullName,
      email: formData.email,
      password: formData.password
    });

    console.log('Signup response:', response.data);
    alert('Signup successful!');
    handleClear();
  } catch (error) {
    console.error(error);
    alert('Signup failed: ' + (error.response?.data?.message || error.message));
  }
};
  const handleClear = () => {
    setFormData({
      fullName: '',
      email: '',
      password: '',
      confirmPassword: ''
    });
  };

  return (
    <div className="signup-wrapper">
      <div className="form-container">
        <h1 className="register">Sign Up</h1>

      <label className="form-row">
  <span>Full Name:</span>
  <input
    type="text"
    placeholder="e.g. Ali Raza"
    value={formData.fullName}
    onChange={(e) => setFormData({ ...formData, fullName: e.target.value })}
  />
</label>


        <label className="form-row">
          <span>Email:</span>
          <input
            type="email"
            placeholder="janedoe@example.com"
            value={formData.email}
            onChange={(e) => setFormData({ ...formData, email: e.target.value })}
          />
        </label>

        <label className="form-row">
          <span>Password:</span>
          <input
            type="password"
            placeholder="***************"
            value={formData.password}
            onChange={(e) => setFormData({ ...formData, password: e.target.value })}
          />
        </label>

        <label className="form-row">
          <span>Confirm Password:</span>
          <input
            type="password"
            placeholder="***************"
            value={formData.confirmPassword}
            onChange={(e) => setFormData({ ...formData, confirmPassword: e.target.value })}
          />
        </label>

        <label className="form-row">
          <input type="checkbox" />
          <span style={{ marginLeft: '10px' }}>
            I agree to the <span className="underline">privacy policy</span>
          </span>
        </label>

        <div className="form-buttons">
          <button className="btn btn-signup" onClick={handleSignup}>Create account</button>
          <button className="btn btn-clear" onClick={handleClear}>Clear</button>
        </div>

        <hr className="my-8" />

        <button className="btn btn-login btn-social">
          <FaGithub style={{ marginRight: '8px' }} />
          Signup with Github
        </button>
        <button className="btn btn-login btn-social" style={{ marginTop: '10px' }}>
          <FaGoogle style={{ marginRight: '8px' }} />
          Signup with Google
        </button>

        <div className="login-section">
          <p className="login-text">Already have an account?</p>
          <Link to="/login" className="btn btn-login">Login</Link>
        </div>
      </div>
    </div>
  );
}

export default Signup;
