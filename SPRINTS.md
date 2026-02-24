# 🎯 Plan de Desarrollo por Sprints - TecnoEcommerce

## 📅 Roadmap de Desarrollo

Este documento organiza el desarrollo del proyecto **TecnoEcommerce** en sprints manejables, siguiendo una metodología ágil.

---

## 🏗️ Sprint 0: Arquitectura Base ✅

**Estado**: COMPLETADO

### Objetivos

- [x] Crear estructura de solución con 4 proyectos
- [x] Configurar referencias entre proyectos
- [x] Crear estructura de carpetas (Clean Architecture)
- [x] Configurar .gitignore
- [x] Crear README.md
- [x] Crear script de instalación de dependencias
- [x] Instalar paquetes NuGet básicos

### Entregables

- Solución compilable
- Arquitectura de carpetas lista
- Documentación inicial

---

## 🎯 Sprint 1: Domain Layer (Capa de Dominio)

**Duración estimada**: 1-2 días  
**Estado**: PENDIENTE

### Objetivos

Implementar todas las entidades del negocio con su lógica de dominio.

### Tareas

#### 1.1 Enumeraciones

- [ ] Crear `Rol.cs` (CLIENTE, ADMINISTRADOR)
- [ ] Crear `EstadoPedido.cs` (PENDIENTE, PAGADO, EN_PREPARACION, ENVIADO, ENTREGADO, CANCELADO)
- [ ] Crear `EstadoPago.cs` (PENDIENTE, PROCESANDO, APROBADO, RECHAZADO, REEMBOLSADO)
- [ ] Crear `EstadoEnvio.cs` (PREPARANDO, EN_TRANSITO, EN_DISTRIBUCION, ENTREGADO, DEVUELTO)

#### 1.2 Entidades

- [ ] Crear `Usuario.cs` con métodos:
  - Registrar()
  - IniciarSesion()
  - ActualizarPerfil()
- [ ] Crear `Categoria.cs` con métodos:
  - Crear()
  - Editar()
- [ ] Crear `Producto.cs` con métodos:
  - Crear()
  - Editar()
  - ActualizarStock()
  - VerificarDisponibilidad()
- [ ] Crear `Carrito.cs` con métodos:
  - Crear()
  - AgregarItem()
  - ModificarItem()
  - EliminarItem()
  - VaciarCarrito()
  - CalcularTotal()
- [ ] Crear `ItemCarrito.cs` con métodos:
  - Crear()
  - ActualizarCantidad()
  - CalcularSubtotal()
- [ ] Crear `Pedido.cs` con métodos:
  - Crear()
  - AgregarDetalle()
  - CambiarEstado()
  - CalcularTotal()
  - Cancelar()
- [ ] Crear `DetallePedido.cs` con métodos:
  - Crear()
  - CalcularSubtotal()
- [ ] Crear `Envio.cs` e `InfoRastreo.cs`

#### 1.3 Interfaces del Dominio

- [ ] Crear `IRepository<T>` (genérico)
- [ ] Crear `IProductoRepository`
- [ ] Crear `ICarritoRepository`
- [ ] Crear `IPedidoRepository`
- [ ] Crear `IPago` (interfaz de servicio de pago)
- [ ] Crear `IEnvioService` (interfaz de servicio de envío)

### Criterios de Aceptación

- Todas las entidades tienen constructores privados
- Métodos de creación estáticos implementados
- Validaciones básicas en métodos de dominio
- Código comentado explicando cada método
- Compilación exitosa sin errores

---

## 🎯 Sprint 2: Application Layer (Capa de Aplicación)

**Duración estimada**: 2-3 días  
**Estado**: PENDIENTE

### Objetivos

Implementar DTOs, interfaces de servicios y casos de uso.

### Tareas

#### 2.1 DTOs (Data Transfer Objects)

- [ ] Crear `UsuarioDto`, `RegistrarUsuarioDto`, `LoginDto`
- [ ] Crear `CategoriaDto`, `CrearCategoriaDto`
- [ ] Crear `ProductoDto`, `CrearProductoDto`, `ActualizarProductoDto`
- [ ] Crear `CarritoDto`, `ItemCarritoDto`, `AgregarItemCarritoDto`
- [ ] Crear `PedidoDto`, `DetallePedidoDto`, `CrearPedidoDto`

#### 2.2 Interfaces de Servicios

- [ ] Crear `IUsuarioService`
- [ ] Crear `ICategoriaService`
- [ ] Crear `IProductoService`
- [ ] Crear `ICarritoService`
- [ ] Crear `IPedidoService`

