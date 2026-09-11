# Estado de Cuenta - Tarjeta de Crédito



## Descripción del proyecto

Aplicación web para la gestión y visualización del estado de cuenta de una tarjeta de crédito. Permite consultar saldos, límites y cálculos financieros (interés bonificable, cuota mínima, monto total a pagar y contado con intereses), registrar compras y pagos, revisar el historial de transacciones del mes, y exportar el estado de cuenta en formato PDF.

La solución está construida sobre **.NET 6**, con una **Web API REST** que expone la lógica de negocio y el acceso a datos, y un **frontend en ASP.NET MVC (Razor + jQuery)** que consume dicha API. La persistencia se maneja en **SQL Server**, con toda la lógica transaccional crítica (compras, pagos, cálculo de saldos) implementada mediante **procedimientos almacenados**, garantizando atomicidad y evitando condiciones de carrera en operaciones concurrentes sobre el saldo de la tarjeta.

### Características principales

- Consulta de estado de cuenta: titular, número de tarjeta (enmascarado), saldo actual, límite de crédito, saldo disponible.
- Cálculo de interés bonificable, cuota mínima a pagar, monto total a pagar y contado con intereses.
- Registro de compras con validación atómica de saldo disponible.
- Registro de pagos con validación de sobrepago.
- Historial de transacciones del mes (compras + pagos unificados, orden descendente por fecha).
- Exportación del estado de cuenta a PDF.
- Manejo centralizado de excepciones (`GlobalExceptionMiddleware`).
- Health check de disponibilidad de la base de datos (`/health`).
- Documentación interactiva de la API vía Swagger.

---

## Arquitectura de la solución

### Estructura de proyectos

La solución sigue el principio de separación en capas, con un mínimo de dos proyectos ejecutables (API y MVC) y capas de soporte para mantener el código desacoplado y alineado con SOLID:

```
CardAPI.sln
│
├── CardAPI.Api                 → Web API REST (ASP.NET Core, Swagger, Health Checks)
├── CardAPI.Web                 → Frontend MVC (Razor + jQuery), consume la API
├── CardAPI.Application          → Casos de uso: Commands, Queries, Handlers (CQRS con MediatR),
│                                   Validators (FluentValidation), Behaviors, Perfiles de AutoMapper
├── CardAPI.Domain               → Entidades de dominio y DTOs
├── CardAPI.Infrastructure        → Repositorios, DbContext (EF Core), acceso a Stored Procedures
database_creation.sql    → Script de creación de esquema, procedimientos almacenados y datos semilla
```

### Patrones y prácticas implementadas

| Práctica | Descripción |
|---|---|
| **CQRS (vía MediatR)** | Separación explícita entre Commands (compras, pagos) y Queries (estado de cuenta, historial), cada uno con su propio Handler. |
| **Unit of Work / Repository** | Acceso a datos encapsulado en repositorios (`ICardRepository`, `IClientRepository`, `IPurchaseRepository`, `IPaymentRepository`). Además de uso de EF 6, que internamente también implementa Unit of Work|
| **AutoMapper** | Mapeo entre entidades de dominio, DTOs de la API y ViewModels del frontend MVC. |
| **FluentValidation** | Validación de forma de los Commands/Queries (ej. `amount > 0`) antes de llegar a la lógica de negocio. |
| **DTOs y ViewModels separados** | La API expone DTOs; el MVC construye sus propios ViewModels a partir de ellos, sin acoplar capas. |
| **GlobalExceptionMiddleware** | Middleware centralizado que traduce excepciones (negocio, validación, infraestructura) a respuestas HTTP consistentes. |
| **Health Checks** | Verificación de disponibilidad de SQL Server expuesta en `/health`. |
| **Procedimientos almacenados** | Toda mutación de saldo (compras y pagos) se ejecuta de forma atómica dentro de una transacción SQL, con bloqueo de fila (`UPDLOCK`/`ROWLOCK`) para evitar condiciones de carrera. |

### Flujo de una transacción (compra/pago)

