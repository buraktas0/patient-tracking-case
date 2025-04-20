namespace PatientTrackingApi.Domain.Entities;

public class Patient : BaseEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime BirthDate { get; set; }
}
