﻿namespace Application.DTO.Requests
{
    public class ServiceRequest
    {
        public string NazwaUslugi { get; set; }
        public string Opis { get; set; }
        public decimal Cena { get; set; }
        public bool Narkoza { get; set; }
        public string Dolegliwosc { get; set; }
    }
}