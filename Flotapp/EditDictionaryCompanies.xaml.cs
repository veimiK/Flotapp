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
    /// Logika interakcji dla klasy EditDictionaryCompanies.xaml
    /// </summary>
    public partial class EditDictionaryCompanies : Window
    {
        DataClasses1DataContext baza = new DataClasses1DataContext();
        public EditDictionaryCompanies()
        {
            InitializeComponent();
            Load();
        }

        private void buttonAddCompany_Click(object sender, RoutedEventArgs e)
        {
            AddCompany instance = new AddCompany();
            instance.Show();
        }

        private void buttonEditCompany_Click(object sender, RoutedEventArgs e)
        {
            if (gridCompanies.SelectedIndex == -1)
            {
                MessageBox.Show("Zaznacz wiersz!");
            }
            else
            {
                try
                {
                    EditCompany instance = new EditCompany(gridCompanies.SelectedItem as Firmy);
                    instance.Show();
                }
                catch
                {
                    MessageBox.Show("Wystąpił błąd podczas wczytywania.");
                }
            }
        } 

        private void buttonRemoveCompany_Click(object sender, RoutedEventArgs e)
        {
            if (gridCompanies.SelectedIndex == -1)
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
                        Firmy body = gridCompanies.SelectedItem as Firmy;
                        final = body.ID_COMPANY;
                    }
                    catch { MessageBox.Show("Zaznacz wiersz!"); }

                    var query = (from p in baza.Firmy
                                 where p.ID_COMPANY == final
                                 select p).FirstOrDefault();
                    if (query != null)
                    {
                        baza.Firmy.DeleteOnSubmit(query);
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
            gridCompanies.ItemsSource = null;
            gridCompanies.ItemsSource = baza.Firmy;
        }
    }
}
