using KinderArtikelBoerse.Utils;
using KinderArtikelBoerse.Viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            _mainViewModel = new MainViewModel( new DemoMarketDataProvider() );//new SellerProvider( "MarketDbContext" ) );

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
