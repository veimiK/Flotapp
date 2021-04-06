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
    /// Logika interakcji dla klasy InsuranceCompaniesWindow.xaml
    /// </summary>
    public partial class InsuranceCompaniesWindow : Window
    {
        DataClasses1DataContext baza = new DataClasses1DataContext();
        public InsuranceCompaniesWindow()
        {
            InitializeComponent();
            Load();

        }
        void Load()
        {
            DataClasses1DataContext baza = new DataClasses1DataContext();
            gridInsuranceCompanies.ItemsSource = null;
            gridInsuranceCompanies.ItemsSource = baza.Ubezpieczyciele;

        }
        private void buttonAddInsuranceCompany_Click(object sender, RoutedEventArgs e)
        {
            AddInsuranceCompanyWindow _instance = new AddInsuranceCompanyWindow();
            _instance.Show();
        }

        private void buttonEditInsuranceCompany_Click(object sender, RoutedEventArgs e)
        {
            if (gridInsuranceCompanies.SelectedIndex == -1)
            {
                MessageBox.Show("Zaznacz wiersz!");
            }
            else
            {
            try
            {
                Ubezpieczyciele ins = gridInsuranceCompanies.SelectedItem as Ubezpieczyciele;
                EditInsuranceCompanyWindow _instance = new EditInsuranceCompanyWindow(ins);
                _instance.Show();
            }
            catch
            {
                MessageBox.Show("Wystąpił błąd podczas wczytywania.");
            }
            }

        }

        private void buttonDeleteInsuranceCompany_Click(object sender, RoutedEventArgs e)
        {
            if (gridInsuranceCompanies.SelectedIndex == -1) { MessageBox.Show("Zaznacz wiersz!"); }
            else
            {
                MessageBoxResult result = MessageBox.Show("Czy jesteś pewien usunięcia danego rekordu?", "Pytanie", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    int final = 0;
                    try
                    {
                        Ubezpieczyciele ins = gridInsuranceCompanies.SelectedItem as Ubezpieczyciele;
                        final = ins.ID_INSURANCE_COMPANY;

                    }
                    catch { MessageBox.Show("Zaznacz wiersz!"); }


                    // Usuwanie rekordu z bazy
                    var query = (from p in baza.Ubezpieczyciele
                                 where p.ID_INSURANCE_COMPANY == final
                                 select p).FirstOrDefault();
                    if (query != null)
                    {
                        baza.Ubezpieczyciele.DeleteOnSubmit(query);
                        baza.SubmitChanges();
                        Load();
                    }
                }
            }
        }

        private void buttonRefresh_Click(object sender, RoutedEventArgs e)
        {
            Load();
        }

        private void buttonStats_Click(object sender, RoutedEventArgs e)
        {
            if (gridInsuranceCompanies.SelectedIndex == -1)
            {
                MessageBox.Show("Zaznacz wiersz!");
            }
            else
            {

                try
                {
                    Ubezpieczyciele insurance = gridInsuranceCompanies.SelectedItem as Ubezpieczyciele;
                    int insuranceID = insurance.ID_INSURANCE_COMPANY;
                    var query = (from p in baza.Ubezpieczenia
                                 where insuranceID == p.ID_INSURANCE_COMPANY_fk
                                 select p).ToList();
                    decimal PLN = 0;
                    int numberOverall = 0;
                    int numberActive = 0;

                    foreach (var z in query)
                    {
                        PLN = PLN + (decimal)z.Cena;
                        numberActive = numberActive + 1;
                    }
                    var query2 = (from k in baza.Ubezpieczenia
                                  where insuranceID == k.ID_INSURANCE_COMPANY_fk && (bool)k.Archiwalny == true
                                  select k).ToList();
                    foreach (var z in query)
                    {
                        numberOverall = numberOverall + 1;
                    }
                    if (query != null)
                    {
                        MessageBox.Show("Liczba zakupionych ubezpieczeń: " + numberOverall + Environment.NewLine +
                                        "Liczba ubezpieczonych samochodów(teraz): " + numberActive + Environment.NewLine +
                                        "Wydane złotówki w danej firmie: " + PLN);
                    }
                }
                catch
                {
                    MessageBox.Show("Wystąpił błąd");
                }

            }
        }
    }
}