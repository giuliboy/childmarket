using KinderArtikelBoerse.Contracts;
using KinderArtikelBoerse.Models;
using System.Data;
using System.Linq;


namespace KinderArtikelBoerse.Viewmodels
{
    public class StatisticsService : IStatisticsService
    {
        public SellStatistic GetStatistics( int sellerId, IMarketService dataService )
        {
            return new SellStatistic()
            {
                SoldItems = dataService.Items
                    .Where( i => i.SellerId == sellerId )
                    .Count( i => i.IsSold ),

                TotalItems = dataService.Items
                    .Count( i => i.SellerId == sellerId ),

                SoldValue = dataService.Items
                    .Where( i => i.SellerId == sellerId )
                    .Where( i => i.IsSold )
                    .Sum( i => i.Price ),
            };
        }
    }

    
}
