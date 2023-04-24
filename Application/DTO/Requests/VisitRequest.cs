namespace Application.DTO.Requests
{
    public class VisitRequest
    {
        public string ID_Harmonogram { get; set; }
        public string ID_Pacjent { get; set; }
        public string Notatka { get; set; }
        public string? ID_Klient { get; set; }
    }
}
