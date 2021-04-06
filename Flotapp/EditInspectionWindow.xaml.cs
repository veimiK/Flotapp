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
    /// Logika interakcji dla klasy EditInspectionWindow.xaml
    /// </summary>
    public partial class EditInspectionWindow : Window
    {
        DataClasses1DataContext baza = new DataClasses1DataContext();
        Przeglady x = new Przeglady();
        public EditInspectionWindow(int ID_INSPECTION_grid)
        {
            var query = from p in baza.Przeglady
                        where p.ID_INSPECTION == ID_INSPECTION_grid
                        select new
                        {
                            ID = p.ID_INSPECTION,
                            p.DataRozpoczecia,
                            p.DataZakonczenia,
                            p.ID_INSPECTION_COMPANY_fk,
                            p.ID_CAR_fk,
                            p.Archiwalny,
                        };
            InitializeComponent();
            foreach (var z in query)
            {
                x.ID_INSPECTION = z.ID;
                x.DataRozpoczecia = z.DataRozpoczecia;
                x.DataZakonczenia = z.DataZakonczenia;
                x.Archiwalny = z.Archiwalny;
                x.ID_CAR_fk = z.ID_CAR_fk;
                x.ID_INSPECTION_COMPANY_fk = z.ID_INSPECTION_COMPANY_fk;
            }
            Load();
        }

        void Load()
        {
            /*var query = from p in baza.Przeglady
                        where p.ID_INSPECTION == ID
                        orderby p.ID_INSPECTION
                        select new
                        {
                            ID = p.ID_INSPECTION,
                            p.Warsztat,
                            p.DataRozpoczecia,
                            p.DataZakonczenia
                            
                        };
            foreach (var x in query)
            {
                comboBoxWarsztat.Text = x.Warsztat;
                datePickerStart.SelectedDate = (DateTime)x.DataRozpoczecia;
                datePickerEnd.SelectedDate = (DateTime)x.DataZakonczenia;
            }*/
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
            //wczytywanie warsztatów do comboBox
            var query = (from p in baza.Warsztaty
                         orderby p.ID_INSPECTION_COMPANY
                         select p).ToList();
            foreach (var x in query)
            {
                comboBoxWarsztat.Items.Add(x.Firma);
            }

            var firma = (from p in baza.Warsztaty
                         where p.ID_INSPECTION_COMPANY == x.ID_INSPECTION_COMPANY_fk
                         select p).First();
                comboBoxWarsztat.Text = firma.Firma.ToString();
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

            if (checkBoxArchiwalne.IsChecked == true)
            {
                var query = (from p in baza.Przeglady
                             where p.ID_INSPECTION == x.ID_INSPECTION
                             orderby p.ID_INSPECTION
                             select p).FirstOrDefault();
                query.DataRozpoczecia = datePickerStart.SelectedDate;
                query.DataZakonczenia = datePickerEnd.SelectedDate;
                query.Archiwalny = checkBoxArchiwalne.IsChecked;
                query.ID_INSPECTION_COMPANY_fk = ID_ICF;
                baza.SubmitChanges();
                }
                else
                {
                if (MessageBox.Show("Ustawiasz ten przegląd jako trwający, niearchiwizowany. Zmieni to archiwizację innych przeglądów dla tego samochodu na archiwizowane. Czy chcesz kontynuować?", "Ustawianie archiwizacji", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    var query2 = (from p in baza.Przeglady
                                  where p.ID_CAR_fk == x.ID_CAR_fk
                                  orderby p.ID_INSPECTION
                                  select p).ToList();
                    foreach (var x in query2)
                        x.Archiwalny = true;
                    baza.SubmitChanges();

                    var query = (from p in baza.Przeglady
                                 where p.ID_INSPECTION == x.ID_INSPECTION
                                 orderby p.ID_INSPECTION
                                 select p).FirstOrDefault();
                    query.DataRozpoczecia = datePickerStart.SelectedDate;
                    query.DataZakonczenia = datePickerEnd.SelectedDate;
                    query.Archiwalny = checkBoxArchiwalne.IsChecked;
                    query.ID_INSPECTION_COMPANY_fk = ID_ICF;
                    baza.SubmitChanges();
                }
                else
                {
                    MessageBox.Show("Wartości nie zostały zmienione");
                    return;
                }
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
