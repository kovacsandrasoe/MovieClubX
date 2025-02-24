using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieClubX.Entities.Dto
{
    public class MovieViewDto
    {
        public string Id { get; set; } = "";
        public string Title { get; set; } = "";
        public int Rate { get; set; }

        public int TitleLenght
        {
            get
            {
                return Title.Length;
            }
        }
    }
}
