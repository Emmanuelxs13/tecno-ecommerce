# 📋 Documentación de Endpoints - TecnoEcommerce API

**Base URL**: `http://localhost:5247`

Todos los endpoints devuelven respuestas en formato **JSON**.

---

## 🔐 Autenticación

### 1. Registro de Usuario

**POST** `/api/usuarios/registro`

**Descripción**: Registra un nuevo usuario en la plataforma.

**Body (JSON)**:

```json
{
  "nombre": "Juan Pérez",
  "email": "juan@example.com",
  "contrasena": "juan123"
}
```

**Respuesta exitosa (201 Created)**:

```json
{
  "id": 1,
  "nombre": "Juan Pérez",
  "email": "juan@example.com",
  "rol": 0
}
```

**Errores**:

- `400 Bad Request`: Datos incompletos o email ya registrado
- `422 Unprocessable Entity`: Validación fallida

---

### 2. Iniciar Sesión

**POST** `/api/usuarios/login`

**Descripción**: Autentica un usuario y devuelve su información con un token JWT.

**Body (JSON)**:

```json
{
  "email": "juan@example.com",
  "contrasena": "password123"
}
```

**Respuesta exitosa (200 OK)**:

```json
{
  "usuario": {
    "id": 1,
    "nombre": "Juan Pérez",
    "email": "juan@example.com",
    "rol": 0
  },
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

**Errores**:

- `401 Unauthorized`: Credenciales inválidas

---

### 3. Obtener Perfil de Usuario

**GET** `/api/usuarios/{id}`

**Descripción**: Obtiene la información pública de un usuario específico.

**Parámetros**:

- `id` (path, requerido): ID del usuario

**Respuesta exitosa (200 OK)**:

```json
{
  "id": 1,
  "nombre": "Juan Pérez",
  "email": "juan@example.com",
  "rol": 0
}
```

**Errores**:

- `404 Not Found`: Usuario no existe

---

## 📦 Productos

### 4. Listar Todos los Productos

**GET** `/api/productos`

**Descripción**: Obtiene la lista de todos los productos disponibles.

**Respuesta exitosa (200 OK)**:

```json
[
  {
    "id": 1,
    "nombre": "Laptop Dell XPS 13",
    "descripcion": "Portátil ultraligera de 13 pulgadas",
    "precio": 999.99,
    "stock": 45,
    "imagenUrl": "https://example.com/laptop1.jpg",
    "categoriaId": 1,
    "categoriaNombre": "Laptops"
  },
  {
    "id": 2,
    "nombre": "iPhone 15 Pro",
    "descripcion": "Smartphone flagship de Apple",
    "precio": 1299.99,
    "stock": 30,
    "imagenUrl": "https://example.com/iphone15.jpg",
    "categoriaId": 2,
    "categoriaNombre": "Smartphones"
  }
]
```

---

### 5. Obtener Producto por ID

**GET** `/api/productos/{id}`

**Descripción**: Obtiene los detalles de un producto específico.

**Parámetros**:

- `id` (path, requerido): ID del producto

**Respuesta exitosa (200 OK)**:

```json
{
  "id": 1,
  "nombre": "Laptop Dell XPS 13",
  "descripcion": "Portátil ultraligera de 13 pulgadas",
  "precio": 999.99,
  "stock": 45,
  "imagenUrl": "https://example.com/laptop1.jpg",
  "categoriaId": 1,
  "categoriaNombre": "Laptops"
}
```

**Errores**:

- `404 Not Found`: Producto no existe

---

### 6. Filtrar Productos por Categoría

**GET** `/api/productos/categoria/{slug}`

**Descripción**: Obtiene todos los productos de una categoría específica.

**Parámetros**:

- `slug` (path, requerido): Slug de la categoría (ej: `laptops`, `smartphones`, `gaming`, `accesorios`, `monitores`)

**Respuesta exitosa (200 OK)**:

```json
[
  {
    "id": 1,
    "nombre": "Laptop Dell XPS 13",
    "descripcion": "Portátil ultraligera de 13 pulgadas",
    "precio": 999.99,
    "stock": 45,
    "imagenUrl": "https://example.com/laptop1.jpg",
    "categoriaId": 1,
    "categoriaNombre": "Laptops"
  }
]
```

---

### 7. Buscar Productos por Nombre

**GET** `/api/productos/buscar?q={termino}`

**Descripción**: Busca productos cuyo nombre contiene el término especificado.

**Parámetros**:

- `q` (query, requerido): Término de búsqueda (ej: `laptop`, `iphone`)

**Ejemplo**: `GET /api/productos/buscar?q=laptop`

**Respuesta exitosa (200 OK)**:

```json
[
  {
    "id": 1,
    "nombre": "Laptop Dell XPS 13",
    "descripcion": "Portátil ultraligera de 13 pulgadas",
    "precio": 999.99,
    "stock": 45,
    "imagenUrl": "https://example.com/laptop1.jpg",
    "categoriaId": 1,
    "categoriaNombre": "Laptops"
  }
]
```

---

## 🏷️ Categorías

### 8. Listar Todas las Categorías

**GET** `/api/categorias`

**Descripción**: Obtiene la lista de todas las categorías de productos.

**Respuesta exitosa (200 OK)**:

```json
[
  {
    "id": 1,
    "nombre": "Laptops",
    "slug": "laptops",
    "descripcion": "Computadoras portátiles"
  },
  {
    "id": 2,
    "nombre": "Smartphones",
    "slug": "smartphones",
    "descripcion": "Teléfonos inteligentes"
  },
  {
    "id": 3,
    "nombre": "Gaming",
    "slug": "gaming",
    "descripcion": "Equipos y accesorios para gaming"
  },
  {
    "id": 4,
    "nombre": "Accesorios",
    "slug": "accesorios",
    "descripcion": "Accesorios para dispositivos"
  },
  {
    "id": 5,
    "nombre": "Monitores",
    "slug": "monitores",
    "descripcion": "Monitores y pantallas"
  }
]
```

---

### 9. Obtener Categoría por ID

**GET** `/api/categorias/{id}`

**Descripción**: Obtiene los detalles de una categoría específica.

**Parámetros**:

- `id` (path, requerido): ID de la categoría

**Respuesta exitosa (200 OK)**:

```json
{
  "id": 1,
  "nombre": "Laptops",
  "slug": "laptops",
  "descripcion": "Computadoras portátiles"
}
```

**Errores**:

- `404 Not Found`: Categoría no existe

---

## 🛒 Carrito

### 10. Obtener Carrito del Usuario

**GET** `/api/carrito`

**Descripción**: Obtiene el carrito de compras del usuario autenticado.

**Headers**:

- `Authorization: Bearer {token}` (requerido si está implementada autenticación)

**Respuesta exitosa (200 OK)**:

```json
{
  "id": 1,
  "usuarioId": 1,
  "items": [
    {
      "id": 101,
      "productoId": 1,
      "nombreProducto": "Laptop Dell XPS 13",
      "cantidad": 1,
      "precioUnitario": 999.99,
      "subtotal": 999.99
    },
    {
      "id": 102,
      "productoId": 2,
      "nombreProducto": "Mouse Logitech MX Master",
      "cantidad": 2,
      "precioUnitario": 99.99,
      "subtotal": 199.98
    }
  ],
  "total": 1199.97
}
```

**Errores**:

- `401 Unauthorized`: Usuario no autenticado

---

### 11. Agregar Producto al Carrito

**POST** `/api/carrito/agregar`

**Descripción**: Agrega un producto al carrito del usuario autenticado. Si el producto ya existe, incrementa la cantidad.

**Headers**:

- `Authorization: Bearer {token}` (requerido si está implementada autenticación)

**Body (JSON)**:

```json
{
  "productoId": 1,
  "cantidad": 2
}
```

**Respuesta exitosa (200 OK)**:

```json
{
  "id": 101,
  "productoId": 1,
  "nombreProducto": "Laptop Dell XPS 13",
  "cantidad": 2,
  "precioUnitario": 999.99,
  "subtotal": 1999.98
}
```

**Errores**:

- `400 Bad Request`: Datos incompletos o stock insuficiente
- `401 Unauthorized`: Usuario no autenticado
- `404 Not Found`: Producto no existe

---

### 12. Eliminar Producto del Carrito

**DELETE** `/api/carrito/{itemId}`

**Descripción**: Elimina un ítem específico del carrito.

**Parámetros**:

- `itemId` (path, requerido): ID del ítem en el carrito

**Headers**:

- `Authorization: Bearer {token}` (requerido si está implementada autenticación)

**Respuesta exitosa (204 No Content)**:
(Sin cuerpo)

**Errores**:

- `401 Unauthorized`: Usuario no autenticado
- `404 Not Found`: Ítem no existe

---

### 13. Vaciar Carrito

**DELETE** `/api/carrito/vaciar`

**Descripción**: Elimina todos los ítems del carrito del usuario.

**Headers**:

- `Authorization: Bearer {token}` (requerido si está implementada autenticación)

**Respuesta exitosa (204 No Content)**:
(Sin cuerpo)

**Errores**:

- `401 Unauthorized`: Usuario no autenticado

---

## 📋 Pedidos

### 14. Crear Pedido

**POST** `/api/pedidos`

**Descripción**: Crea un nuevo pedido a partir del carrito del usuario autenticado.

**Headers**:

- `Authorization: Bearer {token}` (requerido si está implementada autenticación)

**Body (JSON)** (opcional, puede ser vacío):

```json
{}
```

**Respuesta exitosa (201 Created)**:

```json
{
  "id": 1,
  "usuarioId": 1,
  "fecha": "2026-03-17T10:30:00Z",
  "estado": 0,
  "estadoNombre": "Pendiente",
  "total": 1199.97,
  "detalles": [
    {
      "id": 1,
      "pedidoId": 1,
      "productoId": 1,
      "nombreProducto": "Laptop Dell XPS 13",
      "cantidad": 1,
      "precioUnitario": 999.99,
      "subtotal": 999.99
    }
  ]
}
```

**Errores**:

- `400 Bad Request`: Carrito vacío
- `401 Unauthorized`: Usuario no autenticado

---

### 15. Obtener Historial de Pedidos del Usuario

**GET** `/api/pedidos/mis-pedidos/{usuarioId}`

**Descripción**: Obtiene todos los pedidos realizados por un usuario específico.

**Parámetros**:

- `usuarioId` (path, requerido): ID del usuario

**Headers**:

- `Authorization: Bearer {token}` (recomendado para seguridad)

**Respuesta exitosa (200 OK)**:

```json
[
  {
    "id": 1,
    "usuarioId": 1,
    "fecha": "2026-03-17T10:30:00Z",
    "estado": 1,
    "estadoNombre": "Procesando",
    "total": 1199.97,
    "detalles": [
      {
        "id": 1,
        "pedidoId": 1,
        "productoId": 1,
        "nombreProducto": "Laptop Dell XPS 13",
        "cantidad": 1,
        "precioUnitario": 999.99,
        "subtotal": 999.99
      }
    ]
  }
]
```

**Errores**:

- `404 Not Found`: Usuario no existe

---

## 🧪 Instrucciones para Probar en Postman

### 1. Crear Colección

1. Abre Postman
2. Crea una nueva colección llamada **TecnoEcommerce API**
3. En la colección, establece la variable de entorno:
   - **Clave**: `base_url`
   - **Valor**: `http://localhost:5247`

