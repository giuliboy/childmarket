using System.Collections.Generic;
using System.Linq;
using System;
using Market.Service.Contracts;
using Market.Data;

namespace KinderArtikelBoerse.Utils
{
    public class DemoMarketDataProvider : IMarketService
    {
        public DemoMarketDataProvider()
        {
            
        }

        private List<Item> _items;
        public IEnumerable<Item> Items
        {
            get
            {
                if(_items == null )
                {
                    _items = new List<Item>()
                    {
                        new Item() { ItemIdentifier="AB1", Description="roter Pulli", Price=5.0f , Size="36/38", Seller = Sellers.First(),},
                        new Item() { ItemIdentifier="AB2", Description="Pulover", Price=5.0f , Size="38", Seller = Sellers.First(),},
                        new Item() { ItemIdentifier="AB3", Description="jojo", Price=5.5f , Seller = Sellers.First(),},
                        new Item() { ItemIdentifier="AB4", Description="puzzli mit extrem langer beschreibung was es genau für ein puzzli ist", Price=25.0f , Seller = Sellers.First(),},
                        new Item() { ItemIdentifier="AB11", Description="pulli mit stern", Price=55.0f, Seller = Sellers.First(),},

                        new Item() { ItemIdentifier="C1", Description="buch ", Price=2.5f,  Seller = Sellers.Skip(1).First(), },
                        new Item() { ItemIdentifier="C2", Description="bobby car ", Price=2.5f,  Seller = Sellers.Skip(1).First(),  },
                        new Item() { ItemIdentifier="C3", Description="ente", Price=2.5f,  Seller = Sellers.Skip(1).First(),  },

                        new Item() { ItemIdentifier="CD1", Description="jacke blau", Price=11f , Size="108",  Seller = Sellers.Skip(2).First(), },
                        new Item() { ItemIdentifier="CD2", Description="socken", Price=1f , Size="112",  Seller = Sellers.Skip(2).First(), },
                    }
                        .ToList();

                    _items.AddRange( Enumerable.Range( 0, 1000 ).Select( i => new Item() { ItemIdentifier = "GEN" + i, Description = i.ToString(), Price = i / 5, Seller = _sellers[i % 3] } ) );
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
                            Id="1",
                            Name = "Zahner",
                            FirstName = "Nina",
                            FamilientreffPercentage = 0.1f,
                        },
                        new Seller()
                        {
                            Id="2",
                            Name = "Zahner",
                            FirstName = "Nicole",
                            FamilientreffPercentage = 0.2f,
                        },
                        new Seller()
                        {
                            Id="3",
                            Name = "Brunner",
                            FirstName="Marianne",
                            FamilientreffPercentage = 0.0f,
                        },
                    }
                    .ToList();

                }
                return _sellers;
            }
        }

        public bool Save()
        {
            return false;
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

        public Item Add( Item data )
        {
            _items.Add( data );
            return data;
        }

        public Item Remove( Item data )
        {
            _items.Remove( data );
            return data;
        }

        public void Dispose()
        {
            //do nothing
        }
    }
}