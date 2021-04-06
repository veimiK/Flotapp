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
    /// Logika interakcji dla klasy InspectionCompaniesWindow.xaml
    /// </summary>
    public partial class InspectionCompaniesWindow : Window
    {
        DataClasses1DataContext baza = new DataClasses1DataContext();
        public InspectionCompaniesWindow()
        {
            InitializeComponent();
            Load();
        }

        void Load()
        {
            DataClasses1DataContext baza = new DataClasses1DataContext();
            gridInspectionCompanies.ItemsSource = null;
            gridInspectionCompanies.ItemsSource = baza.Warsztaty;

        }

        private void buttonAddInspectionCompany_Click(object sender, RoutedEventArgs e)
        {
            AddInspectionCompanyWindow _instance = new AddInspectionCompanyWindow();
            _instance.Show();
        }


        private void buttonDeleteInspectionCompany_Click(object sender, RoutedEventArgs e)
        {
            if (gridInspectionCompanies.SelectedIndex == -1) { MessageBox.Show("Zaznacz wiersz!"); }
            else
            { 
            MessageBoxResult result = MessageBox.Show("Czy jesteś pewien usunięcia danego rekordu?", "Pytanie", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                int final = 0;
                try
                {
                    Warsztaty ins = gridInspectionCompanies.SelectedItem as Warsztaty;
                    final = ins.ID_INSPECTION_COMPANY;

                }
                catch { MessageBox.Show("Zaznacz wiersz!"); }


                // Usuwanie rekordu z bazy
                var query = (from p in baza.Warsztaty
                             where p.ID_INSPECTION_COMPANY == final
                             select p).FirstOrDefault();
                if (query != null)
                {
                    baza.Warsztaty.DeleteOnSubmit(query);
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
        private void buttonEditInspectionCompany_Click(object sender, RoutedEventArgs e)
        {
            if (gridInspectionCompanies.SelectedIndex == -1)
            {
                MessageBox.Show("Zaznacz wiersz!");
            }
            else
            {
                try
                {
                    Warsztaty ins = gridInspectionCompanies.SelectedItem as Warsztaty;
                    EditInspectionCompanyWindow _instance = new EditInspectionCompanyWindow(ins);
                    _instance.Show();
                }
                catch
                {
                    MessageBox.Show("Wystąpił błąd podczas wczytywania.");
                }
            }
        }

        private void buttonStats_Click(object sender, RoutedEventArgs e)
        {
            if (gridInspectionCompanies.SelectedIndex == -1)
            {
                MessageBox.Show("Zaznacz wiersz!");
            }
            else
            {

                try
                {
                    Warsztaty inspection = gridInspectionCompanies.SelectedItem as Warsztaty;
                    int inspectionID = inspection.ID_INSPECTION_COMPANY;
                    var query = (from p in baza.Przeglady
                                 where inspectionID == p.ID_INSPECTION_COMPANY_fk
                                 select p).ToList();
                    decimal PLN = 0;
                    int numberOverall = 0;
                    int numberActive = 0;

                    foreach (var z in query)
                    {
                        PLN = PLN + 200;
                        numberActive = numberActive + 1;
                    }
                    var query2 = (from k in baza.Przeglady
                                  where inspectionID == k.ID_INSPECTION_COMPANY_fk && (bool)k.Archiwalny == true
                                  select k).ToList();
                    foreach (var z in query)
                    {
                        numberOverall = numberOverall + 1;
                    }
                    if (query != null)
                    {
                        MessageBox.Show("Liczba zakupionych przeglądów: " + numberOverall + Environment.NewLine +
                                        "Liczba sprawdzonych samochodów(teraz): " + numberActive + Environment.NewLine +
                                        "Wydane złotówki w danym warsztacie: " + PLN);
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
