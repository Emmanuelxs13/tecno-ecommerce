# TecnoEcommerce

Plataforma de comercio electronico desarrollada con **.NET 8** aplicando la arquitectura **Modelo - Vista - Controlador (MVC)**, dividida en tres proyectos independientes que representan cada capa de la arquitectura.

---

## ¿Que es la Arquitectura MVC?

**MVC (Modelo - Vista - Controlador)** es un patron de diseño de software que separa una aplicacion en tres capas con responsabilidades bien definidas. El objetivo principal es que cada parte del sistema se encargue de una sola cosa, lo que hace el codigo mas organizado, facil de entender y de mantener.

```
+-------------------+       +-------------------+       +-------------------+
|                   |       |                   |       |                   |
|      MODELO       | <---- |   CONTROLADOR     | <---- |   CLIENTE/VISTA   |
|                   |       |                   |       |  (Swagger / App)  |
|  - Entidades      |       |  - Recibe la      |       |                   |
|  - Reglas de      |       |    solicitud HTTP |       |  Envia peticiones |
|    negocio        |       |  - Llama al       |       |  GET, POST, PUT,  |
|  - Acceso a BD    |       |    Servicio       |       |  DELETE           |
|                   |       |  - Devuelve       |       |                   |
|                   | ----> |    la respuesta   | ----> |                   |
+-------------------+       +-------------------+       +-------------------+
```

### Las tres capas explicadas

#### M — Modelo (TecnoEcommerce.Modelos + TecnoEcommerce.Datos)
Es el corazon de la aplicacion. Contiene:
- **Las entidades**: las clases que representan los datos del negocio (Producto, Usuario, Pedido, etc.)
- **Las interfaces**: los contratos que definen que operaciones se pueden hacer
- **El acceso a datos**: la conexion con la base de datos a traves de Entity Framework Core
- **Los repositorios**: las clases que ejecutan las consultas a la base de datos

> El Modelo NO sabe nada sobre como se muestra la informacion ni quien la pidio.
> Solo sabe como guardar, leer y manipular datos.

#### V — Vista (Cliente externo)
En una API REST, la Vista no es una pagina web dentro del proyecto.
La Vista es el **cliente que consume la API**: puede ser Swagger, una app movil, un frontend en React/Angular, o cualquier aplicacion externa que haga peticiones HTTP.

> En este proyecto, Swagger hace las veces de Vista durante el desarrollo.

#### C — Controlador (TecnoEcommerce.API — Controladores y Servicios)
Es el intermediario entre la Vista y el Modelo. Su trabajo es:
1. Recibir la solicitud HTTP del cliente (GET /api/productos)
2. Delegar la logica al Servicio correspondiente
3. Pedir los datos al Repositorio (a traves del Servicio)
4. Devolver la respuesta HTTP con los datos al cliente

> El Controlador NO contiene logica de negocio compleja. Solo coordina.
> La logica vive en los Servicios.

### Flujo de una solicitud en TecnoEcommerce

```
Cliente (Swagger)
      |
      | GET /api/productos
      v
ProductosControlador       <- Recibe la peticion HTTP
      |
      | llama a
      v
ProductoServicio           <- Aplica la logica de negocio
      |
      | llama a
      v
ProductoRepositorio        <- Consulta la base de datos
      |
      | usa
      v
TecnoEcommerceContexto     <- Entity Framework Core / SQL Server
      |
      | devuelve datos
      v
ProductoServicio           <- Prepara la respuesta (DTO)
      |
      v
ProductosControlador       <- Devuelve HTTP 200 OK + JSON
      |
      v
Cliente (Swagger)          <- Muestra el resultado
```

---

## Estructura del Proyecto

Este proyecto implementa MVC dividido en **3 proyectos de .NET** independientes:

```
TecnoEcommerce.sln
|
+-- TecnoEcommerce.Modelos      (Capa M — parte 1: definicion)
+-- TecnoEcommerce.Datos        (Capa M — parte 2: acceso a datos)
+-- TecnoEcommerce.API          (Capa C — controladores y servicios)
```

### Dependencias entre proyectos

```
TecnoEcommerce.API
    |-- referencia --> TecnoEcommerce.Modelos
    |-- referencia --> TecnoEcommerce.Datos

TecnoEcommerce.Datos
    |-- referencia --> TecnoEcommerce.Modelos

TecnoEcommerce.Modelos
    (no depende de nadie)
```

La regla es: las capas superiores conocen a las inferiores, pero nunca al reves.

---

## Detalle de cada Proyecto

### TecnoEcommerce.Modelos — Definicion del Negocio

Este proyecto es el nucleo de toda la aplicacion. Define **que existe** en el sistema sin importar como se guarda o quien lo pide.

