using KinderArtikelBoerse.Contracts;
using KinderArtikelBoerse.Models;
using KinderArtikelBoerse.Utils;
using System.Linq;
using System.Windows.Input;

namespace KinderArtikelBoerse.Viewmodels
{

    public class ItemViewModel : PropertyChangeNotifier
    {
        private Item _data;

        public ItemViewModel(Item i)
        {
            _data = i;
        }

        private ICommand _soldToggleCommand;

        public ICommand SoldToggleCommand => _soldToggleCommand ?? ( _soldToggleCommand = new ActionCommand<KeyboardEventArgs>( ( args ) =>
        {
        } ) );

        public int SellerId
        {
            get
            {
                return _data.SellerId;
            }
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

        public int Id => _data.Id;
        
        public override string ToString()
        {
            return $"Item:{ItemIdentifier}";
        }
    }



    
}
