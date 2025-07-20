using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientPortal.DTOs;
using PatientPortal.Controllers;
using PatientPortal.Services;

namespace PatientPortal.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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


}