1. El cliente envía la solicitud al **MVC**, que la reenvía a la **API** vía `HttpClient`.
2. El controller de la API valida el shape del request con **FluentValidation** y despacha un **Command** a través de **MediatR**.
3. El **Command Handler** invoca al repositorio correspondiente, que ejecuta el **stored procedure**.
4. El SP realiza, en una sola transacción atómica: validación de saldo/estado de la tarjeta, actualización del saldo, e inserción del movimiento con su estado resultante (procesado/rechazado).
5. El resultado se mapea con **AutoMapper** a un DTO de respuesta y se devuelve al MVC.

---

## Endpoints de la API
 
Todos los endpoints están documentados de forma interactiva en Swagger y también disponibles en la colección de Postman incluida en el repositorio (`AccountDashboard.postman_collection.json`). Base URL de referencia en ambiente local: `https://localhost:7161`.
 
| Método | Endpoint | Descripción |
|---|---|---|
| `GET` | `/api/Client/{clientId}` | Obtiene los datos del cliente junto con sus tarjetas asociadas. |
| `POST` | `/api/Client/Balance` | Obtiene el estado de cuenta completo de una tarjeta: saldo, límite, disponible, interés bonificable, cuota mínima, monto total a pagar, contado con intereses, y compras del mes actual. |
| `POST` | `/api/Client/TransactionHistory` | Obtiene el historial de transacciones del mes actual (compras y pagos unificados, orden descendente por fecha). |
| `POST` | `/api/Card/purchase` | Registra una nueva compra. Valida saldo disponible y estado de la tarjeta de forma atómica. |
| `POST` | `/api/Card/payment` | Registra un nuevo pago. Valida que el monto no exceda el saldo adeudado. |
| `GET` | `/api/AccountStatement/{cardId}/pdf?userId={userId}` | Genera y descarga el estado de cuenta en formato PDF. |
| `GET` | `/health` | Verifica el estado de salud de la API y su conexión a SQL Server. |
 
### Ejemplo de request — `POST /api/Client/Balance`
 
```json
{
  "cardId": "83b0e16a-8630-4704-99a6-b52d13ae222d",
  "userId": "00000001"
}
```
 
### Ejemplo de request — `POST /api/Card/purchase`
 
```json
{
  "cardId": "83b0e16a-8630-4704-99a6-b52d13ae222d",
  "purchaseDate": "2026-09-10",
  "description": "Compra de prueba",
  "price": 10.99
}
```
 
### Ejemplo de request — `POST /api/Card/payment`
 
```json
{
  "cardId": "83b0e16a-8630-4704-99a6-b52d13ae222d",
  "paymentDate": "2026-09-10",
  "description": "Pago mensual",
  "amount": 10.99
}
```
 
---
 
## Cómo probar la aplicación
 
### 1. Requisitos previos
 
- .NET 6 SDK
- SQL Server (local, Docker, o Azure SQL)
- Visual Studio 2022 / VS Code
- Postman (para probar la colección incluida)
### 2. Configurar la base de datos
 
1. Abrir SQL Server Management Studio .
2. Ejecutar el script `Database/database_creation.sql` completo. Esto crea la base de datos `BD_TARJETA`, todas las tablas, los procedimientos almacenados y los catálogos (`TRANSACTION_STATE`, `DOCTYPE`, `CARD_STATUS`).
3. Al final del script de datos de prueba para poblar clientes, tarjetas y transacciones de ejemplo, útiles para validar los cálculos contra los valores del enunciado.
### 3. Configurar la cadena de conexión
 
En `CardAPI.Api/appsettings.json` y `CardAPI.Web/appsettings.json`, ajusta:
 
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=BD_TARJETA;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "ApiBaseUrl": "https://localhost:5001"
}
```
 
### 4. Ejecutar la solución
 
Desde Visual Studio, configura ambos proyectos (`CardAPI.Api` y `CardAPI.Web`) como proyectos de inicio simultáneo (clic derecho en la solución → Propiedades → Múltiples proyectos de inicio), o ejecuta cada uno por separado desde terminal:
 
```bash
cd CardAPI.Api
dotnet run
 
