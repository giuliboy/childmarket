using KinderArtikelBoerse.Contracts;
using KinderArtikelBoerse.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace KinderArtikelBoerse.Viewmodels
{
    public class StatisticsService : IStatisticsService
    {
        public SellStatistic GetStatistics( int sellerId, IEnumerable<ItemViewModel> items )
        {
            return new SellStatistic()
            {
                SoldItems = items
                    .Where( i => i.SellerId == sellerId )
                    .Count( i => i.IsSold ),

                TotalItems = items
                    .Count( i => i.SellerId == sellerId ),

                SoldValue = items
                    .Where( i => i.SellerId == sellerId )
                    .Where( i => i.IsSold )
                    .Sum( i => i.Price ),
            };
        }
    }

    
}
