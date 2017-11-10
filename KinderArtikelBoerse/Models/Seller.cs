using KinderArtikelBoerse.Contracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KinderArtikelBoerse.Models
{
    public class Seller 
    {
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        public int Id { get; set; }

        public string Name { get; set; }

        public string FirstName { get; set; }

        public float FamilientreffPercentage { get; set; }

        public float SoldValue { get; set; }

        public int SoldItems { get; set; }

        public int TotalItems { get; set; }

    }
}
