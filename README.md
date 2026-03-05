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

> **Sprint 4 completado** — Migración a EF Core + PostgreSQL. Los repositorios en memoria han sido
> reemplazados por repositorios EF Core conectados a PostgreSQL.
> Próximo: Sprint 5 — Autenticación JWT + pruebas end-to-end.

---

## Sprint 4 — Base de Datos PostgreSQL

### Requisitos

| Herramienta     | Versión mínima | Descarga                                 |
| --------------- | -------------- | ---------------------------------------- |
| PostgreSQL      | 15 o superior  | https://www.postgresql.org/download/     |
| pgAdmin 4       | 7.x            | https://www.pgadmin.org/download/        |
| dotnet-ef (CLI) | 9.x (opcional) | `dotnet tool install --global dotnet-ef` |

### 1 — Crear la base de datos en pgAdmin 4

1. Abrir **pgAdmin 4** → conectarse al servidor local.
2. Clic derecho sobre **Databases** → **Create → Database**.
3. Nombre: `tecnoecommerce`, Encoding: `UTF8`. Confirmar con **Save**.
4. Seleccionar la base de datos recién creada → abrir la pestaña **Query Tool** (`Alt+Shift+Q`).

### 2 — Ejecutar el script de esquema

Con el Query Tool abierto sobre `tecnoecommerce`:

1. Abrir el archivo `database/01_crear_esquema.sql`
   (menú **File → Open File** dentro del Query Tool).
2. Ejecutar con **F5** o el botón ▶.

Esto crea las 10 tablas:

```
categorias · usuarios · productos · carritos · items_carrito
pedidos · detalles_pedido · envios · resenias · pagos
```

### 3 — Poblar la base de datos con datos reales

Ejecutar en el mismo Query Tool:

1. Abrir `database/02_datos_semilla.sql`.
2. Ejecutar con **F5**.

Datos que se insertan:

| Tabla      | Registros                                                           |
| ---------- | ------------------------------------------------------------------- |
| categorías | 6 (Laptops, Smartphones, Accesorios, Gaming, Audio, Almacenamiento) |
| productos  | 20 productos reales                                                 |
| usuarios   | 5 (1 admin + 4 clientes de prueba)                                  |
| pedidos    | 3 (entregado, en proceso, pendiente)                                |
| detalles   | 5 líneas                                                            |
| envíos     | 3                                                                   |
| pagos      | 3                                                                   |
| reseñas    | 4                                                                   |
| carrito    | 1 (usuario Valentina)                                               |

**Credenciales de prueba:**

| Email                     | Contraseña   | Rol           |
| ------------------------- | ------------ | ------------- |
| admin@tecnoecommerce.com  | `admin123`   | Administrador |
| carlos.ramirez@email.com  | `cliente123` | Cliente       |
| laura.martinez@email.com  | `cliente123` | Cliente       |
| andres.torres@email.com   | `cliente123` | Cliente       |
| valentina.lopez@email.com | `cliente123` | Cliente       |

> ⚠️ Las contraseñas del script son hashes BCrypt. En desarrollo puedes usarlas directamente;
> en producción genera nuevos hashes con `BCrypt.Net.BCrypt.HashPassword("tu_contrasena")`.

### 4 — Configurar la cadena de conexión

Editar `TecnoEcommerce.API/appsettings.json` y reemplazar la contraseña:

```json
"ConnectionStrings": {
  "ConexionPrincipal": "Host=localhost;Port=5432;Database=tecnoecommerce;Username=postgres;Password=TU_CONTRASENA_POSTGRES"
}
```

Parámetros de la cadena:

| Parámetro  | Valor por defecto | Descripción                         |
| ---------- | ----------------- | ----------------------------------- |
| `Host`     | `localhost`       | Servidor PostgreSQL                 |
| `Port`     | `5432`            | Puerto por defecto de PostgreSQL    |
| `Database` | `tecnoecommerce`  | Nombre de la base creada en pgAdmin |
| `Username` | `postgres`        | Usuario de PostgreSQL               |
| `Password` | —                 | Contraseña que elegiste al instalar |

### 5 — Ejecutar la API con PostgreSQL

```bash
# Restaurar paquetes NuGet (incluye Npgsql + EF Core)
dotnet restore

# Compilar
dotnet build

# Ejecutar la API
dotnet run --project TecnoEcommerce.API/TecnoEcommerce.API.csproj
```

