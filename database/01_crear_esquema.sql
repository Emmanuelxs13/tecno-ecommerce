-- ============================================================
-- TecnoEcommerce – Esquema PostgreSQL
-- Sprint 4: Migración de datos en memoria → PostgreSQL
-- Ejecutar en pgAdmin 4 Query Tool sobre la base de datos
-- "tecnoecommerce" (o el nombre que elijas).
-- ============================================================

-- Crear la base de datos si no existe (ejecutar como superusuario)
-- CREATE DATABASE tecnoecommerce
--     WITH ENCODING = 'UTF8'
--          LC_COLLATE = 'es_CO.UTF-8'
--          LC_CTYPE   = 'es_CO.UTF-8'
--          TEMPLATE   = template0;

-- Conectarse a la base antes de ejecutar el resto:
-- \c tecnoecommerce

-- ────────────────────────────────────────────────────────────
-- 1. CATEGORÍAS
-- ────────────────────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS categorias (
    id          SERIAL       PRIMARY KEY,
    nombre      VARCHAR(100) NOT NULL,
    descripcion TEXT         NOT NULL DEFAULT ''
);

-- ────────────────────────────────────────────────────────────
-- 2. USUARIOS
-- Rol: 1 = Cliente, 2 = Administrador
-- ────────────────────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS usuarios (
    id              SERIAL       PRIMARY KEY,
    nombre          VARCHAR(150) NOT NULL,
    email           VARCHAR(200) NOT NULL UNIQUE,
    contrasena_hash VARCHAR(500) NOT NULL,
    rol             SMALLINT     NOT NULL DEFAULT 1
        CONSTRAINT ck_usuarios_rol CHECK (rol IN (1, 2))
);

-- ────────────────────────────────────────────────────────────
-- 3. PRODUCTOS
-- ────────────────────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS productos (
    id           SERIAL        PRIMARY KEY,
    nombre       VARCHAR(200)  NOT NULL,
    descripcion  TEXT          NOT NULL DEFAULT '',
    precio       NUMERIC(10,2) NOT NULL
        CONSTRAINT ck_productos_precio CHECK (precio >= 0),
    stock        INTEGER       NOT NULL DEFAULT 0
        CONSTRAINT ck_productos_stock CHECK (stock >= 0),
    imagen_url   VARCHAR(500)  NOT NULL DEFAULT '',
    categoria_id INTEGER       NOT NULL
        REFERENCES categorias(id) ON DELETE RESTRICT
);

CREATE INDEX IF NOT EXISTS ix_productos_categoria ON productos(categoria_id);

-- ────────────────────────────────────────────────────────────
-- 4. CARRITOS
-- Un usuario → máximo un carrito (UNIQUE en usuario_id)
-- ────────────────────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS carritos (
    id             SERIAL      PRIMARY KEY,
    fecha_creacion TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    usuario_id     INTEGER     NOT NULL UNIQUE
        REFERENCES usuarios(id) ON DELETE CASCADE
);

-- ────────────────────────────────────────────────────────────
-- 5. ÍTEMS DEL CARRITO
-- ────────────────────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS items_carrito (
    id              SERIAL        PRIMARY KEY,
    cantidad        INTEGER       NOT NULL
        CONSTRAINT ck_items_carrito_cantidad CHECK (cantidad > 0),
    precio_unitario NUMERIC(10,2) NOT NULL,
    carrito_id      INTEGER       NOT NULL
        REFERENCES carritos(id) ON DELETE CASCADE,
    producto_id     INTEGER       NOT NULL
        REFERENCES productos(id) ON DELETE RESTRICT,
    CONSTRAINT uq_item_carrito_producto UNIQUE (carrito_id, producto_id)
);

CREATE INDEX IF NOT EXISTS ix_items_carrito_carrito   ON items_carrito(carrito_id);
CREATE INDEX IF NOT EXISTS ix_items_carrito_producto  ON items_carrito(producto_id);

