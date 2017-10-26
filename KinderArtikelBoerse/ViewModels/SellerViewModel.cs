using KinderArtikelBoerse.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace KinderArtikelBoerse.Viewmodels
{

    public class SellerViewModel : PropertyChangeNotifier
    {
        public SellerViewModel(Seller s)
        {
            _data = s;
        }

        public string Number { get; set; }

        private Seller _data;

        public string Name { get { return _data.Name; } }

        public string Vorname { get { return _data.Surname; } }

        public float FamilientreffSharePercentage { get { return _data.FamilientreffPercentage; } }

        private ObservableCollection<ItemViewModel> _items;
        public ObservableCollection<ItemViewModel> Items
        {
            get
            {
                if(_items == null )
                {
                    _items = new ObservableCollection<ItemViewModel>( _data.Items.Select( i => new ItemViewModel( i ) ) );
                }

                return _items;
            }
        }

        private ItemViewModel _selectedItemViewModel;
        public ItemViewModel SelectedItemViewModel
        {
            get { return _selectedItemViewModel; }
            set
            {
                _selectedItemViewModel = value;
                RaisePropertyChanged();
            }
        }



        public override string ToString()
        {
            return $"{Name}, {Vorname}";
        }
    }



    
}
