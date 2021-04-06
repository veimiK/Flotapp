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
    /// Logika interakcji dla klasy AddContactWindow.xaml
    /// </summary>
    public partial class AddContactWindow : Window
    {
        public static DataClasses1DataContext baza = new DataClasses1DataContext();
        public AddContactWindow()
        {
            InitializeComponent();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxEmail.Text == null | textBoxEmail.Text == "" | textBoxEmail.Text == " ")
            {
                MessageBox.Show("Wprowadź email");
                return;
            }
            else
            {
                Zapis();
                this.Close();
            }
        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        void Zapis()
        {
            Kontakty kon = new Kontakty
            {
                Email = textBoxEmail.Text,
                Imie = textBoxImie.Text,
                Nazwisko = textBoxNazwisko.Text,
                };
                baza.Kontakty.InsertOnSubmit(kon);
                baza.SubmitChanges();
                MessageBox.Show("Pomyślnie dodano kontakt.");
            }
        }
    }

