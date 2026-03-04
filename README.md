# TecnoEcommerce

Plataforma de comercio electrónico desarrollada con **.NET 9** aplicando la arquitectura **Modelo - Vista - Controlador (MVC)**, distribuida en cuatro proyectos .NET independientes que separan claramente las responsabilidades del sistema.

---

## ¿Qué es la Arquitectura MVC?

**MVC (Modelo - Vista - Controlador)** es un patrón de diseño de software que divide la aplicación en tres capas con responsabilidades bien definidas, haciendo el código más organizado, fácil de entender y de mantener.

```
+-------------------+       +-------------------+       +-------------------+
|                   |       |                   |       |                   |
|      MODELO       | <---- |   CONTROLADOR     | <---- |   VISTA           |
|                   |       |                   |       |  (Blazor WASM)    |
|  - Entidades      |       |  - Recibe HTTP    |       |                   |
|  - Reglas de      |       |  - Delega al      |       |  Componentes .razor
|    negocio        |       |    Servicio       |       |  Páginas de UI    |
|  - Repositorios   |       |  - Devuelve JSON  |       |                   |
|                   | ----> |                   | ----> |                   |
+-------------------+       +-------------------+       +-------------------+
```

### Las tres capas en TecnoEcommerce

| Capa            | Proyecto(s)                                       | Responsabilidad                                   |
| --------------- | ------------------------------------------------- | ------------------------------------------------- |
| **M**odelo      | `TecnoEcommerce.Modelos` + `TecnoEcommerce.Datos` | Entidades, DTOs, interfaces, acceso a datos       |
| **V**ista       | `TecnoEcommerce.Web` (Blazor WASM)                | UI en el navegador, llama a la API via HttpClient |
| **C**ontrolador | `TecnoEcommerce.API` – carpeta `Controladores/`   | Endpoints REST, delega lógica a `Servicios/`      |

### Flujo de una solicitud

```
Blazor (navegador)
      │  GET /api/productos
      ▼
ProductosController        ← Recibe la petición HTTP
      │  llama a
      ▼
ProductoServicio            ← Aplica la lógica de negocio
      │  llama a
      ▼
IProductoRepositorio        ← Acceso a datos (en memoria ahora / EF Core en Sprint 4)
      │  devuelve datos
      ▼
ProductoServicio            ← Mapea a DTO
      ▼
ProductosController         ← HTTP 200 OK + JSON
      ▼
Blazor (navegador)          ← Renderiza los productos
```

---

## Estructura del Proyecto

```
TecnoEcommerce.sln
├── TecnoEcommerce.Modelos      ← Capa M parte 1: definición del dominio
├── TecnoEcommerce.Datos        ← Capa M parte 2: acceso a datos (EF Core, Sprint 4)
├── TecnoEcommerce.API          ← Capa C: API REST (controladores + servicios)
└── TecnoEcommerce.Web          ← Capa V: frontend Blazor WebAssembly
```

### Dependencias entre proyectos

```
TecnoEcommerce.Web
    └── HttpClient → TecnoEcommerce.API

TecnoEcommerce.API
    ├── → TecnoEcommerce.Modelos
    └── → TecnoEcommerce.Datos

TecnoEcommerce.Datos
    └── → TecnoEcommerce.Modelos

TecnoEcommerce.Modelos
    (sin dependencias externas)
```

---

## Detalle de cada Proyecto

### TecnoEcommerce.Modelos

| Carpeta          | Contenido                                                                                                                                                                                                   |
| ---------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `Entidades/`     | Usuario, Producto, Categoria, Carrito, ItemCarrito, Pedido, DetallePedido, Envio                                                                                                                            |
| `Enumeraciones/` | Rol, EstadoPedido, EstadoPago, EstadoEnvio                                                                                                                                                                  |
| `DTOs/`          | Objetos de transferencia (entrada/salida de la API)                                                                                                                                                         |
| `Interfaces/`    | IRepositorio\<T\>, IProductoRepositorio, ICarritoRepositorio, IPedidoRepositorio, IUsuarioServicio, IProductoServicio, ICategoriaServicio, ICarritoServicio, IPedidoServicio, IPagoServicio, IEnvioServicio |

### TecnoEcommerce.Datos

