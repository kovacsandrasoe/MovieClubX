using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieClubX.Entities.Dto
{
    public class MovieShortViewDto
    {
        public string Id { get; set; } = "";
        public string Title { get; set; } = "";
        public double AvgRate { get; set; } = 0;
        public string Slug { get; set; } = "";

    }
}
