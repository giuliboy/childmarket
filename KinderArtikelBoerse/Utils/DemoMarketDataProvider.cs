﻿using KinderArtikelBoerse.Contracts;
using KinderArtikelBoerse.Models;
using KinderArtikelBoerse.Viewmodels;
using System.Collections.Generic;
using System.Linq;

namespace KinderArtikelBoerse.Utils
{
    public class DemoMarketDataProvider : IMarketService
    {
        public DemoMarketDataProvider()
        {
            Sellers.First().Items = Items.Take( 5 ).ToList();
            Sellers.Skip(1).First().Items = Items.Skip(5).Take( 3 ).ToList(); 
            Sellers.Skip( 2 ).First().Items = Items.Skip(8 ).Take( 2 ).ToList();
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
                            FirstName = "Nina",
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
                            FirstName="Marianne",
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
    }
}