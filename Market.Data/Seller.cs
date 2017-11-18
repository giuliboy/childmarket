
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace KinderArtikelBoerse.Models
{
    public class Seller : IdentityUser
    {
        public string Name { get; set; }

        public string FirstName { get; set; }

        public float FamilientreffPercentage { get; set; }

        public override string ToString()
        {
            return $"[{Id}]{Name},{FirstName}";
        }
    }
}
