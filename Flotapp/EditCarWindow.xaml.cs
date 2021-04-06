using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Logika interakcji dla klasy EditCarWindow.xaml
    /// </summary>
    public partial class EditCarWindow : Window
    {
        public readonly Samochody x;
        public static DataClasses1DataContext baza = new DataClasses1DataContext();
        public EditCarWindow(Samochody samochod)
        {
            /*Rodzaje Rodzaje = new Rodzaje();
            Firmy Firmy = new Firmy();
            Paliwa Paliwa = new Paliwa();
            comboBoxRodzajPaliwa.ItemsSource = Paliwa.listaPaliw;
            comboBoxFirma.ItemsSource = Firmy.listaFirm;
            comboBoxRodzaj.ItemsSource = Rodzaje.listaRodzajow;*/
            InitializeComponent();
            x = samochod;
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
            Load();
            

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
        void Load()
        {
            var query = (from p in baza.Samochody
                         where p.ID_CAR == x.ID_CAR
                         select p).FirstOrDefault();
            textBoxMarka.Text = query.Marka;
            textBoxModel.Text = query.Model;
            textBoxRocznik.Text = query.Rocznik;
            textBoxRejestracja.Text = query.Rejestracja;
            textBoxNrDR.Text = query.NrDowoduRejestracyjnego;
            datePickerDataWydaniaDR.SelectedDate = (DateTime)query.DataWydaniaDowoduRejestracyjnego;
            textBoxVIN.Text = query.VIN;
            datePickerDataPierwszejRej.SelectedDate = (DateTime)query.DataRejestracji;
            textBoxNrKartyPojazdu.Text = query.KartaPojazdu;
            textBoxMoc.Text = Convert.ToString(query.Moc);
            textBoxPojemnosc.Text = Convert.ToString(query.Pojemnosc);
            textboxMasaWlasna.Text = Convert.ToString(query.MasaWlasna);
            textBoxDMC.Text = Convert.ToString(query.DMC);

         var queryFirma = (from p in baza.Firmy
                              where p.ID_COMPANY == query.ID_COMPANY_fk
                              select p).FirstOrDefault();
         comboBoxFirma.Text = queryFirma.Firma;

         var queryPaliwo = (from p in baza.RodzajePaliwa
                               where p.ID_FUEL == query.ID_FUEL_fk
                               select p).FirstOrDefault();
         comboBoxRodzajPaliwa.Text = queryPaliwo.RodzajPaliwa;


         var queryNadwozie = (from p in baza.Nadwozia
                                 where p.ID_BODY == query.ID_BODY_fk
                                 select p).FirstOrDefault();
         comboBoxRodzaj.Text = queryNadwozie.Nazwa;

        }

        void CheckLength()
        {
            if (textBoxMarka.Text.Length > 50)
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
            if (textBoxRocznik.Text.Length != 4)
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
        }

        void Zapis()
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
            //var queryTypy2 = (from p in baza.Nadwozia
            //                 orderby p.ID_BODY
            //                 select p).ToList();
            //string[,] arrayIdTypy = new string[queryTypy2.Count, 2];
            //int i1 = 0;
            //foreach (var x in queryTypy2)
            //{
            //    arrayIdTypy[i1, 0] = Convert.ToString(x.ID_BODY);
            //    arrayIdTypy[i1, 1] = x.Nazwa;
            //    i1++;
            //}
            //int ID_T1 = Convert.ToInt32(arrayIdTypy[comboBoxRodzaj.SelectedIndex, 0]);

            //var queryFirmy = (from p in baza.Firmy
            //                  orderby p.ID_COMPANY
            //                  select p).ToList();
            //string[,] arrayIdFirmy = new string[queryFirmy.Count, 2];
            //int i2 = 0;
            //foreach (var x in queryFirmy)
            //{
            //    arrayIdFirmy[i2, 0] = Convert.ToString(x.ID_COMPANY);
            //    arrayIdFirmy[i2, 1] = x.Firma;
            //    i2++;
            //}
            //int ID_F = Convert.ToInt32(arrayIdFirmy[comboBoxFirma.SelectedIndex, 0]);

            //var queryRodzajPaliwa = (from p in baza.RodzajePaliwa
            //                         orderby p.ID_FUEL
            //                         select p).ToList();
            //string[,] arrayId = new string[queryRodzajPaliwa.Count, 2];
            //int i3 = 0;
            //foreach (var x in queryRodzajPaliwa)
            //{
            //    arrayId[i3, 0] = Convert.ToString(x.ID_FUEL);
            //    arrayId[i3, 1] = x.RodzajPaliwa;
            //    i3++;
            //}
            //int ID_RP = Convert.ToInt32(arrayId[comboBoxRodzajPaliwa.SelectedIndex, 0]);

            var query = (from p in baza.Samochody
                         where p.ID_CAR == x.ID_CAR
                         orderby p.ID_CAR
                         select p).FirstOrDefault();
            query.Marka = textBoxMarka.Text;
            query.Model = textBoxModel.Text;
            query.Rocznik = textBoxRocznik.Text;
            query.Rejestracja = textBoxRejestracja.Text;
            query.NrDowoduRejestracyjnego = textBoxNrDR.Text;
            query.DataWydaniaDowoduRejestracyjnego = datePickerDataWydaniaDR.SelectedDate;
            query.VIN = textBoxVIN.Text;
            query.DataRejestracji = datePickerDataPierwszejRej.SelectedDate;
            query.KartaPojazdu = textBoxNrKartyPojazdu.Text;
            query.Moc = Convert.ToInt32(textBoxMoc.Text);
            query.Pojemnosc = Convert.ToInt32(textBoxPojemnosc.Text);
            query.MasaWlasna = Convert.ToInt32(textboxMasaWlasna.Text);
            query.DMC = Convert.ToInt32(textBoxDMC.Text);
            query.ID_BODY_fk = ID_T;
            query.ID_COMPANY_fk = ID_F;
            query.ID_FUEL_fk = ID_RP;

            baza.SubmitChanges();
            MessageBox.Show("Pomyślnie zapisano.");
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            CheckLength();
            Zapis();
            this.Close();
            
            }
        }
    }

