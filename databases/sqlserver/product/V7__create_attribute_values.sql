USE ecommerce_product_db;

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'attribute_values')
BEGIN
    CREATE TABLE attribute_values (
        id BIGINT IDENTITY(1,1) PRIMARY KEY,
        attribute_id BIGINT NOT NULL,
        value NVARCHAR(100) NOT NULL,
        CONSTRAINT fk_attrval_attribute FOREIGN KEY (attribute_id) REFERENCES product_attributes(id) ON DELETE CASCADE
    );
END;