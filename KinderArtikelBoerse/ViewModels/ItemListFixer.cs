using Microsoft.Office.Interop.Excel;
using System;
using Excel = Microsoft.Office.Interop.Excel;

namespace KinderArtikelBoerse.Viewmodels
{
    public class ItemListFixer
    {
        public void FixItemList( Excel.Application excelApp, string filePath )
        {
            try
            {

                var wb = excelApp.Workbooks.Open( filePath );
                var ws = (Worksheet)wb.ActiveSheet;
                var range = ws.UsedRange;

                range.Replace( ".--", "" );
                range.Replace( ".-", "" );

                wb.Save();

            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex );
            }
        }
    }


}
