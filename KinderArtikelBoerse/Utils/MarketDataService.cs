using KinderArtikelBoerse.Contracts;
using KinderArtikelBoerse.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace KinderArtikelBoerse.Utils
{

    public class MarketDataService : IMarketService
    {
        private readonly MarketDbContext _context;

        public MarketDataService(string connectionString)
        {
            _context = new MarketDbContext( );
        }

        public bool Save()
        {
            return true;// _context() > 0;
        }

        public Seller Add( Seller seller )
        {
            return  _context.SellersDbSet.Add( seller ).Entity;

        }

        public Seller Remove( Seller seller )
        {
            return _context.SellersDbSet.Remove( seller ).Entity; 
        }

        public Item Add( Item data )
        {
            return _context.ItemsDbSet.Add( data ).Entity;
        }

        public Item Remove( Item data )
        {
            return _context.ItemsDbSet.Remove( data ).Entity;

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