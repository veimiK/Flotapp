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
    /// Logika interakcji dla klasy EditFuel.xaml
    /// </summary>
    public partial class EditFuel : Window
    {
        DataClasses1DataContext baza = new DataClasses1DataContext();
        RodzajePaliwa x;
        public EditFuel(RodzajePaliwa x)
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
                MessageBox.Show("Poprawnie zmieniono dane");
            }
            catch
            {
                MessageBox.Show("Wystąpił błąd.");
            }
            this.Close();
        }
        void Load()
        {
            textBoxPaliwo.Text = x.RodzajPaliwa;
        }
        void Zapis()
        {
            var query = (from p in baza.RodzajePaliwa
                         where p.ID_FUEL == x.ID_FUEL
                         orderby p.ID_FUEL
                         select p).FirstOrDefault();
            query.RodzajPaliwa = textBoxPaliwo.Text;
            baza.SubmitChanges();
        }
    }
}
