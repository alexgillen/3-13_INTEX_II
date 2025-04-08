import './App.css'
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom'
import { CartProvider } from './context/CartContext'
import LoginPage from './pages/LoginPage'
import LoginCallbackPage from './pages/LoginCallbackPage'
import UserProfilePage from './pages/UserProfilePage'
import HomePage from './pages/HomePage'

// Debug component to show when no routes match
const NotFoundDebug = () => {
  // Get all localStorage entries
  const localStorageEntries = Object.keys(localStorage).reduce((acc: Record<string, string>, key) => {
    const value = localStorage.getItem(key);
    if (value) {
      acc[key] = value;
    }
    return acc;
  }, {});

  return (
    <div style={{ padding: '2rem', maxWidth: '800px', margin: '0 auto' }}>
      <h1>404 - Page Not Found</h1>
      <p>Sorry, the page you are looking for does not exist.</p>
      
      <div style={{ marginTop: '2rem' }}>
        <h2>Debug Information</h2>
        <h3>Local Storage</h3>
        <pre style={{ backgroundColor: '#f5f5f5', padding: '1rem', borderRadius: '4px', overflow: 'auto' }}>
          {JSON.stringify(localStorageEntries, null, 2)}
        </pre>
      </div>
    </div>
  );
};

function App() {
  // Check if user is logged in
  const isLoggedIn = localStorage.getItem('token') && localStorage.getItem('userData');

  return (
    <>
      <CartProvider>
        <Router>
          <Routes>
            {/* Root path redirects based on login status */}
            <Route path='/' element={
              isLoggedIn ? <Navigate to="/profile" /> : <LoginPage />
            } />
            <Route path='/login' element={<LoginPage />} />
            <Route path='/login-callback' element={<LoginCallbackPage />} />
            <Route path='/signup-callback' element={<LoginCallbackPage />} />
            <Route path='/profile' element={<UserProfilePage />} />
            <Route path='/home' element={<HomePage />} />
            {/* Catch-all route for debugging */}
            <Route path='*' element={<NotFoundDebug />} />
          </Routes>
        </Router>
      </CartProvider>
    </>
  )
}

export default App
