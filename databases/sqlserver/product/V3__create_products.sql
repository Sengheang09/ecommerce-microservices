USE ecommerce_product_db;

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'products')
BEGIN
    CREATE TABLE products (
        id BIGINT IDENTITY(1,1) PRIMARY KEY,
        name NVARCHAR(255) NOT NULL,
        slug NVARCHAR(255) NOT NULL UNIQUE,
        description NVARCHAR(MAX) NULL,
        category_id BIGINT NOT NULL,
        brand_id BIGINT NULL,
        base_price DECIMAL(18,2) NOT NULL,
        status NVARCHAR(30) NOT NULL DEFAULT 'DRAFT',
        created_at DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
        updated_at DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
        CONSTRAINT fk_products_category FOREIGN KEY (category_id) REFERENCES categories(id),
        CONSTRAINT fk_products_brand FOREIGN KEY (brand_id) REFERENCES brands(id)
    );
END;