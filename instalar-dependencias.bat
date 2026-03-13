@echo off
chcp 65001 >nul
title TecnoEcommerce - Instalador de Dependencias
color 0A

echo ============================================================
echo   TecnoEcommerce - Instalacion de Dependencias
echo ============================================================
echo.

REM ─── Verificar que dotnet esta instalado ─────────────────────
echo [1/5] Verificando .NET SDK...
dotnet --version >nul 2>&1
if %ERRORLEVEL% neq 0 (
    echo.
    echo  ERROR: .NET SDK no encontrado.
    echo  Descargalo desde: https://dotnet.microsoft.com/download/dotnet/10
    echo.
    pause
    exit /b 1
)
for /f "tokens=*" %%v in ('dotnet --version') do set DOTNET_VER=%%v
echo  OK - .NET SDK version: %DOTNET_VER%
echo.

REM ─── Restaurar paquetes NuGet de toda la solucion ────────────
echo [2/5] Restaurando paquetes NuGet (toda la solucion)...
echo.
dotnet restore TecnoEcommerce.sln
if %ERRORLEVEL% neq 0 (
    echo.
    echo  ERROR: Fallo la restauracion de paquetes NuGet.
    echo  Revisa tu conexion a internet y vuelve a intentarlo.
    echo.
    pause
    exit /b 1
)
echo.
echo  OK - Paquetes NuGet restaurados correctamente.
echo.

REM ─── Detalle de paquetes por proyecto ────────────────────────
echo [3/5] Paquetes instalados por proyecto:
echo.
echo  TecnoEcommerce.API
echo   - Microsoft.AspNetCore.OpenApi       v10.0.5
echo   - Swashbuckle.AspNetCore             v10.1.5
echo   - Microsoft.EntityFrameworkCore.Design v10.0.5
echo   - BCrypt.Net-Next                    v4.1.0
echo.
echo  TecnoEcommerce.Datos
echo   - Microsoft.EntityFrameworkCore          v10.0.5
echo   - Microsoft.EntityFrameworkCore.Relational v10.0.5
echo   - Microsoft.EntityFrameworkCore.Design   v10.0.5
echo   - Npgsql.EntityFrameworkCore.PostgreSQL  v10.0.1
echo   - BCrypt.Net-Next                        v4.1.0
echo.
echo  TecnoEcommerce.Web (Blazor WASM)
echo   - Microsoft.AspNetCore.Components.WebAssembly           v10.0.5
echo   - Microsoft.AspNetCore.Components.WebAssembly.DevServer v10.0.5
echo.

REM ─── Instalar herramienta dotnet-ef (opcional, para migraciones) ──
echo [4/5] Verificando dotnet-ef (herramienta EF Core CLI)...
dotnet ef >nul 2>&1
if %ERRORLEVEL% neq 0 (
    echo  dotnet-ef no encontrado. Instalando...
    dotnet tool install --global dotnet-ef
    if %ERRORLEVEL% neq 0 (
        echo  ADVERTENCIA: No se pudo instalar dotnet-ef automaticamente.
        echo  Puedes instalarlo manualmente con:
        echo    dotnet tool install --global dotnet-ef
    ) else (
        echo  OK - dotnet-ef instalado correctamente.
    )
) else (
    for /f "tokens=*" %%v in ('dotnet ef --version 2^>nul') do set EF_VER=%%v
    echo  OK - dotnet-ef ya instalado.
)
echo.

REM ─── Compilar la solucion para verificar que todo esta OK ─────
echo [5/5] Compilando la solucion para verificar integridad...
dotnet build TecnoEcommerce.sln --no-restore
if %ERRORLEVEL% neq 0 (
    echo.
    echo  ERROR: La compilacion fallo. Revisa los errores anteriores.
    echo.
    pause
    exit /b 1
)
echo.

echo ============================================================
echo   INSTALACION COMPLETADA EXITOSAMENTE
echo ============================================================
echo.
echo  Proximos pasos:
echo.
echo  1. Configura PostgreSQL y la cadena de conexion en:
echo       TecnoEcommerce.API\appsettings.json
echo.
echo  2. Ejecuta la API:
echo       dotnet run --project TecnoEcommerce.API/TecnoEcommerce.API.csproj
echo.
echo  3. En otra terminal, ejecuta el frontend:
echo       dotnet run --project TecnoEcommerce.Web/TecnoEcommerce.Web.csproj
echo.
echo  4. Abre el navegador en: http://localhost:5074
echo.
echo ============================================================
echo.
pause
