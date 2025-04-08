erDiagram
    users {
        uuid id PK
        varchar external_auth_id
        varchar email
        varchar phone
        int age
        varchar gender
        varchar first_name
        varchar last_name
        varchar profile_image_url
        varchar role
    }
    
    

    users ||--o{ user_ratings : creates
    users ||--o{ user_watchlist : maintains
    users ||--o{ user_viewing_history : generates
    users ||--o{ user_recommendations : receives