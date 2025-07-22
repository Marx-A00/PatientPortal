using PatientPortal.DTOs;

namespace PatientPortal.Services;

public interface IPatientService
{
    Task<IEnumerable<PatientResponseDto>> GetAllPatientsAsync();
    Task<PatientResponseDto?> GetPatientByIdAsync(int id);
    Task<PatientResponseDto> CreatePatientAsync(PatientCreateDto dto);
    Task<PatientResponseDto> UpdatePatientAsync(int id, PatientUpdateDto dto);
    Task DeletePatientAsync(int id);
}