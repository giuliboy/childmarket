using KinderArtikelBoerse.Contracts;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinderArtikelBoerse.Models
{

    public class Item : ISellable
    {
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        public int Id { get; set; }

        public string ItemIdentifier { get; set; }

        public string Description { get; set; } = string.Empty;

        public string Size { get; set; } = string.Empty;

        public float Price { get; set; }

        public bool IsSold { get; set; }

        public ISeller Seller { get; set; }
    }
}
