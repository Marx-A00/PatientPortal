using PatientPortal.Models;

namespace PatientPortal.Repositories;

public interface IPatientRepository
{
    Task<IEnumerable<Patient>> GetAllAsync();
    Task<Patient?> GetByIdAsync(int id);
    Task<Patient> CreateAsync(Patient patinet);
    Task<Patient> UpdateAsync(Patient patient);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}