using Market.Data;
using Market.Service.Contracts;
using Market.WebAPI.ViewModel;
using Market.WebAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market.WebAPI.Controllers.API
{
    [Route( "api/[controller]" )]
    public class AccountController : Controller
    {
        //create user für die identity db
        private readonly UserManager<Seller> _userManager;

        //wird genutzt, um einen user effektiv ein und aus zuloggen
        private readonly SignInManager<Seller> _signInManager;
        private readonly ILogger _logger;
        private readonly IMarketService _marketService;

        public AccountController( UserManager<Seller> userManager, SignInManager<Seller> signInManager, IMarketService marketService, ILogger<AccountController> logger )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _marketService = marketService;
            _logger = logger;

        }

        /// <summary>
        /// gibt den eingeloggten <see cref="UserViewModel"/> zurück
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet()]
        [ProducesResponseType( typeof( UserViewModel ), 200 )]
        [ProducesResponseType( typeof( UnauthorizedResult ), 401 )]
        public UserViewModel Get()
        {
            var userId = _userManager.GetUserId( User );
            var user = _marketService.Sellers.FirstOrDefault( u => u.Id == userId );

            return user.ToViewModel();
        }

        [Route( "/api/[controller]/register" )]
        [HttpPost]
        [ProducesResponseType( typeof( UserViewModel ), 201 )]
        [ProducesResponseType( typeof( BadRequestResult ), 400 )]
        [AllowAnonymous]
        //TODO tut noch nicht, wieso 
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register( [FromBody]RegisterViewModel registration )
        {
            var newUser = new Seller
            {
                Name = registration.Name,
                FirstName = registration.FirstName,
                UserName = registration.Email,
                Email = registration.Email
            };

            try
            {
                var userCreationResult = await _userManager.CreateAsync( newUser, registration.Password );
                if ( !userCreationResult.Succeeded )
                {
                    foreach ( var error in userCreationResult.Errors )
                    {
                        ModelState.AddModelError( string.Empty, error.Description );
                    }

                    var firstModelError = userCreationResult.Errors.FirstOrDefault();
                    if ( firstModelError == null )
                    {
                        return BadRequest( "unknown error during user registration" );
                    }
                    return BadRequest( userCreationResult.Errors.FirstOrDefault() );
                }

                await _signInManager.PasswordSignInAsync( registration.Name, registration.Password, true, false );

                return CreatedAtAction( "Register", newUser.ToViewModel() );
            }
            catch ( Exception ex )
            {
                _logger.LogError( "Register failed {0}", ex );
                return BadRequest( "error while registering" );
            }


        }

       

        [Route( "/api/[controller]/login" )]
        [HttpPost]
        [ProducesResponseType( typeof( Microsoft.AspNetCore.Identity.SignInResult ), 201 )]
        [ProducesResponseType( typeof( BadRequestResult ), 400 )]
        [ProducesResponseType( typeof( UnauthorizedResult ), 401 )]
        public async Task<IActionResult> Login( [FromBody]LoginViewModel loginAttempt )
        {
            try
            {
                var signInResult = await _signInManager.PasswordSignInAsync( loginAttempt.Email, loginAttempt.Password, true, false );

                if ( !signInResult.Succeeded )
                {
                    _logger.LogError( "Login attempt failed {0}", loginAttempt );
                    return Unauthorized();
                }

                return CreatedAtAction( "Login", signInResult );

            }
            catch ( Exception ex )
            {
                _logger.LogError( "error in Login {0}", ex );
                return BadRequest( "error during login" );
            }
        }

        [Route( "/api/[controller]/logout" )]
        [HttpPost]
        [ProducesResponseType( typeof( SignOutResult ), 200 )]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return new SignOutResult( "identity" );
            }
            catch ( Exception ex )
            {
                _logger.LogError( "Logout failed {0}", ex );
                return StatusCode( 500 );
            }
        }
    }
}
