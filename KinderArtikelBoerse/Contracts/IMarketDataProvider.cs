using KinderArtikelBoerse.Models;
using System.Collections.Generic;

namespace KinderArtikelBoerse.Contracts
{
    public interface IMarketDataProvider
    {
        IEnumerable<Seller> Sellers { get; }

        IEnumerable<Item> Items { get; }
    }
}
