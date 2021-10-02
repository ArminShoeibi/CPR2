using CPR2.Api.RabbitMQPublishers;
using CPR2.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CPR2.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AvailableFlightsController : ControllerBase
{
    private readonly AvailableFlightsRequestPublisher _availableFlightsRequestPublisher;

    public AvailableFlightsController(AvailableFlightsRequestPublisher availableFlightsRequestPublisher)
    {
        _availableFlightsRequestPublisher = availableFlightsRequestPublisher;
    }


    [HttpGet]
    public IActionResult GetAvailableFlights([FromQuery] AvailableFlightsRequestDto availableFlightsRequestDto)
    {
        _availableFlightsRequestPublisher.PublishAvailableFlightsRequest(availableFlightsRequestDto);
        return Ok();
    }
}
