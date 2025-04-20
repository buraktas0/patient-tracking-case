using PatientTrackingApi.Controllers;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace PatientTrackingApi.Tests;

public class PredictionContollerTest
{
    private readonly Mock<ILogger<PredictionController>> _mockLogger;
    private readonly PredictionController _controller;

    public PredictionContollerTest()
    {
        _mockLogger = new Mock<ILogger<PredictionController>>();
        _controller = new PredictionController(_mockLogger.Object);
    }

    [Fact]
    public async Task GetPredictionsByPatientId_ReturnsOk_WithMockData()
    {
        int patientId = 1;

        var result = await _controller.GetPredictionsByPatientId(patientId);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var predictions = Assert.IsType<List<object>>(okResult.Value);
        Assert.Equal(3, predictions.Count);
    }
}
