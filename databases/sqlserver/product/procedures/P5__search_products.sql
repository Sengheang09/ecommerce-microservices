USE ecommerce_product_db;

CREATE OR ALTER PROCEDURE sp_search_products
    @Keyword NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.id, p.name, p.slug, p.base_price, p.status
    FROM products p
    WHERE p.name LIKE '%' + @Keyword + '%' OR p.description LIKE '%' + @Keyword + '%'
    ORDER BY p.id DESC;
END;