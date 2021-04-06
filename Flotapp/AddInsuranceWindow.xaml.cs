using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Logika interakcji dla klasy AddInsuranceWindow.xaml
    /// </summary>
    public partial class AddInsuranceWindow : Window
    {
        public static DataClasses1DataContext baza = new DataClasses1DataContext();
        int IdCar;
        public AddInsuranceWindow(Samochody sam)
        {
            InitializeComponent();
            IdCar = sam.ID_CAR;
            textBlockWybrany.Text = "Dodawanie ubezpieczenia do " + sam.Marka + " " + sam.Model + " " + sam.Rejestracja;
            var query = (from p in baza.Ubezpieczyciele
                         orderby p.ID_INSURANCE_COMPANY
                         select p).ToList();
            foreach (var x in query)
            {
                comboBoxFirma.Items.Add(x.Firma);
            }
            comboBoxFirma.SelectedIndex = 1;
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxFirma.Text == null | comboBoxFirma.Text == " ")
            {
                MessageBox.Show("Wprowadź firmę");
                return;
            }
            if (datePickerEnd.SelectedDate == null)
            {
                MessageBox.Show("Wprowadź datę zakończenia");
                return;
            }
            if (textBoxPolisa.Text == null | textBoxPolisa.Text == " ")
            {
                MessageBox.Show("Wprowadź numer polisy");
                return;
            }
            try { Convert.ToDouble(textBoxCena.Text); }
            catch
            {
                MessageBox.Show("Wprowadź poprawną cenę! (grosze po przecinku)");
                return;
            }
            Zapis();
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
        this.Close();
        }
        void Zapis()
        {
            var q = (from p in baza.Ubezpieczyciele
                         orderby p.ID_INSURANCE_COMPANY
                         select p).ToList();
            string[,] arrayId = new string[q.Count, 2];
            int i = 0;
            foreach (var x in q)
            {
                arrayId[i, 0] = Convert.ToString(x.ID_INSURANCE_COMPANY);
                arrayId[i, 1] = x.Firma;
                i++;
            }
            int ID_ICF = Convert.ToInt32(arrayId[comboBoxFirma.SelectedIndex, 0]);
            if (checkBoxArchiwalne.IsChecked == false)
            {
                var query = (from p in baza.Ubezpieczenia
                             where p.ID_CAR_fk == IdCar
                             orderby p.ID_INSURANCE
                             select p).ToList();
                foreach (var x in query)
                x.Archiwalny = true;
                baza.SubmitChanges();



                


                Ubezpieczenia ub2 = new Ubezpieczenia
                {
                    DataRozpoczecia = datePickerStart.SelectedDate,
                    DataZakonczenia = datePickerEnd.SelectedDate,
                    Cena = Convert.ToDecimal(textBoxCena.Text),
                    ID_CAR_fk = IdCar,
                    Archiwalny = checkBoxArchiwalne.IsChecked,
                    NumerPolisy = textBoxPolisa.Text,
                    ID_INSURANCE_COMPANY_fk = ID_ICF, 
                };
                baza.Ubezpieczenia.InsertOnSubmit(ub2);
                baza.SubmitChanges();
                MessageBox.Show("Pomyślnie dodano ubezpieczenie i ustawiono je jako niearchiwalne (trwające).");

            }
            else
            {
                Ubezpieczenia ub = new Ubezpieczenia
                {
                    //Firma = comboBoxFirma.Text,
                    DataRozpoczecia = datePickerStart.SelectedDate,
                    DataZakonczenia = datePickerEnd.SelectedDate,
                    Cena = Convert.ToDecimal(textBoxCena.Text),
                    ID_CAR_fk = IdCar,
                    Archiwalny = checkBoxArchiwalne.IsChecked,
                    NumerPolisy = textBoxPolisa.Text,
                    ID_INSURANCE_COMPANY_fk = ID_ICF,
                };
                baza.Ubezpieczenia.InsertOnSubmit(ub);
                baza.SubmitChanges();
                MessageBox.Show("Pomyślnie dodano ubezpieczenie archiwalne.");
            }
            

            
        }
    }
}