#### 2.3 Implementación de Servicios

- [ ] Implementar `UsuarioService`:
  - GetByIdAsync()
  - RegistrarAsync()
  - LoginAsync()
- [ ] Implementar `CategoriaService`:
  - GetAllAsync()
  - GetByIdAsync()
  - CreateAsync()
  - UpdateAsync()
  - DeleteAsync()
- [ ] Implementar `ProductoService`:
  - GetAllAsync()
  - GetByIdAsync()
  - GetByCategoriaAsync()
  - CreateAsync()
  - UpdateAsync()
  - DeleteAsync()
- [ ] Implementar `CarritoService`:
  - GetByUsuarioIdAsync()
  - AgregarItemAsync()
  - ModificarItemAsync()
  - EliminarItemAsync()
  - VaciarCarritoAsync()
- [ ] Implementar `PedidoService`:
  - GetByIdAsync()
  - GetByUsuarioIdAsync()
  - CrearPedidoDesdeCarritoAsync()
  - CambiarEstadoAsync()
  - CancelarPedidoAsync()

### Criterios de Aceptación

- Servicios implementan sus interfaces
- Lógica de negocio separada de infraestructura
- Uso de async/await en todos los métodos
- Mapeo correcto entre entidades y DTOs
- Compilación exitosa

---

## 🎯 Sprint 3: Infrastructure Layer (Capa de Infraestructura)

**Duración estimada**: 2-3 días  
**Estado**: PENDIENTE

### Objetivos

Implementar acceso a datos, repositorios y servicios externos.

### Tareas

#### 3.1 Configuración de Base de Datos

- [ ] Crear `TecnoEcommerceDbContext.cs`
- [ ] Configurar DbSets para todas las entidades
- [ ] Configurar relaciones entre entidades en `OnModelCreating`
- [ ] Configurar propiedades (Required, MaxLength, Decimals, etc.)
- [ ] Actualizar `appsettings.json` con connection string

#### 3.2 Implementación de Repositorios

- [ ] Implementar `Repository<T>` (genérico):
  - GetByIdAsync()
  - GetAllAsync()
  - AddAsync()
  - UpdateAsync()
  - DeleteAsync()
- [ ] Implementar `ProductoRepository`:
  - Heredar de Repository<Producto>
  - Implementar GetByCategoriaAsync()
  - Implementar GetDisponiblesAsync()
  - Incluir navegación de Categoria
- [ ] Implementar `CarritoRepository`:
  - Heredar de Repository<Carrito>
  - Implementar GetByUsuarioIdAsync()
  - Incluir navegación de Items y Productos
- [ ] Implementar `PedidoRepository`:
  - Heredar de Repository<Pedido>
  - Implementar GetByUsuarioIdAsync()
  - Incluir navegación de Detalles y Productos

#### 3.3 Servicios de Infraestructura

- [ ] Implementar `PagoSimuladoService`:
  - ProcesarPago()
  - ValidarPago()
  - ObtenerEstado()
- [ ] Implementar `EnvioSimuladoService`:
  - GenerarEnvio()
  - ActualizarEstado()
  - RastrearEnvio()

#### 3.4 Migraciones

- [ ] Crear migración inicial
- [ ] Aplicar migración a la base de datos
- [ ] Verificar tablas creadas correctamente

### Criterios de Aceptación

- DbContext configurado correctamente
- Repositorios implementan sus interfaces
- Base de datos creada y migrada
- Servicios simulados funcionando
- Compilación exitosa

---

## 🎯 Sprint 4: API Layer (Capa de Presentación)

**Duración estimada**: 2-3 días  
**Estado**: PENDIENTE

### Objetivos

Implementar controladores REST y configurar la API.

### Tareas

#### 4.1 Configuración de Program.cs

- [ ] Configurar DbContext con dependency injection
- [ ] Registrar repositorios (AddScoped)
- [ ] Registrar servicios de aplicación (AddScoped)
- [ ] Registrar servicios de infraestructura (AddScoped)
- [ ] Configurar Swagger
- [ ] Configurar CORS (si es necesario)
- [ ] Configurar middleware de excepciones

#### 4.2 Implementación de Controllers

- [ ] Crear `UsuariosController`:
  - POST /api/usuarios/registrar
  - POST /api/usuarios/login
  - GET /api/usuarios/{id}
- [ ] Crear `CategoriasController`:
  - GET /api/categorias
  - GET /api/categorias/{id}
  - POST /api/categorias
  - PUT /api/categorias/{id}
  - DELETE /api/categorias/{id}
