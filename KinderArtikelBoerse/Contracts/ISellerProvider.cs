using KinderArtikelBoerse.Viewmodels;
using System.Collections.Generic;

namespace KinderArtikelBoerse.Contracts
{
    public interface ISellerProvider
    {
        IEnumerable<SellerViewModel> Sellers { get; }
    }
}
