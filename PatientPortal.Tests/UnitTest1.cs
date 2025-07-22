using Microsoft.Extensions.Logging;
using Moq;
using PatientPortal.DTOs;
using PatientPortal.Models;
using PatientPortal.Repositories;
using PatientPortal.Services;

namespace PatientPortal.Tests;

[TestFixture]

public class PatientServiceTests
{

    private Mock<IPatientRepository> _mockRepository;
    private Mock<ILogger<PatientService>> _mockLogger;
    private PatientService _patientService;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IPatientRepository>();
        _mockLogger = new Mock<ILogger<PatientService>>();

        _patientService = new PatientService(_mockRepository.Object, _mockLogger.Object);
    }

    [Test]
    public async Task CreatePatient_HappyPath_ShouldCreatePatientSuccessfully()
    {
        // This is THE most important test - does the core functionality work?
        var dto = new PatientCreateDto
        {
            Name = "John Doe",
            DOB = new DateTime(1990, 1, 1),
            Email = "john@test.com"
        };

        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Patient>()))
                      .ReturnsAsync(new Patient { Id = 1, Name = "John Doe", Email = "john@test.com" });

        var result = await _patientService.CreatePatientAsync(dto);

        Assert.That(result.Name, Is.EqualTo("John Doe"));
        // If this fails, your app is broken. Everything else is secondary.
    }

    [Test]
    public void CreatePatientAsync_WithFutureDOB_ShouldThrowArgumentException()
    {
        var createDto = new PatientCreateDto
        {
            Name = "Test Patient",
            DOB = DateTime.Now.AddDays(1),
            Email = "test@example.com"
        };

        var exception = Assert.ThrowsAsync<ArgumentException>(
            async () => await _patientService.CreatePatientAsync(createDto));

        Assert.That(exception.Message, Does.Contain("Date of birth cannot be in the future"));
    }
    [Test]
    public async Task CreatePatientAsync_WithValidData_ShouldReturnPatientResponseDto()
    {
        var createDto = new PatientCreateDto
        {
            Name = "Valid Patient",
            DOB = new DateTime(1990, 1, 1),
            Email = "valid@example.com"
        };

        var expectedPatient = new Patient
        {
            Id = 1,
            Name = createDto.Name,
            DOB = createDto.DOB,
            Email = createDto.Email
        };

        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Patient>()))
                        .ReturnsAsync(expectedPatient);

        var result = await _patientService.CreatePatientAsync(createDto);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(result.Name, Is.EqualTo("Valid Patient"));
        Assert.That(result.Email, Is.EqualTo("valid@example.com"));
    }
}
