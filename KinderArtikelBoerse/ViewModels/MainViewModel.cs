using KinderArtikelBoerse.Contracts;
using KinderArtikelBoerse.Models;
using KinderArtikelBoerse.Utils;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;

/*
 * TODOS:
 * Schalter um verkaufte items zu filtern 
 * 
 */

namespace KinderArtikelBoerse.Viewmodels
{
    
    public class MainViewModel : PropertyChangeNotifier
    {
        public CashRegisterViewModel CashRegisterViewModel { get; }

        public MainViewModel(IMarketService dataService, IStatisticsService statisticsService)
        {
            _itemReader = new ExcelItemReader(dataService);
            _dataService = dataService;
            _statisticsService = statisticsService;
            CashRegisterViewModel = new CashRegisterViewModel( dataService, statisticsService );
        }

        private IItemReader _itemReader;
        private IMarketService _dataService;

        private string _toolTitle = "Kinderartikelbörse Familientreff Kaltbrunn";
        public string ToolTitle
        {
            get { return _toolTitle; }
            set { _toolTitle = value; RaisePropertyChanged(); }
        }


        private string _sellerExcelFilePath = AppDomain.CurrentDomain.BaseDirectory +"verkäufer.xlsx";
        public string SellerExcelFilePath
        {
            get { return _sellerExcelFilePath; }
            set { _sellerExcelFilePath = value; RaisePropertyChanged(); }
        }

        private string _templateFilePath = AppDomain.CurrentDomain.BaseDirectory + "template.xlsx";
        public string TemplateFilePath
        {
            get { return _templateFilePath; }
            set { _templateFilePath = value; RaisePropertyChanged(); }
        }

        private string _outputFileName = @"boerse_2018.xlsx";
        public string OutputFileName
        {
            get { return _outputFileName; }
            set { _outputFileName = value; RaisePropertyChanged(); }
        }

        private string _result;
        public string Result
        {
            get { return _result; }
            set {
                _result = value;
                RaisePropertyChanged();
            }
        }

