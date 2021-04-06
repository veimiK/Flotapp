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
    /// Logika interakcji dla klasy ContactWindow.xaml
    /// </summary>
    public partial class ContactWindow : Window
    {
        DataClasses1DataContext baza = new DataClasses1DataContext();
        public ContactWindow()
        {
            InitializeComponent();
            Load();
        }

        void Load()
        {
            DataClasses1DataContext baza = new DataClasses1DataContext();
            gridContacts.ItemsSource = null;
            gridContacts.ItemsSource = baza.Kontakty;
        }

        private void ButtonAddContact_Click(object sender, RoutedEventArgs e)
        {
            AddContactWindow instance = new AddContactWindow();
            instance.Show();
        }

        private void ButtonEditContact_Click(object sender, RoutedEventArgs e)
        {
            if (gridContacts.SelectedIndex == -1)
            {
                MessageBox.Show("Zaznacz wiersz!");
            }
            else
            {
                try
                {
                    Kontakty kon = gridContacts.SelectedItem as Kontakty;
                    EditContactWindow instance = new EditContactWindow(kon);
                    instance.Show();
                }
                catch
                { MessageBox.Show("Wystąpił błąd podczas wczytywania."); }
            }
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            Load();
        }

        private void ButtonRemoveContact_Click(object sender, RoutedEventArgs e)
        {
            if (gridContacts.SelectedIndex == -1)
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
                        Kontakty kon = gridContacts.SelectedItem as Kontakty;
                        final = kon.ID_CONTACT;
                    }
                    catch { MessageBox.Show("Zaznacz wiersz!"); }

                    var query = (from p in baza.Kontakty
                                 where p.ID_CONTACT == final
                                 select p).FirstOrDefault();
                    if (query != null)
                    {
                        baza.Kontakty.DeleteOnSubmit(query);
                        baza.SubmitChanges();
                        Load();
                    }
                }
                else
                { }
            }
        }
    }
}
