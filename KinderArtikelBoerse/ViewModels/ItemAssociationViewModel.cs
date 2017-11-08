namespace KinderArtikelBoerse.Viewmodels
{
    public class ItemAssociationViewModel : PropertyChangeNotifier
    {
        public ItemViewModel Item { get; set; }

        public SellerViewModel Seller { get; set; }
    }



    
}
