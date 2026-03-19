# CarSale

CarSale is a car sales platform, inspired by <a href="https://av.by">av.by</a> and <a href="https://drom.ru">drom.ru</a>. The project is implemented using a microservices architecture and is intended for learning modern development approaches in C# and TypeScript.

## Content
- [Technologies](#technologies)
- [Microservices](#microservices)
- [Achitecture](#architecture)
- [Communication](#communication)
- [How to run](#howtorun)
- [Roadmap](#roadmap)

## Technologies

**Language/platform**: C#, ASP.NET Core Web API, TypeScript, NestJS 
**Architecture**: Microservices  
**Database**: PostgreSQL, SQLite, Redis, EF Core, Prisma ORM 
**File Storage**: Minio
**Auth**: Keycloak  
**Containerization**: Docker Compose  
**CI**: GitHub Actions  
**Message Broker**: RabbitMQ + MassTransit 

## Microservices

* **Ad service** (REST, Clean Architecture, DDD, CQRS, PostgreSQL, MediatR, xUnit, Redis) – service for managing advertisements. Features:
  * CRUD operations with ads.
  * Associate ads with users and cars from the catalog.
  * Filtering, pagination, and lifecycle statuses (draft/published/archived).
  * Data generation using <a href="https://github.com/bchavez/Bogus">Bogus</a>.
  * Unit tests for ads using <a href="https://github.com/xunit/xunit">xUnit</a>.
  * Caching ads and Auto catalog entities following cache-aside pattern.
  * Policy-based authorization.
  * Synchronous communication with Auto catalog using <a href="https://learn.microsoft.com/en-us/dotnet/core/extensions/httpclient-factory#typed-clients">typed HttpClient</a>.

* **Auto catalog** (REST, VSA, CQRS, PostgreSQL, MediatR) – service for managing cars in the catalog. Features:
  * CRUD operations for cars, brands, models, generations, engines and other auto specs.
  * Search and filtering by attributes.
  * Event publishing when any entity updates.
  * Policy-based authorization.

* **File management** (gRPC, N-Layer, SQLite, Minio) – infrastructure service for file management. Features:
  * Uploading small and large files (< 1.5 GB).
  * Generating thumbnails using <a href="https://github.com/SixLabors/ImageSharp">ImageSharp</a>.
  * Storing files metadata in a SQLite database.

**In progress**:
* **Profile Service** (NestJS, Prisma) – service for managing user profiles.

## Architecture
<img width="1237" height="760" alt="image" src="https://github.com/user-attachments/assets/94593860-c047-487c-b617-1ebd56f5da86" />


## Communication
 <img width="1090" height="681" alt="image" src="https://github.com/user-attachments/assets/55d34398-81c7-4c3b-8f86-2dc8173cebcb" />

   


## Roadmap

* Nginx
* UI
