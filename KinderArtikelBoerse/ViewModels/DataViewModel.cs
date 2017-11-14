using KinderArtikelBoerse.Contracts;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace KinderArtikelBoerse.Viewmodels
{
    public class DataViewModel : PropertyChangeNotifier
    {
        public DataViewModel(ISellerProvider sellerProvider, IItemsProvider itemsProvider)
        {
            _sellerProvider = sellerProvider;
            Sellers = new ObservableCollection<SellerViewModel>( sellerProvider.Sellers );

            Items = new ObservableCollection<ItemViewModel>( itemsProvider.Items );
        }

        public IEnumerable<SellerViewModel> Sellers { get; }
        public IEnumerable<ItemViewModel> Items { get; }

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
        private ISellerProvider _sellerProvider;

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
            if ( SelectedSeller != null && !(SelectedSeller is WildCardSeller) )
            {
                isSellerMatching = item.Seller.Id == SelectedSeller.Id;
            }

            return isSellerMatching && isItemMatching ;
        }

    }

    
}
