namespace Application.DTO.Responses
{
    public class GetServiceResponse
    {
        public string ID_Usluga { get; set; }
        public string NazwaUslugi { get; set; }
        public string Opis { get; set; }
        public decimal Cena { get; set; }
        public bool Narkoza { get; set; }
        public string Dolegliwosc { get; set; }
    }
}