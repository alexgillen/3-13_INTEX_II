import { useNavigate } from "react-router-dom";
import { useCart } from "../context/CartContext";
import { CartItem } from "../types/CartItem";

function CartPage (){
    const navigate = useNavigate();
    const { cart, removeFromCart } = useCart();
    const totalAmount = cart.reduce((sum, item) => sum + item.price, 0);
    return (
        <div>
            <h2>Your Cart</h2>
            <div>
                {cart.length === 0 ? (
                    <p>Your cart is empty.</p>
                ) : (
                    <ul>
                        {cart.map((item: CartItem) => (
                            <li key={item.bookId}>
                                {item.title}: ${item.price.toFixed(2)}
                                <button onClick={() => removeFromCart(item.bookId)}>
                                    Remove
                                </button>
                            </li>
                        ))}
                    </ul>
                )}
            </div>
            <h3>Total: {totalAmount}</h3>
            <button>Checkout</button>
            <button onClick={() => navigate('/books')}>Continue Shopping</button>
        </div>
    )
}

export default CartPage;