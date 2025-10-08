# BackendPro

## Restaurar dependencias y compilar

```bash
dotnet restore BackendPro.sln
dotnet build BackendPro.sln
```

## Ejecutar la aplicación web

```bash
dotnet run --project BackendPro.Web
```

La aplicación quedará disponible (por defecto) en `http://localhost:5000` o el puerto indicado en la salida del comando.

1. Aplica las migraciones:

   ```bash
   dotnet.exe ef database update --project BackendPro.Infrastructure --startup-project BackendPro.Web
   ```

Esto creará o actualizará la base de datos conforme a las migraciones incluidas en el repositorio.
