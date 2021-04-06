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
    /// Logika interakcji dla klasy EditContactWindow.xaml
    /// </summary>
    public partial class EditContactWindow : Window
    {
        DataClasses1DataContext baza = new DataClasses1DataContext();
        Kontakty x;
        public EditContactWindow(Kontakty kon)
        {
            InitializeComponent();
            x = kon;
            Load();
        }
        void Load()
        {
            textBoxEmail.Text = x.Email;
            textBoxImie.Text = x.Imie;
            textBoxNazwisko.Text = x.Nazwisko;
        }

        void Zapis()
        {
            var query = (from p in baza.Kontakty
                where p.ID_CONTACT == x.ID_CONTACT
                orderby p.ID_CONTACT
                select p).FirstOrDefault();
            query.Email = textBoxEmail.Text;
            query.Imie = textBoxImie.Text;
            query.Nazwisko = textBoxNazwisko.Text;
                    baza.SubmitChanges();
        }
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxEmail.Text == null || textBoxEmail.Text == "" || textBoxEmail.Text == " ")
            {
                MessageBox.Show("Wprowadź email");
                return;
            }
            Zapis();
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