| Carpeta | Que contiene | Para que sirve |
|---------|-------------|----------------|
| `Entidades/` | Clases C# (Usuario, Producto, Pedido...) | Representan las tablas de la base de datos y los objetos del negocio |
| `Enumeraciones/` | Enums (Rol, EstadoPedido...) | Definen valores fijos y controlados, evitan "magic strings" |
| `DTOs/` | Clases de transferencia de datos | Controlan que informacion entra y sale de la API (sin exponer la entidad completa) |
| `Interfaces/` | Contratos (IRepositorio, IServicio...) | Definen que metodos deben implementar los repositorios y servicios |

**Entidades planificadas**: Usuario, Producto, Categoria, Carrito, ItemCarrito, Pedido, DetallePedido, Envio

**Enumeraciones planificadas**: Rol, EstadoPedido, EstadoPago, EstadoEnvio

**Interfaces planificadas**: IRepositorio, IProductoRepositorio, ICarritoRepositorio, IPedidoRepositorio, IPagoServicio, IEnvioServicio

---

### TecnoEcommerce.Datos — Acceso a la Base de Datos

Este proyecto se encarga exclusivamente de comunicarse con SQL Server usando **Entity Framework Core**.

| Carpeta | Que contiene | Para que sirve |
|---------|-------------|----------------|
| `Contexto/` | TecnoEcommerceContexto.cs | Es el DbContext de EF Core. Representa la base de datos y todas sus tablas como colecciones de objetos C# |
| `Repositorios/` | Repositorio.cs, ProductoRepositorio.cs... | Implementan las interfaces definidas en Modelos. Aqui estan las consultas reales a la BD (SELECT, INSERT, UPDATE, DELETE) |

**¿Por que un Repositorio separado del Controlador?**
Porque si mañana se cambia SQL Server por MongoDB o se cambia EF Core por Dapper, solo se modifica este proyecto. El resto de la aplicacion no se entera del cambio.

---

### TecnoEcommerce.API — Controladores y Servicios

Este proyecto es el punto de entrada de la aplicacion. Expone la API REST y contiene la logica de negocio.

| Carpeta | Que contiene | Para que sirve |
|---------|-------------|----------------|
| `Controladores/` | Clases que heredan de `ControllerBase` | Reciben las peticiones HTTP, validan los datos de entrada y devuelven respuestas HTTP. Son el punto de contacto con el cliente. |
| `Servicios/` | Clases de logica de negocio | Aqui vive la logica compleja: calcular totales, validar stock, procesar pagos, coordinar entre repositorios. Los controladores los llaman en lugar de hacer la logica ellos mismos. |

**Controladores planificados**: UsuariosControlador, ProductosControlador, CategoriasControlador, CarritoControlador, PedidosControlador

**Servicios planificados**: UsuarioServicio, ProductoServicio, CategoriaServicio, CarritoServicio, PedidoServicio, PagoSimuladoServicio, EnvioSimuladoServicio

---

## Tecnologias

| Tecnologia            | Version | Uso en el proyecto               |
|-----------------------|---------|----------------------------------|
| .NET                  | 8.0     | Framework principal              |
| ASP.NET Core Web API  | 8.0     | Capa de Controladores (API REST) |
| Entity Framework Core | 8.x     | Acceso a datos en capa Datos     |
| SQL Server / LocalDB  | -       | Base de datos relacional         |
| Swashbuckle (Swagger) | 6.x     | Vista durante desarrollo         |

---

## Plan de Sprints

| Sprint   | Capa              | Objetivo                                   |
|----------|-------------------|--------------------------------------------|
| Sprint 1 | Modelos           | Entidades, Enumeraciones e Interfaces      |
| Sprint 2 | API / Servicios   | Implementacion de servicios de negocio     |
| Sprint 3 | Datos             | DbContext, EF Core, Repositorios y migracion |
| Sprint 4 | API / Controladores | Controladores REST y configuracion Swagger |
| Sprint 5 | Integracion       | Pruebas end-to-end y ajustes finales       |

Ver el archivo [SPRINTS.md](SPRINTS.md) para el detalle completo de cada sprint.

---

## Requisitos Previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- SQL Server LocalDB (incluido con Visual Studio 2022)
- Visual Studio 2022 o VS Code

---

## Primeros Pasos

```bash
# Clonar el repositorio
git clone https://github.com/Emmanuelxs13/tecno-ecommerce.git
cd tecno-ecommerce

# Restaurar paquetes NuGet
dotnet restore

# Compilar toda la solucion
dotnet build

# Ejecutar la API (disponible a partir del Sprint 3)
cd TecnoEcommerce.API
dotnet run
```

Una vez ejecutando, abrir en el navegador: `https://localhost:PORT/` para ver Swagger.

---

## Cadena de Conexion

Configurada en `TecnoEcommerce.API/appsettings.json`:

```json
"ConnectionStrings": {
  "ConexionPrincipal": "Server=(localdb)\\mssqllocaldb;Database=TecnoEcommerceDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

---

## Autores

- Juan Esteban Correa
- Andres
- Emmanuel Berrio

---

## Estado del Proyecto

> Sprint 0 completado — Arquitectura base MVC creada y compilando sin errores.
> Esperando inicio del Sprint 1.