| Carpeta         | Contenido                                      |
| --------------- | ---------------------------------------------- |
| `Contexto/`     | `TecnoEcommerceContexto` (DbContext, Sprint 4) |
| `Repositorios/` | Repositorios EF Core (Sprint 4)                |

### TecnoEcommerce.API

| Carpeta          | Contenido                                                                                                                                                                                                          |
| ---------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `Controladores/` | UsuariosController, ProductosController, CategoriasController, CarritoController, PedidosController                                                                                                                |
| `Servicios/`     | UsuarioServicio, ProductoServicio, CategoriaServicio, CarritoServicio, PedidoServicio, PagoSimuladoServicio, EnvioSimuladoServicio                                                                                 |
| `Repositorios/`  | Implementaciones en memoria (temporales hasta Sprint 4): RepositorioEnMemoria\<T\>, ProductoRepositorioEnMemoria, CarritoRepositorioEnMemoria, PedidoRepositorioEnMemoria, EnvioRepositorioEnMemoria, DatosSemilla |

### TecnoEcommerce.Web (Blazor WASM)

| Carpeta      | Contenido                                                                                                            |
| ------------ | -------------------------------------------------------------------------------------------------------------------- |
| `Pages/`     | Inicio, ProductoDetalle, Carrito, Login, Registro, MisPedidos                                                        |
| `Shared/`    | NavMenu, MainLayout, ProductoCard, CargandoSpinner, MensajeAlerta                                                    |
| `Servicios/` | SesionServicio, UsuarioApiServicio, ProductoApiServicio, CategoriaApiServicio, CarritoApiServicio, PedidoApiServicio |

---

## Tecnologías

| Tecnología            | Versión | Uso                       |
| --------------------- | ------- | ------------------------- |
| .NET                  | 9.0     | Framework principal       |
| ASP.NET Core Web API  | 9.0     | API REST (Controladores)  |
| Blazor WebAssembly    | 9.0     | Frontend (Vista)          |
| Entity Framework Core | 9.x     | Acceso a datos (Sprint 4) |
| PostgreSQL            | -       | Base de datos (Sprint 4)  |
| Swagger / Scalar      | -       | Documentación de la API   |

---

## Plan de Sprints

| Sprint   | Capa            | Estado        | Objetivo                               |
| -------- | --------------- | ------------- | -------------------------------------- |
| Sprint 0 | Solución        | ✅ Completado | Estructura base de 4 proyectos         |
| Sprint 1 | Modelos         | ✅ Completado | Entidades, Enumeraciones, Interfaces   |
| Sprint 2 | API / Servicios | ✅ Completado | Implementación de servicios de negocio |
| Sprint 3 | Web (Blazor)    | ✅ Completado | UI completa, servicios API, CORS       |
| Sprint 4 | Datos           | 🔜 Siguiente  | EF Core + PostgreSQL + migraciones     |
| Sprint 5 | Integración     | 🔜 Pendiente  | Pruebas end-to-end y refinamiento      |

Ver [SPRINTS.md](SPRINTS.md) para el detalle completo.

---

## Cómo Ejecutar y Probar la Aplicación

### Requisitos Previos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9)
- Visual Studio 2022 (17.8+) o VS Code con extensión C#
- Un navegador moderno (Chrome, Edge, Firefox)

> **Nota:** En este Sprint 3 la API usa repositorios **en memoria**.
> No se necesita instalar ninguna base de datos para probar la aplicación.
> Los datos se reinician cada vez que se detiene la API.

---

### Paso 1 — Clonar y restaurar

```bash
git clone https://github.com/Emmanuelxs13/tecno-ecommerce.git
cd tecno-ecommerce

dotnet restore
dotnet build
```

---

### Paso 2 — Ejecutar la API

Abrir una terminal en la raíz del repositorio y ejecutar:

```bash
dotnet run --project TecnoEcommerce.API/TecnoEcommerce.API.csproj
```

La API estará disponible en:

| URL                      | Descripción                     |
| ------------------------ | ------------------------------- |
| `http://localhost:5247`  | API REST (HTTP)                 |
| `http://localhost:5247/` | Swagger UI (solo en desarrollo) |

> Al iniciar, la API carga automáticamente **4 categorías** y **8 productos** de ejemplo.

---

### Paso 3 — Ejecutar el Frontend Blazor

Abrir una **segunda terminal** en la raíz del repositorio:

