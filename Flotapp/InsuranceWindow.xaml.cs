using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
    /// Logika interakcji dla klasy InsuranceWindow.xaml
    /// </summary>
    public partial class InsuranceWindow : Window
    {
        public static DataClasses1DataContext baza = new DataClasses1DataContext();
        public InsuranceWindow()
        {
            InitializeComponent();
            Load();
            
        }
        void Load()
        {
            /*DataClasses1DataContext baza = new DataClasses1DataContext();
            gridInsurance.ItemsSource = null;
            gridInsurance.ItemsSource = baza.Ubezpieczenia;*/
            var query = (
                         from p in baza.Ubezpieczenia
                         join z in baza.Ubezpieczyciele on p.ID_INSURANCE_COMPANY_fk equals z.ID_INSURANCE_COMPANY
                         orderby p.ID_INSURANCE
                         select new
                         {
                             p.ID_INSURANCE,
                             z.Firma,
                             p.DataRozpoczecia,
                             p.DataZakonczenia,
                             p.Cena,
                             p.ID_CAR_fk,
                             p.Archiwalny,
                             p.NumerPolisy,
                             p.ID_INSURANCE_COMPANY_fk
                         }).ToList();
            gridInsurance.ItemsSource = null;
            gridInsurance.ItemsSource = query;
            
            textBlockWhatCar.Text = "";
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            Load();
        }

        private void ButtonEditInsurance_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string ubezp = gridInsurance.SelectedCells[0].Item.ToString();
                string[] item = ubezp.Split(new char[] { ' ' });
                int ID_INSURANCE_grid = Convert.ToInt32(item[3].TrimEnd(new char[] { ',' }));
                EditInsuranceWindow instance = new EditInsuranceWindow(ID_INSURANCE_grid);
                instance.Show();
            }
            catch { MessageBox.Show("Zaznacz wiersz!"); }
        }

        private void ButtonRemoveInsurance_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy jesteś pewien usunięcia danego rekordu?", "Pytanie", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                int final = 0;
                try
                {
                    string ubezp = gridInsurance.SelectedCells[0].Item.ToString();
                    string[] item = ubezp.Split(new char[] { ' ' });
                    int ID_INSURANCE_grid = Convert.ToInt32(item[3].TrimEnd(new char[] { ',' }));
                    final = ID_INSURANCE_grid;
                }
                catch { MessageBox.Show("Zaznacz wiersz!"); }

                var query = (from p in baza.Ubezpieczenia
                             where p.ID_INSURANCE == final
                             select p).FirstOrDefault();
                if (query != null)
                {
                    baza.Ubezpieczenia.DeleteOnSubmit(query);
                    baza.SubmitChanges();
                    Load();
                }
            }
            else
            { }


        }

        private void ButtonFilter_Click(object sender, RoutedEventArgs e)
        {
                
            DataClasses1DataContext baza = new DataClasses1DataContext();
            gridCar.ItemsSource = null;
            gridCar.ItemsSource = baza.Samochody;
            buttonFilter.Visibility = Visibility.Hidden;
            gridCar.Visibility = Visibility.Visible;
            buttonFiltrOk.Visibility = Visibility.Visible;
            textBlockWhatCar.Text = "";

        }

        private void ButtonFiltrOk_Click(object sender, RoutedEventArgs e)
        {
            if (gridCar.SelectedIndex == -1)
            {
                MessageBox.Show("Zaznacz wiersz!");
            }
            else
            {
                DataClasses1DataContext baza = new DataClasses1DataContext();
                Samochody wybor = gridCar.SelectedItem as Samochody;
                gridCar.Visibility = Visibility.Hidden;
                buttonFiltrOk.Visibility = Visibility.Hidden;
                buttonFilter.Visibility = Visibility.Visible;
                var query = (from u in baza.Ubezpieczenia
                             join f in baza.Ubezpieczyciele on u.ID_INSURANCE_COMPANY_fk equals f.ID_INSURANCE_COMPANY
                             where u.ID_CAR_fk == wybor.ID_CAR
                             select new
                             {
                                 u.ID_INSURANCE,
                                 f.Firma,
                                 u.DataRozpoczecia,
                                 u.DataZakonczenia,
                                 u.Cena,
                                 u.NumerPolisy,
                                 u.Archiwalny
                             }).ToList();
                gridInsurance.ItemsSource = query;

                /*var query = from p in baza.Ubezpieczenia
                            orderby p.ID_INSURANCE
                            where p.ID_CAR_fk == wybor.ID_CAR
                            select new
                            {
                                ID = p.ID_INSURANCE,
                                p.Firma,
                                p.DataRozpoczecia,
                                p.DataZakonczenia,
                                p.Cena,
                                p.Archiwalny,
                            };*/

                try
                {
                    textBlockWhatCar.Text = "Wyświetlanie historii ubezpieczeń dla " + wybor.Marka + " " + wybor.Model + " " + wybor.Rocznik;
                    gridInsurance.ItemsSource = query;
                }
                catch { MessageBox.Show("Zaznacz wiersz!"); }
            }
        }

        private void buttonAddCompany_Click(object sender, RoutedEventArgs e)
        {
            InsuranceCompaniesWindow _instance = new InsuranceCompaniesWindow();
            _instance.Show();
        }

        private void buttonRefresh_Click(object sender, RoutedEventArgs e)
        {
            Load();
        }
    }
    }

