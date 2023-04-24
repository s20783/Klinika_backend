using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Application.DTO.Request
{
    public partial class VetSpecializationRequest
    {        
        [Required]
        [StringLength(300, MinimumLength = 2, ErrorMessage = "Pole wymaga od 2 do 300 znaków")]
        public string Opis { get; set; }
    }
}
