using Microsoft.AspNetCore.Identity;

namespace Identiry_Sample
{
    public class Customer : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public DateTime dob { get; set; } = DateTime.Now;

    }
}
