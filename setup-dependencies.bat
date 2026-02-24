@echo off
echo ========================================
echo   TecnoEcommerce - Setup de Dependencias
echo ========================================
echo.

echo [1/5] Verificando .NET SDK...
dotnet --version
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: .NET SDK no esta instalado.
    echo Descargalo desde: https://dotnet.microsoft.com/download
    pause
    exit /b 1
)
echo OK - .NET SDK encontrado
echo.

echo [2/5] Restaurando paquetes NuGet para todos los proyectos...
dotnet restore
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: No se pudieron restaurar los paquetes NuGet
    pause
    exit /b 1
)
echo OK - Paquetes restaurados
echo.

echo [3/5] Instalando paquetes de Entity Framework Core en Infrastructure...
cd TecnoEcommerce.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore --version 9.0.0
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 9.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.0
cd ..
echo OK - Entity Framework Core instalado
echo.

echo [4/5] Instalando paquetes en API...
cd TecnoEcommerce.API
dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.0
dotnet add package Swashbuckle.AspNetCore --version 6.5.0
cd ..
echo OK - Paquetes de API instalados
echo.

echo [5/5] Verificando instalacion de dotnet-ef tool...
dotnet tool list --global | findstr "dotnet-ef" >nul
if %ERRORLEVEL% NEQ 0 (
    echo Instalando dotnet-ef...
    dotnet tool install --global dotnet-ef
) else (
    echo dotnet-ef ya esta instalado, actualizando...
    dotnet tool update --global dotnet-ef
)
echo OK - dotnet-ef tool listo
echo.

echo ========================================
echo   Compilando la solucion...
echo ========================================
dotnet build
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: La compilacion fallo
    pause
    exit /b 1
)
echo.

echo ========================================
echo   INSTALACION COMPLETADA
echo ========================================
echo.
echo Todos los paquetes han sido instalados correctamente.
echo El proyecto esta listo para el primer commit.
echo.
echo Proximos pasos:
echo 1. git init
echo 2. git add .
echo 3. git commit -m "Initial commit: Arquitectura base"
echo 4. git remote add origin [URL_REPOSITORIO]
echo 5. git push -u origin main
echo.
pause
