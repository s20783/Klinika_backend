using System;

namespace Application.DTO.Responses
{
    public class GetMedicamentWarehouseResponse
    {
        public string IdStanLeku { get; set; }
        public int Ilosc { get; set; }
        public DateTime DataWaznosci { get; set; }
    }
}