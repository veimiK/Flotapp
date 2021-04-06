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
    /// Logika interakcji dla klasy AddFuel.xaml
    /// </summary>
    public partial class AddFuel : Window
    {
        DataClasses1DataContext baza = new DataClasses1DataContext();
        public AddFuel()
        {
            InitializeComponent();
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxPaliwo.Text == "" || textBoxPaliwo.Text.Trim() == "")
            {
                MessageBox.Show("Wprowadź nazwę paliwa.");
                return;
            }
            else
            {
                try
                {
                    RodzajePaliwa paliwo = new RodzajePaliwa { RodzajPaliwa = textBoxPaliwo.Text };
                    baza.RodzajePaliwa.InsertOnSubmit(paliwo);
                    baza.SubmitChanges();
                    MessageBox.Show("Poprawnie wprowadzono paliwo.");
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
