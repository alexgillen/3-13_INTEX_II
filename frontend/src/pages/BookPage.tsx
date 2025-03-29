import { useState } from "react";
import CategoryFilter from "../components/CategoryFilter";
import BookList from "../components/BookList";

function BookPage() {
    const [selectedCategories, setSelectedCategories] = useState<string[]>([]);

    return (
        <>
            <CategoryFilter 
                selectedCategories={selectedCategories}
                setSelectedCategories={setSelectedCategories}
            />
            <BookList />
        </>
    )
};

export default BookPage;