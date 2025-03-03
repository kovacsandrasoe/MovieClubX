using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieClubX.Entities.Dto
{
    public class RateCreateDto
    {
        public string? Text { get; set; }

        public int Value { get; set; }

        public string MovieId { get; set; } = "";
    }
}
