INSERT INTO role_permissions (role_id, permission_id)
SELECT r.id, p.id
FROM roles r
CROSS JOIN permissions p
WHERE r.name = 'ADMIN';


INSERT INTO role_permissions (role_id, permission_id)
SELECT r.id, p.id
FROM roles r
JOIN permissions p
    ON p.name IN (
        'PRODUCT_READ',
        'ORDER_READ',
        'ORDER_CREATE'
    )
WHERE r.name = 'CUSTOMER';


INSERT INTO role_permissions (role_id, permission_id)
SELECT r.id, p.id
FROM roles r
JOIN permissions p
    ON p.name IN (
        'USER_READ',
        'PRODUCT_READ',
        'PRODUCT_UPDATE',
        'ORDER_READ',
        'ORDER_UPDATE'
    )
WHERE r.name = 'STAFF';