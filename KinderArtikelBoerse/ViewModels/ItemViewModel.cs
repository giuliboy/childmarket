using KinderArtikelBoerse.Contracts;
using KinderArtikelBoerse.Models;

namespace KinderArtikelBoerse.Viewmodels
{
    public class ItemViewModel : PropertyChangeNotifier, IItemViewModel
    {
        private Item _data;

        public ItemViewModel(Item i)
        {
            _data = i;
            _sellerViewModel = new SellerViewModel( i.Seller );
        }

        public string ItemIdentifier { get { return _data.ItemIdentifier; } }

        public string Description {
            get { return _data.Description; }
            set
            {
                _data.Description = value;
                RaisePropertyChanged();
            }
        }

        public string Size { get { return _data.Size; } }

        public float Price { get { return _data.Price; } }

        public bool IsSold {
            get { return _data.IsSold; }
            set
            {
                _data.IsSold = value;
                RaisePropertyChanged();
            }
        }

        private SellerViewModel _sellerViewModel;
        public SellerViewModel SellerViewModel
        {
            get
            {
                return _sellerViewModel;
            }
        }
        public override string ToString()
        {
            return $"Item:{ItemIdentifier}";
        }
    }



    
}
