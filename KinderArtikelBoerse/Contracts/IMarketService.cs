using KinderArtikelBoerse.Models;
using KinderArtikelBoerse.Viewmodels;
using System.Collections.Generic;

namespace KinderArtikelBoerse.Contracts
{

    public interface IMarketService
    {
        Seller Add( Seller seller );

        Seller Remove( Seller seller );
        
        IEnumerable<Seller> Sellers { get; }

        IEnumerable<Item> Items { get; }

    }
}
