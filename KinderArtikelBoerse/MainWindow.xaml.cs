using KinderArtikelBoerse.Utils;
using KinderArtikelBoerse.Viewmodels;
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

            var dataService = new DemoMarketDataProvider();
            var sellersViewModelProvider = new SellersViewModelProvider( dataService );
            var itemsViewModelProvider = new ItemsViewModelProvider( dataService );

            var itemsReader = new ExcelItemReader( dataService );

            _mainViewModel = new MainViewModel( itemsReader, sellersViewModelProvider, itemsViewModelProvider );

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
