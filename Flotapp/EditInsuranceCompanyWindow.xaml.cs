using System;
using System.Collections;
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
    /// Logika interakcji dla klasy EditInsuranceCompanyWindow.xaml
    /// </summary>
    public partial class EditInsuranceCompanyWindow : Window
    {
        DataClasses1DataContext baza = new DataClasses1DataContext();
        Ubezpieczyciele x;
        public EditInsuranceCompanyWindow(Ubezpieczyciele cins)
        {
            InitializeComponent();
            x = cins;
            textBoxCompany.Text = cins.Firma;
        }

        void Zapis()
        {
        var query = (from p in baza.Ubezpieczyciele
            where p.ID_INSURANCE_COMPANY == x.ID_INSURANCE_COMPANY
            orderby p.ID_INSURANCE_COMPANY
            select p).FirstOrDefault();
        query.Firma = textBoxCompany.Text;
        baza.SubmitChanges();
        }
        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            try {
                Zapis();
                MessageBox.Show("Poprawnie zmieniono dane");
                this.Close();
                }
            catch { MessageBox.Show("Coś poszło nie tak!"); }

        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
