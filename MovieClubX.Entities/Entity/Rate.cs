using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieClubX.Entities.Helpers;

namespace MovieClubX.Entities.Entity
{
    public class Rate : IIdEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string? Text { get; set; }

        public int Value { get; set; }

        public string MovieId { get; set; } = "";

        [NotMapped]
        public virtual Movie? Movie { get; set; }
    }
}
