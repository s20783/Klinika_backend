using Application.ReceptaLeki.Queries;
using System;
using System.Collections.Generic;

namespace Application.DTO.Responses
{
    public class GetReceptaResponse
    {
        public string ID_Recepta { get; set; }
        public string? Pacjent { get; set; }
        public string Zalecenia { get; set; }
        public DateTime? WizytaData { get; set; }
        public List<GetReceptaLekResponse> Leki { get; set; }
    }
}