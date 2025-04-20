namespace PatientTrackingApi.Domain.Responses
{
    public class AuthResult
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}