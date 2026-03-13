#Requires -Version 5.1
<#
.SYNOPSIS
    Instala todas las dependencias NuGet del proyecto TecnoEcommerce.
.DESCRIPTION
    Este script verifica los prerrequisitos, restaura los paquetes NuGet
    de toda la solucion, instala la herramienta dotnet-ef y compila
    la solucion para confirmar que todo este correcto.
.EXAMPLE
    .\instalar-dependencias.ps1
#>

$ErrorActionPreference = "Stop"

function Write-Header {
    param([string]$Text)
    Write-Host ""
    Write-Host "============================================================" -ForegroundColor Cyan
    Write-Host "  $Text" -ForegroundColor Cyan
    Write-Host "============================================================" -ForegroundColor Cyan
    Write-Host ""
}

function Write-Step {
    param([string]$Number, [string]$Text)
    Write-Host "[$Number] $Text" -ForegroundColor Yellow
}

function Write-OK {
    param([string]$Text)
    Write-Host "  OK - $Text" -ForegroundColor Green
}

function Write-Warn {
    param([string]$Text)
    Write-Host "  ADVERTENCIA: $Text" -ForegroundColor DarkYellow
}

function Write-Fail {
    param([string]$Text)
    Write-Host ""
    Write-Host "  ERROR: $Text" -ForegroundColor Red
    Write-Host ""
}

# ─── Encabezado ───────────────────────────────────────────────────────────────
Write-Header "TecnoEcommerce - Instalacion de Dependencias"

# ─── Paso 1: Verificar .NET SDK ───────────────────────────────────────────────
Write-Step "1/5" "Verificando .NET SDK instalado..."

try {
    $dotnetVersion = dotnet --version 2>&1
    if ($LASTEXITCODE -ne 0) { throw "no instalado" }
    Write-OK ".NET SDK version: $dotnetVersion"
}
catch {
    Write-Fail ".NET SDK no encontrado."
    Write-Host "  Descargalo desde: https://dotnet.microsoft.com/download/dotnet/10" -ForegroundColor White
    exit 1
}

# Advertir si la version no es 10.x
if ($dotnetVersion -notmatch "^10\.") {
    Write-Warn "Se recomienda .NET 10 SDK. Version detectada: $dotnetVersion"
    Write-Host "  Descarga .NET 10 en: https://dotnet.microsoft.com/download/dotnet/10" -ForegroundColor White
}

Write-Host ""

# ─── Paso 2: Restaurar paquetes NuGet ─────────────────────────────────────────
Write-Step "2/5" "Restaurando paquetes NuGet (toda la solucion)..."
Write-Host ""

dotnet restore TecnoEcommerce.sln
if ($LASTEXITCODE -ne 0) {
    Write-Fail "Fallo la restauracion de paquetes NuGet."
    Write-Host "  Asegurate de tener conexion a internet y vuelve a intentarlo." -ForegroundColor White
    exit 1
}

Write-Host ""
Write-OK "Paquetes NuGet restaurados correctamente."
Write-Host ""

# ─── Paso 3: Detalle de paquetes ──────────────────────────────────────────────
Write-Step "3/5" "Resumen de paquetes instalados por proyecto:"
Write-Host ""

$paquetes = @(
    @{ Proyecto = "TecnoEcommerce.API"; Paquetes = @(
        "Microsoft.AspNetCore.OpenApi              v10.0.5",
        "Swashbuckle.AspNetCore                   v10.1.5",
        "Microsoft.EntityFrameworkCore.Design      v10.0.5",
        "BCrypt.Net-Next                           v4.1.0"
    )},
    @{ Proyecto = "TecnoEcommerce.Datos"; Paquetes = @(
        "Microsoft.EntityFrameworkCore             v10.0.5",
        "Microsoft.EntityFrameworkCore.Relational  v10.0.5",
        "Microsoft.EntityFrameworkCore.Design      v10.0.5",
        "Npgsql.EntityFrameworkCore.PostgreSQL     v10.0.1",
        "BCrypt.Net-Next                           v4.1.0"
    )},
    @{ Proyecto = "TecnoEcommerce.Web (Blazor WASM)"; Paquetes = @(
        "Microsoft.AspNetCore.Components.WebAssembly           v10.0.5",
        "Microsoft.AspNetCore.Components.WebAssembly.DevServer v10.0.5"
    )}
)

foreach ($item in $paquetes) {
    Write-Host "  $($item.Proyecto)" -ForegroundColor Magenta
    foreach ($pkg in $item.Paquetes) {
        Write-Host "    - $pkg" -ForegroundColor White
    }
    Write-Host ""
}

# ─── Paso 4: Instalar dotnet-ef ───────────────────────────────────────────────
Write-Step "4/5" "Verificando dotnet-ef (herramienta EF Core CLI)..."

$efToolInstalled = dotnet tool list --global | Select-String '^dotnet-ef\s'
if (-not $efToolInstalled) {
    Write-Host "  dotnet-ef no encontrado. Instalando globalmente..." -ForegroundColor Yellow
    dotnet tool install --global dotnet-ef
    if ($LASTEXITCODE -ne 0) {
        Write-Warn "No se pudo instalar dotnet-ef automaticamente."
        Write-Host "  Instalalo manualmente con: dotnet tool install --global dotnet-ef" -ForegroundColor White
    } else {
        Write-OK "dotnet-ef instalado correctamente."
    }
} else {
    $efVersion = dotnet tool list --global |
        Select-String '^dotnet-ef\s' |
        ForEach-Object { ($_ -split '\s+')[1] } |
        Select-Object -First 1
    Write-OK "dotnet-ef ya esta instalado. Version: $efVersion"
}

Write-Host ""

# ─── Paso 5: Compilar la solucion ─────────────────────────────────────────────
Write-Step "5/5" "Compilando la solucion para verificar integridad..."
Write-Host ""

dotnet build TecnoEcommerce.sln --no-restore
if ($LASTEXITCODE -ne 0) {
    Write-Fail "La compilacion fallo. Revisa los errores mostrados arriba."
    exit 1
}

# ─── Resumen final ────────────────────────────────────────────────────────────
Write-Header "INSTALACION COMPLETADA EXITOSAMENTE"

Write-Host "  Proximos pasos:" -ForegroundColor White
Write-Host ""
Write-Host "  1. Configura PostgreSQL y la cadena de conexion en:" -ForegroundColor White
Write-Host "       TecnoEcommerce.API\appsettings.json" -ForegroundColor Cyan
Write-Host ""
Write-Host "  2. Ejecuta la API:" -ForegroundColor White
Write-Host "       dotnet run --project TecnoEcommerce.API/TecnoEcommerce.API.csproj" -ForegroundColor Cyan
Write-Host ""
Write-Host "  3. En otra terminal, ejecuta el frontend:" -ForegroundColor White
Write-Host "       dotnet run --project TecnoEcommerce.Web/TecnoEcommerce.Web.csproj" -ForegroundColor Cyan
Write-Host ""
Write-Host "  4. Abre el navegador en: http://localhost:5074" -ForegroundColor White
Write-Host ""
Write-Host "============================================================" -ForegroundColor Cyan
Write-Host ""
