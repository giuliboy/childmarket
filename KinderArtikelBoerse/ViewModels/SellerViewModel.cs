using KinderArtikelBoerse.Contracts;
using KinderArtikelBoerse.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KinderArtikelBoerse.Viewmodels
{
    public class SellerEqualityComparer : IEqualityComparer<SellerViewModel>
    {
        public bool Equals( SellerViewModel x, SellerViewModel y )
        {
            return x.Id == y.Id;
        }

        public int GetHashCode( SellerViewModel obj )
        {
            return obj.Id.GetHashCode();
        }
    }

    public class SellerViewModel : PropertyChangeNotifier
    {

        private IMarketService _dataService;
        private Seller _data;

        public SellerViewModel(int id, IMarketService s)
        {
            _dataService = s;
            Update( id );
        }

        public void Update(int id)
        {
            _data = _dataService.Sellers.First( se => se.Id == id );
            RaisePropertyChanged( string.Empty );
        }

        public int Id
        {
            get { return _data.Id; }
        }

        public string Number { get; set; }

        public string Name { get { return _data.Name; } }

        public string FirstName { get { return _data.FirstName; } }

        public float FamilientreffSharePercentage { get { return _data.FamilientreffPercentage; } }
        
        public int SoldItems
        {
            get
            {
                return _data.SoldItems;
            }
            set
            {
                _data.SoldItems = value;
                RaisePropertyChanged();
            }
        }

        public int TotalItems
        {
            get
            {
                return _data.TotalItems;
            }
            set
            {
                _data.TotalItems = value;
                RaisePropertyChanged();
            }
        }

        public float SoldValue
        {
            get
            {
                return _data.SoldValue;
            }
            set
            {
                _data.SoldValue = value;
                RaisePropertyChanged();
            }
        }


        public override string ToString()
        {
            return $"{Name}, {FirstName}";
        }
    }



    
}
