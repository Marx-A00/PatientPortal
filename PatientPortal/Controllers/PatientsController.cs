using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientPortal.DTOs;
using PatientPortal.Services;

namespace PatientPortal.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "ApiScope")] // Use JWT Bearer token policy
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;
    private readonly ILogger<PatientsController> _logger;

    public PatientsController(IPatientService patientService, ILogger<PatientsController> logger)
    {
        _patientService = patientService;
        _logger = logger;
    }

    /// <summary>
    /// Get all patients
    /// </summary>
    /// <returns>List of patients</returns>
    /// 
    /// 
    /// 
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatientResponseDto>>> GetAllPatients()
    {
        try
        {
            _logger.LogInformation("API: Getting all patients");
            var patients = await _patientService.GetAllPatientsAsync();
            return Ok(patients);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all patients");
            return StatusCode(500, "An error occurred while retrieving patients");
        }

    }

    /// <summary>
    /// Get patient by ID
    /// </summary>
    /// <returns>Patient details</returns>
    /// 
    [HttpGet("{id}")]
    public async Task<ActionResult<PatientResponseDto>> GetPatient(int id)
    {
        try
        {
            _logger.LogInformation("API: Getting patient with ID: {Id}", id);
            var patient = await _patientService.GetPatientByIdAsync(id);

            if (patient == null)
            {
                return NotFound($"Patient with ID {id} not found");
            }
            return Ok(patient);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting patient with ID: {id}", id);
            return StatusCode(500, "An error occurred while retrieving the patient");
        }
    }

    [HttpPost]
    public async Task<ActionResult<PatientResponseDto>> CreatePatient([FromBody] PatientCreateDto createDto)
    {
        try
        {
            _logger.LogInformation("API: Creating new patient: {Email}", createDto.Email);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdPatient = await _patientService.CreatePatientAsync(createDto);

            return CreatedAtAction(
                nameof(GetPatient),
                new { id = createdPatient.Id },
                createdPatient);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid data provided for patient creation");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating patient");
            return StatusCode(500, "An error occurred while creating the patient");
        }

    }
    /// <summary>
    /// Update an existing patient
    /// </summary>
    /// <param name="id">Patient ID</param>
    /// <param name="updateDto">Patient update data</param>
    /// <returns>Updated patient</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<PatientResponseDto>> UpdatePatient(int id, [FromBody] PatientUpdateDto updateDto)
    {
        try
        {
            _logger.LogInformation("API: Updating patient with ID: {Id}", id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedPatient = await _patientService.UpdatePatientAsync(id, updateDto);
            return Ok(updatedPatient);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid data provided for patient update");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating patient with ID: {Id}", id);
            return StatusCode(500, "An error occurred while updating the patient");
        }
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePatient(int id)
    {
        try
        {
            _logger.LogInformation("API: Deleting patient with ID: {Id}", id);
            await _patientService.DeletePatientAsync(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Patient not found for deletion");
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting patient with ID: {Id}", id);
            return StatusCode(500, "An error occurred while deleting the patient");
        }
    }
    [HttpGet("health")]
    public ActionResult<string> HealthCheck()
    {
        return Ok($"API is working! Time: {DateTime.Now}");
    }
}