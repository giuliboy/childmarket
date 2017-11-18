using Market.Data;
using System;
using System.Collections.Generic;

namespace Market.Service.Contracts
{

    public interface IMarketService : IDisposable
    {
        Seller Add( Seller seller );

        Seller Remove( Seller seller );
        
        IEnumerable<Seller> Sellers { get; }

        IEnumerable<Item> Items { get; }

        Item Add( Item data );

        Item Remove( Item data );

        bool Save();
    }
}
