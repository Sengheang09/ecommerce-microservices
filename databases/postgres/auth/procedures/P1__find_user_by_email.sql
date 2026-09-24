CREATE OR REPLACE FUNCTION find_user_by_email(
    p_email VARCHAR
)
RETURNS TABLE (
    id BIGINT,
    email VARCHAR,
    password_hash VARCHAR,
    status VARCHAR
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT
        u.id,
        u.email,
        u.password_hash,
        u.status
    FROM users u
    WHERE u.email = p_email;
END;
$$;