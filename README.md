# 🛒 TecnoEcommerce

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server)
![Clean Architecture](https://img.shields.io/badge/Clean%20Architecture-00ADD8?style=for-the-badge)

## 📋 Descripción

**TecnoEcommerce** es una plataforma de comercio electrónico especializada en productos tecnológicos, desarrollada en **C# con .NET 8** siguiendo los principios de **Clean Architecture** y **SOLID**. El proyecto implementa patrones de diseño modernos y mejores prácticas de desarrollo empresarial.

Este sistema permite gestionar un catálogo de productos tecnológicos, procesar pedidos, administrar carritos de compra y simular servicios de pago y envío, proporcionando una base sólida y escalable para un ecommerce real.

---

## 🏗️ Arquitectura

El proyecto está estructurado en 4 capas principales siguiendo **Clean Architecture**:

```
TecnoEcommerce/
│
├── TecnoEcommerce.Domain/           # Capa de Dominio
│   ├── Entities/                    # Entidades del negocio
│   ├── Enums/                       # Enumeraciones
│   └── Interfaces/                  # Contratos de repositorios y servicios
│
├── TecnoEcommerce.Application/      # Capa de Aplicación
│   ├── DTOs/                        # Data Transfer Objects
│   ├── Interfaces/                  # Interfaces de servicios
│   └── Services/                    # Implementación de casos de uso
│
├── TecnoEcommerce.Infrastructure/   # Capa de Infraestructura
│   ├── Data/                        # DbContext y configuraciones EF Core
│   ├── Repositories/                # Implementación de repositorios
│   └── Services/                    # Servicios externos (Pago, Envío)
│
└── TecnoEcommerce.API/              # Capa de Presentación
    ├── Controllers/                 # API Controllers
    └── Program.cs                   # Configuración de la aplicación
```

### 🎯 Principios Aplicados

- ✅ **Clean Architecture**: Separación clara de responsabilidades
- ✅ **SOLID**: Single Responsibility, Open/Closed, Liskov Substitution, Interface Segregation, Dependency Inversion
- ✅ **Dependency Injection**: Inversión de control completa
- ✅ **Repository Pattern**: Abstracción de acceso a datos
- ✅ **Async/Await**: Operaciones asíncronas para mejor rendimiento

---

## 🛠️ Tecnologías

| Tecnología                | Versión | Propósito                |
| ------------------------- | ------- | ------------------------ |
| **.NET**                  | 8.0     | Framework principal      |
| **C#**                    | 12.0    | Lenguaje de programación |
| **Entity Framework Core** | 9.0+    | ORM para acceso a datos  |
| **SQL Server (LocalDB)**  | 2022+   | Base de datos            |
| **Swagger/OpenAPI**       | 3.0     | Documentación de API     |

---

## 📦 Modelo de Datos

### Entidades Principales

- **Usuario**: Gestión de usuarios con roles (Cliente/Administrador)
- **Categoría**: Clasificación de productos
- **Producto**: Catálogo de productos tecnológicos
- **Carrito**: Carrito de compras por usuario
- **ItemCarrito**: Ítems individuales del carrito
- **Pedido**: Órdenes realizadas por los usuarios
- **DetallePedido**: Líneas de detalle de cada pedido

### Servicios

- **IPago**: Interfaz para procesamiento de pagos
- **IEnvioService**: Interfaz para gestión de envíos

---

## 🚀 Instalación y Configuración

### Prerrequisitos

Antes de comenzar, asegúrate de tener instalado:

1. **SDK de .NET 8.0 o superior**
   - Descarga desde: https://dotnet.microsoft.com/download
   - Verifica la instalación:
     ```bash
     dotnet --version
     ```

2. **SQL Server LocalDB** (incluido en Visual Studio)
   - O SQL Server Express: https://www.microsoft.com/sql-server/sql-server-downloads

3. **Git** (opcional, para clonar el repositorio)
   - https://git-scm.com/downloads

4. **Visual Studio 2022** (recomendado) o **Visual Studio Code**
   - Visual Studio: https://visualstudio.microsoft.com/
   - VS Code: https://code.visualstudio.com/

---

### 📥 Paso 1: Clonar o Descargar el Repositorio

```bash
git clone https://github.com/Emmanuelxs13/tecno-ecommerce.git
cd tecno-ecommerce
```

O descarga el ZIP y extráelo en tu carpeta de trabajo.

---

### 📦 Paso 2: Restaurar Paquetes NuGet

Abre una terminal en la carpeta raíz del proyecto y ejecuta:

```bash
dotnet restore
```

Esto descargará todas las dependencias necesarias.

---

### 🗄️ Paso 3: Configurar la Base de Datos

#### 3.1 Verificar Connection String

Abre el archivo `TecnoEcommerce.API/appsettings.json` y verifica la cadena de conexión:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TecnoEcommerceDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

**Nota**: Si usas SQL Server Express en lugar de LocalDB, cambia la cadena de conexión:

```json
"Server=localhost\\SQLEXPRESS;Database=TecnoEcommerceDb;Trusted_Connection=True;MultipleActiveResultSets=true"
```

#### 3.2 Instalar Herramienta EF Core (si no está instalada)

```bash
dotnet tool install --global dotnet-ef
```

Verifica la instalación:

```bash
dotnet ef --version
```

#### 3.3 Crear y Aplicar Migraciones

Navega a la carpeta del proyecto API:

```bash
cd TecnoEcommerce.API
```

Crea la migración inicial:

```bash
dotnet ef migrations add InitialCreate --project ..\TecnoEcommerce.Infrastructure\TecnoEcommerce.Infrastructure.csproj
```

Aplica la migración a la base de datos:

```bash
dotnet ef database update --project ..\TecnoEcommerce.Infrastructure\TecnoEcommerce.Infrastructure.csproj
```

Esto creará la base de datos `TecnoEcommerceDb` con todas las tablas necesarias.

---

### ▶️ Paso 4: Ejecutar la Aplicación

Desde la carpeta `TecnoEcommerce.API`, ejecuta:

```bash
dotnet run
```

O si usas Visual Studio:

1. Abre la solución `TecnoEcommerce.sln`
2. Establece `TecnoEcommerce.API` como proyecto de inicio
3. Presiona `F5` o haz clic en el botón "Play"

La aplicación se iniciará y verás algo como:

```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7001
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5001
```

---

### 📖 Paso 5: Acceder a Swagger (Documentación de API)

Una vez que la aplicación esté ejecutándose, abre tu navegador y ve a:

```
https://localhost:7001
```

O el puerto que te indique la consola. Verás la interfaz de **Swagger UI** con todos los endpoints documentados.

---

## 🧪 Probando la API

### Endpoints Principales

#### 👤 Usuarios

- `POST /api/usuarios/registrar` - Registrar nuevo usuario
- `POST /api/usuarios/login` - Iniciar sesión
- `GET /api/usuarios/{id}` - Obtener usuario por ID

#### 📁 Categorías

- `GET /api/categorias` - Listar todas las categorías
- `POST /api/categorias` - Crear categoría
- `PUT /api/categorias/{id}` - Actualizar categoría
- `DELETE /api/categorias/{id}` - Eliminar categoría

#### 📦 Productos

- `GET /api/productos` - Listar todos los productos
- `GET /api/productos/{id}` - Obtener producto por ID
- `GET /api/productos/categoria/{categoriaId}` - Productos por categoría
- `POST /api/productos` - Crear producto
- `PUT /api/productos/{id}` - Actualizar producto
- `DELETE /api/productos/{id}` - Eliminar producto

#### 🛒 Carrito

- `GET /api/carrito/{usuarioId}` - Obtener carrito del usuario
- `POST /api/carrito/{usuarioId}/items` - Agregar item al carrito
- `PUT /api/carrito/{usuarioId}/items/{itemId}` - Modificar cantidad
- `DELETE /api/carrito/{usuarioId}/items/{itemId}` - Eliminar item
- `DELETE /api/carrito/{usuarioId}` - Vaciar carrito

#### 📋 Pedidos

- `GET /api/pedidos/{id}` - Obtener pedido por ID
- `GET /api/pedidos/usuario/{usuarioId}` - Pedidos de un usuario
- `POST /api/pedidos` - Crear pedido desde carrito
- `PATCH /api/pedidos/{id}/estado` - Cambiar estado del pedido
- `POST /api/pedidos/{id}/cancelar` - Cancelar pedido

---

## 📝 Ejemplo de Uso Completo

### 1️⃣ Registrar un Usuario

```http
POST /api/usuarios/registrar
Content-Type: application/json

{
  "nombre": "Juan Pérez",
  "email": "juan@example.com",
  "password": "MiPassword123",
  "rol": 0
}
```

### 2️⃣ Crear una Categoría

```http
POST /api/categorias
Content-Type: application/json

{
  "nombre": "Laptops",
  "descripcion": "Computadoras portátiles de última generación"
}
```

### 3️⃣ Crear un Producto

```http
POST /api/productos
Content-Type: application/json

{
  "nombre": "Laptop HP Pavilion 15",
  "descripcion": "Intel Core i7, 16GB RAM, 512GB SSD",
  "precio": 3499.99,
  "stock": 10,
  "categoriaId": "{ID-CATEGORIA}"
}
```

### 4️⃣ Agregar Producto al Carrito

```http
POST /api/carrito/{usuarioId}/items
Content-Type: application/json

{
  "productoId": "{ID-PRODUCTO}",
  "cantidad": 1
}
```

### 5️⃣ Crear Pedido

```http
POST /api/pedidos
Content-Type: application/json

{
  "usuarioId": "{ID-USUARIO}",
  "direccionEnvio": "Calle Principal 123, Ciudad"
}
```

---

## 🧑‍💻 Equipo de Desarrollo

| Nombre                  | Rol               | GitHub                                           |
| ----------------------- | ----------------- | ------------------------------------------------ |
| **Juan Esteban Correa** | Backend Developer | -                                                |
| **Andres**              | Backend Developer | -                                                |
| **Emmanuel Berrio**     | Backend Developer | [@Emmanuelxs13](https://github.com/Emmanuelxs13) |

---

## 📚 Documentación Adicional

### Estructura de Carpetas Detallada

```
TecnoEcommerce/
│
├── TecnoEcommerce.sln                    # Archivo de solución
│
├── TecnoEcommerce.Domain/
│   ├── Entities/
│   │   ├── Usuario.cs
│   │   ├── Categoria.cs
│   │   ├── Producto.cs
│   │   ├── Carrito.cs
│   │   ├── ItemCarrito.cs
│   │   ├── Pedido.cs
│   │   ├── DetallePedido.cs
│   │   └── Envio.cs
│   ├── Enums/
│   │   ├── Rol.cs
│   │   ├── EstadoPedido.cs
│   │   ├── EstadoPago.cs
│   │   └── EstadoEnvio.cs
│   └── Interfaces/
│       ├── IRepository.cs
│       ├── IProductoRepository.cs
│       ├── ICarritoRepository.cs
│       ├── IPedidoRepository.cs
│       ├── IPago.cs
│       └── IEnvioService.cs
│
├── TecnoEcommerce.Application/
│   ├── DTOs/
│   │   ├── UsuarioDto.cs
│   │   ├── CategoriaDto.cs
│   │   ├── ProductoDto.cs
│   │   ├── CarritoDto.cs
│   │   └── PedidoDto.cs
│   ├── Interfaces/
│   │   ├── IUsuarioService.cs
│   │   ├── ICategoriaService.cs
│   │   ├── IProductoService.cs
│   │   ├── ICarritoService.cs
│   │   └── IPedidoService.cs
│   └── Services/
│       ├── UsuarioService.cs
│       ├── CategoriaService.cs
│       ├── ProductoService.cs
│       ├── CarritoService.cs
│       └── PedidoService.cs
│
├── TecnoEcommerce.Infrastructure/
│   ├── Data/
│   │   └── TecnoEcommerceDbContext.cs
│   ├── Repositories/
│   │   ├── Repository.cs
│   │   ├── ProductoRepository.cs
│   │   ├── CarritoRepository.cs
│   │   └── PedidoRepository.cs
│   └── Services/
│       ├── PagoSimuladoService.cs
│       └── EnvioSimuladoService.cs
│
└── TecnoEcommerce.API/
    ├── Controllers/
    │   ├── UsuariosController.cs
    │   ├── CategoriasController.cs
    │   ├── ProductosController.cs
    │   ├── CarritoController.cs
    │   └── PedidosController.cs
    ├── Program.cs
    └── appsettings.json
```

---

## ⚠️ Solución de Problemas Comunes

### Error: "No se puede conectar a la base de datos"

**Solución**:

1. Verifica que SQL Server LocalDB esté instalado
2. Ejecuta: `sqllocaldb info`
3. Si no aparece, ejecuta: `sqllocaldb create MSSQLLocalDB`
4. Verifica la cadena de conexión en `appsettings.json`

### Error: "dotnet-ef no se reconoce"

**Solución**:

```bash
dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef
```

### Error: "Las migraciones no se aplican"

**Solución**:

1. Elimina la carpeta `Migrations` si existe
2. Ejecuta:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

### Puerto ya en uso

**Solución**:
Edita `Properties/launchSettings.json` y cambia los puertos en la sección `applicationUrl`.

---

## 🔐 Consideraciones de Seguridad

⚠️ **IMPORTANTE**: Esta es una versión de demostración/educativa.

Para uso en producción, considera implementar:

- ✅ Autenticación JWT
- ✅ Hash de contraseñas con BCrypt
- ✅ HTTPS obligatorio
- ✅ Rate limiting
- ✅ Validación de datos exhaustiva
- ✅ Logging y monitoreo
- ✅ Integración real de pasarelas de pago
- ✅ Servicios de envío reales

---

## 🚀 Próximas Mejoras

- [ ] Implementar autenticación JWT
- [ ] Agregar paginación a los listados
- [ ] Implementar búsqueda y filtros avanzados
- [ ] Agregar imágenes de productos
- [ ] Sistema de valoraciones y comentarios
- [ ] Panel de administración
- [ ] Notificaciones por email
- [ ] Integración con pasarelas de pago reales
- [ ] Implementar cache con Redis
- [ ] Unit Tests y Integration Tests

---

## 📄 Licencia

Este proyecto es de código abierto y está disponible para fines educativos.

---

## 🤝 Contribuciones

Las contribuciones son bienvenidas. Por favor:

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/NuevaCaracteristica`)
3. Commit tus cambios (`git commit -m 'Agregar nueva característica'`)
4. Push a la rama (`git push origin feature/NuevaCaracteristica`)
5. Abre un Pull Request

---

## 📞 Contacto

Para preguntas o sugerencias sobre el proyecto:

- **GitHub**: [@Emmanuelxs13](https://github.com/Emmanuelxs13)
- **Proyecto**: Tecnología en Desarrollo de Software - 4° Semestre

---

<div align="center">

**🛠️ Desarrollado con ❤️ usando .NET 8 y Clean Architecture**

⭐ Si te gustó este proyecto, no olvides darle una estrella en GitHub

</div>
