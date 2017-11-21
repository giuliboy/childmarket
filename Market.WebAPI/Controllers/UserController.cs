using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Market.WebAPI.ViewModel;

namespace Market.WebAPI.Controllers
{

    [Route("[controller]")]
    public class UserController : Controller
    {
        private API.UserController _api;

        public UserController( API.UserController api)
        {
            _api = api;
        }


        [Route( "register" )]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View("Login");
        }

        [Route( "register" )]
        [HttpPost]
        [ProducesResponseType( typeof( UserViewModel ), 201 )]
        [ProducesResponseType( typeof( BadRequestResult ), 400 )]
        public async Task<IActionResult> Register( RegisterViewModel registration )
        {
            await _api.Register( registration );
            return Redirect( "/Items" );
        }
        
        [Route( "login" )]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View( "Login" );
        }

        [Route( "login" )]
        [HttpPost]
        [ProducesResponseType( typeof( BadRequestResult ), 400 )]
        public async Task<IActionResult> Login( LoginViewModel login )
        {
            var result = await _api.Login( login );

            if((result is BadRequestResult ))
            {
                return result;
            }
            return Redirect( "/Items" );
        }
    }
}