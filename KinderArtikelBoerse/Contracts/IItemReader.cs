using Market.Data;
using System.Collections.Generic;

namespace KinderArtikelBoerse.Contracts
{

    public interface IItemReader
    {
        IEnumerable<Item> ReadItems( string filePath );
    }
}
