
using System.ComponentModel.DataAnnotations.Schema;

namespace KinderArtikelBoerse.Models
{
    public class Item
    {
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        public int Id { get; set; }

        public string ItemIdentifier { get; set; }

        public string Description { get; set; } = string.Empty;

        public string Size { get; set; } = string.Empty;

        public float Price { get; set; }

        public bool IsSold { get; set; }

        public Seller Seller { get; set; }

        public override string ToString()
        {
            return $"[{Id}({Seller.Name},{Seller.FirstName}]:{ItemIdentifier}/{Price}";
        }

    }
}
