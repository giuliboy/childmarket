using KinderArtikelBoerse.Contracts;
using KinderArtikelBoerse.Models;
using KinderArtikelBoerse.Viewmodels;
using System.Collections.Generic;
using System.Linq;
using System;

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