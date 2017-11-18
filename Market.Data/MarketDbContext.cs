
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KinderArtikelBoerse.Models
{
    public class MarketDbContext : IdentityDbContext
    {
        //static MarketDbContext()
        //{
        //    Database.SetInitializer( new MigrateDatabaseToLatestVersion<MarketDbContext, Configuration>( true ) );
        //}

        public MarketDbContext()
        {
            //default constructor for migration
        }

        public MarketDbContext( DbContextOptions options )
            : base( options )
        {

        }

        public DbSet<Seller> SellersDbSet { get; set; }

        public DbSet<Item> ItemsDbSet { get; set; }
    }
}
