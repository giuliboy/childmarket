using KinderArtikelBoerse.Contracts;
using KinderArtikelBoerse.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace KinderArtikelBoerse.Utils
{

    public class MarketDataProvider : IMarketDataProvider
    {
        private readonly string _connectionString;

        public MarketDataProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Seller> Sellers
        {
            get
            {
                using(var ctx = new MarketDbContext( _connectionString ) )
                {
                    return ctx.SellersDbSet
                        .AsNoTracking()
                        //.Include( s => s.Items )
                        .ToList();

                }
            }
        }

        public IEnumerable<Item> Items
        {
            get
            {
                using ( var ctx = new MarketDbContext( _connectionString ) )
                {
                    return ctx.ItemsDbSet
                        .AsNoTracking()
                        .Include( s => s.Seller )
                        .ToList();
                }
            }
        }
    }
}