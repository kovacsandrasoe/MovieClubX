using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieClubX.Entities.Dto
{
    public class MovieCreateUpdateDto
    {
        public string Title { get; set; } = "";
        public int Rate { get; set; }
    }
}
