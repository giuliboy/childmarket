using Market.Data;
using Market.Service.Contracts;
using Market.WebAPI.ViewModel;
using Market.WebAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market.WebAPI.Controllers
{
    [Authorize]
    [Route( "api/[controller]" )]
    [ProducesResponseType( typeof( UnauthorizedResult ), 401 )]
    public class ItemsController : Controller
    {
        private readonly IMarketService _service;
        private readonly IConfiguration _config;
        private readonly UserManager<Seller> _userManager;


        public ItemsController( UserManager<Seller> userManager, IMarketService context, IConfiguration config )
        {
            _service = context;
            _userManager = userManager;
            _config = config;
        }

        [Route( "/api/[controller]" )]
        [HttpGet]
        [ProducesResponseType( typeof( IEnumerable<ItemViewModel> ), 200 )]
        public IEnumerable<ItemViewModel> GetItems()
        {
            var userId = _userManager.GetUserId( User );
            return _service.Items.Where( o => o.Seller.Id == userId )
                .ToViewModels();
        }
    }
}
