# API Gestión Máquinas Virtuales

API RESTful desarrollada en .NET Core 9 para la gestión de máquinas virtuales con notificaciones en tiempo real y autenticación de usuarios.

## Características

- Gestión completa de máquinas virtuales (CRUD)
- Autenticación y autorización basada en JWT
- Gestión de usuarios
- Notificaciones en tiempo real con SignalR
- Arquitectura en capas
- Documentación API con Swagger

## Requisitos Previos

- .NET Core 9 SDK
- SQL Server (local o en la nube)(En el json ya esta quemada la base de datos de azure)
- Visual Studio 2022 o posterior (recomendado)

## Instalación

1. Clonar el repositorio
   ```
https://github.com/Matriuz1999/ApiGestionMaquinasVirtuales.git
   cd api-gestion-maquinas-virtuales
   ```

2. Restaurar dependencias
   ```
   dotnet restore
   ```

3. Configurar la cadena de conexión
   
   Modifica el archivo `appsettings.json` para configurar la conexión a tu base de datos:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=your_server;Database=GestionMaquinasVirtuales;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   ```

4. Aplicar migraciones
   ```
   dotnet ef database update
   ```

5. Ejecutar la aplicación
   ```
   dotnet run
   ```

   La API estará disponible en https://localhost:5001

## Estructura del Proyecto

- **Controllers**: Controladores API que manejan las solicitudes HTTP
- **DTOs**: Objetos de transferencia de datos
- **Models**: Modelos de datos y entidades
- **Services**: Lógica de negocio
- **Repositories**: Acceso a datos
- **Interfaces**: Contratos para implementaciones
- **Hubs**: Componentes SignalR para comunicación en tiempo real

## Endpoints Principales

### Autenticación
- `POST /api/Auth/login`: Iniciar sesión y obtener token JWT

### Usuarios
- `POST /api/Users`: Crear un nuevo usuario
- `GET /api/Users`: Obtener todos los usuarios
- `GET /api/Users/{id}`: Obtener usuario por ID

### Máquinas Virtuales
- `GET /api/MaquinasVirtuales`: Obtener todas las máquinas virtuales
- `GET /api/MaquinasVirtuales/{id}`: Obtener máquina virtual por ID
- `POST /api/MaquinasVirtuales`: Crear una nueva máquina virtual (requiere rol Administrador)
- `PUT /api/MaquinasVirtuales/{id}`: Actualizar una máquina virtual (requiere rol Administrador)
- `DELETE /api/MaquinasVirtuales/{id}`: Eliminar una máquina virtual (requiere rol Administrador)

## SignalR Hub

El proyecto incluye un hub SignalR que permite recibir notificaciones en tiempo real cuando se crean, actualizan o eliminan máquinas virtuales.

