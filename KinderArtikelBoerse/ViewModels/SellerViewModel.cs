using KinderArtikelBoerse.Contracts;
using KinderArtikelBoerse.Models;

namespace KinderArtikelBoerse.Viewmodels
{
    public class SellerViewModel : PropertyChangeNotifier
    {

        private Seller _data;

        public SellerViewModel(Seller s)
        {
            _data = s;
        }

        public int Id
        {
            get { return _data.Id; }
        }

        public string Number { get; set; }

        public string Name { get { return _data.Name; } }

        public string FirstName { get { return _data.FirstName; } }

        public float FamilientreffSharePercentage { get { return _data.FamilientreffPercentage; } }

        public override string ToString()
        {
            return $"{Name}, {FirstName}";
        }
    }



    
}
