using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market.WebAPI.ViewModel
{

    public class UserViewModel
    {
        public string Id { get; internal set; }

        public string Name { get; set; }

        public string FirstName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
