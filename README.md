# Patient Portal

A web application for managing patient information and healthcare services.

## Project Structure

```
PatientPortalApp/
├── PatientPortal/        # Main application
└── PatientPortal.Tests/  # Test project
```

## Prerequisites

- .NET 9.0 SDK
- Visual Studio Code or Visual Studio 2022

## Getting Started

1. Clone the repository
```bash
git clone https://github.com/Marx-A00/PatientPortal.git
cd PatientPortal
```

2. Restore dependencies
```bash
dotnet restore
```

3. Run the application
```bash
cd PatientPortal
dotnet run
```
The application will be available at:
- https://localhost:7047
- http://localhost:5245

## Running Tests

You can run tests in several ways:

1. Run all tests from the solution root:
```bash
dotnet test
```

2. Run tests from the test project directory:
```bash
cd PatientPortal.Tests
dotnet test
```

3. Run tests with detailed output:
```bash
dotnet test --logger "console;verbosity=detailed"
```

4. Run a specific test by name:
```bash
dotnet test --filter "FullyQualifiedName~TestName"
```

5. Run tests with coverage:
```bash
dotnet test /p:CollectCoverage=true
```

## Development

- The project uses NUnit for testing
- Test files are located in the `PatientPortal.Tests` directory
- Each new feature should include corresponding unit tests

## Contributing

1. Create a new branch for your feature
```bash
git checkout -b feat/your-feature-name
```

2. Make your changes and write tests
3. Ensure all tests pass
4. Create a pull request

## License

[MIT License](LICENSE)
