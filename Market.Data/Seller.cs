
using Microsoft.AspNetCore.Identity;

namespace Market.Data
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
