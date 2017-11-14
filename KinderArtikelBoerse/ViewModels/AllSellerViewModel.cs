﻿using KinderArtikelBoerse.Contracts;
using KinderArtikelBoerse.Models;
using System.Collections.Generic;
using System.Linq;

namespace KinderArtikelBoerse.Viewmodels
{
    public class AllSellerViewModel : SellerViewModel
    {
        public AllSellerViewModel( IItemsProvider itemsProvider ) 
            : base( new Seller() {
                Id = -1,
            } , itemsProvider )
        {
        }

        public override IEnumerable<ItemViewModel> Items
        {
            get
            {
                return _itemsProvider.Items;
                    
            }
        }

        public override float FamilientreffPercentage
        {
            get { return Items.Any()? Items.Average( i => i.Seller.FamilientreffPercentage ) : 0; }
        }

        public override float Revenue
        {
            get
            {
                return Items
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
