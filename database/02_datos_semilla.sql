-- ============================================================
-- TecnoEcommerce – Datos semilla
-- Sprint 4: Productos, categorías, usuarios y transacciones
-- de prueba con información verosímil.
-- Ejecutar DESPUÉS de 01_crear_esquema.sql
-- ============================================================

-- ────────────────────────────────────────────────────────────
-- CATEGORÍAS
-- ────────────────────────────────────────────────────────────
INSERT INTO categorias (nombre, descripcion) VALUES
    ('Laptops',      'Portátiles, ultrabooks y workstations móviles'),
    ('Smartphones',  'Teléfonos inteligentes Android e iOS'),
    ('Accesorios',   'Periféricos, cables, fundas y complementos'),
    ('Gaming',       'Hardware gamer, periféricos y sillas'),
    ('Audio',        'Audífonos, altavoces y sistemas de sonido'),
    ('Almacenamiento','Discos SSD, HDD, memorias USB y tarjetas SD');

-- ────────────────────────────────────────────────────────────
-- PRODUCTOS  (precios en pesos colombianos – COP)
-- ────────────────────────────────────────────────────────────

-- ── Laptops (categoria_id = 1) ─────────────────────────────
INSERT INTO productos (nombre, descripcion, precio, stock, imagen_url, categoria_id) VALUES
(
    'ASUS VivoBook 15 X1502',
    'Intel Core i5-12500H · 16 GB DDR4 · SSD 512 GB NVMe · Pantalla FHD 15.6" 60 Hz · Sin SO',
    1249900, 8,
    'https://m.media-amazon.com/images/I/71ZXZM7WCOL._AC_SL1500_.jpg',
    1
),
(
    'MacBook Air M2 13"',
    'Apple M2 8 núcleos · 8 GB RAM unificada · SSD 256 GB · Pantalla Liquid Retina 13.6" 500 nits · macOS Sonoma',
    2499900, 5,
    'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/macbook-air-midnight-select-20220606.jpg',
    1
),
(
    'Lenovo IdeaPad Slim 5i',
    'Intel Core i7-1255U · 16 GB DDR5 · SSD 1 TB NVMe · Pantalla IPS 2K 14" · Windows 11 Home',
    1799900, 6,
    'https://p1-ofp.static.pub/medias/bWFzdGVyfHJvb3R8MjUzNDA5fGltYWdlL3BuZ3xoNTEvaGRiLzE0NDI5NjE4MzI3NTE4LnBuZ3w/lenovo-laptop-ideapad-slim-5i-14-intel-subseries-hero.png',
    1
),
(
    'HP Pavilion Gaming 15',
    'AMD Ryzen 5 7535HS · NVIDIA RTX 2050 4 GB · 16 GB RAM · SSD 512 GB · FHD 144 Hz',
    1999900, 4,
    'https://ssl-product-images.www8-hp.com/digmedialib/prodimg/knowledgebase/c08161130.png',
    1
),

-- ── Smartphones (categoria_id = 2) ────────────────────────
(
    'Apple iPhone 15 Pro 256 GB',
    'Chip A17 Pro · 48 MP Fusion Camera · USB-C 3.0 · Titanio natural · iOS 17',
    2299900, 12,
    'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone-15-pro-finish-select-202309-6-1inch-naturaltitanium.jpg',
    2
),
(
    'Samsung Galaxy S24 128 GB',
    'Snapdragon 8 Gen 3 · 8 GB RAM · Pantalla AMOLED 2X 6.2" 120 Hz · Batería 4000 mAh · Android 14',
    1649900, 15,
    'https://images.samsung.com/is/image/samsung/p6pim/co/2401/gallery/co-galaxy-s24-s921-sm-s921bzadeoo-thumb-539573088',
    2
),
(
    'Xiaomi 14 Ultra 512 GB',
    'Snapdragon 8 Gen 3 · Leica 50 MP cuádruple · 90W HyperCharge · IP68 · Pantalla AMOLED 6.73" 120 Hz',
    2099900, 7,
    'https://i02.appmifile.com/mi-com-product/fly-birds/xiaomi-14-ultra/pc/img-hero.png',
    2
),
(
    'Google Pixel 8 128 GB',
    'Tensor G3 · 8 GB RAM · 50 MP + 10.5 MP · Batería 4575 mAh · Android 14 · 7 años de actualizaciones',
    1499900, 9,
    'https://lh3.googleusercontent.com/JZWMGa8EuPKN6FMb9OVqf_H6dEwiqrJZ1uRuJ2OYeaU8CXE03Yew7KKTM1tBOJkm7qOEKAr0sBQbAiFcPIAjGJBt',
    2
),

