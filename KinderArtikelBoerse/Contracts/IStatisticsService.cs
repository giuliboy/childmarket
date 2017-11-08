using KinderArtikelBoerse.Models;

namespace KinderArtikelBoerse.Contracts
{
    public interface IStatisticsService
    {
        SellStatistic GetStatistics( int sellerId, IMarketService dataService );
    }
}
