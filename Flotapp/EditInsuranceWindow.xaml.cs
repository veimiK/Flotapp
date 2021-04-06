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
    /// Logika interakcji dla klasy EditInsuranceWindow.xaml
    /// </summary>
    public partial class EditInsuranceWindow : Window
    {
        Ubezpieczenia x = new Ubezpieczenia();
        DataClasses1DataContext baza = new DataClasses1DataContext();
        public EditInsuranceWindow(int ID_INSURANCE_grid)
        {
            var query = from p in baza.Ubezpieczenia
                        where p.ID_INSURANCE == ID_INSURANCE_grid
                        select new
                        {
                            ID = p.ID_INSURANCE,
                            p.DataRozpoczecia,
                            p.DataZakonczenia,
                            p.Cena,
                            p.ID_INSURANCE_COMPANY_fk,
                            p.ID_CAR_fk,
                            p.Archiwalny,
                            p.NumerPolisy,
                        };

            InitializeComponent();
            foreach (var z in query)
            {
                x.ID_INSURANCE = z.ID;
                x.DataRozpoczecia = z.DataRozpoczecia;
                x.DataZakonczenia = z.DataZakonczenia;
                x.Cena = z.Cena;
                x.Archiwalny = z.Archiwalny;
                x.NumerPolisy = z.NumerPolisy;
                x.ID_CAR_fk = z.ID_CAR_fk;
                x.ID_INSURANCE_COMPANY_fk = z.ID_INSURANCE_COMPANY_fk;
            }
            Load();
        }
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxPolisa.Text == null | textBoxPolisa.Text == " ")
            {
                MessageBox.Show("Wprowadź firmę");
                return;
            }
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
            try { Convert.ToDouble(textBoxCena.Text); }
            catch
            {
                MessageBox.Show("Wprowadź poprawną cenę! (grosze po przecinku)");
                return;
            }
            Zapis();

        }
        void Load()
        {
            /*var query = from p in baza.Ubezpieczenia
                        where p.ID_INSURANCE == ID
                        orderby p.ID_INSURANCE
                        select new
                        {
                            ID = p.ID_INSURANCE,
                            p.Firma,
                            p.DataRozpoczecia,
                            p.DataZakonczenia,
                            p.Cena
                        };
            foreach (var x in query)
            {
                textBoxFirma.Text = x.Firma;
                textBoxCena.Text = Convert.ToString(x.Cena);
                datePickerStart.SelectedDate = (DateTime)x.DataRozpoczecia;
                datePickerEnd.SelectedDate = (DateTime)x.DataZakonczenia;
                MessageBox.Show(x.DataZakonczenia+ " ")
                
                ;          
            }*/
            textBoxCena.Text = Convert.ToString(x.Cena);
            textBoxPolisa.Text = x.NumerPolisy;
            try
            {
                datePickerStart.SelectedDate = (DateTime)x.DataRozpoczecia;
            }
            catch
            {
                MessageBox.Show("Nie udało się wczytać daty rozpoczęcia");
            }
            try
            {
                datePickerEnd.SelectedDate = (DateTime)x.DataZakonczenia;
            }
            catch
            {
                MessageBox.Show("Nie udało się wczytać daty zakończenia.");
            }
            checkBoxArchiwalne.IsChecked = x.Archiwalny;

            //wczytywanie firm do comboBox
            var query = (from p in baza.Ubezpieczyciele
                         orderby p.ID_INSURANCE_COMPANY
                         select p).ToList();
            foreach (var x in query)
            {
                comboBoxFirma.Items.Add(x.Firma);
            }
            var firma = (from p in baza.Ubezpieczyciele
                         where p.ID_INSURANCE_COMPANY == x.ID_INSURANCE_COMPANY_fk
                         select p).First();
                comboBoxFirma.Text = firma.Firma.ToString();
        }
        void Zapis()
        {
            if (checkBoxArchiwalne.IsChecked == true)
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

                var query = (from p in baza.Ubezpieczenia
                             where p.ID_INSURANCE == x.ID_INSURANCE
                             orderby p.ID_INSURANCE
                             select p).FirstOrDefault();
                query.DataRozpoczecia = datePickerStart.SelectedDate;
                query.DataZakonczenia = datePickerEnd.SelectedDate;
                query.Cena = Convert.ToDecimal(textBoxCena.Text);
                query.Archiwalny = checkBoxArchiwalne.IsChecked;
                query.NumerPolisy = textBoxPolisa.Text;
                query.ID_INSURANCE_COMPANY_fk = ID_ICF;
                baza.SubmitChanges();

                MessageBox.Show("Pomyślnie zmieniono wartości.");
                this.Close();
            }
            else
            {
                if (MessageBox.Show("Ustawiasz to ubezpieczenie jako trwające (niearchiwizowane). Zmieni to archiwizację innych ubezpieczeń dla tego samochodu na archiwizowane. Czy chcesz kontynuować?", "Ustawianie archiwizacji", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    MessageBox.Show("Wartości nie zostały zmienione");
                }
                else
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

                    var query2 = (from p in baza.Ubezpieczenia
                                  where p.ID_CAR_fk == x.ID_CAR_fk
                                  orderby p.ID_INSURANCE
                                  select p).ToList();
                    foreach (var x in query2)
                    {
                        x.Archiwalny = true;
                        baza.SubmitChanges();
                    }
                    MessageBox.Show("Poprawnie zmieniono ubezpieczenia");

                    var query = (from p in baza.Ubezpieczenia
                                 where p.ID_INSURANCE == x.ID_INSURANCE
                                 orderby p.ID_INSURANCE
                                 select p).FirstOrDefault();
                    query.DataRozpoczecia = datePickerStart.SelectedDate;
                    query.DataZakonczenia = datePickerEnd.SelectedDate;
                    query.Cena = Convert.ToDecimal(textBoxCena.Text);
                    query.Archiwalny = checkBoxArchiwalne.IsChecked;
                    query.NumerPolisy = textBoxPolisa.Text;
                    query.ID_INSURANCE_COMPANY_fk = ID_ICF;
                    baza.SubmitChanges();
                }
            }
            
        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
