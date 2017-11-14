using KinderArtikelBoerse.Contracts;
using System.Collections.Generic;
using System.Linq;

/*
 * TODOS:
 * Schalter um verkaufte items zu filtern 
 * Kassa VM isolieren
 * "neuer Kunde" und Summe als Knopf kombiniert
 * 
 */

namespace KinderArtikelBoerse.Viewmodels
{
    public class SellersViewModelProvider : ISellerProvider
    {
        public SellersViewModelProvider(IMarketService service, IItemsProvider itemsProvider)
        {
            Sellers = new[] { new AllSellerViewModel(itemsProvider) }
                .Concat(
                    service.Sellers
                    .Select( s => new SellerViewModel( s, itemsProvider ) )
                    .ToList() );
        }
        public IEnumerable<SellerViewModel> Sellers { get; }
    }

    
}
