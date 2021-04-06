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
    /// Logika interakcji dla klasy AddInspectionCompanyWindow.xaml
    /// </summary>
    public partial class AddInspectionCompanyWindow : Window
    {
        DataClasses1DataContext baza = new DataClasses1DataContext();
        public AddInspectionCompanyWindow()
        {
            InitializeComponent();
        }
        public void Check()
        {
            int i = 0;
            var query = (from p in baza.Warsztaty
                         where p.Firma == textBoxFirma.Text
                         orderby p.ID_INSPECTION_COMPANY
                         select p).ToList();
            foreach (var z in query)
                if (z.Firma == textBoxFirma.Text)
                {
                    {
                        MessageBox.Show("Taki warsztat już istnieje w bazie danych.");
                        i++;
                        this.Close();
                    }
                }
            
         if (i==0)
            {
                Zapis();
            }
        }

        public void Zapis()
        {
            if (textBoxFirma.Text == "" || textBoxFirma.Text == " ")
            {
                MessageBox.Show("Wprowadź nazwę warsztatu!");
                return;
            }
            else
            {


                try
                {
                    Warsztaty firma = new Warsztaty
                    {
                        Firma = textBoxFirma.Text,
                    };
                    baza.Warsztaty.InsertOnSubmit(firma);
                    baza.SubmitChanges();
                    MessageBox.Show("Warsztat został poprawnie dodany.");
                }
                catch
                {
                    MessageBox.Show("Wystąpił błąd");
                }
            }
        }

        private void buttonAccept_Click(object sender, RoutedEventArgs e)
        {
            Check();
            this.Close();
        }
    }
}
