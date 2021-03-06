﻿using KinderArtikelBoerse.Utils;
using KinderArtikelBoerse.Viewmodels;
using Market.Service;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;

namespace KinderArtikelBoerse
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _mainViewModel;

        public MainWindow()
        {
            InitializeComponent();

            var optionsBuilder = new DbContextOptionsBuilder<MarketDbContext>();
            //optionsBuilder.UseInMemoryDatabase( "memory.db" );
            optionsBuilder.UseSqlServer( "Server=(localdb)\\mssqllocaldb;Database=MarketDatabase;Trusted_Connection=True;MultipleActiveResultSets=true" );
            var dataService = new MarketService( optionsBuilder.Options ); // new DemoMarketDataProvider();
            
            //var dataService = new DemoMarketDataProvider();
            var itemsViewModelProvider = new ItemsViewModelProvider( dataService );
            var sellersViewModelProvider = new SellersViewModelProvider( dataService , itemsViewModelProvider );
            var itemsReader = new ExcelItemReader( dataService );

            _mainViewModel = new MainViewModel( itemsReader, dataService, sellersViewModelProvider, itemsViewModelProvider );

            DataContext = _mainViewModel;
        }

        private void Window_Drop( object sender, DragEventArgs e )
        {
            if ( e.Data.GetDataPresent( DataFormats.FileDrop ) )
            {
                var file = ((string[])e.Data.GetData( DataFormats.FileDrop ))
                    .Where(f => System.IO.Path.GetExtension(f).Contains("xlsx"))
                    .FirstOrDefault();

                if(file == null )
                {
                    return;
                }

                _mainViewModel.SellerExcelFilePath = file;
            }
        }
    }
}
