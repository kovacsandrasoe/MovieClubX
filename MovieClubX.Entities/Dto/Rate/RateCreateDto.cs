using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieClubX.Entities.Dto.Rate
{
    public class RateCreateDto
    {
        public string? Text { get; set; }

        [Range(1, 5)]
        public int Value { get; set; }

        [Required]
        [MinLength(3)]
        public string MovieId { get; set; } = "";
    }
}
