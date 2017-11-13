using KinderArtikelBoerse.Contracts;
using KinderArtikelBoerse.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace KinderArtikelBoerse.Viewmodels
{
    public class WildCardSeller : SellerViewModel
    {
        public WildCardSeller(  ) 
            : base( new Seller() {
                Id = -1,
            } )
        {
        }

        public override string ToString()
        {
            return "Alle";
        }
    }

    public class SellerViewModel : PropertyChangeNotifier
    {

        public Seller Data { get; }

        public SellerViewModel(Seller s)
        {
            Data = s;
        }

        public int Id
        {
            get { return Data.Id; }
        }

        public string Number { get; set; }

        public string Name { get { return Data.Name; } }

        public string FirstName { get { return Data.FirstName; } }

        public float FamilientreffSharePercentage { get { return Data.FamilientreffPercentage; } }
        
        public int SoldItems
        {
            get
            {
                return Data.Items.Count(i => i.IsSold);
            }
        }

        public int TotalItems
        {
            get
            {
                return Data.Items.Count();
            }
           
        }

        public float SoldValue
        {
            get
            {
                return Data.Items
                    .Where( i => i.IsSold )
                    .Sum( i => i.Price);
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
