using KinderArtikelBoerse.Contracts;
using KinderArtikelBoerse.Models;
using KinderArtikelBoerse.Utils;
using System.Linq;
using System.Windows.Input;

namespace KinderArtikelBoerse.Viewmodels
{
    public class ItemViewModel : PropertyChangeNotifier
    {
        public Item Data { get; }

        public ItemViewModel(Item i)
        {
            Data = i;
        }

        private ICommand _soldToggleCommand;

        public ICommand SoldToggleCommand => _soldToggleCommand ?? ( _soldToggleCommand = new ActionCommand<KeyboardEventArgs>( ( args ) =>
        {
        } ) );

        public Seller Seller
        {
            get { return Data.Seller; }
        }


        public string SellerId
        {
            get
            {
                return Seller.Id;
            }
        }

        public string ItemIdentifier
        {
            get
            {
                return Data.ItemIdentifier;
            }
            set
            {
                Data.ItemIdentifier = value;
                RaisePropertyChanged();
            }
        }

        public string Description {
            get { return Data.Description; }
            set
            {
                Data.Description = value;
                RaisePropertyChanged();
            }
        }

        public string Size {
            get { return Data.Size; }
            set
            {
                Data.Size = value;
                RaisePropertyChanged();
            }
        }

        public float Price {
            get { return Data.Price; }
            set
            {
                Data.Price = value;
                RaisePropertyChanged();
            }
        }

        public bool IsSold {
            get { return Data.IsSold; }
            set
            {
                Data.IsSold = value;
                RaisePropertyChanged();
            }
        }

        public int Id => Data.Id;
        
        public override string ToString()
        {
            return $"Item:{ItemIdentifier}";
        }
    }



    
}