Al iniciar, la consola mostrará:

```
✅ Conexión a PostgreSQL establecida correctamente.
```

Si aparece ❌, verificar que:

- PostgreSQL está corriendo (servicio `postgresql-x64-15` en Windows).
- La contraseña en `appsettings.json` es correcta.
- La base de datos `tecnoecommerce` existe.

### Diagrama de tablas

```
categorias ──< productos ──< items_carrito >── carritos >── usuarios
                     │                                         │
                     └──< detalles_pedido >── pedidos >────────┘
                     │                            │
                  resenias                   envios, pagos
```

### Arquitectura de acceso a datos (Sprint 4)

```
API Controller
     │
     ▼
  Servicio (sin cambios)
     │  depende de
     ▼
IRepositorio<T> / IProductoRepositorio / ICarritoRepositorio / IPedidoRepositorio
     │  implementado por
     ▼
RepositorioEfCore<T>          → TecnoEcommerce.Datos
ProductoRepositorioEfCore     → TecnoEcommerce.Datos
CarritoRepositorioEfCore      → TecnoEcommerce.Datos
PedidoRepositorioEfCore       → TecnoEcommerce.Datos
     │  usa
     ▼
TiendaContexto (DbContext)    → Npgsql → PostgreSQL
```

Los **servicios y controladores no se modificaron**; solo se cambiaron las
implementaciones concretas en el contenedor DI de `Program.cs`.
Esto cumple el **Principio OCP (Open/Closed)**.

---

---

## Modo Visual con Datos de Prueba

> **Importante:** La rama `feature_frontend` utiliza **datos completamente quemados (hardcoded)**
> con el unico proposito de demostrar la experiencia visual y de usuario del diseno Apple-inspired.

### Por que datos mock?

El objetivo de esta rama es validar el diseno de UI/UX antes de conectar la capa de datos real.
Esto permite iterar sobre la estetica visualmente sin depender del estado de la API o la base de datos.

### Que significa esto?

- Los productos, precios, categorias e imagenes son **ficticios y estaticos** (no provienen de la API).
- El carrito incluye articulos precargados para visualizacion inmediata.
- El historial de pedidos muestra ordenes de ejemplo con lineas de tiempo.
- Los formularios de Login y Registro simulan autenticacion (cualquier credencial valida funciona).
- Las llamadas a la API real ocurren en segundo plano; si fallan, los datos mock se mantienen.

### Productos mock incluidos

| ID  | Producto           | Categoria   | Precio     |
| --- | ------------------ | ----------- | ---------- |
| 1   | MacBook Air M3     | Laptops     | $1,299 USD |
| 2   | iPhone 15 Pro      | Smartphones | $1,199 USD |
| 3   | Samsung Galaxy S24 | Smartphones | $999 USD   |
| 4   | ASUS VivoBook 15   | Laptops     | $899 USD   |
| 5   | LG UltraWide 34"   | Monitores   | $449 USD   |
| 6   | Keychron K2 V2     | Accesorios  | $89 USD    |
| 7   | RTX 4060 8GB       | Gaming      | $399 USD   |
| 8   | HyperX Cloud III   | Gaming      | $149 USD   |

### Paginas rediseñadas en `feature_frontend`

| Pagina      | Archivo                       | Descripcion                                                |
| ----------- | ----------------------------- | ---------------------------------------------------------- |
| Inicio      | `Pages/Inicio.razor`          | Hero, tiles de producto, categorias, reseñas, banner promo |
| Detalle     | `Pages/ProductoDetalle.razor` | Selector de color, specs, cantidad, productos relacionados |
| Carrito     | `Pages/Carrito.razor`         | Bolsa de compra stilizada, resumen de orden, confirmacion  |
| Login       | `Pages/Login.razor`           | Formulario estilo Apple ID, mock auth                      |
| Registro    | `Pages/Registro.razor`        | Creacion de cuenta estilo Apple, validaciones              |
| Mis Pedidos | `Pages/MisPedidos.razor`      | Historial con linea de tiempo de 5 pasos                   |

### Para usar datos reales

Cambiar a la rama `main` o asegurarse de que la API este corriendo en el puerto configurado en `appsettings.json`.
El frontend intentara automaticamente las llamadas a la API; los datos mock solo se muestran como fallback.

---
