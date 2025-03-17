using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MovieClubX.Endpoint.Helpers
{
    public class AppUser : IdentityUser
    {
        [StringLength(200)]
        public required string FamilyName { get; set; } = "";

        [StringLength(200)]
        public required string GivenName { get; set; } = "";
    }
}
