# Vegetable holding

## News website - work in progress

### Currently swapping to Clean architecture from all-in-one project structure
Structure will looks something like 
```
Vegetable/
├── Core/               # Enterprise/domain entities & business rules
├── Application/        # Business logic & use cases
├── Infrastructure/     # External concerns (database, file systems, etc.)
├── WebApi/             # User interface & API endpoints
└── UnitTests/          # Unit tests for all layers
```
### Layer Details

#### Domain Layer
- Contains enterprise/business logic
- Entities
- Value Objects
- Domain Events
- Interfaces
- Business Rules
- No dependencies on other layers

#### Application Layer
- Contains application logic
- Implements use cases
- DTOs
- Interfaces
- Service implementations
- Dependencies: Domain layer

#### Infrastructure Layer
- Implementation of interfaces from Domain/Application layers
- Database contexts
- Repositories implementations
- External service implementations
- Dependencies: Domain and Application layers

#### WebApi Layer
- API Controllers
- API Models
- Middleware
- Dependencies: Application layer
