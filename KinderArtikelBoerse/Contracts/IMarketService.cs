using KinderArtikelBoerse.Models;
using KinderArtikelBoerse.Viewmodels;
using System.Collections.Generic;

namespace KinderArtikelBoerse.Contracts
{

    public interface IMarketService
    {
        IEnumerable<SellerViewModel> Sellers { get; }

        IEnumerable<ItemViewModel> Items { get; }
    }
}
