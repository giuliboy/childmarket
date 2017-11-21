using Microsoft.AspNetCore.Mvc;
using Market.WebAPI.ViewModel;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Market.Data;
using Market.Service.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Market.WebAPI.Utils;
using System.Linq;

namespace Market.WebAPI.Controllers
{
    [Authorize]
    [Route( "/[controller]" )]
    public class ItemsController : Controller
    {
        protected readonly IMarketService _service;
        protected readonly IConfiguration _config;
        protected readonly UserManager<Seller> _userManager;

        public ItemsController( UserManager<Seller> userManager, IMarketService context, IConfiguration config )
        {
            _service = context;
            _userManager = userManager;
            _config = config;
        }

        [Route( "/[controller]/" )]
        [HttpGet]
        [ProducesResponseType( typeof( IEnumerable<ItemViewModel> ), 200 )]
        public IActionResult Items()
        {
            var userId = _userManager.GetUserId( User );
            return View( _service.Items.Where( i => i.Seller.Id == userId )
                            .ToViewModels() );
        }


        [Route( "/[controller]/id" )]
        [HttpGet]
        [ProducesResponseType( typeof( ItemViewModel ), 200 )]
        public IActionResult Item( string id )
        {
            var userId = _userManager.GetUserId( User );
            return View( _service.Items.Where( i => i.Seller.Id == userId )
                            .FirstOrDefault( i => i.ItemIdentifier == id )
                            .ToViewModel() );
        }
    }
}