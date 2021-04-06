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
    /// Logika interakcji dla klasy AddBody.xaml
    /// </summary>
    public partial class AddBody : Window
    {
        public static DataClasses1DataContext baza = new DataClasses1DataContext();
        public AddBody()
        {
            InitializeComponent();
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if(textBoxNadwozie.Text == "" || textBoxNadwozie.Text.Trim() == "" )
            {
                MessageBox.Show("Wprowadź nazwę nadwozia.");
                return;
            }
            else
            {
                try
                {
                    Nadwozia nad = new Nadwozia { Nazwa = textBoxNadwozie.Text };
                    baza.Nadwozia.InsertOnSubmit(nad);
                    baza.SubmitChanges();
                    MessageBox.Show("Poprawnie wprowadzono nadwozie.");
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
