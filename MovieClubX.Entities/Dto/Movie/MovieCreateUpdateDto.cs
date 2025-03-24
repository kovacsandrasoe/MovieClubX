using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieClubX.Entities.Dto.Movie
{
    public class MovieCreateUpdateDto
    {
        public string Title { get; set; } = "";
        public string Genre { get; set; } = "";
    }
}
