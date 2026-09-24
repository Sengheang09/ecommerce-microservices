USE ecommerce_product_db;

CREATE OR ALTER PROCEDURE sp_update_product
    @ProductId BIGINT,
    @Name NVARCHAR(255),
    @Description NVARCHAR(MAX),
    @BasePrice DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE products
    SET name = @Name,
        description = @Description,
        base_price = @BasePrice,
        updated_at = SYSUTCDATETIME()
    WHERE id = @ProductId;
END;