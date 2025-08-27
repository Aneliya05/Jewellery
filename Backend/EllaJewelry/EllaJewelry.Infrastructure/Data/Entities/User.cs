using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EllaJewelry.Infrastructure.Data.Entities
{
    public class User : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        public bool SubscribedToNewsletter { get; set; } = false;

        public User()
        {

        }
        public User(string email, string firstName, string surname)
        {
            this.UserName = email;
            this.Email = email;
            this.NormalizedEmail = email.ToUpper();
            this.FirstName = firstName;
            this.LastName = surname;
        }
    }
}
