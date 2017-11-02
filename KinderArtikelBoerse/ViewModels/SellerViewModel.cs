using KinderArtikelBoerse.Contracts;
using KinderArtikelBoerse.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace KinderArtikelBoerse.Viewmodels
{

    public class SellerViewModel : PropertyChangeNotifier
    {
        public SellerViewModel(ISeller s)
        {
            _data = s;
        }

        public string Number { get; set; }

        private ISeller _data;

        public string Name { get { return _data.Name; } }

        public string FirstName { get { return _data.FirstName; } }

        public float FamilientreffSharePercentage { get { return _data.FamilientreffPercentage; } }

        //private ObservableCollection<ItemViewModel> _items;
        //public ObservableCollection<ItemViewModel> Items
        //{
        //    get
        //    {
        //        if(_items == null )
        //        {
        //            _items = new ObservableCollection<ItemViewModel>( _data.Items.Select( i => new ItemViewModel( i ) ) );
        //        }

        //        return _items;
        //    }
        //}

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
            return $"{Name}, {FirstName}";
        }
    }



    
}
