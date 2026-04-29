TicketApp - Sistema de Gestión de Eventos
Proyecto desarrollado para la materia Software Project. La aplicación es una plataforma robusta que permite gestionar la reserva de butacas para eventos específicos, organizados por sectores, aplicando Arquitectura Hexagonal.

Integrantes
Rocio Jazmin Romero,
Matias Cruz


Instalación y Ejecución
1. Clonar el repositorio
git clone https://github.com/jazminromero9025/Proyecto_De_SoftwareTP.git
cd Proyecto_De_SoftwareTP


2. Configuración y Ejecución del Backend (.NET 8)
 1.Abrir la solución Proyecto_De_SoftwareTP.sln en Visual Studio.

 Verificar en API/appsettings.json la cadena de conexión:
 "ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=TicketingDB;Trusted_Connection=True;TrustServerCertificate=True"
} 

 2. Configuración y Ejecución del Backend
Abrir la solución Proyecto_De_SoftwareTP.sln en Visual Studio.
Presionar F5.
El sistema está configurado para crear la base de datos, aplicar migraciones y cargar datos de prueba automáticamente al iniciar.
Nota: En caso de que la base de datos no se cree automáticamente, puede ejecutar el comando Update-Database en la Consola de Administración de Paquetes.
La API estará disponible en: https://localhost:7016 (Swagger UI: /swagger).


3. Ejecución del Frontend
 1.cd frontend
 2.Ejecutar los comandos:
  npm install
  npm run dev
3.El sitio estará disponible en: http://localhost:5173

Roles y Usuarios de Prueba
El sistema se inicializa con perfiles precargados para facilitar la evaluación:

| Usuario | Email | Contraseña | Rol |
| :--- | :--- | :--- | :--- |
| **María** | maria@test.com | hash123 | Cliente de prueba (Recomendado) |
| **Juan** | juan@test.com | hash123 | Cliente de prueba |
| **Carlos** | carlos@test.com | hash123 | Administrador |


Nota sobre el Admin: Aunque el sistema ya contempla el rol administrador en la base de datos para futuras gestiones, 
esta entrega está optimizada para el flujo de reserva del cliente.


Arquitectura y Diseño:

El proyecto implementa Arquitectura Hexagonal (Clean Architecture), logrando un desacoplamiento total entre la lógica de negocio y los agentes externos:
Dominio (Domain): El corazón del sistema. Contiene las entidades de negocio puras (User, Event, Seat, Sector, AuditLog, Reservation) sin dependencias de frameworks.
Aplicación (Application): Orquestador de la lógica.
Interfaces: Definiciones de contratos para la comunicación entre capas.
Models (DTOs): Objetos de transferencia de datos para optimizar la comunicación.
Services: Lógica intermedia para procesos específicos del negocio.
UseCases: Implementación de patrones Command y Query para cada acción del sistema.
Infraestructura (Infrastructure): Detalles de implementación.
Persistence: Contiene el AppDbContext y la clase DataSeeder que inicializa el sistema.
Migrations: Historial de versiones de la estructura de la base de datos.
Repositories: Implementación del acceso a datos desacoplado del ORM.
API (Presentation): Punto de entrada del sistema. Contiene los Controllers, el archivo de configuración appsettings.json y el Program.cs donde se inyectan todas las dependencias.


Tecnologías y Herramientas
Backend: C# / .NET 8 (Core)
Frontend: React (Biblioteca de UI) ejecutado sobre Node.js
Base de Datos: SQL Server / Entity Framework Core
Arquitectura: Hexagonal (Clean Architecture)
Documentación: Swagger / OpenAPI


























