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
    /// Logika interakcji dla klasy InspectionWindow.xaml
    /// </summary>
    public partial class InspectionWindow : Window
    {
        public static DataClasses1DataContext baza = new DataClasses1DataContext();
        public InspectionWindow()
        {
            InitializeComponent();
            Load();
        }
        public void Load()
        {
            /*var query = from p in baza.Przeglady
                        orderby p.ID_INSPECTION
                        select new
                        {
                            ID = p.ID_INSPECTION,
                            p.Warsztat,
                            p.DataRozpoczecia,
                            p.DataZakonczenia,
                        };*/
            var query = (
                         from p in baza.Przeglady
                         join z in baza.Warsztaty on p.ID_INSPECTION_COMPANY_fk equals z.ID_INSPECTION_COMPANY
                         orderby p.ID_INSPECTION
                         select new
                         {
                             p.ID_INSPECTION,
                             z.Firma,
                             p.DataRozpoczecia,
                             p.DataZakonczenia,
                             p.ID_CAR_fk,
                             p.Archiwalny,
                             p.ID_INSPECTION_COMPANY_fk
                         }).ToList();
            gridInspection.ItemsSource = null;
            gridInspection.ItemsSource = query;

            textBlockWhatCar.Text = "";
        }

        private void ButtonEditInspection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string inspect = gridInspection.SelectedCells[0].Item.ToString();
                string[] item = inspect.Split(new char[] { ' ' });
                int ID_INSPECTION_grid = Convert.ToInt32(item[3].TrimEnd(new char[] { ',' }));
                EditInspectionWindow instance = new EditInspectionWindow(ID_INSPECTION_grid);
                instance.Show();
            }
            catch { MessageBox.Show("Zaznacz wiersz!"); }
        }

        private void ButtonRemoveInspection_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy jesteś pewien usunięcia danego rekordu?", "Pytanie", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                int final = 0;
                try
                {
                    string inspect = gridInspection.SelectedCells[0].Item.ToString();
                    string[] item = inspect.Split(new char[] { ' ' });
                    int ID_INSPECTION_grid = Convert.ToInt32(item[3].TrimEnd(new char[] { ',' }));
                    final = ID_INSPECTION_grid;
                }
                catch { MessageBox.Show("Zaznacz wiersz!"); }

                var query = (from p in baza.Przeglady
                             where p.ID_INSPECTION == final
                             select p).FirstOrDefault();
                if (query != null)
                {
                    baza.Przeglady.DeleteOnSubmit(query);
                    baza.SubmitChanges();
                    Load();
                }
            }
            else
            { }
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            Load();
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

        private void ButtonFilterOk_Click(object sender, RoutedEventArgs e)
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
                var query = (from u in baza.Przeglady
                             join f in baza.Warsztaty on u.ID_INSPECTION_COMPANY_fk equals f.ID_INSPECTION_COMPANY
                             where u.ID_CAR_fk == wybor.ID_CAR
                             select new
                             {
                                 u.ID_INSPECTION,
                                 f.Firma,
                                 u.DataRozpoczecia,
                                 u.DataZakonczenia,
                                 u.Archiwalny
                             }).ToList();
                gridInspection.ItemsSource = query;
                /*var query = from p in baza.Przeglady
                            orderby p.ID_INSPECTION
                            where p.ID_CAR_fk == wybor.ID_CAR
                            select new
                            {
                                ID = p.ID_INSPECTION,
                                p.Warsztat,
                                p.DataRozpoczecia,
                                p.DataZakonczenia,
                                p.Archiwalny,
                            };*/
                gridInspection.ItemsSource = query;
                textBlockWhatCar.Text = "Wyświetlanie historii przeglądów dla " + wybor.Marka + " " + wybor.Model + " " + wybor.Rocznik;
            }
        }

        private void buttonAddInspectionCompany_Click(object sender, RoutedEventArgs e)
        {
            InspectionCompaniesWindow _instance = new InspectionCompaniesWindow();
            _instance.Show();
        }
    }
    }

