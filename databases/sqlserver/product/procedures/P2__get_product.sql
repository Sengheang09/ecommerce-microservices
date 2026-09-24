USE ecommerce_product_db;

CREATE OR ALTER PROCEDURE sp_get_product_by_id
    @ProductId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        p.id, p.name, p.slug, p.description, p.base_price, p.status, p.created_at,
        c.name AS category_name,
        b.name AS brand_name
    FROM products p
    JOIN categories c ON p.category_id = c.id
    LEFT JOIN brands b ON p.brand_id = b.id
    WHERE p.id = @ProductId;
END;