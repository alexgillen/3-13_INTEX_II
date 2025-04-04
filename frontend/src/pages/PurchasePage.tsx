import { useNavigate, useParams } from "react-router-dom";
import { useCart } from "../context/CartContext";
import { CartItem } from "../types/CartItem";

function PurchasePage() {
    const navigate = useNavigate(); 
    const {bookName, bookId, bookPrice} = useParams();
    const {addToCart} = useCart();

    const handleAddToCart = () => {
        const newItem: CartItem = {
            bookId: Number(bookId),
            title: bookName || "Unknown",
            price: Number(bookPrice) || 0,
        };
        addToCart(newItem);
        navigate('/cart');
    };
    
    return (
        <>
            <h2>Purchase {bookName}</h2>
            <h4>Price: ${bookPrice}</h4>
            <button onClick={handleAddToCart}>Add to cart</button>

            <button onClick={() => navigate(-1)}>Go back</button>
        </>
    );
};

export default PurchasePage;