- [ ] Crear `ProductosController`:
  - GET /api/productos
  - GET /api/productos/{id}
  - GET /api/productos/categoria/{categoriaId}
  - POST /api/productos
  - PUT /api/productos/{id}
  - DELETE /api/productos/{id}
- [ ] Crear `CarritoController`:
  - GET /api/carrito/{usuarioId}
  - POST /api/carrito/{usuarioId}/items
  - PUT /api/carrito/{usuarioId}/items/{itemId}
  - DELETE /api/carrito/{usuarioId}/items/{itemId}
  - DELETE /api/carrito/{usuarioId}
- [ ] Crear `PedidosController`:
  - GET /api/pedidos/{id}
  - GET /api/pedidos/usuario/{usuarioId}
  - POST /api/pedidos
  - PATCH /api/pedidos/{id}/estado
  - POST /api/pedidos/{id}/cancelar

#### 4.3 Documentación

- [ ] Agregar comentarios XML a los endpoints
- [ ] Configurar Swagger con descripciones
- [ ] Probar todos los endpoints en Swagger

### Criterios de Aceptación

- Todos los controllers implementados
- Inyección de dependencias configurada
- Swagger funcionando correctamente
- Endpoints probados y funcionando
- Manejo de errores implementado
- Compilación y ejecución exitosa

---

## 🎯 Sprint 5: Pruebas y Ajustes Finales

**Duración estimada**: 1-2 días  
**Estado**: PENDIENTE

### Objetivos

Probar flujos completos y realizar ajustes finales.

### Tareas

#### 5.1 Pruebas de Integración

- [ ] Probar flujo completo de registro de usuario
- [ ] Probar flujo completo de creación de producto
- [ ] Probar flujo completo de agregar al carrito
- [ ] Probar flujo completo de crear pedido
- [ ] Probar flujo completo de cancelar pedido

#### 5.2 Validaciones y Manejo de Errores

- [ ] Revisar validaciones en todos los endpoints
- [ ] Mejorar mensajes de error
- [ ] Agregar try-catch donde sea necesario
- [ ] Probar casos de error (stock insuficiente, etc.)

#### 5.3 Optimizaciones

- [ ] Revisar queries N+1
- [ ] Agregar índices si es necesario
- [ ] Optimizar includes en repositorios

#### 5.4 Documentación Final

- [ ] Actualizar README con ejemplos reales
- [ ] Documentar colección de Postman/Thunder Client
- [ ] Crear guía de pruebas

### Criterios de Aceptación

- Todos los flujos funcionan correctamente
- Errores manejados apropiadamente
- Documentación completa
- Proyecto listo para producción (demo)

---

## 📊 Resumen de Estimaciones

| Sprint   | Descripción          | Duración      | Complejidad |
| -------- | -------------------- | ------------- | ----------- |
| Sprint 0 | Arquitectura Base    | ✅ Completado | Baja        |
| Sprint 1 | Domain Layer         | 1-2 días      | Media       |
| Sprint 2 | Application Layer    | 2-3 días      | Media-Alta  |
| Sprint 3 | Infrastructure Layer | 2-3 días      | Media-Alta  |
| Sprint 4 | API Layer            | 2-3 días      | Media       |
| Sprint 5 | Pruebas y Ajustes    | 1-2 días      | Baja-Media  |

**Total estimado**: 9-15 días de desarrollo

---

## 🎯 Metodología de Trabajo

### Para cada Sprint:

1. **Planificación** (15 min):
   - Revisar objetivos del sprint
   - Confirmar tareas a realizar

2. **Desarrollo** (según duración):
   - Codificar siguiendo Clean Architecture
   - Aplicar principios SOLID
   - Comentar el código
   - Hacer commits frecuentes

3. **Revisión** (30 min):
   - Compilar y verificar que no hay errores
   - Probar funcionalidad implementada
   - Revisar que cumple criterios de aceptación

4. **Retrospectiva** (15 min):
   - Documentar lecciones aprendidas
   - Ajustar plan si es necesario

---

## 📝 Notas Importantes

- ✅ Hacer commits pequeños y frecuentes
- ✅ Seguir nomenclatura de commits: `feat:`, `fix:`, `refactor:`, etc.
- ✅ Probar cada funcionalidad antes de pasar al siguiente sprint
- ✅ Mantener el README actualizado
- ✅ Documentar decisiones importantes

---

## 🚀 ¿Listo para Empezar?

El proyecto está configurado y listo para comenzar con **Sprint 1: Domain Layer**.

Cuando estés listo, avísame y comenzamos a codificar el primer sprint. 🎯
