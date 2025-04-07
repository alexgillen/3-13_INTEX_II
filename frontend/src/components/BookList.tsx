import { useState, useEffect } from 'react'
import { Book } from '../types/Book'
import { useNavigate } from 'react-router-dom';
import { fetchBooks } from '../api/MovieAPI';
import Pagination from './Pagination';

function BookList({selectedCategories}: {selectedCategories: string[]}) {
    const [books, setBooks] = useState<Book[]>([]);
    const [sortedBooks, setSortedBooks] = useState<Book[]>([]);
    const [pageSize, setPageSize] = useState<number>(5);
    const [pageNum, setPageNum] = useState<number>(1);
    const [totalPages, setTotalPages] = useState<number>(0);
    const [isSorted, setIsSorted] = useState<boolean>(false);
    const navigate = useNavigate();
    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const loadBooks = async () => {
            try {
                setLoading(true);
                const data = await fetchBooks(pageSize, pageNum, selectedCategories);
                setBooks(data.books);
                setTotalPages(Math.ceil(data.totalNumBooks / pageSize));
            } catch (error) {
                setError((error as Error).message);
            } finally {
                setLoading(false);
            }   
        };

        loadBooks();
    }, [pageSize, pageNum, selectedCategories]);

    useEffect(() => {
        if (isSorted) {
            setSortedBooks([...books].sort((a, b) => a.title.localeCompare(b.title)));
        } else {
            setSortedBooks(books);
        }
    }, [books, isSorted]);

    if (loading) return <p>Loading projects...</p>
    if (error) return <p className='text-red-500'>Error: {error}</p>

    return (
        <>
            {!isSorted && (
                <button
                    className="btn btn-primary mb-3"
                    onClick={() => setIsSorted(true)}
                >
                    Sort by Title
                </button>
            )}

            {sortedBooks.map((book) => (
                <div 
                    id="bookCard" 
                    className="card mb-3" 
                    key={book.bookId}
                    style={{ transition: ".03s", cursor: "pointer" }}
                    onMouseOver={(e) => e.currentTarget.style.transform = "scale(1.02)"}
                    onMouseOut={(e) => e.currentTarget.style.transform = "scale(1)"}
                >
                    <h3 className="card-title p-2">{book.title}</h3>
                    <div className="card-body">
                        <ul className="list-unstyled">
                            <li>
                                <strong>Author:</strong> {book.author}
                            </li>
                            <li>
                                <strong>Publisher:</strong> {book.publisher}
                            </li>
                            <li>
                                <strong>ISBN:</strong> {book.isbn}
                            </li>
                            <li>
                                <strong>Classification:</strong> {book.classification}
                            </li>
                            <li>
                                <strong>Category:</strong> {book.category}
                            </li>
                            <li>
                                <strong>Page Count:</strong> {book.pageCount}
                            </li>
                            <li>
                                <strong>Price:</strong> ${book.price}
                            </li>
                        </ul>

                        <button
                            className='btn btn-success'
                            onClick={() => navigate(`/purchase/${book.title}/${book.bookId}/${book.price}`)}
                            style={{ transition: ".03s", cursor: "pointer" }}
                            onMouseOver={(e) => e.currentTarget.style.transform = "scale(1.02)"}
                            onMouseOut={(e) => e.currentTarget.style.transform = "scale(1)"}
                        >
                            Add to Cart
                        </button>
                    </div>
                </div>
            ))}
            <Pagination
                currentPage={pageNum}
                totalPages={totalPages}
                pageSize={pageSize}
                onPageChange={setPageNum}
                onPageSizeChange={(newSize) => {
                    setPageSize(newSize);
                    setPageNum(1);
                }}
            />
        </>
    );
}

export default BookList