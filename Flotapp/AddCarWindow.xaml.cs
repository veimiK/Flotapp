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
using System.Text.RegularExpressions;

namespace Flotapp
{
    /// <summary>
    /// Logika interakcji dla klasy AddEditCarWindow.xaml
    /// </summary>
    public partial class AddCarWindow : Window
    {
        public static DataClasses1DataContext baza = new DataClasses1DataContext();
        public AddCarWindow()
        {
            /*Rodzaje Rodzaje = new Rodzaje();
            Firmyf Firmy = new Firmyf();
            Paliwa Paliwa = new Paliwa();
            
            comboBoxRodzaj.ItemsSource = Rodzaje.listaRodzajow;
            comboBoxFirma.ItemsSource = Firmy.listaFirm;
            comboBoxRodzajPaliwa.ItemsSource = Paliwa.listaPaliw;
            textBoxVIN.Text = "12345678901234567";*/
            InitializeComponent();

            var queryRodzaje = (from p in baza.Nadwozia
                                orderby p.ID_BODY
                                select p).ToList();
            foreach (var x in queryRodzaje)
            {
                comboBoxRodzaj.Items.Add(x.Nazwa);
            }

            var queryFirmy = (from p in baza.Firmy
                              orderby p.ID_COMPANY
                              select p).ToList();
            foreach (var x in queryFirmy)
            {
                comboBoxFirma.Items.Add(x.Firma);
            }

            var queryPaliwa = (from p in baza.RodzajePaliwa
                               orderby p.ID_FUEL
                               select p).ToList();
            foreach (var x in queryPaliwa)
            {
                comboBoxRodzajPaliwa.Items.Add(x.RodzajPaliwa);
            }
            comboBoxFirma.SelectedIndex = 0;
            comboBoxRodzaj.SelectedIndex = 0;
            comboBoxRodzajPaliwa.SelectedIndex = 0;

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
           this.Close();
        }
        void Zapis()
        {
            try
            {
                var queryTypy = (from p in baza.Nadwozia
                                 where comboBoxRodzaj.Text == p.Nazwa
                                 select p).FirstOrDefault();
                int ID_T = queryTypy.ID_BODY;

                var queryRodzajePaliwa = (from p in baza.RodzajePaliwa
                                          where comboBoxRodzajPaliwa.Text == p.RodzajPaliwa
                                          select p).FirstOrDefault();
                int ID_RP = queryRodzajePaliwa.ID_FUEL;

                var queryFirma = (from p in baza.Firmy
                                  where comboBoxFirma.Text == p.Firma
                                  select p).FirstOrDefault();
                int ID_F = queryFirma.ID_COMPANY;


                Samochody car = new Samochody
                {
                    Marka = textBoxMarka.Text,
                    Model = textBoxModel.Text,
                    Rocznik = textBoxRocznik.Text,
                    Rejestracja = textBoxRejestracja.Text,
                    NrDowoduRejestracyjnego = textBoxNrDR.Text,
                    DataWydaniaDowoduRejestracyjnego = datePickerDataWydaniaDR.SelectedDate,
                    VIN = textBoxVIN.Text,
                    DataRejestracji = datePickerDataPierwszejRej.SelectedDate,
                    KartaPojazdu = textBoxNrKartyPojazdu.Text,
                    Moc = Convert.ToInt32(textBoxMoc.Text),
                    Pojemnosc = Convert.ToInt32(textBoxPojemnosc.Text),
                    MasaWlasna = Convert.ToInt32(textboxMasaWlasna.Text),
                    DMC = Convert.ToInt32(textBoxDMC.Text),
                    ID_COMPANY_fk = ID_F,
                    ID_FUEL_fk = ID_RP,
                    ID_BODY_fk = ID_F
                };
                baza.Samochody.InsertOnSubmit(car);
                baza.SubmitChanges();
            }
            catch
            {
                MessageBox.Show("Wprowadzono niepoprawne dane.");
            }
        }
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxMarka.Text.Length>50)
            {
                MessageBox.Show("Wprowadzono za dużą liczbę znaków w 'Marka'. Maksymalnie 50 znaków.");
                return;
            }
            if (textBoxModel.Text.Length > 50)
            {
                MessageBox.Show("Wprowadzono za duzą liczbę znaków w 'Model'. Maksymalnie 50 znaków.");
                return;
            }
            if (textBoxRejestracja.Text.Length > 25)
            {
                MessageBox.Show("Wprowadzono za dużą liczbę znaków w 'Rejestracja'. Maksymalnie 25 znaków.");
                return;
            }
            if (textBoxNrDR.Text.Length > 25)
            {
                MessageBox.Show("Wprowadzono za dużą liczbę znaków w 'Numer dowodu rejestracyjnego'. Maksymalnie 25 znaków.");
                return;
            }
            if (textBoxRocznik.Text.Length!=4)
            {
                MessageBox.Show("Wprowadzono niepoprawny rocznik.");
                return;
            }
            if (textBoxRejestracja.Text.Length > 25)
            {
                MessageBox.Show("Wprowadzono za dużą liczbę znaków w 'Rejestracja'. Maksymalnie 25 znaków.");
                return;
            }
            if (textBoxVIN.Text.Length != 17)
            {
                MessageBox.Show("Wprowadzono niepoprawna liczbę znaków w 'VIN'. Poprawna liczba to 12 lub 17 znaków.");
                return;
            }
            if (textBoxNrKartyPojazdu.Text.Length > 25)
            {
                MessageBox.Show("Wprowadzono za dużą liczbę znaków w 'Numer karty pojazdu'. Maksymalnie 30 znaków.");
                return;
            }
            Zapis();
            this.Close();
        }  
    }
}

