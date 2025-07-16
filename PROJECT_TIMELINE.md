# PatientPortal Development Timeline (Week 1-2)

## Project Overview
Building a patient portal application with Blazor Server, Okta authentication, Entity Framework Core, and SQL Server.

## Current Status
- ✅ Basic Blazor Server project scaffolded
- ✅ Entity Framework Core installed
- ✅ Patient model created
- ⚠️ Database migration needs fixing
- ❌ No authentication implemented
- ❌ No API layer
- ❌ No patient UI components
- ❌ No testing infrastructure

## Week 1 Schedule (Days 1-7)

### Day 1-2: Database & Core Infrastructure
**Goal**: Establish solid database foundation and fix current issues

#### Morning Session (4 hours)
- [ ] Fix empty migration issue
  - Delete existing migration
  - Recreate migration with proper Patient table
  - Run `Update-Database` to create tables
- [ ] Add seed data for testing
- [ ] Create repository pattern implementation
  - `IPatientRepository` interface
  - `PatientRepository` implementation
- [ ] Configure dependency injection in Program.cs

#### Afternoon Session (4 hours)
- [ ] Create service layer
  - `IPatientService` interface
  - `PatientService` implementation
- [ ] Add DTOs for Patient operations
  - `PatientCreateDto`
  - `PatientUpdateDto`
  - `PatientResponseDto`
- [ ] Implement basic validation logic
- [ ] Add logging configuration

### Day 3-4: Okta Authentication Setup
**Goal**: Complete authentication integration

#### Day 3 Morning (4 hours)
- [ ] Create Okta developer account
- [ ] Register application in Okta
- [ ] Install required NuGet packages:
  - `Okta.AspNetCore`
  - `Microsoft.AspNetCore.Authentication.OpenIdConnect`
- [ ] Configure authentication in Program.cs
- [ ] Set up Okta settings in appsettings.json

#### Day 3 Afternoon (4 hours)
- [ ] Create login/logout components
- [ ] Implement `AuthorizeView` in main layout
- [ ] Create user profile display component
- [ ] Test authentication flow end-to-end

#### Day 4 Morning (4 hours)
- [ ] Configure authorization policies
- [ ] Add role-based access control
- [ ] Secure existing pages with `[Authorize]`
- [ ] Create unauthorized access page

#### Day 4 Afternoon (4 hours)
- [ ] Implement token storage and retrieval
- [ ] Configure CORS for API calls
- [ ] Add authentication state provider
- [ ] Create authenticated HTTP client service

### Day 5-6: API Development
**Goal**: Build robust Web API with authentication

#### Day 5 Morning (4 hours)
- [ ] Create API controllers project structure
- [ ] Implement `PatientController` with CRUD operations:
  - `GET /api/patients` (list all)
  - `GET /api/patients/{id}` (get single)
  - `POST /api/patients` (create)
  - `PUT /api/patients/{id}` (update)
  - `DELETE /api/patients/{id}` (delete)

#### Day 5 Afternoon (4 hours)
- [ ] Add Okta token validation middleware
- [ ] Implement API authorization attributes
- [ ] Configure Swagger/OpenAPI documentation
- [ ] Add request/response logging

#### Day 6 Morning (4 hours)
- [ ] Add API versioning
- [ ] Implement global exception handling
- [ ] Create health check endpoints
- [ ] Add API rate limiting

#### Day 6 Afternoon (4 hours)
- [ ] Test all API endpoints with Postman/Swagger
- [ ] Implement pagination for list endpoints
- [ ] Add search/filter functionality
- [ ] Document API endpoints

### Day 7: Initial UI Components
**Goal**: Create basic patient management UI

#### Morning Session (4 hours)
- [ ] Create patient list component
  - Display patients in data grid
  - Add sorting and pagination
  - Implement search functionality
- [ ] Wire up to API with authenticated HTTP calls

#### Afternoon Session (4 hours)
- [ ] Create patient registration form
  - All required fields with validation
  - Date picker for DOB
  - Submit to API endpoint
- [ ] Add success/error notifications
- [ ] Test full registration flow

