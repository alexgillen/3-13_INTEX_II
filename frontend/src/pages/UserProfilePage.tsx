import { useState, useEffect } from 'react';
import React from 'react';

const UserProfilePage = () => {
  const [age, setAge] = useState('');
  const [gender, setGender] = useState('');
  const [phone, setPhone] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [debugInfo, setDebugInfo] = useState({});

  useEffect(() => {
    // Debug: Check if we have user data
    const userDataStr = localStorage.getItem('userData');
    const token = localStorage.getItem('token');
    console.log('UserProfilePage - Initial check:', {
      hasUserData: Boolean(userDataStr),
      hasToken: Boolean(token)
    });
    
    if (token) {
      try {
        // For debugging - logging token structure
        const tokenParts = token.split('.');
        if (tokenParts.length === 3) {
          // JWT structure confirmed
          const payload = JSON.parse(atob(tokenParts[1]));
          console.log('Token payload:', payload);
          // Check if 'sub' claim exists
          if (!payload.sub) {
            console.warn('Token missing required "sub" claim!');
          }
        } else {
          console.warn('Token does not have valid JWT structure!');
        }
      } catch (err) {
        console.error('Error parsing token:', err);
      }
    }
    
    // Set debug info for display
    setDebugInfo({
      pathname: window.location.pathname,
      hasUserData: Boolean(userDataStr),
      hasToken: Boolean(token),
      userData: userDataStr ? JSON.parse(userDataStr) : null
    });
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError('');

    try {
      // Get user data from local storage
      const userDataStr = localStorage.getItem('userData');
      const token = localStorage.getItem('token');
      
      console.log('UserProfilePage - Submit:', {
        age,
        gender,
        phone,
        hasUserData: Boolean(userDataStr),
        hasToken: Boolean(token)
      });
      
      if (!userDataStr || !token) {
        throw new Error('User not authenticated');
      }

      const userData = JSON.parse(userDataStr);
      
      // Update the user profile with age and gender
      const response = await fetch('http://localhost:5000/api/User/profile', {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify({
          age: parseInt(age),
          gender,
          phone
        })
      });

      if (!response.ok) {
        // More detailed error handling
        const contentType = response.headers.get("content-type");
        if (contentType && contentType.indexOf("application/json") !== -1) {
          // JSON error response
          const errorData = await response.json();
          console.error('API error response:', errorData);
          throw new Error(errorData.message || `Request failed with status: ${response.status}`);
        } else {
          // Non-JSON error response
          const errorText = await response.text();
          console.error('API error response (non-JSON):', errorText);
          throw new Error(`Request failed with status: ${response.status} - ${errorText}`);
        }
      }

      const updatedUser = await response.json();
      console.log('Profile update response:', updatedUser);
      
      // Update user data in local storage
      localStorage.setItem('userData', JSON.stringify({
        ...userData,
        age: parseInt(age),
        gender,
        phone
      }));

      // Redirect to home page using direct window location change
      console.log('Redirecting to home page');
      window.location.href = '/home';
    } catch (err: unknown) {
      console.error('Profile update error:', err);
      // Safely handle the error 
      setError(err instanceof Error ? err.message : String(err));
    } finally {
      setLoading(false);
    }
  };

  const containerStyle = {
    display: 'flex',
    flexDirection: 'column' as const,
    alignItems: 'center',
    justifyContent: 'center',
    minHeight: '100vh',
    backgroundColor: '#f3f4f6'
  };

  const formContainerStyle = {
    width: '100%',
    maxWidth: '450px',
    padding: '2rem',
    backgroundColor: 'white',
    borderRadius: '8px',
    boxShadow: '0 4px 6px rgba(0, 0, 0, 0.1)'
  };

  const headerStyle = {
    marginBottom: '1.5rem',
    textAlign: 'center' as const
  };

  const inputGroupStyle = {
    marginBottom: '1.5rem'
  };

  const labelStyle = {
    display: 'block',
    marginBottom: '0.5rem',
    fontWeight: 'bold' as const
  };

  const inputStyle = {
    width: '100%',
    padding: '0.75rem',
    border: '1px solid #d1d5db',
    borderRadius: '4px',
    fontSize: '1rem'
  };

  const buttonStyle = {
    width: '100%',
    padding: '0.75rem',
    backgroundColor: '#3b82f6',
    color: 'white',
    border: 'none',
    borderRadius: '4px',
    fontSize: '1rem',
    fontWeight: 'bold' as const,
    cursor: 'pointer',
    opacity: loading ? 0.7 : 1,
    marginBottom: '0.5rem'
  };

  const errorStyle = {
    color: '#ef4444',
    marginBottom: '1rem'
  };

  const debugStyle = {
    marginTop: '2rem',
    padding: '1rem',
    backgroundColor: '#f1f5f9',
    borderRadius: '4px',
    fontSize: '0.875rem',
    width: '100%',
    maxWidth: '450px'
  };

  const debugToken = async () => {
    try {
      const token = localStorage.getItem('token');
      if (!token) {
        alert('No token found in localStorage!');
        return;
      }
      
      const response = await fetch('http://localhost:5000/api/Auth/debug-token', {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });
      
      const result = await response.json();
      alert(`Token Debug Result: ${JSON.stringify(result, null, 2)}`);
      console.log('Token debug result:', result);
      
      if (!result.isValid || !result.userId) {
        alert('Your token is invalid. Please login again.');
      }
    } catch (error) {
      alert(`Error debugging token: ${error instanceof Error ? error.message : String(error)}`);
      console.error('Token debug error:', error);
    }
  };

  return (
    <div style={containerStyle}>
      <div style={formContainerStyle}>
        <div style={headerStyle}>
          <h1>Complete Your Profile</h1>
          <p>Please provide a few more details to personalize your experience</p>
        </div>
        
        {error && <div style={errorStyle}>{error}</div>}
        
        <form onSubmit={handleSubmit}>
          <div style={inputGroupStyle}>
            <label style={labelStyle} htmlFor="age">Age</label>
            <input
              id="age"
              type="number"
              style={inputStyle}
              value={age}
              onChange={(e) => setAge(e.target.value)}
              required
              min="13"
              max="120"
              placeholder="Enter your age"
            />
          </div>
          
          <div style={inputGroupStyle}>
            <label style={labelStyle} htmlFor="gender">Gender</label>
            <select
              id="gender"
              style={inputStyle}
              value={gender}
              onChange={(e) => setGender(e.target.value)}
              required
            >
              <option value="" disabled>Select your gender</option>
              <option value="male">Male</option>
              <option value="female">Female</option>
              <option value="non-binary">Non-binary</option>
              <option value="prefer-not-to-say">Prefer not to say</option>
            </select>
          </div>
          
          <div style={inputGroupStyle}>
            <label style={labelStyle} htmlFor="phone">Phone Number</label>
            <input
              id="phone"
              type="tel"
              style={inputStyle}
              value={phone}
              onChange={(e) => setPhone(e.target.value)}
              placeholder="Enter your phone number (optional)"
            />
          </div>
          
          <button
            type="submit"
            style={buttonStyle}
            disabled={loading}
          >
            {loading ? 'Saving...' : 'Save and Continue'}
          </button>
          
          <button
            type="button"
            style={{...buttonStyle, backgroundColor: '#4b5563'}}
            onClick={() => {
              // Get the current user data
              const userDataStr = localStorage.getItem('userData');
              if (userDataStr) {
                // Add default age and gender values
                const userData = JSON.parse(userDataStr);
                userData.age = 18; // Default age
                userData.gender = 'prefer-not-to-say'; // Default gender
                userData.phone = ''; // Default empty phone
                
                // Save back to localStorage
                localStorage.setItem('userData', JSON.stringify(userData));
                console.log('Added default age and gender to user data');
              }
              
              // Redirect to home page
              window.location.href = '/home';
            }}
          >
            Skip for Now
          </button>
        </form>
      </div>
      
      {/* Debug Information */}
      <div style={debugStyle}>
        <h3>Debug Information</h3>
        <pre style={{overflow: 'auto', maxHeight: '200px'}}>
          {JSON.stringify(debugInfo, null, 2)}
        </pre>
        <div style={{marginTop: '1rem'}}>
          <button 
            style={{...buttonStyle, padding: '0.5rem'}} 
            onClick={() => window.location.href = '/home'}
          >
            Go to Home Page
          </button>
          <button 
            style={{...buttonStyle, padding: '0.5rem', backgroundColor: '#dc2626'}} 
            onClick={() => {
              localStorage.clear();
              window.location.href = '/login';
            }}
          >
            Clear Storage & Logout
          </button>
          <button 
            style={{...buttonStyle, padding: '0.5rem', backgroundColor: '#059669', marginTop: '0.5rem'}} 
            onClick={debugToken}
          >
            Debug Token
          </button>
        </div>
      </div>
    </div>
  );
};

export default UserProfilePage; 