using Microsoft.Extensions.Logging;
using PatientPortal.DTOs;
using PatientPortal.Models;
using PatientPortal.Repositories;

namespace PatientPortal.Services;


public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly ILogger<PatientService> _logger;

    public PatientService(IPatientRepository patientRepository, ILogger<PatientService> logger)
    {
        _patientRepository = patientRepository;
        _logger = logger;
    }
}