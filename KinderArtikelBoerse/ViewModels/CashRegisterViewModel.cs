using KinderArtikelBoerse.Contracts;
using KinderArtikelBoerse.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

/*
 * TODOS:
 * Schalter um verkaufte items zu filtern 
 * Kassa VM isolieren
 * "neuer Kunde" und Summe als Knopf kombiniert
 * 
 */

namespace KinderArtikelBoerse.Viewmodels
{

    public class CashRegisterViewModel : PropertyChangeNotifier
    {
        public CashRegisterViewModel( ISellerProvider sellerProvider, IItemsProvider itemsProvider )
        {
            _sellerProvider = sellerProvider;
            Items = itemsProvider.Items;
        }

        private string _searchItemText = string.Empty;
        public string SearchItemText
        {
            get
            {
                return _searchItemText;
            }
            set
            {
                var oldValue = _searchItemText;
                _searchItemText = value;

                if ( oldValue != value )
                {

                    RaisePropertyChanged();
                    ItemsCollectionView.Refresh();
                }
            }
        }

        private ItemViewModel _searchItem;
        public ItemViewModel SearchItem
        {
            get
            {
                return _searchItem;
            }
            set
            {
                var oldValue = _searchItem;
                _searchItem = value;

                if ( oldValue != value )
                {
                    RaisePropertyChanged();
                }

            }
        }

        public IEnumerable<ItemViewModel> Items { get; }

        public IEnumerable<SellerViewModel> Sellers => _sellerProvider.Sellers;

        private ICollectionView _itemsCollectionView;
        public ICollectionView ItemsCollectionView
        {
            get
            {
                if ( _itemsCollectionView == null )
                {
                    _itemsCollectionView = CollectionViewSource.GetDefaultView( Items );

                    _itemsCollectionView.Filter += ( obj ) => ItemFilterPredicate( (ItemViewModel)obj );

                }
                return _itemsCollectionView;
            }
        }

        private bool ItemFilterPredicate( ItemViewModel item )
        {
            var normalizedSearchText = SearchItemText.ToLowerInvariant();

            if ( item.IsSold )
            {
                return false;
            }

            var isItemMatching = item.ItemIdentifier.ToLowerInvariant().StartsWith( normalizedSearchText ) ||
                    item.Description.ToLowerInvariant().Contains( normalizedSearchText ) ||
                    item.Price.ToString().ToLowerInvariant().StartsWith( normalizedSearchText ) ||
                    item.Size.ToLowerInvariant().Contains( normalizedSearchText );


            var isSellerMatching = item.Seller.Name.ToLowerInvariant().StartsWith( normalizedSearchText ) ||
                item.Seller.FirstName.ToLowerInvariant().StartsWith( normalizedSearchText );

            return isItemMatching || isSellerMatching;
        }

        public AutoCompleteFilterPredicate<object> SearchItemFilter
        {
            get
            {
                return ( searchText, obj ) =>
                {
                    return ItemFilterPredicate( (ItemViewModel)obj );
                };
            }
        }

        private ICommand _toggleSellCommand;
        public ICommand ToggleSellCommand => _toggleSellCommand ?? ( _toggleSellCommand = new ActionCommand<ItemViewModel>( HandleSold ) );

        private void HandleSold( ItemViewModel item )
        {
            if ( item.IsSold )
            {
                SearchItemText = string.Empty;
                if ( !Batch.Contains( item ) )
                {
                    Batch.Add( item );
                }
            }
            else
            {
                Batch.Remove( item );
            }


            var seller = Sellers.Where( s => s.Id == item.Seller.Id || s is AllSellerViewModel )
                .Select(s => {
                    s.Update();
                    return s; })
                    .ToList();

            RaisePropertyChanged( nameof( BatchValue ) );
        }

        private ICommand _keyDownCommand;
        public ICommand KeyDownCommand => _keyDownCommand ?? ( _keyDownCommand = new ActionCommand<object>( ( args ) =>
        {
            if( string.IsNullOrEmpty( SearchItemText ) )
            {
                return;
            }

            if ( !Keyboard.IsKeyDown( Key.Enter ) )
            {
                return;
            }

            var filteredCollection = ItemsCollectionView
            .Cast<ItemViewModel>()
            .ToList()
            ;

            if ( filteredCollection.Any() )
            {
                var ass = filteredCollection.First();

                ass.IsSold = true;
                HandleSold( ass );
            }

        } ) );

        private ICommand _newBatchCommand;
        public ICommand NewBatchCommand => _newBatchCommand ?? ( _newBatchCommand = new ActionCommand( () =>
        {
            Batch.Clear();
            RaisePropertyChanged( nameof( BatchValue ) );
        } ) );

        public float BatchValue
        {
            get
            {
                return Batch.Sum( b => b.Price );
            }
        }

        private ObservableCollection<ItemViewModel> _batch = new ObservableCollection<ItemViewModel>();
       
        private ISellerProvider _sellerProvider;

        public ObservableCollection<ItemViewModel> Batch
        {
            get
            {
                return _batch;
            }
        }

    }

    
}
