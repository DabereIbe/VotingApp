using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingApp.Models
{
    public class AppUser : IdentityUser
    {
        public bool Voted { get; set; }
    }
}
