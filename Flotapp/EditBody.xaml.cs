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
    /// Logika interakcji dla klasy EditBody.xaml
    /// </summary>
    public partial class EditBody : Window
    {
        DataClasses1DataContext baza = new DataClasses1DataContext();
        Nadwozia x;
        public EditBody(Nadwozia x)
        {
            InitializeComponent();
            this.x = x;
            Load();
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxNadwozie.Text == "" || textBoxNadwozie.Text.Trim() == "")
            {
                MessageBox.Show("Wprowadź nazwę nadwozia.");
                return;
            }
            else
            {
                try
                {
                    Zapis();
                    MessageBox.Show("Pomyślnie zmieniono nazwę.");
                    this.Close();
                }
                catch
                {
                    MessageBox.Show("Wystąpił błąd!");
                }
            }
        }
        void Load()
        {
            textBoxNadwozie.Text = x.Nazwa;
        }
        void Zapis()
        {
            var query = (from p in baza.Nadwozia
                         where p.ID_BODY == x.ID_BODY
                         orderby p.ID_BODY
                         select p).FirstOrDefault();
            query.Nazwa = textBoxNadwozie.Text;
            baza.SubmitChanges();
        }
    }
}