-- ── Accesorios (categoria_id = 3) ─────────────────────────
(
    'Monitor LG UltraWide 29WN600 29"',
    'IPS · 2560×1080 · 75 Hz · FreeSync · HDR10 · 2× HDMI · 1× DisplayPort',
    529900, 6,
    'https://www.lg.com/content/dam/channel/wcms/co/images/monitors/29wn600-w_afco_eum_co_c',
    3
),
(
    'Teclado Mecánico Keychron K2 Pro',
    'Inalámbrico Bluetooth 5.1 / USB-C · Compacto 75% · Switches Gateron Red · Retroiluminación RGB · Hot-swap',
    379900, 20,
    'https://cdn.shopify.com/s/files/1/0059/0630/1017/products/KeychronK2Pro-A3_1920x.jpg',
    3
),
(
    'Mouse Logitech MX Master 3S',
    'Inalámbrico 2.4 GHz + Bluetooth · 8000 DPI · 7 botones · Desplazamiento horizontal · Recargable USB-C',
    289900, 18,
    'https://resource.logitech.com/w_800,c_lpad,ar_4:3,q_auto,f_auto,dpr_1.0/d_transparent.gif/content/dam/logitech/en/products/mice/mx-master-3s/gallery/mx-master-3s-gallery-1-graphite.png',
    3
),
(
    'Webcam Logitech C920 HD Pro',
    '1080p 30 fps · Gran angular 78° · Micrófonos dobles · Autoenfoque Full HD · Compatible Zoom/Teams',
    249900, 14,
    'https://resource.logitech.com/w_800,c_lpad,ar_4:3,q_auto,f_auto,dpr_1.0/d_transparent.gif/content/dam/logitech/en/products/video-conferencing/c920s/gallery/c920s-gallery-1.png',
    3
),

-- ── Gaming (categoria_id = 4) ─────────────────────────────
(
    'GPU NVIDIA GeForce RTX 4060 8 GB',
    'Arquitectura Ada Lovelace · 8 GB GDDR6 · DLSS 3 · Ray Tracing · PCIe 4.0 · 115W TDP',
    879900, 4,
    'https://www.nvidia.com/content/dam/en-zz/Solutions/geforce/ada/rtx-4060/geforce-rtx-4060-product-photo-001-c.jpg',
    4
),
(
    'Silla Gaming Cougar Armor One',
    'Espuma de alta densidad · Ajuste lumbar · Reposabrazos 4D · Reclinable 90–160° · Soporta hasta 120 kg',
    699900, 3,
    'https://cdn.cougar.com/armor-one-gaming-chair',
    4
),
(
    'Headset HyperX Cloud Alpha S',
    'Drivers 50 mm · Micrófono desmontable 7.1 virtual · USB + Jack 3.5 mm · Control en línea · 300 horas',
    319900, 10,
    'https://www.hyperxgaming.com/content/images/hyperx-cloud-alpha-s-gaming-headset-gallery-1.jpg',
    4
),
(
    'Control Xbox Series X Wireless',
    'Inalámbrico 2.4 GHz + Bluetooth · USB-C · Botón Compartir · Gatillos hápticos · Compatible PC/Android',
    259900, 22,
    'https://img-prod-cms-rt-microsoft-com.akamaized.net/cms/api/am/imageFileData/RE4mPuA',
    4
),