### 2. Crear Variables de Entorno

En Postman, crea un nuevo entorno llamado **TecnoEcommerce** con estas variables:

| Variable       | Valor Inicial                 | Descripción                    |
| -------------- | ----------------------------- | ------------------------------ |
| `base_url`     | `http://localhost:5247`       | URL base de la API             |
| `token`        | (vacío, se obtiene del login) | Token JWT para autenticación   |
| `userId`       | (se obtiene del registro)     | ID del usuario autenticado     |
| `productId`    | `1`                           | ID de un producto para pruebas |
| `categorySlug` | `laptops`                     | Categoría para pruebas         |

### 3. Matriz de Pruebas

#### Fase 1: Autenticación

1. **Registrar Usuario** (POST `/api/usuarios/registro`)
   - Cuerpo: email único, contraseña ≥ 6 caracteres
   - Guarda el `id` en variable `userId`

2. **Iniciar Sesión** (POST `/api/usuarios/login`)
   - Cuerpo: credenciales correctas
   - Guarda el `token` en variable `token`

3. **Obtener Perfil** (GET `/api/usuarios/{{userId}}`)
   - Verifica que devuelve datos correctos

#### Fase 2: Productos

4. **Listar Categorías** (GET `/api/categorias`)
   - Debe devolver 5 categorías

