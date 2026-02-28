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
- [ ] `Usuario.cs` — Datos del usuario (Id, Nombre, Email, Contraseña, Rol)
- [ ] `Categoria.cs` — Categoría de productos (Id, Nombre, Descripcion)
- [ ] `Producto.cs` — Producto del catálogo (Id, Nombre, Precio, Stock, CategoriaId)
- [ ] `Carrito.cs` — Carrito de compras del usuario (Id, UsuarioId, Items)
- [ ] `ItemCarrito.cs` — Ítem dentro del carrito (Id, ProductoId, Cantidad, Precio)
- [ ] `Pedido.cs` — Pedido realizado (Id, UsuarioId, Total, Estado, Fecha)
- [ ] `DetallePedido.cs` — Línea de detalle del pedido (Id, PedidoId, ProductoId, Cantidad)
- [ ] `Envio.cs` — Envío asociado al pedido (Id, PedidoId, Direccion, Estado)

### Enumeraciones (`Enumeraciones/`)
- [ ] `Rol.cs` — Cliente, Administrador
- [ ] `EstadoPedido.cs` — Pendiente, Procesando, Enviado, Entregado, Cancelado
- [ ] `EstadoPago.cs` — Pendiente, Aprobado, Rechazado
- [ ] `EstadoEnvio.cs` — Preparando, EnCamino, Entregado

### Interfaces (`Interfaces/`)
- [ ] `IRepositorio.cs` — Contrato genérico (ObtenerTodos, ObtenerPorId, Agregar, Actualizar, Eliminar)
- [ ] `IProductoRepositorio.cs` — Contrato específico para productos
- [ ] `ICarritoRepositorio.cs` — Contrato específico para carrito
- [ ] `IPedidoRepositorio.cs` — Contrato específico para pedidos
- [ ] `IPagoServicio.cs` — Contrato del servicio de pago
- [ ] `IEnvioServicio.cs` — Contrato del servicio de envío

---

## Sprint 2 — Capa Servicios (Lógica de Negocio)
**Proyecto**: `TecnoEcommerce.API`  
**Carpeta**: `Servicios/`  
**Objetivo**: Implementar la lógica de negocio de la aplicación.

- [ ] `UsuarioServicio.cs` — Registro, login, perfil
- [ ] `ProductoServicio.cs` — Listar, buscar, filtrar productos
- [ ] `CategoriaServicio.cs` — Gestión de categorías
- [ ] `CarritoServicio.cs` — Agregar, eliminar, vaciar carrito
- [ ] `PedidoServicio.cs` — Crear pedido, consultar historial
- [ ] `PagoSimuladoServicio.cs` — Simular procesamiento de pago
- [ ] `EnvioSimuladoServicio.cs` — Simular seguimiento de envío

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
