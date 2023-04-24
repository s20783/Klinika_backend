using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Application.DTO.Request
{
    public partial class DiscountRequest
    {
        [Required]
        [StringLength(40, MinimumLength = 2, ErrorMessage = "Pole wymaga od 2 do 40 znaków")]
        public string NazwaZnizki { get; set; }

        [Required]
        [Range(0,100)]
        public float ProcentZnizki { get; set; }

        public DateTime? DoKiedy { get; set; }
    }
}
