namespace Application.DTO.Requests
{
    public class KontoChangePasswordRequest
    {
        public string CurrentHaslo { get; set; }

        public string NewHaslo { get; set; }

        public string NewHaslo2 { get; set; }
    }
}
