using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Market.WebAPI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if ( !User.Identity.IsAuthenticated )
            {
                return Redirect( "user/login" );
            }

            return Redirect("items");
        }
    }
}