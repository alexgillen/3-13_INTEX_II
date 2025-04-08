import './App.css'
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import { CartProvider } from './context/CartContext'
import AdminMoviesPage from './pages/AdminMoviesPage'

function App() {

  return (
    <>
      <CartProvider>
        <Router>
          <Routes>
            <Route path='/admin' element={<AdminMoviesPage />} />
          </Routes>
        </Router>
      </CartProvider>

    </>
  )
}

export default App
