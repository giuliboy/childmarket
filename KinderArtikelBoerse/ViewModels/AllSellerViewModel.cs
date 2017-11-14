using KinderArtikelBoerse.Contracts;
using KinderArtikelBoerse.Models;
using System.Linq;

namespace KinderArtikelBoerse.Viewmodels
{
    public class AllSellerViewModel : SellerViewModel
    {
        public AllSellerViewModel( IItemsProvider itemsProvider ) 
            : base( new Seller() {
                Id = -1,
                Items = itemsProvider.Items.Select(i => i.Data).ToList()
            } )
        {
        }

        public override float FamilientreffPercentage
        {
            get { return Data.Items.Any()? Data.Items.Average( i => i.Seller.FamilientreffPercentage ) : 0; }
        }

        public override float Revenue
        {
            get
            {
                return Data.Items
                    .Where( i => i.IsSold )
                    .Sum( i => i.Price * ( 1.0f - i.Seller.FamilientreffPercentage ) / 1.0f );
            }
        }

        public override string ToString()
        {
            return "Alle";
        }
    }



    
}
