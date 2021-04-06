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
    /// Logika interakcji dla klasy AddCompany.xaml
    /// </summary>
    public partial class AddCompany : Window
    {
        DataClasses1DataContext baza = new DataClasses1DataContext();
        public AddCompany()
        {
            InitializeComponent();
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxFirma.Text == "" || textBoxFirma.Text.Trim() == "")
            {
                MessageBox.Show("Wprowadź nazwę nadwozia.");
                return;
            }
            else
            {
                try
                {
                    Firmy fir = new Firmy { Firma = textBoxFirma.Text };
                    baza.Firmy.InsertOnSubmit(fir);
                    baza.SubmitChanges();
                    MessageBox.Show("Poprawnie wprowadzono firmę.");
                }
                catch
                {
                    MessageBox.Show("Wystąpił błąd.");
                }
                this.Close();
            }
        }
    }
}
