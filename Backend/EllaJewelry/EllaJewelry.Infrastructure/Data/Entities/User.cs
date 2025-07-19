using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EllaJewelry.Infrastructure.Data.Entities
{
    public class User : IdentityUser
    {
        public bool SubscribedToNewsletter { get; set; } = false;

    }
}
