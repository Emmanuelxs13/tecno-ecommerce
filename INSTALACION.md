# Instalación de Dependencias — TecnoEcommerce

Este documento explica cómo descargar e instalar todas las dependencias NuGet del proyecto, tanto con los scripts automáticos incluidos como manualmente desde la terminal.

---

## Requisito previo: .NET 10 SDK

Asegúrate de tener instalado el **.NET 10 SDK** antes de continuar.

```bash
dotnet --version
# Debe mostrar 10.x.x
```

Si no está instalado, descárgalo desde:
**https://dotnet.microsoft.com/download/dotnet/10**

---

## Opción A — Script automático (recomendado)

Se incluyen dos scripts en la raíz del repositorio listos para ejecutar:

| Archivo                     | Terminal      | Cómo usarlo                      |
| --------------------------- | ------------- | -------------------------------- |
| `instalar-dependencias.bat` | CMD (Windows) | Doble clic, o ejecutar desde CMD |
| `instalar-dependencias.ps1` | PowerShell    | Ver instrucciones abajo          |

### Ejecutar el script `.bat` desde CMD

```cmd
cd ruta\al\repositorio
instalar-dependencias.bat
```

También puedes hacer **doble clic** sobre `instalar-dependencias.bat` directamente desde el Explorador de Windows.

### Ejecutar el script `.ps1` desde PowerShell

```powershell
# 1. Abre PowerShell en la raíz del repositorio
# 2. Habilita la ejecución de scripts solo para esta sesión:
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass

# 3. Ejecuta el script:
.\instalar-dependencias.ps1
```

> **¿Por qué `Set-ExecutionPolicy`?**
> Windows bloquea la ejecución de scripts `.ps1` por defecto como medida de seguridad.
> El parámetro `-Scope Process` lo habilita únicamente para la sesión actual del terminal,
> sin modificar ninguna configuración global del sistema.

### ¿Qué hacen los scripts?

Ambos scripts realizan los mismos 5 pasos automáticamente:

| Paso | Acción                                                               |
| ---- | -------------------------------------------------------------------- |
| 1    | Verifica que .NET 10 SDK esté instalado                              |
| 2    | Ejecuta `dotnet restore` para restaurar todos los paquetes NuGet     |
| 3    | Muestra un resumen de los paquetes instalados por proyecto           |
| 4    | Instala la herramienta `dotnet-ef` globalmente si no está presente   |
| 5    | Ejecuta `dotnet build` para confirmar que todo compile correctamente |

---

## Opción B — Instalación manual desde la terminal

Si prefieres ejecutar los comandos paso a paso:

### Paso 1 — Verificar .NET 10 SDK

```bash
dotnet --version
```

### Paso 2 — Restaurar paquetes NuGet

Desde la **raíz del repositorio** (donde está `TecnoEcommerce.sln`):

```bash
dotnet restore TecnoEcommerce.sln
```

Esto descarga e instala todos los paquetes NuGet declarados en los cuatro proyectos.

### Paso 3 — Instalar dotnet-ef (opcional, para migraciones EF Core)

```bash
dotnet tool install --global dotnet-ef
```

Verificar que se instaló correctamente:

```bash
dotnet ef --version
```

### Paso 4 — Compilar la solución

```bash
dotnet build TecnoEcommerce.sln
```

Si no hay errores, las dependencias están instaladas y el proyecto está listo para ejecutarse.

---

## Paquetes NuGet del proyecto

### TecnoEcommerce.API

| Paquete                                | Versión | Descripción                    |
| -------------------------------------- | ------- | ------------------------------ |
| `Microsoft.AspNetCore.OpenApi`         | 10.0.5  | Soporte OpenAPI / Swagger      |
| `Swashbuckle.AspNetCore`               | 10.1.5  | Swagger UI                     |
| `Microsoft.EntityFrameworkCore.Design` | 10.0.5  | Herramientas de diseño EF Core |
| `BCrypt.Net-Next`                      | 4.1.0   | Hash de contraseñas            |

### TecnoEcommerce.Datos

| Paquete                                    | Versión | Descripción                       |
| ------------------------------------------ | ------- | --------------------------------- |
| `Microsoft.EntityFrameworkCore`            | 10.0.5  | ORM principal                     |
| `Microsoft.EntityFrameworkCore.Relational` | 10.0.5  | Soporte relacional EF Core        |
| `Microsoft.EntityFrameworkCore.Design`     | 10.0.5  | Herramientas de diseño EF Core    |
| `Npgsql.EntityFrameworkCore.PostgreSQL`    | 10.0.1  | Proveedor PostgreSQL para EF Core |
| `BCrypt.Net-Next`                          | 4.1.0   | Hash de contraseñas               |

### TecnoEcommerce.Web (Blazor WebAssembly)

| Paquete                                                 | Versión | Descripción                   |
| ------------------------------------------------------- | ------- | ----------------------------- |
| `Microsoft.AspNetCore.Components.WebAssembly`           | 10.0.5  | Runtime Blazor WASM           |
| `Microsoft.AspNetCore.Components.WebAssembly.DevServer` | 10.0.5  | Servidor de desarrollo Blazor |

### TecnoEcommerce.Modelos

> Sin dependencias externas de NuGet. Solo referencias entre proyectos de la solución.

---

## Árbol de dependencias entre proyectos

```
TecnoEcommerce.Web
    └── (HttpClient) → TecnoEcommerce.API

TecnoEcommerce.API
    ├── → TecnoEcommerce.Modelos
    └── → TecnoEcommerce.Datos

TecnoEcommerce.Datos
    └── → TecnoEcommerce.Modelos

TecnoEcommerce.Modelos
    (sin dependencias)
```

---

## Problemas frecuentes

### `dotnet` no se reconoce como comando

.NET SDK no está instalado o no está en el PATH del sistema.
Descargar desde: https://dotnet.microsoft.com/download/dotnet/10

### Error de restauración NuGet (sin internet / red corporativa)

Verificar que `nuget.config` en la raíz del proyecto apunta a `https://api.nuget.org/v3/index.json`.
En redes con proxy, configurar el proxy en el archivo `nuget.config`.

### El script `.ps1` dice "no se puede cargar el archivo"

Ejecutar primero:

```powershell
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
```

### La compilación falla con errores de EF Core

Asegúrate de tener configurada la cadena de conexión en `TecnoEcommerce.API/appsettings.json`.
Consulta el [README.md](README.md) principal para los detalles de configuración de PostgreSQL.

---

## Siguientes pasos después de instalar

1. Configura PostgreSQL y la cadena de conexión en `TecnoEcommerce.API/appsettings.json`
2. Ejecuta la API: `dotnet run --project TecnoEcommerce.API/TecnoEcommerce.API.csproj`
3. Ejecuta el frontend: `dotnet run --project TecnoEcommerce.Web/TecnoEcommerce.Web.csproj`
4. Abre el navegador en `http://localhost:5074`

Para más detalles sobre la ejecución completa del proyecto, consulta el [README.md](README.md) principal.
