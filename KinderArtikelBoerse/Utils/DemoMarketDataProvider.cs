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
            new Item() { ItemIdentifier="AB2", Description="Pulover", Price=5.0f , Size="38", Seller= Sellers.Last()},
            new Item() { ItemIdentifier="AB3", Description="jojo", Price=5.5f , Seller= Sellers.Last()},
            new Item() { ItemIdentifier="AB4", Description="puzzli", Price=25.0f , Seller= Sellers.Last()},

            new Item() { ItemIdentifier="C1", Description="buch ", Price=2.5f, Seller = Sellers.First() },
            new Item() { ItemIdentifier="C2", Description="bobby car ", Price=2.5f, Seller = Sellers.First() },
            new Item() { ItemIdentifier="C3", Description="ente", Price=2.5f, Seller = Sellers.First() },

            new Item() { ItemIdentifier="CD1", Description="jacke blau", Price=11f , Size="108", Seller=Sellers.Skip(1).First()},
            new Item() { ItemIdentifier="CD2", Description="socken", Price=1f , Size="112", Seller=Sellers.Skip(1).First()},

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