using KinderArtikelBoerse.Contracts;
using KinderArtikelBoerse.Models;
using System.Collections.Generic;
using System.Linq;

namespace KinderArtikelBoerse.Utils
{

    public class MarketDataService : IMarketService
    {
        private readonly MarketDbContext _context;

        public MarketDataService(string connectionString)
        {
            _context = new MarketDbContext( connectionString);
        }

        public Seller Add( Seller seller )
        {
            var addedEntity = _context.SellersDbSet.Add( seller );

            _context.SaveChanges();
            return addedEntity;
        }

        public Seller Remove( Seller seller )
        {
            var removedEntity = _context.SellersDbSet.Remove( seller );

            _context.SaveChanges();
            return removedEntity;
        }

        public Item Add( Item data )
        {
            var addedEntity = _context.ItemsDbSet.Add( data );

            _context.SaveChanges();
            return addedEntity;
        }

        public Item Remove( Item data )
        {
            var removedEntity = _context.ItemsDbSet.Remove( data );

            _context.SaveChanges();
            return removedEntity;
        }

        public void Dispose()
        {
            
        }

        public IEnumerable<Seller> Sellers
        {
            get
            {
                return _context.SellersDbSet
                        .AsEnumerable();
            }
        }

        public IEnumerable<Item> Items
        {
            get
            {
                return _context.ItemsDbSet
                    .AsEnumerable();
            }
        }
    }
}