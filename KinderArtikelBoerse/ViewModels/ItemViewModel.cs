using KinderArtikelBoerse.Contracts;
using KinderArtikelBoerse.Models;

namespace KinderArtikelBoerse.Viewmodels
{
    public class ItemViewModel : PropertyChangeNotifier, ISellable
    {
        private ISellable _data;

        public ItemViewModel(ISellable i)
        {
            _data = i;
            _sellerViewModel = new SellerViewModel( i.Seller );
        }

        public string ItemIdentifier
        {
            get
            {
                return _data.ItemIdentifier;
            }
            set
            {
                _data.ItemIdentifier = value;
                RaisePropertyChanged();
            }
        }

        public string Description {
            get { return _data.Description; }
            set
            {
                _data.Description = value;
                RaisePropertyChanged();
            }
        }

        public string Size {
            get { return _data.Size; }
            set
            {
                _data.Size = value;
                RaisePropertyChanged();
            }
        }

        public float Price {
            get { return _data.Price; }
            set
            {
                _data.Price = value;
                RaisePropertyChanged();
            }
        }

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

        public int Id => _data.Id;

        public ISeller Seller { get => _data.Seller; set => _data.Seller = value; }

        public override string ToString()
        {
            return $"Item:{ItemIdentifier}";
        }
    }



    
}
