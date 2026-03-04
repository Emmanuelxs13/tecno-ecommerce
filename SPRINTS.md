# Plan de Sprints — TecnoEcommerce

Arquitectura: **Modelo - Vista - Controlador (MVC)**  
Framework: **.NET 8 Web API**

---

## Sprint 0 — Arquitectura Base ✅

**Objetivo**: Crear la estructura vacía de los 3 proyectos.

- [x] Crear solución TecnoEcommerce.sln
- [x] Crear proyecto TecnoEcommerce.Modelos (classlib)
- [x] Crear proyecto TecnoEcommerce.Datos (classlib)
- [x] Crear proyecto TecnoEcommerce.API (webapi)
- [x] Configurar referencias: Datos→Modelos, API→Modelos+Datos
- [x] Crear carpetas vacías con .gitkeep
- [x] Limpiar Program.cs con comentarios TODO
- [x] Agregar cadena de conexión placeholder en appsettings.json

---

## Sprint 1 — Capa Modelo (Entidades e Interfaces)

**Proyecto**: `TecnoEcommerce.Modelos`  
**Objetivo**: Definir las entidades del negocio y los contratos (interfaces).

### Entidades (`Entidades/`)

- [x] `Usuario.cs` — Datos del usuario (Id, Nombre, Email, Contraseña, Rol)
- [x] `Categoria.cs` — Categoría de productos (Id, Nombre, Descripcion)
- [x] `Producto.cs` — Producto del catálogo (Id, Nombre, Precio, Stock, CategoriaId)
- [x] `Carrito.cs` — Carrito de compras del usuario (Id, UsuarioId, Items)
- [x] `ItemCarrito.cs` — Ítem dentro del carrito (Id, ProductoId, Cantidad, Precio)
- [x] `Pedido.cs` — Pedido realizado (Id, UsuarioId, Total, Estado, Fecha)
- [x] `DetallePedido.cs` — Línea de detalle del pedido (Id, PedidoId, ProductoId, Cantidad)
- [x] `Envio.cs` — Envío asociado al pedido (Id, PedidoId, Direccion, Estado)

### Enumeraciones (`Enumeraciones/`)

- [x] `Rol.cs` — Cliente, Administrador
- [x] `EstadoPedido.cs` — Pendiente, Procesando, Enviado, Entregado, Cancelado
- [x] `EstadoPago.cs` — Pendiente, Aprobado, Rechazado
- [x] `EstadoEnvio.cs` — Preparando, EnCamino, Entregado

### Interfaces (`Interfaces/`)

- [x] `IRepositorio.cs` — Contrato genérico (ObtenerTodos, ObtenerPorId, Agregar, Actualizar, Eliminar)
- [x] `IProductoRepositorio.cs` — Contrato específico para productos
- [x] `ICarritoRepositorio.cs` — Contrato específico para carrito
- [x] `IPedidoRepositorio.cs` — Contrato específico para pedidos
- [x] `IPagoServicio.cs` — Contrato del servicio de pago
- [x] `IEnvioServicio.cs` — Contrato del servicio de envío

---

## Sprint 2 — Capa Servicios (Lógica de Negocio)

**Proyecto**: `TecnoEcommerce.API`  
**Carpeta**: `Servicios/`  
**Objetivo**: Implementar la lógica de negocio de la aplicación.

- [x] `UsuarioServicio.cs` — Registro, login, perfil
- [x] `ProductoServicio.cs` — Listar, buscar, filtrar productos
- [x] `CategoriaServicio.cs` — Gestión de categorías
- [x] `CarritoServicio.cs` — Agregar, eliminar, vaciar carrito
- [x] `PedidoServicio.cs` — Crear pedido, consultar historial
- [x] `PagoSimuladoServicio.cs` — Simular procesamiento de pago
- [x] `EnvioSimuladoServicio.cs` — Simular seguimiento de envío

---

## Sprint 3 — Capa de Datos (EF Core + Repositorios)

**Proyecto**: `TecnoEcommerce.Datos`  
**Objetivo**: Conectar con SQL Server mediante Entity Framework Core.

### Instalación de paquetes NuGet

```bash
dotnet add TecnoEcommerce.Datos package Microsoft.EntityFrameworkCore
dotnet add TecnoEcommerce.Datos package Microsoft.EntityFrameworkCore.SqlServer
dotnet add TecnoEcommerce.Datos package Microsoft.EntityFrameworkCore.Tools
dotnet add TecnoEcommerce.API package Microsoft.EntityFrameworkCore.Design
```

### Contexto (`Contexto/`)

- [ ] `TecnoEcommerceContexto.cs` — DbContext con DbSets de todas las entidades

### Repositorios (`Repositorios/`)

- [ ] `Repositorio.cs` — Implementación genérica de IRepositorio<T>
- [ ] `ProductoRepositorio.cs` — Repositorio específico de productos
- [ ] `CarritoRepositorio.cs` — Repositorio específico de carrito
- [ ] `PedidoRepositorio.cs` — Repositorio específico de pedidos

### Migraciones

```bash
dotnet ef migrations add MigracionInicial --project TecnoEcommerce.Datos --startup-project TecnoEcommerce.API
dotnet ef database update --project TecnoEcommerce.Datos --startup-project TecnoEcommerce.API
```

---

## Sprint 4 — Capa Controladores (Endpoints REST)

**Proyecto**: `TecnoEcommerce.API`  
**Carpeta**: `Controladores/`  
**Objetivo**: Exponer los endpoints REST y configurar Swagger.

### Controladores

- [ ] `UsuariosControlador.cs` — POST /api/usuarios/registro, POST /api/usuarios/login
- [ ] `ProductosControlador.cs` — GET /api/productos, GET /api/productos/{id}, POST, PUT, DELETE
- [ ] `CategoriasControlador.cs` — CRUD de categorías
- [ ] `CarritoControlador.cs` — GET, POST (agregar ítem), DELETE (eliminar ítem)
- [ ] `PedidosControlador.cs` — POST (crear), GET /api/pedidos/mis-pedidos

### Swagger

- [ ] Configurar Swagger con título y versión en Program.cs

---

## Sprint 5 — Integración y Pruebas Finales

**Objetivo**: Probar el sistema completo end-to-end.

- [ ] Registrar e iniciar sesión como usuario
- [ ] Listar productos y categorías
- [ ] Agregar productos al carrito
- [ ] Crear pedido desde el carrito
- [ ] Simular pago y verificar estado del pedido
- [ ] Simular envío y verificar estado del envío
- [ ] Revisar todos los endpoints en Swagger
