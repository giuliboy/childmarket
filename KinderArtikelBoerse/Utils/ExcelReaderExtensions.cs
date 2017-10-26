using System.Data;
using System.Data.OleDb;

namespace KinderArtikelBoerse.Utils
{
    public static class ExcelReaderExtensions
    {
        public static DataSet ToDataSet(this string excelFilePath )
        {
            OleDbConnection conn = new System.Data.OleDb.OleDbConnection( ( "provider=Microsoft.ACE.OLEDB.12.0; " + ( $"data source={excelFilePath}; " + "Extended Properties=Excel 12.0;" ) ) );

            try
            {
                // Select the data from Sheet1 of the workbook.
                OleDbDataAdapter ada = new OleDbDataAdapter( $"select * from [{"Tabelle1"}$]", conn );
                DataSet ds = new DataSet();
                ada.Fill( ds );

                return ds;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}