5. **Listar Productos** (GET `/api/productos`)
   - Debe devolver lista de productos

6. **Obtener Producto** (GET `/api/productos/{{productId}}`)
   - Debe devolver detalles corretos

7. **Filtrar por Categoría** (GET `/api/productos/categoria/{{categorySlug}}`)
   - Prueba con: `laptops`, `smartphones`, `gaming`, `accesorios`, `monitores`

8. **Buscar Producto** (GET `/api/productos/buscar?q=laptop`)
   - Experimenta con diferentes términos

#### Fase 3: Carrito

9. **Agregar al Carrito** (POST `/api/carrito/agregar`)
   - Header: `Authorization: Bearer {{token}}`
   - Cuerpo: `{"productoId": {{productId}}, "cantidad": 1}`

10. **Ver Carrito** (GET `/api/carrito`)
    - Header: `Authorization: Bearer {{token}}`
    - Debe mostrar los ítems agregados

11. **Agregar Otro Producto** (POST `/api/carrito/agregar`)
    - Cambia `productId` para agregar otro producto

12. **Eliminar del Carrito** (DELETE `/api/carrito/{itemId}`)
    - Header: `Authorization: Bearer {{token}}`
    - Cuerpo vacío

#### Fase 4: Pedidos

13. **Crear Pedido** (POST `/api/pedidos`)
    - Header: `Authorization: Bearer {{token}}`
    - Cuerpo: `{}`
    - Guarda el `orderId` en variable

