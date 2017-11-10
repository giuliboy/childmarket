using KinderArtikelBoerse.Contracts;
using KinderArtikelBoerse.Models;
using KinderArtikelBoerse.Viewmodels;
using System.Collections.Generic;
using System.Linq;

namespace KinderArtikelBoerse.Utils
{
    public class DemoMarketDataProvider : IMarketService
    {
        private List<Item> _items;
        public IEnumerable<Item> Items
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
                        new Item() { ItemIdentifier="AB4", Description="puzzli mit extrem langer beschreibung was es genau für ein puzzli ist", Price=25.0f , SellerId= Sellers.Last().Id},
                        new Item() { ItemIdentifier="AB11", Description="pulli mit stern", Price=55.0f , SellerId= Sellers.Last().Id},

                        new Item() { ItemIdentifier="C1", Description="buch ", Price=2.5f, SellerId = Sellers.First().Id },
                        new Item() { ItemIdentifier="C2", Description="bobby car ", Price=2.5f, SellerId = Sellers.First().Id  },
                        new Item() { ItemIdentifier="C3", Description="ente", Price=2.5f, SellerId = Sellers.First().Id  },

                        new Item() { ItemIdentifier="CD1", Description="jacke blau", Price=11f , Size="108", SellerId=Sellers.Skip(1).First().Id },
                        new Item() { ItemIdentifier="CD2", Description="socken", Price=1f , Size="112", SellerId=Sellers.Skip(1).First().Id },

                    }
                        .ToList();
                }

                return _items;
            }
        }

        private List<Seller> _sellers;
        public IEnumerable<Seller> Sellers
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
                        .ToList();
                }
                return _sellers;
            }
        }

        public Seller Add( Seller seller )
        {
            seller.Id = _sellers.Select( s => s.Id ).Distinct().Max() + 1;
            _sellers.Add( seller );

            return seller;
        }

        public Seller Remove( Seller seller )
        {
            _sellers.Remove( seller );

            return seller;
        }

        public void Update( int sellerId, Seller seller )
        {
            var updatingSeller =_sellers.FirstOrDefault( s => s.Id == sellerId );
            if(updatingSeller == null )
            {
                return;
            }

            updatingSeller.SoldItems = seller.SoldItems;
            updatingSeller.TotalItems = seller.TotalItems;
            updatingSeller.SoldValue = seller.SoldValue;
            updatingSeller.Name = seller.Name;
            updatingSeller.FirstName = seller.FirstName;
            updatingSeller.FamilientreffPercentage = seller.FamilientreffPercentage;
        }
    }
}