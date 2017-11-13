using KinderArtikelBoerse.Migrations;
using System.Data.Entity;
using System.Linq;

namespace KinderArtikelBoerse.Models
{
    public class MarketDbContext : DbContext
    {
        static MarketDbContext()
        {
            Database.SetInitializer( new MigrateDatabaseToLatestVersion<MarketDbContext, Configuration>( true ) );
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

        public DbSet<Item> ItemsDbSet { get; set; }
    }
}
