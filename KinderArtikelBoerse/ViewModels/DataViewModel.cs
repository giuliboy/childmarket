using KinderArtikelBoerse.Contracts;
using KinderArtikelBoerse.Models;
using KinderArtikelBoerse.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace KinderArtikelBoerse.Viewmodels
{
    public class DataViewModel : PropertyChangeNotifier
    {
        public DataViewModel(ISellerProvider sellerProvider, IItemsProvider itemsProvider)
        {
            Sellers = new ObservableCollection<SellerViewModel>( sellerProvider.Sellers );
            Items = new ObservableCollection<ItemViewModel>( itemsProvider.Items );
        }

        public IEnumerable<SellerViewModel> Sellers { get; }
        public ObservableCollection<ItemViewModel> Items { get; }

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
        public ICommand AddItemCommand => _addItemCommand ?? ( _addItemCommand = new ActionCommand( AddItem ) );
        
        private void AddItem()
        {
            var item = new Item()
            {
                Seller = SelectedSeller.Data,
                ItemIdentifier = "TODO"
            };

            SelectedSeller.Data.Items.Add( item );
            Items.Add( new ItemViewModel(item) );

            Sellers.First( s => s is AllSellerViewModel ).Data.Items.Add( item );

            Sellers.Select( s =>
            {
                s.Update();
                return s;
            } ).ToList();
        }

        private ICommand _removeItemCommand;
        public ICommand RemoveItemCommand => _removeItemCommand ?? ( _removeItemCommand = new ActionCommand<ItemViewModel>( RemoveItem ) );

        private void RemoveItem(ItemViewModel item)
        {
            item.Seller.Items.Remove( item.Data );

            Sellers.First( s => s is AllSellerViewModel ).Data.Items.Remove( item.Data );

            Items.Remove( item );

            Sellers.Select( s =>
             {
                 s.Update();
                 return s;
             } ).ToList();

        }

    }


}
