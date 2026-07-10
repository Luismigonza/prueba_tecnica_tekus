# ProviderServices

Sistema de gestión de proveedores y servicios con autenticación JWT. Permite registrar proveedores, asociarles servicios con tarifa por hora, y consultar un resumen estadístico por país.

---

## Tecnologías

### Backend
- **.NET 10** — ASP.NET Core Web API
- **Entity Framework Core 10** — ORM con SQL Server
- **FluentValidation 12** — Validación de requests
- **Swashbuckle 6** — Documentación Swagger con soporte JWT
- **JWT Bearer** — Autenticación stateless

### Frontend
- **Angular 17+** — Standalone components, signals
- **Angular Material** — Componentes UI
- **RxJS** — Manejo de observables

---

## Arquitectura

El backend sigue **Clean Architecture** con separación en 4 capas:

```
ProviderServices.Domain          → Entidades, eventos de dominio, excepciones
ProviderServices.Application     → AppServices, DTOs, interfaces, validadores
ProviderServices.Infrastructure  → EF Core, repositorios, JWT, SMTP, eventos
ProviderServices.Api             → Controllers, middleware, Program.cs
```

Patrones aplicados: Domain Events, Repository, CQRS-lite (SummaryQueries), DI sin MediatR.

---

## Estructura del proyecto

```
ProviderServices/
├── backend/
│   ├── src/
│   │   ├── ProviderServices.Api/
│   │   ├── ProviderServices.Application/
│   │   ├── ProviderServices.Domain/
│   │   └── ProviderServices.Infrastructure/
│   └── test/
│       └── ProviderServices.Domain.Tests/
└── frontend/
    └── src/app/
        ├── core/               (config, guards, interceptors)
        └── features/
            ├── auth/
            ├── providers/
            └── summary/
```

---

## Diagrama ER

```mermaid
erDiagram
    Providers {
        uniqueidentifier Id PK
        nvarchar(20) Nit UK
        nvarchar(200) Name
        nvarchar(300) Website
        nvarchar(200) Email
        nvarchar(100) Country
        datetime2 CreatedAt
        datetime2 UpdatedAt
    }

    Services {
        uniqueidentifier Id PK
        nvarchar(200) Name
        decimal(10,2) HourlyRateUsd
        datetime2 CreatedAt
        uniqueidentifier ProviderId FK
    }

    Providers ||--o{ Services : "tiene"
```

---

## Requisitos previos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/)
- SQL Server (LocalDB, Express o instancia completa)
- Angular CLI: `npm install -g @angular/cli`

---

## Configuración

### Connection string

Edita `backend/src/ProviderServices.Api/appsettings.json` y ajusta la cadena de conexión a tu instancia de SQL Server:

```json
"ConnectionStrings": {
  "Default": "Server=(localdb)\\mssqllocaldb;Database=ProviderServicesDb;Trusted_Connection=True;"
}
```

### Credenciales por defecto

```json
"DefaultUser": {
  "Username": "admin",
  "Password": "Admin123!"
}
```

> El sistema no gestiona usuarios — hay un único usuario fijo definido en configuración.

---

## Ejecución

### Backend

```bash
cd backend

# Crear y aplicar migración (primera vez)
dotnet ef migrations add Initial --project src/ProviderServices.Infrastructure --startup-project src/ProviderServices.Api
dotnet ef database update --project src/ProviderServices.Infrastructure --startup-project src/ProviderServices.Api

# Levantar la API
dotnet run --project src/ProviderServices.Api
```

La API queda disponible en `https://localhost:5155`.  
Swagger UI: `https://localhost:5155/swagger`

### Frontend

```bash
cd frontend
npm install
npm start
```

La app queda disponible en `http://localhost:4200`.

---

## Endpoints principales

| Método | Ruta | Descripción | Auth |
|--------|------|-------------|------|
| `POST` | `/api/auth/login` | Obtener JWT | No |
| `GET` | `/api/providers` | Listar proveedores (paginado, filtro, orden) | Si |
| `POST` | `/api/providers` | Crear proveedor | Si |
| `GET` | `/api/providers/{id}` | Obtener proveedor por ID | Si |
| `PUT` | `/api/providers/{id}` | Actualizar proveedor | Si |
| `GET` | `/api/providers/{id}/services` | Listar servicios del proveedor | Si |
| `POST` | `/api/providers/{id}/services` | Agregar servicio | Si |
| `GET` | `/api/summary` | Resumen proveedores/servicios por país | Si |

> Al agregar un servicio se dispara un evento de dominio que envía un email de notificación al destinatario configurado en `Notifications:ServiceAddedRecipient`.

---

## Tests

```bash
cd backend
dotnet test
```

- **ProviderServices.Domain.Tests** — 11 tests unitarios de entidades (sin mocks, sin base de datos)

---

## Notas de diseño

- El **Nit** del proveedor se trata como identificador estable: se valida unicidad al crear y no se puede modificar.
- La tarifa por hora (`HourlyRateUsd`) debe ser mayor a cero — regla de dominio protegida en la entidad.
- El envío de email es un efecto secundario: si falla, el servicio ya quedó persistido y se loguea el error sin retornar 500.
- El token se guarda en `localStorage` (trade-off consciente por tiempo; en producción se preferiría cookie httpOnly).
