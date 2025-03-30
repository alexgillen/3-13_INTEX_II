import './App.css'
import BookPage from './pages/BookPage'
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import PurchasePage from './pages/PurchasePage'
import { CartProvider } from './context/CartContext'
import CartPage from './pages/CartPage'

function App() {

  return (
    <>
      <CartProvider>
        <Router>
          <Routes>
            <Route path='/' element={<BookPage />} />
            <Route path='/books/' element={<BookPage />} />
            <Route 
              path='/purchase/:bookName/:bookId/:bookPrice' 
              element={<PurchasePage />}
            />
            <Route path='/cart' element={<CartPage />} />
          </Routes>
        </Router>
      </CartProvider>

    </>
  )
}

export default App
