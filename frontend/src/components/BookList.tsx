import { useState, useEffect } from 'react'
import { Book } from '../types/Book'

function BookList() {
    const [books, setBooks] = useState<Book[]>([]);
    const [sortedBooks, setSortedBooks] = useState<Book[]>([]);
    const [pageSize, setPageSize] = useState<number>(5);
    const [pageNum, setPageNum] = useState<number>(1);
    const [totalItems, setTotalItems] = useState<number>(0);
    const [totalPages, setTotalPages] = useState<number>(0);
    const [isSorted, setIsSorted] = useState<boolean>(false);

    useEffect(() => {
        const fetchBooks = async () => {
            const response = await fetch(
                `https://localhost:5001/api/BookStore?pageSize=${pageSize}&pageNum=${pageNum}`
            )
            const data = await response.json();
            setBooks(data.books);
            setTotalItems(data.totalNumBooks);
            setTotalPages(Math.ceil(data.totalNumBooks / pageSize));
        }
        fetchBooks();

    }, [pageSize, pageNum]);

    useEffect(() => {
        if (isSorted) {
            setSortedBooks([...books].sort((a, b) => a.title.localeCompare(b.title)));
        } else {
            setSortedBooks(books);
        }
    }, [books, isSorted]);

    return (
        <>
            <h1>Book List</h1>
            <br />

            {!isSorted && (
                <button
                    className="btn btn-primary mb-3"
                    onClick={() => setIsSorted(true)}
                >
                    Sort by Title
                </button>
            )}

            {sortedBooks.map((book) => (
                <div id="bookCard" className="card mb-3" key={book.bookId}>
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
                    </div>
                </div>
            ))}

            <button
                className='btn btn-secondary'
                onClick={() => setPageNum(pageNum - 1)}
                disabled={pageNum === 1}
            >
                Previous
            </button>

            {[...Array(totalPages)].map((_, index) => (
                <button
                    key={index + 1}
                    className={`btn ${pageNum === index + 1 ? 'btn-primary' : 'btn-secondary'} mx-1`}
                    onClick={() => setPageNum(index + 1)}
                    disabled={pageNum === index + 1}
                >
                    {index + 1}
                </button>
            ))}

            <button
                className='btn btn-secondary'
                onClick={() => setPageNum(pageNum + 1)}
                disabled={pageNum === totalPages}
            >
                Next
            </button>

            <br />
            <label className='mt-3'>
                <strong>Results per page: </strong>
                <select
                    className='form-select w-auto d-inline-block ms-2'
                    value={pageSize}
                    onChange={(p) => {
                        setPageSize(Number(p.target.value))
                        setPageNum(1);
                    }}
                >
                    <option value={5}>5</option>
                    <option value={10}>10</option>
                    <option value={20}>20</option>
                </select>
            </label>
        </>
    );
}

export default BookList