14. **Ver Mis Pedidos** (GET `/api/pedidos/mis-pedidos/{{userId}}`)
    - Header: `Authorization: Bearer {{token}}`
    - Debe mostrar el pedido creado

### 4. Tests Automatizados en Postman

Ejemplo de script para guardar el token después del login (agregalo en la pestaña **Tests**):

```javascript
if (pm.response.code === 200) {
  var jsonData = pm.response.json();
  pm.environment.set("token", jsonData.token);
  pm.environment.set("userId", jsonData.usuario.id);
  console.log("✅ Token guardado: " + jsonData.token.substring(0, 20) + "...");
  console.log("✅ Usuario ID: " + jsonData.usuario.id);
}
```

---

## ⚠️ Notas Importantes

- **Base de datos**: Asegúrate de que PostgreSQL está corriendo en `localhost:5432` con:
  - Database: `tecnoecommerce`
  - Usuario: `postgres`
  - Contraseña: `postgres`

- **CORS**: La API permite solicitudes desde `http://localhost:3000` y `http://localhost:3001` (Blazor WASM)

- **Autenticación**: Los endpoints protegidos requieren token JWT en el header `Authorization: Bearer {token}`

- **Orden de pruebas**: Siempre regístrate e inicia sesión ANTES de probar carrito y pedidos

- **Datos de prueba**: La base de datos incluye:
  - 5 categorías: Laptops, Smartphones, Gaming, Accesorios, Monitores
  - 20+ productos

---

## 📞 Contacto / Soporte

Si encuentras algún error, verifica:

1. ✅ Que el servidor está corriendo: `dotnet run` en `TecnoEcommerce.API`
2. ✅ Que PostgreSQL está corriendo
3. ✅ La cadena de conexión en `appsettings.json`
4. ✅ Los logs en la consola al ejecutar la API

---

**Última actualización**: Marzo 17, 2026  
**Versión de API**: 1.0  
**Versión de .NET**: 10.0
