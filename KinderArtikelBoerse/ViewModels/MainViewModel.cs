﻿using KinderArtikelBoerse.Contracts;
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


namespace KinderArtikelBoerse.Viewmodels
{
    public partial class MainViewModel : PropertyChangeNotifier
    {
        public MainViewModel(IMarketDataProvider provider)
        {
            _itemReader = new ExcelItemReader();
            _provider = provider;
        }

        private IItemReader _itemReader;
        private IMarketDataProvider _provider;

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

        private string _inputFilePath;
        public string InputFilePath
        {
            get { return _inputFilePath; }
            set { _inputFilePath = value; RaisePropertyChanged(); }
        }


        //private SellerViewModel _selectedSellerViewModel;
        //public SellerViewModel SelectedSellerViewModel
        //{
        //    get { return _selectedSellerViewModel; }
        //    set { _selectedSellerViewModel = value;
        //        RaisePropertyChanged();
        //    }
        //}

        private ObservableCollection<ItemViewModel> _items;
        public IEnumerable<ItemViewModel> Items
        {
            get
            {
                if(_items == null )
                {
                    _items = new ObservableCollection<ItemViewModel>( _provider.Items.Select( i => new ItemViewModel( i ) ) );
                }

                return _items;
            }
        }

        private ICollectionView _unSoldItemsCollectionView;
        public ICollectionView UnSoldItemsCollectionView
        {
            get
            {
                if ( _unSoldItemsCollectionView == null )
                {
                    _unSoldItemsCollectionView = CollectionViewSource.GetDefaultView( Items );

                    _unSoldItemsCollectionView.Filter += ( obj ) =>
                    {
                        var item = obj as ISellable;
                        return !item.IsSold;
                    };

                }
                return _unSoldItemsCollectionView;
            }
        }

        private ObservableCollection<string> _itemsText;
        public IEnumerable<string> ItemsText
        {
            get
            {
                if ( _itemsText == null )
                {
                    _itemsText = new ObservableCollection<string>( _provider.Items.Select( i => i.ItemIdentifier ) );
                }

                return _itemsText;
            }
        }

        private ISellable _searchItem;
        public ISellable SearchItem
        {
            get
            {
                return _searchItem;
            }
            set
            {
                var oldValue = _searchItem;
                _searchItem = value;
                
                if(oldValue != value )
                {
                    RaisePropertyChanged();
                }
                
            }
        }

        private string _searchItemText = string.Empty;
        public string SearchItemText
        {
            get
            {
                return _searchItemText;
            }
            set
            {
                var oldValue = _searchItemText;
                _searchItemText = value;

                if(oldValue != value )
                {
                    ItemsCollectionView.Refresh();

                    var filteredCollection = ItemsCollectionView
                        .Cast<ISellable>()
                        .ToList()
                        ;

                    if(filteredCollection.Count == 1 )
                    {
                        SearchItem = filteredCollection.First();
                    }

                    RaisePropertyChanged();
                }
                
            }
        }

        private ICollectionView _itemsCollectionView;
        public ICollectionView ItemsCollectionView
        {
            get
            {
                if(_itemsCollectionView == null )
                {
                    _itemsCollectionView = CollectionViewSource.GetDefaultView( Items );

                    _itemsCollectionView.Filter += (obj) => ItemFilterPredicate( (ISellable)obj );

                }
                return _itemsCollectionView;
            }
        }

        private bool ItemFilterPredicate(ISellable item )
        {
            var normalizedSearchText = SearchItemText.ToLowerInvariant();

            return item.ItemIdentifier.ToLowerInvariant().StartsWith( normalizedSearchText ) ||
                    item.Description.ToLowerInvariant().Contains( normalizedSearchText ) ||
                    item.Price.ToString().ToLowerInvariant().StartsWith( normalizedSearchText ) ||
                    item.Size.ToLowerInvariant().Contains(normalizedSearchText) ||
                    //seller name
                    item.Seller.Name.ToLowerInvariant().StartsWith(normalizedSearchText) ||
                    item.Seller.FirstName.ToLowerInvariant().StartsWith( normalizedSearchText ) 
                    
                    ;
        }

        public AutoCompleteFilterPredicate<object> SearchItemFilter
        {
            get
            {
                return ( searchText, obj ) =>
                {
                    return ItemFilterPredicate( (ISellable)obj );
                };
            }
        }

        private ICommand _isSoldCheckedCommand;
        public ICommand IsSoldCheckedCommand => _isSoldCheckedCommand ?? ( _isSoldCheckedCommand = new ActionCommand<bool>( ( isChecked ) =>
        {
            if ( isChecked )
            {
                //SearchItem = null;
                SearchItemText = string.Empty;
                //IsAutoCompleteBoxFocused = true;
            }
        } ) );

        private ICommand _refreshCollectionViewCommand;
        public ICommand RefreshCollectionViewCommand => _refreshCollectionViewCommand ?? ( _refreshCollectionViewCommand = new ActionCommand( ( ) =>
        {
            UnSoldItemsCollectionView.Refresh();
        } ) );

        
        private ICommand _resetFocusCommand;
        public ICommand ResetFocusCommand => _resetFocusCommand ?? ( _resetFocusCommand = new ActionCommand<AutoCompleteBox>( ( acb ) =>
        {
            //acb.Focus();
            
        } ) );

        //private bool _isAutoCompleteBoxFocused;
        //public bool IsAutoCompleteBoxFocused
        //{
        //    get { return _isAutoCompleteBoxFocused; }
        //    set
        //    {
        //        _isAutoCompleteBoxFocused = value;
        //        RaisePropertyChanged();
        //    }
        //}

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

        public ICommand ReadExcelCommand => _readExcelCommand ?? ( _readExcelCommand = new ActionCommand( () =>
        {
            //ein Excel file eines verkäufers einlesen 
            //die artikel in die Db migrieren

            //_itemReader.ReadItems()

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
                    return new SellerViewModel( s );
                   
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
