using KinderArtikelBoerse.Contracts;
using KinderArtikelBoerse.Models;
using System.Collections.Generic;
using System.Linq;

namespace KinderArtikelBoerse.Utils
{

    public class SellerProvider : ISellerProvider
    {
        private readonly string _connectionString;

        public SellerProvider(string connectionString)
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
    }
}