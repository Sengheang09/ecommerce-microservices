CREATE OR REPLACE FUNCTION create_user(
    p_email VARCHAR,
    p_password_hash VARCHAR
)
RETURNS BIGINT
LANGUAGE plpgsql
AS $$
DECLARE
    new_user_id BIGINT;
BEGIN

    INSERT INTO users (
        email,
        password_hash
    )
    VALUES (
        p_email,
        p_password_hash
    )
    RETURNING id INTO new_user_id;

    RETURN new_user_id;

END;
$$;