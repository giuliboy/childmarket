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
        public CashRegisterViewModel(IMarketService dataService, IStatisticsService statisticsService )
        {
            _dataService = dataService;
            _statisticsService = statisticsService;
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

        private ItemAssociationViewModel _searchItem;
        public ItemAssociationViewModel SearchItem
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

        private ObservableCollection<ItemAssociationViewModel> _items;
        private IMarketService _dataService;
        private IStatisticsService _statisticsService;

        public IEnumerable<ItemAssociationViewModel> Items
        {
            get
            {
                if ( _items == null )
                {
                    var associations = from i in _dataService.Items
                                       join s in _dataService.Sellers on i.SellerId equals s.Id
                                       select new ItemAssociationViewModel() { Item = i, Seller = s };

                    _items = new ObservableCollection<ItemAssociationViewModel>( associations );
                }

                return _items;
            }
        }

        private ICollectionView _itemsCollectionView;
        public ICollectionView ItemsCollectionView
        {
            get
            {
                if ( _itemsCollectionView == null )
                {
                    _itemsCollectionView = CollectionViewSource.GetDefaultView( Items );

                    _itemsCollectionView.Filter += ( obj ) => ItemFilterPredicate( (ItemAssociationViewModel)obj );

                }
                return _itemsCollectionView;
            }
        }

        private bool ItemFilterPredicate( ItemAssociationViewModel association )
        {
            var normalizedSearchText = SearchItemText.ToLowerInvariant();

            var item = association.Item;

            if ( item.IsSold )
            {
                return false;
            }

            var isItemMatching = item.ItemIdentifier.ToLowerInvariant().StartsWith( normalizedSearchText ) ||
                    item.Description.ToLowerInvariant().Contains( normalizedSearchText ) ||
                    item.Price.ToString().ToLowerInvariant().StartsWith( normalizedSearchText ) ||
                    item.Size.ToLowerInvariant().Contains( normalizedSearchText );


            var isSellerMatching = association.Seller.Name.ToLowerInvariant().StartsWith( normalizedSearchText ) ||
                association.Seller.FirstName.ToLowerInvariant().StartsWith( normalizedSearchText );

            return isItemMatching || isSellerMatching;
        }

        public AutoCompleteFilterPredicate<object> SearchItemFilter
        {
            get
            {
                return ( searchText, obj ) =>
                {
                    return ItemFilterPredicate( (ItemAssociationViewModel)obj );
                };
            }
        }

        private ICommand _sellCommand;
        public ICommand SellCommand => _sellCommand ?? ( _sellCommand = new ActionCommand<ItemAssociationViewModel>( HandleSold ) );

        private void HandleSold( ItemAssociationViewModel association )
        {
            var item = association.Item;

            if ( item.IsSold )
            {
                SearchItemText = string.Empty;
            }

            if ( !Batch.Contains( association ) )
            {
                Batch.Add( association );
            }

            association.Seller.Update( _statisticsService.GetStatistics( association.Seller.Id, Items.Select( i => i.Item ) ) );

            RaisePropertyChanged( nameof( BatchValue ) );

            
        }

        private ICommand _keyDownCommand;
        public ICommand KeyDownCommand => _keyDownCommand ?? ( _keyDownCommand = new ActionCommand<object>( ( args ) =>
        {
            if ( Keyboard.IsKeyDown( Key.Enter ) )
            {
                var filteredCollection = ItemsCollectionView
               .Cast<ItemAssociationViewModel>()
               .ToList()
               ;

                if ( filteredCollection.Any() )
                {
                    var ass = filteredCollection.First();

                    ass.Item.IsSold = true;
                    HandleSold( ass );
                }
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
                return Batch.Sum( b => b.Item.Price );
            }
        }

        private ObservableCollection<ItemAssociationViewModel> _batch = new ObservableCollection<ItemAssociationViewModel>();
        public ObservableCollection<ItemAssociationViewModel> Batch
        {
            get
            {
                return _batch;
            }
        }

    }

    
}
