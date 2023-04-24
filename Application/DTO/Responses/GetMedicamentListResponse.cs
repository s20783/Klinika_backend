namespace Application.DTO.Responses
{
    public class GetMedicamentListResponse
    {
        public string IdLek { get; set; }
        public string Nazwa { get; set; }
        public int Ilosc { get; set; }
        public string JednostkaMiary { get; set; }
        public string Producent { get; set; }
    }
}
