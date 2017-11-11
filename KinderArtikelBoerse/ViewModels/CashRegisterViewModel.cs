﻿using KinderArtikelBoerse.Contracts;
using KinderArtikelBoerse.Models;
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
        public CashRegisterViewModel(IMarketService dataService, IEnumerable<SellerViewModel> sellers )
        {
            _dataService = dataService;
            _sellers = sellers;
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

        private ObservableCollection<ItemViewModel> _items;
        private IMarketService _dataService;

        public IEnumerable<ItemViewModel> Items
        {
            get
            {
                if ( _items == null )
                {
                    _items = new ObservableCollection<ItemViewModel>( _dataService.Items.Select(i => new ItemViewModel( i ) ) );
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


            var seller = _sellers.FirstOrDefault( s => s.Id == item.Seller.Id );
            seller.Update();

           // var sellerToUpdate =_dataService.Sellers.First( s => s.Id == item.SellerId );
            //TODO
            //var updatedStats = _statisticsService.GetStatistics( association.Seller.Id, Items.Select( i => i.Item ) );
            //sellerToUpdate.SoldItems = updatedStats.SoldItems;
            //sellerToUpdate.TotalItems = updatedStats.TotalItems;
            //sellerToUpdate.SoldValue = updatedStats.SoldValue;

            //association.Seller.Update( item.SellerId );
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
        private IEnumerable<SellerViewModel> _sellers;

        public ObservableCollection<ItemViewModel> Batch
        {
            get
            {
                return _batch;
            }
        }

    }

    
}
