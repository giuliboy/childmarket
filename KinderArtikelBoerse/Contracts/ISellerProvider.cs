using KinderArtikelBoerse.Models;
using System.Collections.Generic;

namespace KinderArtikelBoerse.Contracts
{
    public interface ISellerProvider
    {
        IEnumerable<Seller> Sellers { get; }
    }
}