-- ── Audio (categoria_id = 5) ──────────────────────────────
(
    'Sony WH-1000XM5',
    'ANC líder en la industria · 30 h batería · LDAC Hi-Res · Multipoint Bluetooth · USB-C · Funda incluida',
    849900, 8,
    'https://www.sony.com/image/5d02da5df552836db894cead401b0068?fmt=pjpeg&wid=330&bgcolor=FFFFFF',
    5
),
(
    'Parlante JBL Charge 5',
    'Potencia 40 W · IP67 · 20 h batería · PartyBoost · USB-A out · Graves potentes · Varios colores',
    459900, 11,
    'https://www.jbl.com/dw/image/v2/AAUJ_PRD/on/demandware.static/-/Sites-masterCatalog_Harman/default/dw3931571e/JBL_CHARGE5_HERO_001_BLK_2584x1089px.png',
    5
),

-- ── Almacenamiento (categoria_id = 6) ─────────────────────
(
    'SSD Samsung 870 EVO 1 TB',
    'SATA III 2.5" · Velocidad lectura 560 MB/s · Escritura 530 MB/s · V-NAND MLC · 5 años garantía',
    229900, 25,
    'https://images.samsung.com/is/image/samsung/p6pim/co/mz-77e1t0b/gallery/co-870-evo-sata-iii-2-5-ssd-mz-77e1t0b-mz-77e1t0b-thumb-399754742',
    6
),
(
    'Kingston DataTraveler 256 GB USB 3.2',
    'USB 3.2 Gen 1 · 200 MB/s lectura · 60 MB/s escritura · Tapa metálica · Retro-compatible USB 2.0',
    69900, 50,
    'https://www.kingston.com/dataSheets/DTIG4_256gb_us.jpg',
    6
);

-- ────────────────────────────────────────────────────────────
-- USUARIOS
-- ⚠️ Las contraseñas son hashes BCrypt del texto plano indicado.
--    admin123, cliente123 son solo para desarrollo local.
-- ────────────────────────────────────────────────────────────
INSERT INTO usuarios (nombre, email, contrasena_hash, rol) VALUES
(
    'Admin TecnoEcommerce',
    'admin@tecnoecommerce.com',
    -- BCrypt hash de 'admin123'
    '$2a$11$7fAQ.SyxilaNdRmTHzl3bOmC0vS1a9D5qH7g1QoHpCAZvwJdWgBIq',
    2  -- Administrador
),
(
    'Carlos Ramírez',
    'carlos.ramirez@email.com',
    -- BCrypt hash de 'cliente123'
    '$2a$11$MiC.5u3TzRQFkHuBqWjKaOM4fBX8kLY3JzJa7QfGCFoFoLFqWh7qG',
    1  -- Cliente
),
(
    'Laura Martínez',
    'laura.martinez@email.com',
    '$2a$11$MiC.5u3TzRQFkHuBqWjKaOM4fBX8kLY3JzJa7QfGCFoFoLFqWh7qG',
    1
),
(
    'Andrés Torres',
    'andres.torres@email.com',
    '$2a$11$MiC.5u3TzRQFkHuBqWjKaOM4fBX8kLY3JzJa7QfGCFoFoLFqWh7qG',
    1
),
(
    'Valentina López',
    'valentina.lopez@email.com',
    '$2a$11$MiC.5u3TzRQFkHuBqWjKaOM4fBX8kLY3JzJa7QfGCFoFoLFqWh7qG',
    1
);

