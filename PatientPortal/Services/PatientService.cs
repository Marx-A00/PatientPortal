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

    public async Task<IEnumerable<PatientResponseDto>> GetAllPatientsAsync()
    {
        _logger.LogInformation("Retrieving all patients");
        var patients = await _patientRepository.GetAllAsync();
        return patients.Select(PatientResponseDto.FromPatient);
    }

    public async Task<PatientResponseDto?> GetPatientByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving patient with ID: {Id}", id);
        var patient = await _patientRepository.GetByIdAsync(id);
        return patient != null ? PatientResponseDto.FromPatient(patient) : null;
    }

    public async Task<PatientResponseDto> CreatePatientAsync(PatientCreateDto dto)
    {
        _logger.LogInformation("Creating new patient: {Email}", dto.Email);
        
        // Business validation
        if (dto.DOB > DateTime.Now)
        {
            throw new ArgumentException("Date of birth cannot be in the future");
        }

        var patient = new Patient
        {
            Name = dto.Name,
            DOB = dto.DOB,
            Email = dto.Email
        };

        var createdPatient = await _patientRepository.CreateAsync(patient);
        _logger.LogInformation("Patient created successfully: {Id}", createdPatient.Id);
        
        return PatientResponseDto.FromPatient(createdPatient);
    }

    public async Task<PatientResponseDto> UpdatePatientAsync(int id, PatientUpdateDto dto)
    {
        _logger.LogInformation("Updating patient with ID: {Id}", id);
        
        var existingPatient = await _patientRepository.GetByIdAsync(id);
        if (existingPatient == null)
        {
            throw new ArgumentException($"Patient with ID {id} not found");
        }

        // Business validation
        if (dto.DOB > DateTime.Now)
        {
            throw new ArgumentException("Date of birth cannot be in the future");
        }

        existingPatient.Name = dto.Name;
        existingPatient.DOB = dto.DOB;
        existingPatient.Email = dto.Email;

        var updatedPatient = await _patientRepository.UpdateAsync(existingPatient);
        _logger.LogInformation("Patient updated successfully: {Id}", updatedPatient.Id);
        
        return PatientResponseDto.FromPatient(updatedPatient);
    }

    public async Task DeletePatientAsync(int id)
    {
        _logger.LogInformation("Deleting patient with ID: {Id}", id);
        
        if (!await _patientRepository.ExistsAsync(id))
        {
            throw new ArgumentException($"Patient with ID {id} not found");
        }

        await _patientRepository.DeleteAsync(id);
        _logger.LogInformation("Patient deleted successfully: {Id}", id);
    }
}