## Week 2 Schedule (Days 8-14)

### Day 8-9: Complete UI Development
**Goal**: Full CRUD UI for patient management

#### Day 8 Morning (4 hours)
- [ ] Create patient detail/view component
- [ ] Implement edit patient form
- [ ] Add delete confirmation dialog
- [ ] Create navigation between components

#### Day 8 Afternoon (4 hours)
- [ ] Add loading states and spinners
- [ ] Implement error handling UI
- [ ] Add form validation feedback
- [ ] Create reusable UI components

#### Day 9 Morning (4 hours)
- [ ] Polish UI with better styling
- [ ] Add responsive design
- [ ] Implement keyboard navigation
- [ ] Add accessibility features

#### Day 9 Afternoon (4 hours)
- [ ] Create dashboard/home page
- [ ] Add user welcome message
- [ ] Display patient statistics
- [ ] Add quick actions menu

### Day 10-11: Testing Infrastructure
**Goal**: Comprehensive test coverage

#### Day 10 Morning (4 hours)
- [ ] Create xUnit test projects
  - `PatientPortal.Tests.Unit`
  - `PatientPortal.Tests.Integration`
- [ ] Set up test database
- [ ] Configure test authentication

#### Day 10 Afternoon (4 hours)
- [ ] Write unit tests for:
  - Patient service methods
  - Repository operations
  - Validation logic
- [ ] Achieve 80% code coverage

#### Day 11 Morning (4 hours)
- [ ] Write integration tests for:
  - API endpoints
  - Authentication flow
  - Database operations
- [ ] Test error scenarios

#### Day 11 Afternoon (4 hours)
- [ ] Add UI component tests
- [ ] Create test data builders
- [ ] Set up continuous testing
- [ ] Document testing approach

### Day 12: Advanced Features
**Goal**: Enhancement and optimization

#### Morning Session (4 hours)
- [ ] Add audit logging
- [ ] Implement soft delete
- [ ] Add patient photo upload
- [ ] Create export functionality

#### Afternoon Session (4 hours)
- [ ] Add real-time notifications
- [ ] Implement caching strategy
- [ ] Optimize database queries
- [ ] Add performance monitoring

### Day 13: Security & Best Practices
**Goal**: Production-ready security

#### Morning Session (4 hours)
- [ ] Security audit all endpoints
- [ ] Implement input sanitization
- [ ] Add SQL injection prevention
- [ ] Configure secure headers

#### Afternoon Session (4 hours)
- [ ] Set up environment configurations
- [ ] Implement secrets management
- [ ] Add security logging
- [ ] Create security documentation

### Day 14: Final Polish & Documentation
**Goal**: Project completion and handoff

#### Morning Session (4 hours)
- [ ] Complete README documentation
- [ ] Create API documentation
- [ ] Write deployment guide
- [ ] Add architecture diagrams

#### Afternoon Session (4 hours)
- [ ] Final testing pass
- [ ] Performance optimization
- [ ] Code cleanup and refactoring
- [ ] Prepare demo presentation

## Daily Checklist
- [ ] Morning standup (self-review)
- [ ] Code commits with meaningful messages
- [ ] Update task progress
- [ ] Test new features
- [ ] Document challenges/learnings

## Success Metrics
- ✅ Fully functional authentication with Okta
- ✅ Complete CRUD operations for patients
- ✅ 80%+ test coverage
- ✅ Clean, documented codebase
- ✅ Responsive, accessible UI
- ✅ Secure API with proper validation
- ✅ Production-ready configuration

## Resources
- [Okta Blazor Sample](https://github.com/okta/samples-blazor)
- [Okta Developer Docs](https://developer.okta.com/blog/2022/01/07/blazor-server-side-mfa)
- [Entity Framework Core Docs](https://docs.microsoft.com/en-us/ef/core/)
- [Blazor Documentation](https://docs.microsoft.com/en-us/aspnet/core/blazor/)

## Notes
- Focus on one major feature per day
- Test as you build, don't leave it until the end
- Commit code frequently
- Ask for help when stuck for >30 minutes
- Keep security in mind from the start