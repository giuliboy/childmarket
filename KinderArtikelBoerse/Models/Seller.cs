using KinderArtikelBoerse.Contracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KinderArtikelBoerse.Models
{
    public class Seller : ISeller
    {
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        public int Id { get; set; }

        public string Name { get; set; }

        public string FirstName { get; set; }

        public float FamilientreffPercentage { get; set; }

    }
}