-- ────────────────────────────────────────────────────────────
-- 6. PEDIDOS
-- estado_pedido: 1=Pendiente 2=Procesando 3=Enviado 4=Entregado 5=Cancelado
-- estado_pago:   1=Pendiente 2=Aprobado   3=Rechazado
-- ────────────────────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS pedidos (
    id              SERIAL        PRIMARY KEY,
    fecha_creacion  TIMESTAMPTZ   NOT NULL DEFAULT NOW(),
    total           NUMERIC(10,2) NOT NULL,
    estado_pedido   SMALLINT      NOT NULL DEFAULT 1
        CONSTRAINT ck_pedidos_estado_pedido CHECK (estado_pedido BETWEEN 1 AND 5),
    estado_pago     SMALLINT      NOT NULL DEFAULT 1
        CONSTRAINT ck_pedidos_estado_pago   CHECK (estado_pago   BETWEEN 1 AND 3),
    usuario_id      INTEGER       NOT NULL
        REFERENCES usuarios(id) ON DELETE RESTRICT
);

CREATE INDEX IF NOT EXISTS ix_pedidos_usuario ON pedidos(usuario_id);

-- ────────────────────────────────────────────────────────────
-- 7. DETALLES DE PEDIDO
-- ────────────────────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS detalles_pedido (
    id              SERIAL        PRIMARY KEY,
    cantidad        INTEGER       NOT NULL
        CONSTRAINT ck_detalles_pedido_cantidad CHECK (cantidad > 0),
    precio_unitario NUMERIC(10,2) NOT NULL,
    pedido_id       INTEGER       NOT NULL
        REFERENCES pedidos(id) ON DELETE CASCADE,
    producto_id     INTEGER       NOT NULL
        REFERENCES productos(id) ON DELETE RESTRICT
);

CREATE INDEX IF NOT EXISTS ix_detalles_pedido_pedido   ON detalles_pedido(pedido_id);
CREATE INDEX IF NOT EXISTS ix_detalles_pedido_producto ON detalles_pedido(producto_id);

-- ────────────────────────────────────────────────────────────
-- 8. ENVÍOS
-- estado_envio: 1=Preparando 2=EnCamino 3=Entregado
-- ────────────────────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS envios (
    id                       SERIAL       PRIMARY KEY,
    direccion                VARCHAR(500) NOT NULL,
    transportista            VARCHAR(100) NOT NULL DEFAULT '',
    estado_envio             SMALLINT     NOT NULL DEFAULT 1
        CONSTRAINT ck_envios_estado CHECK (estado_envio BETWEEN 1 AND 3),
    fecha_estimada_entrega   TIMESTAMPTZ,
    pedido_id                INTEGER      NOT NULL UNIQUE
        REFERENCES pedidos(id) ON DELETE CASCADE
);

-- ────────────────────────────────────────────────────────────
-- 9. RESEÑAS
-- calificacion: 1–5 estrellas
-- Un usuario solo puede reseñar un producto una vez.
-- ────────────────────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS resenias (
    id             SERIAL      PRIMARY KEY,
    calificacion   SMALLINT    NOT NULL
        CONSTRAINT ck_resenias_calificacion CHECK (calificacion BETWEEN 1 AND 5),
    comentario     TEXT        NOT NULL DEFAULT '',
    fecha_creacion TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    usuario_id     INTEGER     NOT NULL
        REFERENCES usuarios(id) ON DELETE CASCADE,
    producto_id    INTEGER     NOT NULL
        REFERENCES productos(id) ON DELETE CASCADE,
    CONSTRAINT uq_resenia_usuario_producto UNIQUE (usuario_id, producto_id)
);

CREATE INDEX IF NOT EXISTS ix_resenias_producto ON resenias(producto_id);

-- ────────────────────────────────────────────────────────────
-- 10. PAGOS
-- metodo: 'Tarjeta', 'Transferencia', 'Efectivo', 'PSE', etc.
-- estado: 1=Pendiente 2=Aprobado 3=Rechazado
-- Un pedido tiene como máximo un registro de pago (UNIQUE en pedido_id).
-- ────────────────────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS pagos (
    id         SERIAL        PRIMARY KEY,
    monto      NUMERIC(10,2) NOT NULL,
    metodo     VARCHAR(50)   NOT NULL,
    referencia VARCHAR(200)  NOT NULL DEFAULT '',
    fecha_pago TIMESTAMPTZ   NOT NULL DEFAULT NOW(),
    estado     SMALLINT      NOT NULL DEFAULT 1
        CONSTRAINT ck_pagos_estado CHECK (estado BETWEEN 1 AND 3),
    pedido_id  INTEGER       NOT NULL UNIQUE
        REFERENCES pedidos(id) ON DELETE CASCADE
);

-- ────────────────────────────────────────────────────────────
-- FIN DEL ESQUEMA
-- ────────────────────────────────────────────────────────────
