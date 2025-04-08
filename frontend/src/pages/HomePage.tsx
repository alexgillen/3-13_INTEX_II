import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

const HomePage = () => {
  const navigate = useNavigate();
  const [userData, setUserData] = useState<any>(null);

  useEffect(() => {
    // Check if user is logged in
    const userDataStr = localStorage.getItem('userData');
    console.log('HomePage - userData from localStorage:', userDataStr);
    
    if (!userDataStr) {
      console.log('HomePage - No userData found, redirecting to login');
      navigate('/login');
      return;
    }

    setUserData(JSON.parse(userDataStr));
  }, [navigate]);

  const handleLogout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('userData');
    navigate('/login');
  };

  const containerStyle = {
    padding: '2rem',
    maxWidth: '1200px',
    margin: '0 auto'
  };

  const headerStyle = {
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: '2rem',
    paddingBottom: '1rem',
    borderBottom: '1px solid #eaeaea'
  };

  const contentStyle = {
    backgroundColor: 'white',
    padding: '2rem',
    borderRadius: '8px',
    boxShadow: '0 2px 10px rgba(0, 0, 0, 0.1)'
  };

  const buttonStyle = {
    backgroundColor: '#3b82f6',
    color: 'white',
    fontWeight: 'bold',
    padding: '0.5rem 1rem',
    borderRadius: '4px',
    border: 'none',
    cursor: 'pointer',
    marginRight: '0.5rem'
  };

  const debugContainerStyle = {
    marginTop: '2rem',
    padding: '1rem',
    backgroundColor: '#f1f5f9',
    borderRadius: '8px'
  };

  if (!userData) {
    return (
      <div style={{padding: '2rem'}}>
        <h2>Loading...</h2>
        <div style={debugContainerStyle}>
          <h3>Debug Navigation</h3>
          <p>If you're stuck here, try one of these links:</p>
          <button 
            style={buttonStyle} 
            onClick={() => window.location.href = '/login'}
          >
            Go to Login
          </button>
          <button 
            style={buttonStyle} 
            onClick={() => window.location.href = '/profile'}
          >
            Go to Profile
          </button>
        </div>
      </div>
    );
  }

  return (
    <div style={containerStyle}>
      <header style={headerStyle}>
        <h1>CineNiche</h1>
        <div>
          <span style={{ marginRight: '1rem' }}>
            Welcome, {userData.firstName || userData.email}
          </span>
          <button style={buttonStyle} onClick={handleLogout}>
            Logout
          </button>
        </div>
      </header>

      <div style={contentStyle}>
        <h2 style={{ marginBottom: '1rem' }}>Welcome to CineNiche!</h2>
        <p>
          Your movie rating and recommendation platform. This is a placeholder home page.
        </p>

        <div style={{ marginTop: '2rem' }}>
          <h3>Your Profile:</h3>
          <ul style={{ listStyle: 'none', padding: 0 }}>
            <li><strong>Email:</strong> {userData.email}</li>
            <li><strong>Name:</strong> {userData.firstName} {userData.lastName}</li>
            {userData.age && <li><strong>Age:</strong> {userData.age}</li>}
            {userData.gender && <li><strong>Gender:</strong> {userData.gender}</li>}
            <li><strong>Role:</strong> {userData.role}</li>
          </ul>
        </div>
        
        <div style={debugContainerStyle}>
          <h3>Debug Navigation</h3>
          <button 
            style={buttonStyle} 
            onClick={() => window.location.href = '/profile'}
          >
            Edit Profile
          </button>
          <button 
            style={buttonStyle} 
            onClick={() => {
              console.log('Current localStorage:', localStorage);
            }}
          >
            Log LocalStorage
          </button>
        </div>
      </div>
    </div>
  );
};

export default HomePage; 