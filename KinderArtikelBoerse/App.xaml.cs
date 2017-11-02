using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace KinderArtikelBoerse
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof( FrameworkElement ),
                new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage( CultureInfo.CurrentCulture.IetfLanguageTag ) ) );
        }

        protected override void OnStartup( StartupEventArgs e )
        {
            base.OnStartup( e );
            CultureInfo culture = new CultureInfo( ConfigurationManager.AppSettings["DefaultCulture"] );
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }
    }
}
