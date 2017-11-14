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
            Sellers = new[] { new WildCardSeller(itemsProvider) }
                .Concat(
                    service.Sellers
                    .Select( s => new SellerViewModel( s ) )
                    .ToList() );
        }
        public IEnumerable<SellerViewModel> Sellers { get; }
    }

    
}
