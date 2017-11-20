using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Market.WebAPI.ViewModel;
using Market.WebAPI.Controllers.API;

namespace Market.WebAPI.Controllers
{
    public class AccountController : Controller
    {
        private API.AccountController _api;

        public AccountController( API.AccountController api)
        {
            _api = api;
        }


        [Route( "/register" )]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [Route( "/register" )]
        [HttpPost]
        [ProducesResponseType( typeof( UserViewModel ), 201 )]
        [ProducesResponseType( typeof( BadRequestResult ), 400 )]
        public async Task<IActionResult> Register( RegisterViewModel registration )
        {
            return await _api.Register( registration );
        }
    }
}