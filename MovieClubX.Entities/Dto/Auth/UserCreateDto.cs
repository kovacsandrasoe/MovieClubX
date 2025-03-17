using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieClubX.Entities.Dto.Auth
{
    public class UserCreateDto
    {
        public required string Email { get; set; } = "";

        public required string Password { get; set; } = "";

        public required string FamilyName { get; set; } = "";

        public required string GivenName { get; set; } = "";
    }
}
