import './App.css'
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import { CartProvider } from './context/CartContext'
import MovieTester from './pages/MovieTester'

function App() {

  return (
    <>
      <CartProvider>
        <Router>
          <Routes>
            <Route path='/' element={<MovieTester />} />
          </Routes>
        </Router>
      </CartProvider>

    </>
  )
}

export default App
