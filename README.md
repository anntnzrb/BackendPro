# BackendPro

Sistema de gestión de películas desarrollado con ASP.NET Core MVC y Entity Framework Core.

## Requisitos Previos

- .NET 8.0 SDK
- Docker Desktop (si no tienes SQL Server instalado localmente)
- SQL Server (SQL Server Express) o acceso a un servidor existente

## Configuración de la Base de Datos

### Docker

1. Inicie Docker Desktop y, desde la raíz del repositorio, levante el contenedor:
   ```bash
   docker compose up -d sqlserver
   ```
2. Establezca el entorno `Docker` para que la aplicación utilice el archivo `appsettings.Docker.json`:
   - macOS / Linux:
     ```bash
     ASPNETCORE_ENVIRONMENT=Docker dotnet.exe run --project BackendPro.Web
     ```
   - Windows (PowerShell):
     ```powershell
     $env:ASPNETCORE_ENVIRONMENT="Docker"
     dotnet.exe run --project BackendPro.Web
     ```
   Ajuste la contraseña en `BackendPro.Web/appsettings.Docker.json` y en la variable `SA_PASSWORD` de `docker-compose.yml` si lo considera necesario.

### 1. Verificar SQL Server

Asegúrese de que SQL Server esté en ejecución:

**Para SQL Server Express:**
```powershell
Get-Service -Name "MSSQL$SQLEXPRESS" | Select-Object Name, Status
```

Si el servicio está detenido, inícielo:
```powershell
Start-Service -Name "MSSQL$SQLEXPRESS"
```

### 2. Configurar Cadena de Conexión

La cadena de conexión está configurada en `BackendPro.Web/appsettings.json`:

### 3. Crear la Base de Datos desde Migraciones

Ejecute el siguiente comando desde la raíz del proyecto:

```bash
dotnet.exe ef database update --project BackendPro.Infrastructure --startup-project BackendPro.Web
```

Cuando use el contenedor, asegúrese de que esté en ejecución (`docker compose up -d sqlserver`) y, si necesita la conexión Docker, ejecute el comando con `ASPNETCORE_ENVIRONMENT=Docker` configurado.

Este comando:
- Creará la base de datos `BackendPro` si no existe
- Aplicará todas las migraciones incluidas en el repositorio
- Insertará datos de prueba (seed data)

## Instalación y Ejecución

### 1. Restaurar Dependencias

```bash
dotnet restore BackendPro.sln
```

### 2. Compilar el Proyecto

```bash
dotnet build BackendPro.sln
```

### 3. Ejecutar la Aplicación

```bash
dotnet run --project BackendPro.Web
```

La aplicación estará disponible en:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`

O el puerto indicado en la salida del comando.

## Estructura del Proyecto

- **BackendPro.Core**: Entidades de dominio, interfaces, servicios y DTOs
- **BackendPro.Infrastructure**: Implementación de repositorios, DbContext y migraciones
- **BackendPro.Web**: Controllers, vistas, view models y configuración MVC
