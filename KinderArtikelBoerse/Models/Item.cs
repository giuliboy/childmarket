using KinderArtikelBoerse.Contracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KinderArtikelBoerse.Models
{

    public class Ownership
    {
        public Seller Seller { get; set; }

        public IEnumerable<Item> Items { get; set; }
    }

    public class Item : ISellable
    {
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        public int Id { get; set; }

        public string ItemIdentifier { get; set; }

        public string Description { get; set; } = string.Empty;

        public string Size { get; set; } = string.Empty;

        public float Price { get; set; }

        public bool IsSold { get; set; }

        public int SellerId { get; set; }
    }
}
