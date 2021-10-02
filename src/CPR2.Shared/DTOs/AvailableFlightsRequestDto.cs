namespace CPR2.Shared.DTOs;

public record AvailableFlightsRequestDto
{
    public string Destination { get; init; }
    public string Origin { get; init; }
    public DateTimeOffset DepartureDate { get; init; }
}