```bash
dotnet run --project TecnoEcommerce.Web/TecnoEcommerce.Web.csproj
```

El frontend estará disponible en:

| URL                      | Descripción         |
| ------------------------ | ------------------- |
| `http://localhost:5074`  | Blazor WASM (HTTP)  |
| `https://localhost:7154` | Blazor WASM (HTTPS) |

---

### Paso 4 — Probar el flujo completo

Abrir `http://localhost:5074` en el navegador y seguir esta secuencia:

#### 1. Registro e inicio de sesión

- Ir a **Registro** (menú superior)
- Completar el formulario y crear la cuenta
- Ir a **Login** e iniciar sesión con el usuario creado

#### 2. Explorar el catálogo

- La página de **Inicio** muestra los 8 productos de ejemplo
- Usar el buscador o filtrar por categoría
- Hacer clic en cualquier producto para ver su detalle

#### 3. Carrito de compras

- Desde la página de detalle, hacer clic en **"Agregar al carrito"**
- Ir a **Carrito** (menú superior)
- Verificar que el producto aparece con su precio y cantidad
- Se puede aumentar/disminuir cantidad o eliminar items

#### 4. Realizar un pedido

- Desde el carrito, hacer clic en **"Proceder al pago"**
- Ingresar una dirección de entrega
- Confirmar el pedido (el pago y envío son simulados)

#### 5. Historial de pedidos

- Ir a **Mis Pedidos** (menú superior)
- Verificar que el pedido creado aparece con su estado

---

### Probar la API directamente con Swagger

Navegar a `http://localhost:5247/` para acceder a Swagger UI.

Endpoints disponibles:

| Método | Ruta                                      | Descripción                |
| ------ | ----------------------------------------- | -------------------------- |
| POST   | `/api/usuarios/registro`                  | Crear cuenta               |
| POST   | `/api/usuarios/login`                     | Iniciar sesión             |
| GET    | `/api/productos`                          | Listar todos los productos |
| GET    | `/api/productos/{id}`                     | Obtener producto por ID    |
| GET    | `/api/productos/categoria/{id}`           | Productos por categoría    |
| GET    | `/api/productos/buscar?nombre=...`        | Buscar por nombre          |
| GET    | `/api/categorias`                         | Listar categorías          |
| GET    | `/api/carrito/{usuarioId}`                | Ver carrito del usuario    |
| POST   | `/api/carrito/{usuarioId}/items`          | Agregar item al carrito    |
| DELETE | `/api/carrito/{usuarioId}/items/{itemId}` | Eliminar item              |
| POST   | `/api/pedidos/{usuarioId}`                | Crear pedido               |
| GET    | `/api/pedidos/mis-pedidos/{usuarioId}`    | Ver mis pedidos            |

---

## Datos de Ejemplo (Precargados)

Al iniciar la API se cargan automáticamente:

**Categorías:** Laptops, Smartphones, Accesorios, Gaming

**Productos:**

| Nombre                   | Categoría   | Precio    |
| ------------------------ | ----------- | --------- |
| ASUS VivoBook 15         | Laptops     | $899.99   |
| MacBook Air M2           | Laptops     | $1,299.99 |
| iPhone 15 Pro            | Smartphones | $1,199.99 |
| Samsung Galaxy S24       | Smartphones | $999.99   |
| Monitor LG UltraWide 34" | Accesorios  | $449.99   |
| Teclado Keychron K2      | Accesorios  | $89.99    |
| NVIDIA GeForce RTX 4060  | Gaming      | $399.99   |
| HyperX Cloud III         | Gaming      | $149.99   |

---

## Cadena de Conexión (Sprint 4)

Configurada en `TecnoEcommerce.API/appsettings.json` (se activa en Sprint 4):

```json
"ConnectionStrings": {
  "ConexionPrincipal": "Host=localhost;Database=TecnoEcommerceDB;Username=postgres;Password=tu_password"
}
```

---

## Autores

- Juan Esteban Correa
- Andrés Quiroz Gómez
- Emmanuel Berrío

---

## Estado del Proyecto

> **Sprint 3 completado** — Frontend Blazor WebAssembly + API REST con repositorios en memoria.
> La aplicación es completamente funcional para pruebas sin base de datos.
> Próximo: Sprint 4 — Migración a EF Core + PostgreSQL.

---
