using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using Market.Service.Contracts;
using Market.Data;

namespace Market.Service
{

    public class MarketService : IMarketService
    {
        private readonly MarketDbContext _context;

        public MarketService( DbContextOptions options )
        {
            
            _context = new MarketDbContext( options );

        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public Seller Add( Seller seller )
        {
            return  _context.Users.Add( seller ).Entity;

        }

        public Seller Remove( Seller seller )
        {
            return _context.Users.Remove( seller ).Entity; 
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
                return _context.Users
                        .AsEnumerable();
            }
        }

        public IEnumerable<Item> Items
        {
            get
            {
                return _context.ItemsDbSet
                    .Include(i => i.Seller)
                    .AsEnumerable();
            }
        }
    }
}