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
    /// Logika interakcji dla klasy AddInsuranceCompanyWindow.xaml
    /// </summary>
    public partial class AddInsuranceCompanyWindow : Window
    {
        DataClasses1DataContext baza = new DataClasses1DataContext();
        public AddInsuranceCompanyWindow()
        {
            InitializeComponent();
        }

        public void Check()
        {
            int i=0;
            var query = (from p in baza.Ubezpieczyciele
                         where p.Firma == textBoxFirma.Text
                         orderby p.ID_INSURANCE_COMPANY
                         select p).ToList();
            foreach (var p in query)
            {
                if (p.Firma == textBoxFirma.Text)
                {
                    {
                        MessageBox.Show("Taka firma już istnieje w bazie danych.");
                        i++;
                        this.Close();
                    }
                }
            }
            if(i==0)
            {
                Zapis();
            }
            
        }
        public void Zapis()
        {
            Ubezpieczyciele firma = new Ubezpieczyciele
            {
                Firma = textBoxFirma.Text,
            };
            baza.Ubezpieczyciele.InsertOnSubmit(firma);
            baza.SubmitChanges();
            MessageBox.Show("Firma została poprawnie dodana do bazy.");
            
        }
        private void buttonAccept_Click(object sender, RoutedEventArgs e)
        {
            Check();
            this.Close();
        }
    }
}