        private string _inputFilePath = @"C:\Users\Dg8\Source\Repos\ChildMarket\KinderArtikelBoerse\Assets\testitems.xlsx";
        public string InputFilePath
        {
            get { return _inputFilePath; }
            set { _inputFilePath = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<ItemAssociationViewModel> _items;
        public IEnumerable<ItemAssociationViewModel> Items
        {
            get
            {
                if(_items == null )
                {
                    var associations = from i in _dataService.Items
                            join s in _dataService.Sellers on i.SellerId equals s.Id
                            select new ItemAssociationViewModel() { Item = new ItemViewModel(i), Seller = new SellerViewModel(s.Id, _dataService) };

                    _items = new ObservableCollection<ItemAssociationViewModel>( associations );
                }

                return _items;
            }
        }

        private ObservableCollection<SellerViewModel> _sellers;
        public IEnumerable<SellerViewModel> Sellers
        {
            get
            {
                var seq = new SellerEqualityComparer();
                if ( _sellers == null )
                {
                    _sellers = new ObservableCollection<SellerViewModel> ( 
                        Items
                        .Select( i => i.Seller )
                        .Distinct( seq ) 
                        .ToList()
                    );
                }

                return _sellers;
            }
        }
       
        private ICommand _createExcelCommand;
        public ICommand CreateExcelCommand => _createExcelCommand ?? ( _createExcelCommand = new ActionCommand<string>( async ( unused ) =>
                                                          {
                                                              Result =  await Task.Run( () =>
                                                              {
                                                                  Result = "In Bearbeitung.. Bitte warten";
                                                                  return GenerateExcel();
                                                              } );
                                                          } ) );

        private ICommand _fixItemExcelCommand;
        public ICommand FixItemExcelCommand => _fixItemExcelCommand ?? ( _fixItemExcelCommand = new ActionCommand<string>( ( path ) =>
        {
            var excelItemLists = Directory.GetFiles( path )
                .Where( f => f.EndsWith( ".xlsx" ) );
               // .Select( f => Read( f ) );

            var excelApp = new Excel.Application();
            excelApp.Visible = false;
            excelApp.DisplayAlerts = false;

            var itemListFixer = new ItemListFixer();
            try
            {
                foreach ( var f in excelItemLists )
                {
                    itemListFixer.FixItemList( excelApp, f );
                }
            }
            finally
            {
                excelApp.Quit();
            }               
        } ) );

        private ICommand _readExcelCommand;
        private IStatisticsService _statisticsService;

        public ICommand ReadExcelCommand => _readExcelCommand ?? ( _readExcelCommand = new ActionCommand( () =>
        {
            //ein Excel file eines verkäufers einlesen 
            //die artikel in die Db migrieren

            var seller = _itemReader.ReadItems( InputFilePath );

            //var sellers = _provider.Sellers.ToList();

            //var existingSeller = sellers.FirstOrDefault( s => s.Name + s.Surname == Path.GetFileNameWithoutExtension( InputFilePath ) );
            //if ()

        } ) );

        private string GenerateExcel()
        {
            var excelApp = new Excel.Application();
            try
            {
                excelApp.Visible = false;

                var sellersDataSet =  SellerExcelFilePath.ToDataSet();

                var sellers = sellersDataSet.Tables[0]
                .AsEnumerable()
                .Select( ( c ) =>
                {
                    var s = new Seller()
                    {
                        // = c[0].ToString(),
                        FirstName = c[1].ToString(),
                        Name = c[2].ToString(),
                        FamilientreffPercentage = int.Parse( c[3].ToString() )

                    };
                    return new SellerViewModel( s.Id, _dataService );
                   
                } )
                 .OrderByDescending( o => o.Name )
                 .ToList();

                var book = excelApp.Workbooks.Add();
                
                

               
                


                //artikelliste pro verkäufer
                var templateWorkbook = excelApp.Workbooks.Open( TemplateFilePath );
                var templateSheet = (Worksheet)templateWorkbook.Sheets[1];

                templateSheet.UsedRange.Copy();
                foreach ( var seller in sellers )
                {
                    var sheet = (Worksheet)book.Sheets.Add();
                    
                    sheet.UsedRange.PasteSpecial( XlPasteType.xlPasteColumnWidths );
                    sheet.UsedRange.PasteSpecial( XlPasteType.xlPasteValuesAndNumberFormats );
                    sheet.UsedRange.PasteSpecial( XlPasteType.xlPasteFormulas );
                    sheet.UsedRange.PasteSpecial( XlPasteType.xlPasteFormats );

                    sheet.Cells[3, "C"].Value = GetSheetName(seller);

                    sheet.Cells[9, "D"].Value = $"{seller.FamilientreffSharePercentage}%";

                    sheet.Name = GetSheetName(seller);

                    //doesnt work... ffs

                    //sheet.PageSetup.LeftHeader = templateSheet.PageSetup.LeftHeader;
                    //sheet.PageSetup.CenterHeader = templateSheet.PageSetup.CenterHeader;
                    //sheet.PageSetup.RightHeader = templateSheet.PageSetup.RightHeader;

                    //sheet.PageSetup.RightFooter = templateSheet.PageSetup.RightFooter;

                    //sheet.PageSetup.FitToPagesWide = 1;

                }

                //übersichts tabelle
                var overviewSheet = (Worksheet)book.Sheets.Add();
                overviewSheet.Name = $"Übersicht";
                overviewSheet.Columns["A:A"].ColumnWidth = 30;
                overviewSheet.Cells[1, "A"].Value = "Übersicht";
                overviewSheet.Cells[1, "A"].Font.Bold = true;
                overviewSheet.Cells[3, "A"].Value = "Total Artikel";

                var firstSheetName = GetSheetName( sellers.First() );
                var lastSheetName = GetSheetName( sellers.Last() );
                overviewSheet.Cells[3, "B"].FormulaLocal = $"=SUMME('{ firstSheetName }:{ lastSheetName }'!D5)";
                overviewSheet.Cells[3, "C"].FormulaLocal = $"=SUMME('{firstSheetName}:{lastSheetName}'!E5)";
                overviewSheet.Cells[3, "C"].NumberFormat = "CHF * #'##0.00";

                overviewSheet.Cells[4, "A"].Value = "davon verkauft";
                overviewSheet.Cells[4, "B"].FormulaLocal = $"=SUMME('{ firstSheetName }:{ lastSheetName }'!D7)";
                overviewSheet.Cells[4, "C"].FormulaLocal = $"=SUMME('{firstSheetName}:{lastSheetName}'!E7)";
                overviewSheet.Cells[4, "C"].NumberFormat = "CHF * #'##0.00";

                overviewSheet.Cells[6, "A"].Value = "Anteil Familientreff";
                overviewSheet.Cells[6, "C"].FormulaLocal = $"=SUMME('{firstSheetName}:{lastSheetName}'!E9)";
                overviewSheet.Cells[6, "C"].NumberFormat = "CHF * #'##0.00";

                ( (Worksheet)book.Sheets["Tabelle1"] ).Delete();

                var savePath = Path.Combine( Path.GetDirectoryName( SellerExcelFilePath ), OutputFileName );
                book.SaveAs( savePath );

                return "OK";
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex );
                return $"{ex.Message}\n{ex.StackTrace}";
            }
            finally
            {
                excelApp.Quit();
            }
        }

        private string GetSheetName( SellerViewModel sellerViewModel )
        {
            return $"{sellerViewModel.Name} {sellerViewModel.FirstName}";
        }

        public string[] GetRange( string range, Worksheet excelWorksheet )
        {
            Range workingRangeCells = excelWorksheet.get_Range( range, Type.Missing );
            //workingRangeCells.Select();

            var array = (Array)workingRangeCells.Cells.Value2;
            return (string[])array;
        }
    }

    
}
