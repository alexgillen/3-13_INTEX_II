import { useEffect, useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import axios from 'axios';

// Changed to explicitly use HTTP to avoid certificate issues
const API_BASE_URL = 'http://localhost:5000/api';

const LoginCallbackPage = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    const handleCallback = async () => {
      try {
        console.log('LoginCallbackPage: Starting authentication process');
        // Extract token from URL
        const params = new URLSearchParams(location.search);
        const token = params.get('token');
        
        console.log('Token from URL:', token ? 'Token exists' : 'No token found');
        
        if (!token) {
          setError('No authentication token found in URL.');
          setLoading(false);
          return;
        }
        
        // Verify token with backend
        console.log('Sending token to backend for verification');
        const response = await axios.post(`${API_BASE_URL}/auth/authenticate-token`, { token });
        console.log('Token verification response:', response.status, response.data);
        
        // Save token and user data to localStorage
        localStorage.setItem('token', response.data.token);
        localStorage.setItem('userData', JSON.stringify({
          userId: response.data.userId,
          email: response.data.email,
          firstName: response.data.firstName,
          lastName: response.data.lastName,
          role: response.data.role
        }));
        
        console.log('Token and user data saved to localStorage');
        console.log('User role:', response.data.role);
        
        // Get source parameter from URL to determine if this is from a signup or login
        const source = params.get('source') || '';
        const isNewUser = source.includes('signup');
        
        // Use direct window location change instead of navigate
        // This is more reliable than React Router's navigate in some cases
        if (response.data.role === 'Admin') {
          console.log('Redirecting to /admin');
          window.location.href = '/admin';
        } else if (isNewUser) {
          // New user - go to profile completion
          console.log('New user signup detected, redirecting to /profile');
          window.location.href = '/profile';
        } else {
          // Existing user login - go directly to home
          console.log('Existing user login detected, redirecting to /home');
          window.location.href = '/home';
        }
      } catch (err: any) {
        console.error('Authentication error:', err);
        setError(err.response?.data?.message || 'Authentication failed. Please try logging in again.');
        setLoading(false);
      }
    };

    handleCallback();
  }, [location, navigate]);

  const containerStyle = {
    minHeight: '100vh',
    display: 'flex',
    flexDirection: 'column' as const,
    alignItems: 'center',
    justifyContent: 'center'
  };

  const spinnerStyle = {
    height: '3rem',
    width: '3rem',
    borderRadius: '50%',
    borderTop: '2px solid #3b82f6',
    borderBottom: '2px solid #3b82f6',
    animation: 'spin 1s linear infinite',
    marginBottom: '1rem'
  };

  const errorContainerStyle = {
    backgroundColor: '#ffebee',
    border: '1px solid #f44336',
    color: '#d32f2f',
    padding: '1rem',
    borderRadius: '0.25rem',
    marginBottom: '1rem',
    maxWidth: '400px'
  };

  const errorTitleStyle = {
    fontWeight: 'bold'
  };

  const buttonStyle = {
    backgroundColor: '#3b82f6',
    color: 'white',
    fontWeight: 'bold',
    padding: '0.5rem 1rem',
    borderRadius: '0.25rem',
    border: 'none',
    cursor: 'pointer'
  };

  if (loading) {
    return (
      <div style={containerStyle}>
        <div style={spinnerStyle}></div>
        <p>Completing your authentication...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div style={containerStyle}>
        <div style={errorContainerStyle}>
          <p style={errorTitleStyle}>Authentication Error</p>
          <p>{error}</p>
        </div>
        <button
          style={buttonStyle}
          onClick={() => navigate('/login')}
        >
          Back to Login
        </button>
      </div>
    );
  }

  return null;
};

export default LoginCallbackPage; 