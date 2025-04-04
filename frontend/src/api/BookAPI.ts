import { Book } from "../types/Book";

interface FetchBooksResponse {
    books: Book[];
    totalNumBooks: number;
}

const API_URL = "https://localhost:5001/api/BookStore";

export const fetchBooks = async (
    pageSize: Number,
    pageNum: Number,
    selectedCategories: string[]
): Promise<FetchBooksResponse> => {
    try{
        const categoryParams = selectedCategories
            .map((category) => `categories=${encodeURIComponent(category)}`)
            .join('&');

        const response = await fetch(
            `${API_URL}?pageSize=${pageSize}&pageNum=${pageNum}${selectedCategories.length ? `&${categoryParams}` : ""}`
        );

    if (!response.ok) {
        throw new Error('Network response was not ok');
    }

    return await response.json();
    } catch (error) {
        console.error("Error fetching books:", error);
        throw error;
    }      
};

export const addBook = async (book: Book): Promise<Book> => {
    try {
        const response = await fetch (`${API_URL}/AddBook`, {
            method : "POST",
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(book),
        });

        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        return await response.json();
    } catch (error) {
        console.error("Error adding book:", error);
        throw error;
    }
};

export const updateBook = async (bookId: number, updatedBook: Book): Promise<Book> => {
    try {
        const response = await fetch(`${API_URL}/UpdateBook/${bookId}`, {
            method: "PUT",
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(updatedBook),
        });

        return await response.json();
    } catch (error) {
        console.error("Error updating book:", error);
        throw error;
    }
};

export const deleteBook = async (bookId: number): Promise<void> => {
    try {
        const response = await fetch(`${API_URL}/DeleteBook/${bookId}`, {
            method: "DELETE",
        });

        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
    } catch (error) {
        console.error("Error deleting book:", error);
        throw error;
    }
};