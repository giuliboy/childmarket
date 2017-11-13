using KinderArtikelBoerse.Models;
using System;
using System.Collections.Generic;

namespace KinderArtikelBoerse.Contracts
{

    public interface IMarketService : IDisposable
    {
        Seller Add( Seller seller );

        Seller Remove( Seller seller );
        
        IEnumerable<Seller> Sellers { get; }

        IEnumerable<Item> Items { get; }

        Item Add( Item data );

        Item Remove( Item data );
    }
}
