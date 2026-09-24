USE ecommerce_user_db;

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'customer_profiles')
BEGIN
    CREATE TABLE customer_profiles (
        id BIGINT IDENTITY(1,1) PRIMARY KEY,
        user_id BIGINT NOT NULL UNIQUE,
        first_name NVARCHAR(100) NOT NULL,
        last_name NVARCHAR(100) NOT NULL,
        phone NVARCHAR(30),
        date_of_birth DATE,
        gender NVARCHAR(20),
        created_at DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
        updated_at DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );
END;

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'addresses')
BEGIN
    CREATE TABLE addresses (
        id BIGINT IDENTITY(1,1) PRIMARY KEY,
        customer_id BIGINT NOT NULL,
        address_line NVARCHAR(255) NOT NULL,
        city NVARCHAR(100),
        province NVARCHAR(100),
        postal_code NVARCHAR(20),
        country NVARCHAR(100),
        is_default BIT NOT NULL DEFAULT 0,
        created_at DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
        CONSTRAINT fk_addresses_customer FOREIGN KEY (customer_id) REFERENCES customer_profiles(id) ON DELETE CASCADE
    );
END;