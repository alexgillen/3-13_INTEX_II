import React, { useState } from 'react';
import axios from 'axios';

// Changed to explicitly use HTTP to avoid certificate issues
const API_BASE_URL = 'http://localhost:5000/api';

// Password validation helper
const isValidPassword = (password: string): boolean => {
  return password.length >= 10;
};

const getPasswordErrorMessage = (): string => {
  return "Password must be at least 10 characters long";
};

const LoginPage = () => {
  const [isLogin, setIsLogin] = useState(true);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  
  // Login form state
  const [loginEmail, setLoginEmail] = useState('');
  const [loginPassword, setLoginPassword] = useState('');
  
  // Register form state
  const [registerEmail, setRegisterEmail] = useState('');
  const [registerPassword, setRegisterPassword] = useState('');
  const [registerFirstName, setRegisterFirstName] = useState('');
  const [registerLastName, setRegisterLastName] = useState('');
  const [passwordError, setPasswordError] = useState('');

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setPasswordError('');
    
    // Validate password before sending request
    if (!isValidPassword(loginPassword)) {
      setPasswordError(getPasswordErrorMessage());
      return;
    }
    
    setLoading(true);
    
    try {
      console.log('LoginPage: Sending login request to', `${API_BASE_URL}/auth/login`);
      const response = await axios.post(`${API_BASE_URL}/auth/login`, {
        email: loginEmail,
        password: loginPassword
      });
      
      console.log('LoginPage: Login response received', response.status, response.data);
      
      // Save token and user data to localStorage
      localStorage.setItem('token', response.data.token);
      localStorage.setItem('userData', JSON.stringify({
        userId: response.data.userId,
        email: response.data.email,
        firstName: response.data.firstName,
        lastName: response.data.lastName,
        role: response.data.role
      }));
      
      console.log('LoginPage: Saved token and userData to localStorage');
      
      // Redirect based on role using direct window.location for more reliable navigation
      if (response.data.role === 'Admin') {
        console.log('LoginPage: Redirecting admin to /admin');
        window.location.href = '/admin';
      } else {
        console.log('LoginPage: Redirecting user to /profile');
        window.location.href = '/profile';
      }
    } catch (err: any) {
      console.error('LoginPage: Login error', err);
      setError(err.response?.data?.message || 'Failed to login. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  const handleRegister = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setPasswordError('');
    
    // Validate password before sending request
    if (!isValidPassword(registerPassword)) {
      setPasswordError(getPasswordErrorMessage());
      return;
    }
    
    setLoading(true);
    
    try {
      await axios.post(`${API_BASE_URL}/auth/register`, {
        email: registerEmail,
        password: registerPassword,
        firstName: registerFirstName,
        lastName: registerLastName
      });
      
      // Show success message and switch to login form
      alert('Registration successful! Please check your email for verification and login.');
      setIsLogin(true);
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to register. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  const formContainerStyle = {
    minHeight: '100vh',
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: '#f0f2f5'
  };

  const formStyle = {
    backgroundColor: 'white',
    padding: '2rem',
    borderRadius: '8px',
    boxShadow: '0 2px 10px rgba(0, 0, 0, 0.1)',
    width: '100%',
    maxWidth: '400px'
  };

  const headingStyle = {
    fontSize: '1.5rem',
    fontWeight: 'bold',
    marginBottom: '1.5rem',
    textAlign: 'center' as const
  };

  const errorStyle = {
    backgroundColor: '#ffebee',
    border: '1px solid #f44336',
    color: '#d32f2f',
    padding: '0.75rem 1rem',
    borderRadius: '4px',
    marginBottom: '1rem'
  };

  const inputGroupStyle = {
    marginBottom: '1rem'
  };

  const labelStyle = {
    display: 'block',
    fontSize: '0.875rem',
    fontWeight: 'bold',
    marginBottom: '0.5rem'
  };

  const inputStyle = {
    width: '100%',
    padding: '0.5rem 0.75rem',
    borderRadius: '4px',
    border: '1px solid #ddd',
    fontSize: '1rem'
  };

  const buttonContainerStyle = {
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: '1rem'
  };

  const primaryButtonStyle = {
    backgroundColor: '#3b82f6',
    color: 'white',
    fontWeight: 'bold',
    padding: '0.5rem 1rem',
    borderRadius: '4px',
    border: 'none',
    cursor: 'pointer'
  };

  const secondaryButtonStyle = {
    background: 'none',
    border: 'none',
    color: '#3b82f6',
    fontWeight: 'bold',
    fontSize: '0.875rem',
    cursor: 'pointer'
  };

  const testApiConnection = async () => {
    try {
      alert('Testing API connection to: ' + API_BASE_URL);
      const response = await fetch(API_BASE_URL + '/health', { 
        method: 'GET',
        mode: 'cors'
      });
      
      if (response.ok) {
        const data = await response.text();
        alert(`API is reachable! Response: ${data}`);
      } else {
        alert(`API returned status: ${response.status} ${response.statusText}`);
      }
    } catch (error: unknown) {
      alert(`API connection error: ${error instanceof Error ? error.message : String(error)}`);
      console.error('API test error:', error);
    }
  };

  return (
    <div style={formContainerStyle}>
      <div style={formStyle}>
        <h1 style={headingStyle}>
          {isLogin ? 'Login to CineNiche' : 'Create an Account'}
        </h1>
        
        {error && (
          <div style={errorStyle}>
            {error}
          </div>
        )}
        
        {isLogin ? (
          // Login Form
          <form onSubmit={handleLogin}>
            <div style={inputGroupStyle}>
              <label style={labelStyle} htmlFor="email">
                Email
              </label>
              <input
                style={inputStyle}
                id="email"
                type="email"
                placeholder="Email"
                value={loginEmail}
                onChange={(e) => setLoginEmail(e.target.value)}
                required
              />
            </div>
            
            <div style={inputGroupStyle}>
              <label style={labelStyle} htmlFor="password">
                Password
              </label>
              <input
                style={inputStyle}
                id="password"
                type="password"
                placeholder="Password (min. 10 characters)"
                value={loginPassword}
                onChange={(e) => setLoginPassword(e.target.value)}
                required
              />
              {passwordError && (
                <div style={{ color: '#d32f2f', fontSize: '0.8rem', marginTop: '0.25rem' }}>
                  {passwordError}
                </div>
              )}
            </div>
            
            <div style={buttonContainerStyle}>
              <button
                style={primaryButtonStyle}
                type="submit"
                disabled={loading}
              >
                {loading ? 'Logging in...' : 'Sign In'}
              </button>
              <button
                style={secondaryButtonStyle}
                type="button"
                onClick={() => setIsLogin(false)}
              >
                Create an account
              </button>
            </div>
          </form>
        ) : (
          // Register Form
          <form onSubmit={handleRegister}>
            <div style={inputGroupStyle}>
              <label style={labelStyle} htmlFor="register-email">
                Email
              </label>
              <input
                style={inputStyle}
                id="register-email"
                type="email"
                placeholder="Email"
                value={registerEmail}
                onChange={(e) => setRegisterEmail(e.target.value)}
                required
              />
            </div>
            
            <div style={inputGroupStyle}>
              <label style={labelStyle} htmlFor="register-first-name">
                First Name
              </label>
              <input
                style={inputStyle}
                id="register-first-name"
                type="text"
                placeholder="First Name"
                value={registerFirstName}
                onChange={(e) => setRegisterFirstName(e.target.value)}
                required
              />
            </div>
            
            <div style={inputGroupStyle}>
              <label style={labelStyle} htmlFor="register-last-name">
                Last Name
              </label>
              <input
                style={inputStyle}
                id="register-last-name"
                type="text"
                placeholder="Last Name"
                value={registerLastName}
                onChange={(e) => setRegisterLastName(e.target.value)}
                required
              />
            </div>
            
            <div style={inputGroupStyle}>
              <label style={labelStyle} htmlFor="register-password">
                Password
              </label>
              <input
                style={inputStyle}
                id="register-password"
                type="password"
                placeholder="Password (min. 10 characters)"
                value={registerPassword}
                onChange={(e) => setRegisterPassword(e.target.value)}
                required
              />
              {passwordError && (
                <div style={{ color: '#d32f2f', fontSize: '0.8rem', marginTop: '0.25rem' }}>
                  {passwordError}
                </div>
              )}
            </div>
            
            <div style={buttonContainerStyle}>
              <button
                style={primaryButtonStyle}
                type="submit"
                disabled={loading}
              >
                {loading ? 'Registering...' : 'Sign Up'}
              </button>
              <button
                style={secondaryButtonStyle}
                type="button"
                onClick={() => setIsLogin(true)}
              >
                Already have an account?
              </button>
            </div>
          </form>
        )}
        
        {/* Debug Section */}
        <div style={{ 
          marginTop: '2rem',
          padding: '1rem',
          borderTop: '1px solid #eee',
          backgroundColor: '#f9fafb'
        }}>
          <h3 style={{ fontSize: '1rem', marginBottom: '0.5rem' }}>Troubleshooting</h3>
          <div style={{ display: 'flex', flexDirection: 'column', gap: '0.5rem' }}>
            <button 
              style={{ ...primaryButtonStyle, fontSize: '0.8rem', backgroundColor: '#4b5563' }}
              onClick={() => window.location.href = '/profile'}
            >
              Go directly to Profile
            </button>
            <button 
              style={{ ...primaryButtonStyle, fontSize: '0.8rem', backgroundColor: '#4b5563' }}
              onClick={() => window.location.href = '/home'}
            >
              Go directly to Home
            </button>
            <button 
              style={{ ...primaryButtonStyle, fontSize: '0.8rem', backgroundColor: '#4b5563' }}
              onClick={() => {
                localStorage.clear();
                alert('LocalStorage cleared');
              }}
            >
              Clear LocalStorage
            </button>
            <button 
              style={{ ...primaryButtonStyle, fontSize: '0.8rem', backgroundColor: '#dc2626' }}
              onClick={() => {
                const testUserData = {
                  userId: "test-user-id",
                  email: "test@example.com",
                  firstName: "Test",
                  lastName: "User",
                  role: "User"
                };
                localStorage.setItem('token', 'test-token');
                localStorage.setItem('userData', JSON.stringify(testUserData));
                alert('Set test token and userData in localStorage');
              }}
            >
              Set Test User Data
            </button>
            <button 
              style={{ ...primaryButtonStyle, fontSize: '0.8rem', backgroundColor: '#059669' }}
              onClick={testApiConnection}
            >
              Test API Connection
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default LoginPage; 