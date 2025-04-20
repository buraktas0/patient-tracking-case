using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PatientTrackingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "User")]
public class PredictionController : ControllerBase
{
    private readonly ILogger<PredictionController> logger;

    public PredictionController(ILogger<PredictionController> logger)
    {
        this.logger = logger;
    }

    /// <summary>
    /// Mock endpoint for ai supported predictions.
    /// </summary>
    /// <param name="patientId">The ID of the patient.</param>
    /// <returns>A list of predictions for the specified patient.</returns>
    /// <response code="200">Returns a list of predictions.</response>
    /// <response code="500">An error occurred while retrieving predictions.</response>
    [HttpGet("{patientId}", Name = "GetPredictionsByPatientId")]
    public async Task<ActionResult<List<object>>> GetPredictionsByPatientId(int patientId)
    {
        try
        {
            var predictions = new List<object>
            {
                new { Id = 1, PatientId = patientId, Detail = "Prediction Result 1" },
                new { Id = 2, PatientId = patientId, Detail = "Prediction Result 2" },
                new { Id = 3, PatientId = patientId, Detail = "Prediction Result 3" }
            };

            return Ok(predictions);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while retrieving the prediction.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

}