using KinderArtikelBoerse.Contracts;
using KinderArtikelBoerse.Models;
using KinderArtikelBoerse.Viewmodels;
using System.Collections.Generic;
using System.Linq;

namespace KinderArtikelBoerse.Utils
{
    public class DemoMarketDataProvider : IMarketService
    {
        private List<ItemViewModel> _items;
        public IEnumerable<ItemViewModel> Items
        {
            get
            {
                if(_items == null )
                {
                    _items = new List<Item>()
                    {
                        new Item() { ItemIdentifier="AB1", Description="roter Pulli", Price=5.0f , Size="36/38", SellerId= Sellers.Last().Id},
                        new Item() { ItemIdentifier="AB2", Description="Pulover", Price=5.0f , Size="38", SellerId= Sellers.Last().Id},
                        new Item() { ItemIdentifier="AB3", Description="jojo", Price=5.5f , SellerId= Sellers.Last().Id},
                        new Item() { ItemIdentifier="AB4", Description="puzzli", Price=25.0f , SellerId= Sellers.Last().Id},
                        new Item() { ItemIdentifier="AB11", Description="pulli mit stern", Price=55.0f , SellerId= Sellers.Last().Id},

                        new Item() { ItemIdentifier="C1", Description="buch ", Price=2.5f, SellerId = Sellers.First().Id },
                        new Item() { ItemIdentifier="C2", Description="bobby car ", Price=2.5f, SellerId = Sellers.First().Id  },
                        new Item() { ItemIdentifier="C3", Description="ente", Price=2.5f, SellerId = Sellers.First().Id  },

                        new Item() { ItemIdentifier="CD1", Description="jacke blau", Price=11f , Size="108", SellerId=Sellers.Skip(1).First().Id },
                        new Item() { ItemIdentifier="CD2", Description="socken", Price=1f , Size="112", SellerId=Sellers.Skip(1).First().Id },

                    }
                        .Select( i => new ItemViewModel( i ) )
                        .ToList();
                }

                return _items;
            }
        }

        private List<SellerViewModel> _sellers;
        public IEnumerable<SellerViewModel> Sellers
        {
            get
            {
                if ( _sellers == null )
                {
                    _sellers = new List<Seller>()
                    {
                        new Seller()
                        {
                            Id=1,
                            Name = "Zahner",
                            FirstName = "Nina"
                        },
                        new Seller()
                        {
                            Id=2,
                            Name = "Zahner",
                            FirstName = "Nicole"
                        },
                        new Seller()
                        {
                            Id=3,
                            Name = "Brunner",
                            FirstName="Marianne"
                        },
                    }
                        .Select( s => new SellerViewModel( s ) )
                        .ToList();
                }
                return _sellers;
            }
        }
       
        
    }
}