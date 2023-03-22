namespace Application.ReceptaLeki.Queries
{
    public class GetReceptaLekResponse
    {
        public string ID_Lek { get; set; }
        public string Nazwa { get; set; }
        public string JednostkaMiary { get; set; }
        public string Producent { get; set; }
        public int Ilosc { get; set; }
    }
}