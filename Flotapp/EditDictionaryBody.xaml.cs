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
    /// Logika interakcji dla klasy EditDictionaryBody.xaml
    /// </summary>
    public partial class EditDictionaryBody : Window
    {
        DataClasses1DataContext baza = new DataClasses1DataContext();
        public EditDictionaryBody()
        {
            InitializeComponent();
            Load();
        }

        private void buttonAddBody_Click(object sender, RoutedEventArgs e)
        {
            AddBody instance = new AddBody();
            instance.Show();
        }

        private void buttonEditBody_Click(object sender, RoutedEventArgs e)
        {
            if (gridBodies.SelectedIndex == -1)
            {
                MessageBox.Show("Zaznacz wiersz!");
            }
            else
            {
                try
                {
                    EditBody instance = new EditBody(gridBodies.SelectedItem as Nadwozia);
                    instance.Show();
                }
                catch { MessageBox.Show("Wystąpił błąd podczas wczytywania."); }
            }
        }

        private void buttonRemoveBody_Click(object sender, RoutedEventArgs e)
        {
            if (gridBodies.SelectedIndex == -1)
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
                        Nadwozia body = gridBodies.SelectedItem as Nadwozia;
                        final = body.ID_BODY;
                    }
                    catch { MessageBox.Show("Zaznacz wiersz!"); }

                    var query = (from p in baza.Nadwozia
                                 where p.ID_BODY == final
                                 select p).FirstOrDefault();
                    if (query != null)
                    {
                        baza.Nadwozia.DeleteOnSubmit(query);
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
            gridBodies.ItemsSource = null;
            gridBodies.ItemsSource = baza.Nadwozia;
        }
    }
}
