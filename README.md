# CarSale

CarSale is a microservices-based project inspired by <a href="https://av.by">av.by<a/> and <a href="https://drom.ru">drom.ru<a/>.

## Technologies

**Language/platform** : C#, ASP.NET Core

**Architecture**: Microservices

**Database**: PostgreSQL, SQLite, EF Core

**File Storage**: Minio

**Auth**: Keycloak

**Containerization**: Docker Compose

**CI**: GitHub Actions

**Message broker**: RabbitMq (in progress)


## Microservices

* **Ad service** (REST, Clean Architecture, DDD, CQRS, PostgreSQL, MediatR, xUnit) - service for managing ads. Features:
  
  * CRUD operations with ads.
  * Associate ads with users and cars from the catalog.
  * Filtering, pagination, and lifecycle statuses (draft/published/archived).
  * Faking data using <a href="https://github.com/bchavez/Bogus">Bogus</a>
  * Unit testing ads using <a href="https://github.com/xunit/xunit">xUnit</a>.
  
* **Auto catalog** (REST, VSA, CQRS, PostgreSQL, MediatR) - service for managing cars in catalog. Features:

   * CRUD with cars, brands, models, generations and engines.
   * Search and filtering by attributes.
   * Faking data using <a href="https://github.com/bchavez/Bogus">Bogus</a>.
 
* **File management** (gRPC, N-Layer, SQLite, Minio) - infrastructure service for file managing. Features:

    * Uploading small and large files ( < 1.5 GB)
    * Generating thumbnails using <a href="https://github.com/SixLabors/ImageSharp">ImageSharp</a>
    * Storing metadata in SQLite database

**In progress**:

* **Profile Service** - service for managing user profiles.

## Roadmap

* Redis caching
* Event bus pattern




  
