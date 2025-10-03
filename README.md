# BackendPro

Sistema de gestión de películas desarrollado con ASP.NET Core MVC y Entity Framework Core.

## Requisitos Previos

- .NET 8.0 SDK
- SQL Server (SQL Server Express)
- SQL Server Management Studio

## Configuración de la Base de Datos

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

**SQL Server Express (predeterminado):**
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=BackendPro;Trusted_Connection=True;TrustServerCertificate=True"
}
```

### 3. Crear la Base de Datos desde Migraciones

Ejecute el siguiente comando desde la raíz del proyecto:

```bash
dotnet.exe ef database update --project BackendPro.Infrastructure --startup-project BackendPro.Web
```

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
