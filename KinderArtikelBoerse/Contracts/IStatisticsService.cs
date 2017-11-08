using KinderArtikelBoerse.Models;
using KinderArtikelBoerse.Viewmodels;
using System.Collections.Generic;

namespace KinderArtikelBoerse.Contracts
{
    public interface IStatisticsService
    {
        SellStatistic GetStatistics( int sellerId, IEnumerable<ItemViewModel> items );
    }
}
