using KinderArtikelBoerse.Contracts;
using KinderArtikelBoerse.Models;
using System.Collections.Generic;

namespace KinderArtikelBoerse.Utils
{
    public class DemoSellerProvider : ISellerProvider
    {
        public IEnumerable<Seller> Sellers => new List<Seller>()
        {
            new Seller()
            {
                Name = "DemoName1",
                Items= new List<Item>
                {
                    new Item() { ItemIdentifier="AB1", Description="Demo Item AB1" },
                    new Item() { ItemIdentifier="AB2", Description="Demo Item AB2" },
                    new Item() { ItemIdentifier="AB3", Description="Demo Item AB3" },
                }
            },
            new Seller()
            {
                Name = "DemoName2",
                Items= new List<Item>
                {
                    new Item() { ItemIdentifier="CX1", Description="Demo Item CX1" },
                    new Item() { ItemIdentifier="CX2", Description="Demo Item CX2" },
                    new Item() { ItemIdentifier="CX3", Description="Demo Item CX3" },
                    new Item() { ItemIdentifier="CX4", Description="Demo Item CX4" },
                    new Item() { ItemIdentifier="CX5", Description="Demo Item CX5" },
                    new Item() { ItemIdentifier="CX6", Description="Demo Item CX6" },
                    new Item() { ItemIdentifier="CX7", Description="Demo Item CX7" },
                }
            },
            new Seller()
            {
                Name = "DemoName3",
                Items= new List<Item>
                {
                    new Item() { ItemIdentifier="D1", Description="Demo Item D1" },
                }
            },
        };
    }
}