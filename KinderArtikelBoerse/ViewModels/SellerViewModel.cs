using KinderArtikelBoerse.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace KinderArtikelBoerse.Viewmodels
{

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

        public virtual float FamilientreffPercentage { get { return Data.FamilientreffPercentage; } }

        public virtual float Revenue
        {
            get
            {
                return SoldValue * ( 1.0f - FamilientreffPercentage ) / 1.0f;
            }
        }
        
        public int SoldItems
        {
            get
            {
                return Data.Items.Count(i => i.IsSold);
            }
        }

        public virtual int TotalItems
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

        public float TotalValue
        {
            get
            {
                return Data.Items
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