-- ────────────────────────────────────────────────────────────
-- PEDIDOS  (2 pedidos entregados, 1 en proceso)
-- ────────────────────────────────────────────────────────────
INSERT INTO pedidos (fecha_creacion, total, estado_pedido, estado_pago, usuario_id) VALUES
-- Pedido 1: Carlos – entregado y pagado
('2026-01-15 10:30:00+00', 3899800, 4, 2, 2),
-- Pedido 2: Laura – procesando, pago aprobado
('2026-02-20 15:45:00+00', 1649900, 2, 2, 3),
-- Pedido 3: Andrés – pendiente, pago pendiente
('2026-03-01 09:00:00+00', 1879800, 1, 1, 4);

-- ────────────────────────────────────────────────────────────
-- DETALLES DE PEDIDO
-- ────────────────────────────────────────────────────────────
INSERT INTO detalles_pedido (cantidad, precio_unitario, pedido_id, producto_id) VALUES
-- Pedido 1: MacBook Air M2 (prod 2) + Samsung Galaxy S24 (prod 6)
(1, 2499900, 1, 2),
(1, 1399900, 1, 6),
-- Pedido 2: Samsung Galaxy S24 (prod 6)
(1, 1649900, 2, 6),
-- Pedido 3: Lenovo IdeaPad (prod 3) + Kingston USB (prod 20)
(1, 1799900, 3, 3),
(1,   69900, 3, 20);

-- ────────────────────────────────────────────────────────────
-- ENVÍOS
-- ────────────────────────────────────────────────────────────
INSERT INTO envios (direccion, transportista, estado_envio, fecha_estimada_entrega, pedido_id) VALUES
(
    'Calle 93 # 15-25, Bogotá D.C., Colombia, CP 110221',
    'Servientrega',
    3,  -- Entregado
    '2026-01-20 18:00:00+00',
    1
),
(
    'Carrera 7 # 45-60, Medellín, Antioquia, Colombia, CP 050001',
    'Coordinadora',
    2,  -- EnCamino
    '2026-02-25 18:00:00+00',
    2
),
(
    'Avenida Insurgentes Sur 1588, Ciudad de México, CDMX, México, CP 03920',
    'DHL',
    1,  -- Preparando
    '2026-03-08 18:00:00+00',
    3
);

-- ────────────────────────────────────────────────────────────
-- PAGOS
-- ────────────────────────────────────────────────────────────
INSERT INTO pagos (monto, metodo, referencia, fecha_pago, estado, pedido_id) VALUES
(3899800, 'Tarjeta',       'TXN-2026-0115-001', '2026-01-15 10:32:00+00', 2, 1),
(1649900, 'PSE',           'PSE-2026-0220-003', '2026-02-20 15:48:00+00', 2, 2),
(1879800, 'Transferencia', 'TRF-2026-0301-007', '2026-03-01 09:05:00+00', 1, 3);

-- ────────────────────────────────────────────────────────────
-- RESEÑAS
-- ────────────────────────────────────────────────────────────
INSERT INTO resenias (calificacion, comentario, usuario_id, producto_id) VALUES
(5, 'Excelente laptop, ligera y con batería que dura todo el día. La pantalla Retina es hermosa.', 2, 2),
(4, 'Muy buen teléfono, la cámara es increíble pero la batería podría ser mayor.', 3, 6),
(5, 'El mejor teclado mecánico que he tenido. Vale cada peso.', 2, 10),
(4, 'Sonido espectacular para el precio. El ANC funciona muy bien en transporte público.', 4, 17);

-- ────────────────────────────────────────────────────────────
-- CARRITO DE PRUEBA (usuario 5 – Valentina)
-- ────────────────────────────────────────────────────────────
INSERT INTO carritos (usuario_id) VALUES (5);

INSERT INTO items_carrito (cantidad, precio_unitario, carrito_id, producto_id) VALUES
(1, 879900,  1, 13),  -- RTX 4060
(2, 229900,  1, 19);  -- 2× SSD Samsung

-- ────────────────────────────────────────────────────────────
-- FIN DEL SCRIPT DE DATOS
-- ────────────────────────────────────────────────────────────
