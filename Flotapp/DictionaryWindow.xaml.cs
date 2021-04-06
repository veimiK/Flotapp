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
using System.Windows.Shapes;

namespace Flotapp
{
    /// <summary>
    /// Logika interakcji dla klasy DictionaryWindow.xaml
    /// </summary>
    public partial class DictionaryWindow : Window
    {
        public DictionaryWindow()
        {
            InitializeComponent();
        }

        private void buttonFirmy_Click(object sender, RoutedEventArgs e)
        {
            EditDictionaryCompanies _instance = new EditDictionaryCompanies();
            _instance.Show();
        }

        private void buttonNadwozia_Click(object sender, RoutedEventArgs e)
        {
            EditDictionaryBody _instance = new EditDictionaryBody();
            _instance.Show();
        }

        private void buttonPaliwa_Click(object sender, RoutedEventArgs e)
        {
            EditDictionaryFuel _instance = new EditDictionaryFuel();
            _instance.Show();
        }
    }
}
