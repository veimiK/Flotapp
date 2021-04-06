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
    /// Logika interakcji dla klasy AddInspectionWindow.xaml
    /// </summary>
    public partial class AddInspectionWindow : Window
    {
        Samochody x;
        int IdCar;
        public static DataClasses1DataContext baza = new DataClasses1DataContext();
        public AddInspectionWindow(Samochody sam)
        {
            InitializeComponent();
            x = sam;
            IdCar = sam.ID_CAR;
            textBlockWybrany.Text = "Dodawanie przeglądu do " + sam.Marka + " " + sam.Model + " " + sam.Rejestracja;
            var query = (from p in baza.Warsztaty
                         orderby p.ID_INSPECTION_COMPANY
                         select p).ToList();
            foreach (var x in query)
            {
                comboBoxWarsztat.Items.Add(x.Firma);
            }
            comboBoxWarsztat.SelectedIndex = 1;
        }
        void Zapis()
        {

            var q = (from p in baza.Warsztaty
                     orderby p.ID_INSPECTION_COMPANY
                     select p).ToList();
            string[,] arrayId = new string[q.Count, 2];
            int i = 0;
            foreach (var x in q)
            {
                arrayId[i, 0] = Convert.ToString(x.ID_INSPECTION_COMPANY);
                arrayId[i, 1] = x.Firma;
                i++;
            }
            int ID_ICF = Convert.ToInt32(arrayId[comboBoxWarsztat.SelectedIndex, 0]);

            if (checkBoxArchiwalne.IsChecked == false)
            {
                var query = (from p in baza.Przeglady
                             where p.ID_CAR_fk == IdCar
                             orderby p.ID_INSPECTION
                             select p).ToList();
                foreach (var x in query)
                    x.Archiwalny = true;
                baza.SubmitChanges();

                Przeglady prz2 = new Przeglady
                {
                    DataRozpoczecia = datePickerStart.SelectedDate,
                    DataZakonczenia = datePickerEnd.SelectedDate,
                    ID_CAR_fk = IdCar,
                    Archiwalny = checkBoxArchiwalne.IsChecked,
                    ID_INSPECTION_COMPANY_fk = ID_ICF
                };
                baza.Przeglady.InsertOnSubmit(prz2);
                baza.SubmitChanges();
                MessageBox.Show("Pomyślnie dodano przegląd i ustawiono go jako niearchiwalny (trwający).");
            }
            else
            {
                Przeglady prz = new Przeglady
                {
                    DataRozpoczecia = datePickerStart.SelectedDate,
                    DataZakonczenia = datePickerEnd.SelectedDate,
                    ID_CAR_fk = IdCar,
                    Archiwalny = true,
                    ID_INSPECTION_COMPANY_fk = ID_ICF,
                };
                baza.Przeglady.InsertOnSubmit(prz);
                baza.SubmitChanges();
                MessageBox.Show("Pomyślnie dodano ubezpieczenie archiwalne.");
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxWarsztat.Text == null | comboBoxWarsztat.Text == " ")
            {
                MessageBox.Show("Wprowadź firmę");
                return;
            }
            if (datePickerEnd.SelectedDate == null)
            {
                MessageBox.Show("Wprowadź datę zakończenia");
                return;
            }
            Zapis();
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}