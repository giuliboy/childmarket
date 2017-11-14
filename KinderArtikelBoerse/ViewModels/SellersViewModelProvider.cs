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
        public SellersViewModelProvider(IMarketService service)
        {
            Sellers = service.Sellers.Select( s => new SellerViewModel( s ) );
        }
        public IEnumerable<SellerViewModel> Sellers { get; }
    }

    
}
