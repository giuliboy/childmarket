using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using KinderArtikelBoerse.Contracts;
using Market.Data;

namespace KinderArtikelBoerse.Viewmodels
{

    public class SellerViewModel : PropertyChangeNotifier
    {
        protected IItemsProvider _itemsProvider;

        public Seller Data { get; }

        public SellerViewModel(Seller s , IItemsProvider itemsProvider)
        {
            Data = s;
            _itemsProvider = itemsProvider;
        }

        public string Id
        {
            get { return Data.Id; }
        }

        public string Number { get; set; }

        public string Name { get { return Data.Name; } }

        public string FirstName { get { return Data.FirstName; } }

        public virtual float FamilientreffPercentage { get { return Data.FamilientreffPercentage; } }

        public virtual float Revenue
        {
            get
            {
                return SoldValue * ( 1.0f - FamilientreffPercentage ) / 1.0f;
            }
        }

        public virtual IEnumerable<ItemViewModel> Items
        {
            get
            {
                return _itemsProvider.Items
                    .Where( i => i.Seller == Data );
            }
        }
        
        public int SoldItems
        {
            get
            {
                return Items.Count(i => i.IsSold);
            }
        }

        public virtual int TotalItems
        {
            get
            {
                return Items.Count();
            }
           
        }

        public float SoldValue
        {
            get
            {
                return Items
                    .Where( i => i.IsSold )
                    .Sum( i => i.Price);
            }
        }

        public float TotalValue
        {
            get
            {
                return Items
                    .Sum( i => i.Price );
            }
        }
        
        public override string ToString()
        {
            return $"{Name}, {FirstName}";
        }

        internal void Update()
        {
            RaisePropertyChanged( string.Empty );
        }
    }



    
}
