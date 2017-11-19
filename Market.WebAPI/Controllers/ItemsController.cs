using Market.Data;
using Market.Service.Contracts;
using Market.WebAPI.ViewModel;
using Market.WebAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

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

        [Route( "/[controller]" )]
        [HttpGet]
        [ProducesResponseType( typeof( IEnumerable<ItemViewModel> ), 200 )]
        public IActionResult Items()
        {
            return View(GetItems());
        }

        [Route( "/api/[controller]/id" )]
        [HttpGet]
        [ProducesResponseType( typeof(ItemViewModel ), 200 )]
        public ItemViewModel GetItem(string id)
        {
            var userId = _userManager.GetUserId( User );
            return _service.Items.Where( o => o.Seller.Id == userId )
                .FirstOrDefault(i => i.ItemIdentifier == id)
                .ToViewModel();
        }

        [Route( "/[controller]/id" )]
        [HttpGet]
        [ProducesResponseType( typeof( ItemViewModel ), 200 )]
        public IActionResult Item( string id )
        {
            return View( GetItem( id ) );
        }

        // POST api/orders
        //[FromBody] weglassen : ASP wird das standard model binding system verwenden. 
        //ist json oder xml im Spiel für die repräsentation der objekte => [FromBody] einsetzen
        [HttpPost]
        [ProducesResponseType( typeof( ItemViewModel ), 201 )]
        [ProducesResponseType( typeof( BadRequestResult ), 400 )]
        public IActionResult Post( [FromBody]ItemViewModel item )
        {

            var userId = _userManager.GetUserId( User );

            var newItem = new Item
            {
                ItemIdentifier = item.ItemIdentifier,
                Seller = _service.Sellers.First( s => s.Id == userId ),
                Description = item.Description,
                Size = item.Size,
                Price = item.Price
            };

            _service.Add( newItem );
            _service.Save();

            return CreatedAtAction( "Get", newItem.ToViewModel() );
        }
    }
}
