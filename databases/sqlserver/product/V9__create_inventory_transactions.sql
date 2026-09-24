USE ecommerce_product_db;

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'inventory_transactions')
BEGIN
    CREATE TABLE inventory_transactions (
        id BIGINT IDENTITY(1,1) PRIMARY KEY,
        variant_id BIGINT NOT NULL,
        change_quantity INT NOT NULL,
        transaction_type NVARCHAR(50) NOT NULL,
        reference_id NVARCHAR(100) NULL,
        created_at DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
        CONSTRAINT fk_invtrans_variant FOREIGN KEY (variant_id) REFERENCES product_variants(id)
    );
END;