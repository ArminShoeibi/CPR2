using CPR2.Api.RabbitMQPublishers;
using CPR2.Shared.DTOs;
using Google.Protobuf.WellKnownTypes;
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
        
        _availableFlightsRequestPublisher.Publish(new() 
        {
            DepartureDate = availableFlightsRequestDto.DepartureDate.ToTimestamp(),
            Destination = availableFlightsRequestDto.Destination,
            Origin = availableFlightsRequestDto.Origin
        });
        return Ok();
    }
}
