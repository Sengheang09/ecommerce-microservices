USE ecommerce_product_db;

CREATE OR ALTER PROCEDURE sp_delete_product
    @ProductId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM products WHERE id = @ProductId;
END;