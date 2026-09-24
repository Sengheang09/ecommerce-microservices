USE ecommerce_product_db;

CREATE OR ALTER PROCEDURE sp_create_product
    @Name NVARCHAR(255),
    @Slug NVARCHAR(255),
    @Description NVARCHAR(MAX),
    @CategoryId BIGINT,
    @BrandId BIGINT,
    @BasePrice DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO products (name, slug, description, category_id, brand_id, base_price, status)
    VALUES (@Name, @Slug, @Description, @CategoryId, @BrandId, @BasePrice, 'PUBLISHED');

    SELECT SCOPE_IDENTITY() AS product_id;
END;