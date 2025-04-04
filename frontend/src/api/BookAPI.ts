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