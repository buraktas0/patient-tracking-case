namespace PatientTrackingApi.Domain.Entities;

public class Log
{
    public int Id { get; set; }
    public string Action { get; set; }
    public string Endpoint { get; set; }
    public string IpAddress { get; set; }
    public int? UserId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}