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
    /// Logika interakcji dla klasy EditDictionaryFuel.xaml
    /// </summary>
    public partial class EditDictionaryFuel : Window
    {
        DataClasses1DataContext baza = new DataClasses1DataContext();
        public EditDictionaryFuel()
        {
            InitializeComponent();
            Load();
        }

        private void buttonAddFuel_Click(object sender, RoutedEventArgs e)
        {
            AddFuel _instance = new AddFuel();
            _instance.Show();
        }

        private void buttonEditFuel_Click(object sender, RoutedEventArgs e)
        {
            if (gridFuel.SelectedIndex == -1)
            {
                MessageBox.Show("Zaznacz wiersz!");
            }
            else
            {
                try
                {
                    EditFuel _instance = new EditFuel(gridFuel.SelectedItem as RodzajePaliwa);
                    _instance.Show();
                }
                catch { MessageBox.Show("Wystąpił błąd podczas wczytywania"); }
            }
        }

        private void buttonRemoveFuel_Click(object sender, RoutedEventArgs e)
        {
            if (gridFuel.SelectedIndex == -1)
            {
                MessageBox.Show("Zaznacz wiersz!");
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Czy jesteś pewien usunięcia danego rekordu?", "Pytanie", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    int final = 0;
                    try
                    {
                        RodzajePaliwa fuel = gridFuel.SelectedItem as RodzajePaliwa;
                        final = fuel.ID_FUEL;
                    }
                    catch { MessageBox.Show("Zaznacz wiersz!"); }

                    var query = (from p in baza.RodzajePaliwa
                                 where p.ID_FUEL == final
                                 select p).FirstOrDefault();
                    if (query != null)
                    {
                        baza.RodzajePaliwa.DeleteOnSubmit(query);
                        baza.SubmitChanges();
                        Load();
                    }
                }
                else
                { }
            }
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            Load();
        }

        void Load()
        {
            DataClasses1DataContext baza = new DataClasses1DataContext();
            gridFuel.ItemsSource = null;
            gridFuel.ItemsSource = baza.RodzajePaliwa; 
        }
    }
}
