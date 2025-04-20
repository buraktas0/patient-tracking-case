using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientTrackingApi.Data.Repositories;
using PatientTrackingApi.Domain.Entities;
using PatientTrackingApi.Domain.Requests;

namespace PatientTrackingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "User")]
public class PatientController : ControllerBase
{
    private readonly PatientRepository patientRepo;
    private readonly ILogger<PatientController> logger;

    public PatientController(PatientRepository patientRepo, ILogger<PatientController> logger)
    {
        this.patientRepo = patientRepo;
        this.logger = logger;
    }

    /// <summary>
    /// Retrieves all patients from the database.   
    /// </summary>
    /// <returns>A list of patients.</returns>  
    /// <response code="200">Returns a list of patients.</response>
    /// <response code="404">No patients found.</response>
    /// <response code="500">An error occurred while retrieving patients.</response>
    [HttpGet(Name = "GetAllPatients")]
    public async Task<ActionResult<List<PatientVM>>> GetAll()
    {
        try
        {
            var patients = await patientRepo.GetAllAsync();
            if (patients != null && patients.Count > 0)
            {
                var patientVMs = patients.Select(p => new PatientVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    Surname = p.Surname,
                    BirthDate = p.BirthDate
                }).ToList();

                return Ok(patientVMs);
            }

            return NotFound("No patients found.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while retrieving patients.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    /// <summary>
    /// Retrieves a patient by ID from the database.
    /// </summary>
    /// <param name="id">The ID of the patient.</param>
    /// <returns>The patient with the specified ID.</returns>
    /// <response code="200">Returns the patient.</response>
    /// <response code="404">Patient not found.</response>
    /// <response code="500">An error occurred while retrieving the patient.</response>
    [HttpGet("{id}", Name = "GetPatientById")]
    public async Task<ActionResult<PatientVM>> GetById(int id)
    {
        try
        {
            var patient = await patientRepo.GetByIdAsync(id);
            if (patient != null)
            {
                var patientVM = new PatientVM
                {
                    Id = patient.Id,
                    Name = patient.Name,
                    Surname = patient.Surname,
                    BirthDate = patient.BirthDate
                };
                return Ok(patientVM);
            }

            return NotFound($"Patient with ID {id} not found.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred while retrieving patient with ID {id}.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    /// <summary>
    /// Creates a new patient in the database.
    /// </summary>
    /// <param name="request">The patient data to create.</param>
    /// <returns>The created patient.</returns>
    /// <response code="200">Returns the created patient.</response>
    /// <response code="400">Invalid patient data.</response>
    /// <response code="500">An error occurred while creating the patient.</response>
    [HttpPost(Name = "CreatePatient")]
    public async Task<ActionResult<PatientVM>> Create([FromBody] PatientCreateRequest request)
    {
        try
        {
            if (request == null)
                return BadRequest("Patient data is required.");

            var createdPatient = await patientRepo.CreateAsync(request);
            if (createdPatient != null)
            {
                var patientVM = new PatientVM
                {
                    Id = createdPatient.Id,
                    Name = createdPatient.Name,
                    Surname = createdPatient.Surname,
                    BirthDate = createdPatient.BirthDate
                };

                return Ok(patientVM);
            }

            return StatusCode(500, "An error occurred while creating the patient.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating a new patient.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    /// <summary>
    /// Updates an existing patient in the database.
    /// </summary>
    /// <param name="request">The updated patient data.</param>
    /// <returns>The updated patient.</returns>
    /// <response code="200">Returns the updated patient.</response>
    /// <response code="400">Invalid patient data.</response>
    /// <response code="404">Patient not found.</response>
    /// <response code="500">An error occurred while updating the patient.</response>
    [HttpPut(Name = "UpdatePatient")]
    public async Task<ActionResult<PatientVM>> Update([FromBody] PatientUpdateRequest request)
    {
        try
        {
            if (request == null)
                return BadRequest("Patient data is required.");

            var updatedPatient = await patientRepo.UpdateAsync(request);
            if (updatedPatient != null)
            {
                var patientVM = new PatientVM
                {
                    Id = updatedPatient.Id,
                    Name = updatedPatient.Name,
                    Surname = updatedPatient.Surname,
                    BirthDate = updatedPatient.BirthDate
                };

                return Ok(patientVM);
            }

            return NotFound($"Patient with ID {request.Id} not found.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while updating the patient.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    /// <summary>
    /// Deletes a patient by ID from the database.
    /// </summary>
    /// <param name="id">The ID of the patient to delete.</param>
    /// <returns>No content if the deletion was successful.</returns>
    /// <response code="204">Patient deleted successfully.</response>
    /// <response code="404">Patient not found.</response>
    /// <response code="500">An error occurred while deleting the patient.</response>
    [HttpDelete("{id}", Name = "DeletePatient")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var deleted = await patientRepo.DeleteAsync(id);
            if (deleted)
                return NoContent();

            return NotFound($"Patient with ID {id} not found.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred while deleting patient with ID {id}.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}
