namespace PatientTrackingApi.Domain.Entities;

public class PatientVM
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime BirthDate { get; set; }
}
