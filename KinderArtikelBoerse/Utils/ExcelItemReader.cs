using KinderArtikelBoerse.Contracts;
using System;
using KinderArtikelBoerse.Models;
using System.Data;
using System.Linq;
using System.Collections.Generic;

namespace KinderArtikelBoerse.Utils
{

    public class ExcelItemReader : IItemReader
    {
        private IMarketService _dataService;

        public ExcelItemReader(IMarketService dataService)
        {
            _dataService = dataService;
        }
        public IEnumerable<Item> ReadItems( string filePath )
        {
            var ds = filePath.ToDataSet();

            var items = ds.Tables[0]
                .AsEnumerable()
                .Select( ( c ) =>
                {
                    var i = new Item()
                    {
                        ItemIdentifier = c[0].ToString(),
                        Description = c[1].ToString(),
                        Size = c[2].ToString(),
                        Price = float.Parse( c[3].ToString() ),

                        SellerId = GetSeller( c[4].ToString(), c[5].ToString() ).Id,

                    };
                    return i;

                } )
                 .OrderByDescending( o => o.ItemIdentifier )
                 .ToList();


            return items;
        }

        private Seller GetSeller( string name, string firstName )
        {
            var existingSeller =_dataService.Sellers.FirstOrDefault( s => s.Name == name && s.FirstName == firstName );

            if(existingSeller == null )
            {
                existingSeller = _dataService.Add( new Seller() { Name = name, FirstName = firstName, FamilientreffPercentage = 0.0f } );
            }

            return existingSeller;
        }
    }
}