using KinderArtikelBoerse.Viewmodels;
using System.Collections.Generic;

namespace KinderArtikelBoerse.Contracts
{
    public interface IItemsProvider
    {
        IList<ItemViewModel> Items { get; }
    }
}
