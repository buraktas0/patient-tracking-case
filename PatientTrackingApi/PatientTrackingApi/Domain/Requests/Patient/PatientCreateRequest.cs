namespace PatientTrackingApi.Domain.Requests
{
    public class PatientCreateRequest
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
    }
}