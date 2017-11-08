using KinderArtikelBoerse.Models;


namespace KinderArtikelBoerse.Viewmodels
{
    public class SellerStatisticsViewModel : PropertyChangeNotifier
    {
        public SellerViewModel Seller { get; }

        private SellStatistic _data = new SellStatistic();

        public SellerStatisticsViewModel(SellerViewModel seller)
        {
            Seller = seller;
        }

        public int SoldItems
        {
            get
            {
                return _data.SoldItems;
            }
        }

        public int TotalItems
        {
            get
            {
                return _data.TotalItems;
            }
        }

        public float SoldValue
        {
            get
            {
                return _data.SoldValue;
            }
        }

        public void Update(SellStatistic ss)
        {
            _data = ss;
            RaisePropertyChanged( string.Empty );
        }

        public override string ToString()
        {
            return $"{Seller.Name}, {Seller.FirstName}: {SoldItems}/{TotalItems}";
        }
    }

    
}
