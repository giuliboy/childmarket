using KinderArtikelBoerse.Contracts;
using KinderArtikelBoerse.Models;
using KinderArtikelBoerse.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace KinderArtikelBoerse.Viewmodels
{
    public class DataViewModel : PropertyChangeNotifier
    {
        public DataViewModel(ISellerProvider sellerProvider, IItemsProvider itemsProvider, IMarketService dataService, IItemReader itemReader)
        {
            _dataService = dataService;
            _itemReader = itemReader;
            Sellers = new ObservableCollection<SellerViewModel>( sellerProvider.Sellers );
            Items = itemsProvider.Items;
        }

        public IEnumerable<SellerViewModel> Sellers { get; }
        public IList<ItemViewModel> Items { get; }

        private SellerViewModel _selectedSeller;
        public SellerViewModel SelectedSeller
        {
            get
            {
                if(_selectedSeller == null )
                {
                    _selectedSeller = Sellers.First();
                }
                return _selectedSeller;
            }
            set
            {
                _selectedSeller = value;
                RaisePropertyChanged();
                RaisePropertyChanged( nameof( CanAddItem ) );

                ItemsCollectionView.Refresh();
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

                    _itemsCollectionView.Filter += ( obj ) => Filter( (ItemViewModel)obj );

                }
                return _itemsCollectionView;
            }
        }

        private string _filterText = string.Empty;
        
        public string FilterText
        {
            get
            {
                return _filterText;
            }
            set
            {
                _filterText = value;
                RaisePropertyChanged();

                ItemsCollectionView.Refresh();
            }
        }

        bool Filter(ItemViewModel item )
        {
            
            var normalizedSearchText = FilterText.ToLowerInvariant();
            
            var isItemMatching = item.ItemIdentifier.ToLowerInvariant().StartsWith( normalizedSearchText ) ||
                    item.Description.ToLowerInvariant().Contains( normalizedSearchText ) ||
                    item.Price.ToString().ToLowerInvariant().StartsWith( normalizedSearchText ) ||
                    item.Size.ToLowerInvariant().Contains( normalizedSearchText );

            var isSellerMatching = true;
            if ( SelectedSeller != null && !(SelectedSeller is AllSellerViewModel) )
            {
                isSellerMatching = item.Seller.Id == SelectedSeller.Id;
            }

            return isSellerMatching && isItemMatching ;
        }

        public bool CanAddItem
        {
            get { return !(SelectedSeller is AllSellerViewModel); }
        }

        private ICommand _addItemCommand;
        public ICommand AddItemCommand => _addItemCommand ?? ( _addItemCommand = new ActionCommand( () => {
            var item = new Item()
            {
                Seller = SelectedSeller.Data,
                ItemIdentifier = "TODO"
            };
            AddItem( item );
        } ) );
        
        private void AddItem(Item data)
        {
            var item = _dataService.Add( data );
            Items.Add( new ItemViewModel( item ) );
            UpdateSellers();
        }

        private ICommand _removeItemCommand;
        public ICommand RemoveItemCommand => _removeItemCommand ?? ( _removeItemCommand = new ActionCommand<ItemViewModel>( RemoveItem ) );

        private void RemoveItem(ItemViewModel item)
        {
            Items.Remove( item );
            _dataService.Remove( item.Data );

            UpdateSellers();
        }

        private ICommand _exportCommand;
        public ICommand ExportCommand => _exportCommand ?? ( _exportCommand = new ActionCommand( Export ) );

        private void Export()
        {
            //todo excel oder pdf generieren
        }

        private ICommand _updateCommand;
        public ICommand UpdateCommand => _updateCommand ?? ( _updateCommand = new ActionCommand( UpdateSellers ) );

        private void UpdateSellers()
        {
            Sellers.Select( s =>
            {
                s.Update();
                return s;
            } ).ToList();

            //TODO nur wenn validation ok ist, abspeichern
            _dataService.Save();
        }

        private ICommand _dropCommand;
        private IMarketService _dataService;
        private IItemReader _itemReader;

        public ICommand DropCommand
        {
            get
            {
                return _dropCommand ?? ( _dropCommand = new ActionCommand<DragEventArgs>( args =>
                {
                    if ( args.Data.GetDataPresent( DataFormats.FileDrop ) )
                    {
                        string[] files = (string[])args.Data.GetData( DataFormats.FileDrop );

                        var droppedFile = files
                        .FirstOrDefault();

                        try
                        {
                            //auslesen aus file
                            //kein excel installiert => exception
                            //var items = _itemReader.ReadItems( droppedFile );
                            //in db einfügen

                            var item = new Item() { ItemIdentifier = "Z1", Description = "neues item", Price = 5.0f, Size = "36/38", Seller = SelectedSeller.Data };

                            AddItem( item );
                            //_dataService.Add( new Item() );
                        }
                        catch ( Exception ex )
                        {
                            
                        }
                        
                    }
                } ) );
            }
        }
    }


}
