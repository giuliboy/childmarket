using KinderArtikelBoerse.Contracts;
using KinderArtikelBoerse.Models;
using KinderArtikelBoerse.Viewmodels;
using System.Collections.Generic;
using System.Linq;

namespace KinderArtikelBoerse.Utils
{

    public class MarketDataService : IMarketService
    {
        private readonly string _connectionString;

        public MarketDataService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<SellerViewModel> Sellers
        {
            get
            {
                using(var ctx = new MarketDbContext( _connectionString ) )
                {
                    return ctx.SellersDbSet
                        .AsNoTracking()
                        .Select(s => new SellerViewModel(s))
                        .ToList();
                }
            }
        }

        public IEnumerable<ItemViewModel> Items
        {
            get
            {
                using ( var ctx = new MarketDbContext( _connectionString ) )
                {
                    return ctx.ItemsDbSet
                        .AsNoTracking()
                        .Select(i => new ItemViewModel(i))
                        .ToList();
                }
            }
        }
    }
}