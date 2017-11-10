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

        public Seller Add( Seller seller )
        {
            using(var ctx = new MarketDbContext( _connectionString ) )
            {
                var addedEntity = ctx.SellersDbSet.Add( seller );

                ctx.SaveChanges();
                return addedEntity;
            }
        }

        public Seller Remove( Seller seller )
        {
            using ( var ctx = new MarketDbContext( _connectionString ) )
            {
                var removedEntity = ctx.SellersDbSet.Remove( seller );

                ctx.SaveChanges();
                return removedEntity;
            }
        }

        public void Update( int sellerId, Seller seller )
        {
            using ( var ctx = new MarketDbContext( _connectionString ) )
            {
                var updatingSeller = ctx.SellersDbSet.FirstOrDefault( s => s.Id == sellerId );
                if ( updatingSeller == null )
                {
                    return;
                }

                updatingSeller.SoldItems = seller.SoldItems;
                updatingSeller.TotalItems = seller.TotalItems;
                updatingSeller.SoldValue = seller.SoldValue;
                updatingSeller.Name = seller.Name;
                updatingSeller.FirstName = seller.FirstName;
                updatingSeller.FamilientreffPercentage = seller.FamilientreffPercentage;

                ctx.SaveChanges();
            }
        }

        public IEnumerable<Seller> Sellers
        {
            get
            {
                using(var ctx = new MarketDbContext( _connectionString ) )
                {
                    return ctx.SellersDbSet
                        .AsNoTracking()
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
                        .ToList();
                }
            }
        }
    }
}