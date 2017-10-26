using KinderArtikelBoerse.Contracts;
using System;
using KinderArtikelBoerse.Models;

namespace KinderArtikelBoerse.Utils
{

    public class ExcelItemReader : IItemReader
    {
        public Seller ReadItems( string filePath )
        {
            var ds = filePath.ToDataSet();

            return null;
        }
    }
}