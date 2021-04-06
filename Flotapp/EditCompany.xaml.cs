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
    /// Logika interakcji dla klasy EditCompany.xaml
    /// </summary>
    public partial class EditCompany : Window
    {
        DataClasses1DataContext baza = new DataClasses1DataContext();
        Firmy x;
        public EditCompany(Firmy x)
        {
            InitializeComponent();
            this.x = x;
            Load();
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Zapis();
                MessageBox.Show("Poprawnie zmieniono nazwę firmy");
            }
            catch
            {
                MessageBox.Show("Wystąpił błąd!");
            }
            this.Close();
        }
        void Load()
        {
            textBoxFirma.Text = x.Firma;
        }
        void Zapis()
        {
            var query = (from p in baza.Firmy
                         where p.ID_COMPANY == x.ID_COMPANY
                         orderby p.ID_COMPANY
                         select p).FirstOrDefault();
            query.Firma = textBoxFirma.Text;
            baza.SubmitChanges();
        }
    }
}
