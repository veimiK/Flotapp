using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Flotapp
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static DataClasses1DataContext baza = new DataClasses1DataContext();
        public MainWindow()
        {   
            InitializeComponent();
            Dni();
            Load();
        }
        private void buttonAddCar_Click(object sender, RoutedEventArgs e)
        {
            AddCarWindow instance = new AddCarWindow();
            instance.Show();
        }

        private void buttonEditCar_Click(object sender, RoutedEventArgs e)
        {
            if (gridCar.SelectedIndex == -1)
            {
                MessageBox.Show("Zaznacz wiersz!");
            }
            else
            {
                try
                {
                    Samochody sam = gridCar.SelectedItem as Samochody;
                    EditCarWindow instance = new EditCarWindow(sam);
                    instance.Show();

                }
                catch { MessageBox.Show("Wystąpił błąd."); }
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            //var cellInfos = gridCar.SelectedCells;
            //var lista = new List<string>();
            //foreach (DataGridCellInfo cellInfo in cellInfos)
            //{
            //    var content = cellInfo.Column.GetCellContent(cellInfo.Item);
            //    var row = (DataRowView)content.DataContext;
            //    object[] obj = row.Row.ItemArray;
            //    lista.Add(obj[0].ToString());
            //}
            
            //MessageBox.Show("" + lista.Count);
            Dni();
            Load();
        }

        private void ButtonRemoveCar_Click(object sender, RoutedEventArgs e)
        {
            if (gridCar.SelectedIndex == -1)
            {
                MessageBox.Show("Zaznacz wiersz!");
            }
            else
            {

                MessageBoxResult result = MessageBox.Show("Czy jesteś pewien usunięcia danego rekordu?", "Pytanie", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    int final = 0;
                    try
                    {
                        Samochody sam = gridCar.SelectedItem as Samochody;
                        final = sam.ID_CAR;

                    }
                    catch { MessageBox.Show("Zaznacz wiersz!"); }


                    // Usuwanie rekordu z bazy
                    var query = (from p in baza.Samochody
                                 where p.ID_CAR == final
                                 select p).FirstOrDefault();
                    if (query != null)
                    {
                        baza.Samochody.DeleteOnSubmit(query);
                        baza.SubmitChanges();

                    }
                }
                else { }

                // Sprawdzanie czy dany samochód ma ubezpieczenie/przegląd i pytanie użytkownika, czy je też usunąć
                var sampow = from u in baza.Ubezpieczenia
                             where (u.ID_CAR_fk == 22)
                             select new
                             {
                                 u.ID_CAR_fk
                             };
                if (sampow.Any())
                {
                    if (MessageBox.Show("Usuwasz samochód który ma powiązane ubezpieczenia, czy usunąć je?", "Usuwasz Samochód z ubezpieczeniami", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        MessageBox.Show("Ubezpieczenia nie zostały usunięte");
                    }
                    else
                    {
                        foreach (var x in sampow)
                        {
                            var query = (from p in baza.Ubezpieczenia
                                         where (p.ID_CAR_fk == x.ID_CAR_fk)
                                         select p).FirstOrDefault();
                            if (query != null)
                            {
                                baza.Ubezpieczenia.DeleteOnSubmit(query);
                                baza.SubmitChanges();

                            }
                        }
                    }
                }
                Load();
            }
        }
        public void Load()
        {
            

            DataClasses1DataContext baza = new DataClasses1DataContext();
            //var query = (from s in baza.Samochody
            //             join rp in baza.RodzajePaliwa on s.ID_FUEL_fk equals rp.ID_FUEL
            //             join n in baza.Nadwozia on s.ID_BODY_fk equals n.ID_BODY
            //             join f in baza.Firmy on s.ID_COMPANY_fk equals f.ID_COMPANY
            //             orderby s.ID_CAR
            //             select new
            //             {
            //                 s.ID_CAR,
            //                 f.Firma,
            //                 s.Marka,
            //                 s.Model,
            //                 s.Rocznik,
            //                 s.Rejestracja,
            //                 s.NrDowoduRejestracyjnego,
            //                 s.DataWydaniaDowoduRejestracyjnego,
            //                 Rodzaj = n.Nazwa,
            //                 s.VIN,
            //                 s.DataRejestracji,
            //                 s.KartaPojazdu,
            //                 s.Moc,
            //                 s.Pojemnosc,
            //                 RodzajPaliwa = rp.RodzajPaliwa,
            //                 s.MasaWlasna,
            //                 s.DMC,
            //                 s.DniDoUbezpieczenia,
            //                 s.DniDoPrzegladu
            //             }).ToList();
            
            gridCar.ItemsSource = null;
            gridCar.ItemsSource = baza.Samochody;
        }

        public static void Dni()
        {
            // Obliczanie dni do końca ubezpieczenia
            
            var ubezpieczenia = from u in baza.Ubezpieczenia
                        //join s in baza.Samochody on u.ID_CAR_fk equals s.ID_CAR
                        where (u.Archiwalny == false)
                        select new
                        {
                            u.ID_CAR_fk,
                            u.DataZakonczenia,
                            //s.ID_CAR,
                            //s.DniDoUbezpieczenia
                        };
            if (ubezpieczenia != null)
            {
                foreach (var x in ubezpieczenia)
                {
                    var query = (from p in baza.Samochody
                                 where p.ID_CAR == x.ID_CAR_fk
                                 orderby p.ID_CAR
                                 select p).FirstOrDefault();
                    if (query != null)
                    {
                        TimeSpan wynik = (DateTime)x.DataZakonczenia - DateTime.Today;
                        query.DniDoUbezpieczenia = Convert.ToInt32(wynik.TotalDays);   //procedura liczenia daty  od x.DataZakonczenia
                        baza.SubmitChanges();
                    }
                }
            }

            // Obliczanie dni do końca przeglądu
            
            var przeglady = from p in baza.Przeglady
                            //join s in baza.Samochody on u.ID_CAR_fk equals s.ID_CAR
                        where (p.Archiwalny == false)
                        select new
                        {
                            p.ID_CAR_fk,
                            p.DataZakonczenia,
                        };
            if (przeglady != null)
            {
                foreach (var x in przeglady)
                {
                    var query = (from p in baza.Samochody
                                 where p.ID_CAR == x.ID_CAR_fk
                                 orderby p.ID_CAR
                                 select p).FirstOrDefault();
                    if (query != null)
                    {
                        TimeSpan wynik = (DateTime)x.DataZakonczenia - DateTime.Today;
                        query.DniDoPrzegladu = Convert.ToInt32(wynik.TotalDays);   //procedurka liczenia daty  od x.DataZakonczenia
                        baza.SubmitChanges();
                    }
                }
            }
        }

        private void ButtonInsuraces_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InsuranceWindow instance = new InsuranceWindow();
                instance.Show();
            }
            catch
            {
                MessageBox.Show("Zaznacz wiersz!");
                return;
            }
        }

        private void ButtonInspections_Click(object sender, RoutedEventArgs e)
        {
                InspectionWindow instance = new InspectionWindow();
                instance.Show();
        }

        private void ButtonInsuranceCar_Click(object sender, RoutedEventArgs e)
        {
            if (gridCar.SelectedIndex == -1)
            {
                MessageBox.Show("Zaznacz wiersz!");
            }
            else
            {
                try
                {
                    Samochody sam = gridCar.SelectedItem as Samochody;
                    AddInsuranceWindow instance = new AddInsuranceWindow(sam);
                    instance.Show();
                }
                catch
                {
                    MessageBox.Show("Wystąpił błąd.");
                }
            }
        }

        private void ButtonInspectionCar_Click(object sender, RoutedEventArgs e)
        {
            if (gridCar.SelectedIndex == -1)
            {
                MessageBox.Show("Zaznacz wiersz!");
            }
            else
            {
                try
                {
                    Samochody sam = gridCar.SelectedItem as Samochody;
                    AddInspectionWindow instance = new AddInspectionWindow(sam);
                    instance.Show();
                }
                catch
                {
                    MessageBox.Show("Wystąpił błąd.");
                }
            }
        }
        private void InspectionWindow_Closed(object sender, EventArgs e)
        {
            Load();
        }

        private void ButtonContacts_Click(object sender, RoutedEventArgs e)
        {
            ContactWindow instance = new ContactWindow();
            instance.Show();
        }

        private void buttonDictionary_Click(object sender, RoutedEventArgs e)
        {
            DictionaryWindow instance = new DictionaryWindow();
            instance.Show();
        }

        private void buttonInfoCheck_Click(object sender, RoutedEventArgs e)
        {
            if (gridCar.SelectedIndex != -1)
            {
                try
                {
                    Samochody sam = gridCar.SelectedItem as Samochody;
                    var queryFind = (from p in baza.Samochody
                                     where p.ID_CAR == sam.ID_CAR
                                     select p).FirstOrDefault();

                    var queryShow = (from s in baza.Samochody
                                     join rp in baza.RodzajePaliwa on queryFind.ID_FUEL_fk equals rp.ID_FUEL
                                     join f in baza.Firmy on queryFind.ID_COMPANY_fk equals f.ID_COMPANY
                                     join n in baza.Nadwozia on queryFind.ID_BODY_fk equals n.ID_BODY
                                     where s.ID_CAR == queryFind.ID_CAR
                                     select new { rp.RodzajPaliwa, f.Firma, n.Nazwa }).FirstOrDefault();

                    MessageBox.Show("ID Samochodu: " + queryFind.ID_CAR + Environment.NewLine +
                        "Marka: " + queryFind.Marka + Environment.NewLine +
                        "Model: " + queryFind.Model + Environment.NewLine +
                        "Rocznik: " + queryFind.Rocznik + Environment.NewLine +
                        "Rejestracja: " + queryFind.Rejestracja + Environment.NewLine +
                        "Paliwo: " + queryShow.RodzajPaliwa + Environment.NewLine +
                        "Firma: " + queryShow.Firma + Environment.NewLine +
                        "Nadwozie: " + queryShow.Nazwa + Environment.NewLine +
                        "Numer dowodu rejestracyjnego: " + queryFind.NrDowoduRejestracyjnego + Environment.NewLine +
                        "Data wydania dowodu rejestracyjnego: " + queryFind.DataWydaniaDowoduRejestracyjnego + Environment.NewLine +
                        "VIN: " + queryFind.VIN + Environment.NewLine +
                        "Data Rejestracji: " + queryFind.DataRejestracji + Environment.NewLine +
                        "Numer karty pojazdu: " + queryFind.KartaPojazdu + Environment.NewLine +
                        "Moc: " + queryFind.Moc + Environment.NewLine +
                        "Pojemność: " + queryFind.Pojemnosc + Environment.NewLine +
                        "Masa własna: " + queryFind.MasaWlasna + Environment.NewLine +
                        "Dopuszczalna masa całkowita: " + queryFind.DMC + Environment.NewLine +
                        "Dni do końca ubezpieczenia: " + queryFind.DniDoUbezpieczenia + Environment.NewLine +
                        "Dni do końca przeglądu: " + queryFind.DniDoPrzegladu + Environment.NewLine,
                        "Informacje o " + queryFind.Marka + " " + queryFind.Model + " " + queryFind.Rocznik);
                }
                catch
                {
                    MessageBox.Show("Wystąpił błąd podczas wczytywania.");
                }
            }
            else
            {
                MessageBox.Show("Zaznacz wiersz!");
            }
        }
    }
}
