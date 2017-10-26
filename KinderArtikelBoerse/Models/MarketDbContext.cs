﻿using System.Data.Entity;

namespace KinderArtikelBoerse.Models
{
    public class MarketDbContext : DbContext
    {
        static MarketDbContext()
        {
            Database.SetInitializer( new CreateDatabaseIfNotExists<MarketDbContext>() );
        }

        public MarketDbContext()
        {
            //default constructor for migration
        }

        public MarketDbContext( string connString )
            : base( connString )
        {
            
        }

        public DbSet<Seller> SellersDbSet { get; set; }
    }
}