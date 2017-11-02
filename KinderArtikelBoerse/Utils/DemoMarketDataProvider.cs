using KinderArtikelBoerse.Contracts;
using KinderArtikelBoerse.Models;
using System.Collections.Generic;
using System.Linq;

namespace KinderArtikelBoerse.Utils
{
    public class DemoMarketDataProvider : IMarketDataProvider
    {
        public IEnumerable<Item> Items => new List<Item>()
        {
            new Item() { ItemIdentifier="AB1", Description="roter Pulli", Price=5.0f , Size="36/38", Seller= Sellers.Last()},
            new Item() { ItemIdentifier="AB2", Description="lego ", Price=2.5f, Seller = Sellers.First() },
            new Item() { ItemIdentifier="AB3", Description="jacke rot", Price=15f , Size="112", Seller=Sellers.Skip(1).First()},
        };

        public IEnumerable<Seller> Sellers => new List<Seller>()
        {
            new Seller()
            {
                Name = "Zahner",
                FirstName = "Nina"
            },
            new Seller()
            {
                Name = "Zahner",
                FirstName = "Nicole"
            },
            new Seller()
            {
                Name = "Brunner",
                FirstName="Marianne"
            },
        };
    }
}