# en otra terminal
cd CardAPI.Web
dotnet run
```
 
### 5. Probar la API con Swagger
 
Con la API corriendo, abrir en el navegador:
 
```
https://localhost:5001/swagger
```
 
Desde ahí puedes ejecutar cada endpoint directamente, revisar los esquemas de request/response, y probar los distintos escenarios (tarjeta con saldo, tarjeta sin fondos, tarjeta inactiva, etc.) usando los IDs de tarjeta de los datos de prueba.
 
### 6. Probar con la colección de Postman
 
1. Importa el archivo `AccountDashboard.postman_collection.json` incluido en el repositorio.
2. Ajusta el puerto en las URLs si tu API corre en un puerto distinto a `7161` (puerto HTTPS por defecto asignado por Visual Studio en este proyecto). Se recomienda extraer el host y puerto a una variable de colección (`{{baseUrl}}`) para facilitar cambios de ambiente — ver sugerencias más abajo.
3. Ejecuta las requests en el siguiente orden sugerido, usando el `cardId` de una tarjeta de prueba existente en la base de datos:
   1. `GetCliente` — confirma que el cliente y sus tarjetas existen.
   2. `GetBalance` — revisa el estado de cuenta inicial.
   3. `PostPurchase` — registra una compra y verifica que el saldo disponible se reduzca.
   4. `PostPayment` — registra un pago y verifica que el saldo disponible aumente.
   5. `AllTransaction` — confirma que ambos movimientos aparecen en el historial, ordenados por fecha descendente.
   6. `GeneratePDF` — descarga el estado de cuenta actualizado en PDF.
### 7. Probar el flujo completo desde el frontend MVC
 
Con ambos proyectos corriendo, abre:
 
```
https://localhost:5000
```
 
Desde ahí puedes navegar al estado de cuenta, registrar compras/pagos desde los formularios, y exportar el PDF desde el botón correspondiente.
 
### 8. Verificar el health check
 
```bash
curl https://localhost:5001/health
```
 
Respuesta esperada:
 
```json
{ "status": "Healthy", "entries": { "sql-server": { "status": "Healthy" } } }
```
 
---
 
## Decisiones de diseño relevantes
 
- **Número de tarjeta enmascarado:** se muestra únicamente `LASTD_CARD` (últimos 4 dígitos) en vez del número completo, siguiendo buenas prácticas de seguridad para datos financieros sensibles, aunque el número completo se almacena en base de datos para efectos de negocio.
- **Atomicidad en SQL, no en C#:** toda validación de saldo (compras y pagos) se resuelve dentro del stored procedure mediante una única sentencia `UPDATE` condicional con bloqueo de fila, evitando condiciones de carrera que ocurrirían si la validación se hiciera en dos pasos separados desde la capa de aplicación.
- **`userId` como parámetro explícito:** en el estado actual del proyecto, `userId` se recibe como parámetro en ciertos endpoints (ej. exportación de PDF) en lugar de extraerse de un contexto de autenticación. En un entorno productivo, este valor debería obtenerse de los claims del usuario autenticado (JWT), no del cliente. Esta es una limitación conocida, documentada aquí por transparencia dado el alcance y tiempo de la prueba técnica.
- **Estado `Failed`/`Finished` en catálogo `TRANSACTION_STATE`:** las compras rechazadas por saldo insuficiente se registran igualmente en el historial con estado `Failed`, para mantener trazabilidad del intento; los pagos inválidos (que exceden la deuda), en cambio, se rechazan como error de validación sin persistir un registro, ya que no representan un intento de transacción real sobre fondos.
---
 
## Puntos de Mejora 

* Enmascarar el número de tarjeta en la base de datos y almacenar solo los últimos 4 dígitos, para mejorar la seguridad.
* Implementar un sistema de logging detallado para rastrear todas las operaciones de compra y pago.
* Agregar pruebas unitarias y de integración para garantizar la calidad del código.
* Extender desarrollo para múltiples tarjetas por cliente 
* Agregar autenticación y autorización para proteger los endpoints de la API y el frontend MVC, usando JWT o IdentityServer.
* Finalizar implementacion de regex e identificacion por Documento o Usuario/Contraseña para el login de usuarios y clientes.

 
## Autor


Victor Peraza