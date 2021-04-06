using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FlotappService
{
    class Program
    {
        static void Main(string[] args)
        {
            DniDoPrzegladu();
            DniDoUbezpieczenia();
        }
        public static void DniDoPrzegladu()
        {
            
            // Obliczanie dni do końca przeglądu
            DataClasses1DataContext baza = new DataClasses1DataContext();
            var przeglady = from p in baza.Przeglady
                            join s in baza.Samochody on p.ID_CAR_fk equals s.ID_CAR
                            where (p.Archiwalny == false)
                            select new
                            {
                                p.ID_CAR_fk,
                                p.DataZakonczenia,
                                s.Marka,
                                s.Model,
                                s.Rocznik,
                                s.Rejestracja
                            };
                foreach (var x in przeglady)
                {
                    var query = (from p in baza.Samochody
                                 where p.ID_CAR == x.ID_CAR_fk
                                 orderby p.ID_CAR
                                 select p).FirstOrDefault();
                    TimeSpan wynik = (DateTime)x.DataZakonczenia - DateTime.Today;
                    query.DniDoPrzegladu = Convert.ToInt32(wynik.TotalDays);   //procedurka liczenia daty  od x.DataZakonczenia
                    baza.SubmitChanges();
                    if (query.DniDoPrzegladu <= 10)
                {
                    var message = new MailMessage();

                    string DataWydaniaDR = Convert.ToString(query.DataWydaniaDowoduRejestracyjnego);
                    DataWydaniaDR = DataWydaniaDR.Substring(0, DataWydaniaDR.Length - 9);

                    string DataRejestracji = Convert.ToString(query.DataRejestracji);
                    DataRejestracji = DataRejestracji.Substring(0, DataRejestracji.Length - 9);

                    var queryKontakty = from k in baza.Kontakty
                                        select new
                                        {
                                            k.Email
                                        };
                    foreach (var qK in queryKontakty)
                    {
                        message.To.Add(qK.Email);
                    }

                    message.IsBodyHtml = true;
                    message.From = new MailAddress("info@starpol.pl", "Powiadamiacz");
                    message.Subject = "Bliski termin przeglądu dla " + query.Rocznik + " " + query.Marka + " " + query.Model + " " + query.Rejestracja;
                    message.Body = "Zbliża się koniec ubezpieczenia dla auta firmy STARPOL " + query.Firma + "<br><br>" +
                                        "Pełne dane auta:" + "<br>" +
                                        "Marka: " + query.Marka + "<br>" +
                                        "Model: " + query.Model + "<br>" +
                                        "Rocznik: " + query.Rocznik + "<br>" +
                                        "Rejestracja: " + query.Rejestracja + "<br>" +
                                        "Numer dowodu rejestracyjnego: " + query.NrDowoduRejestracyjnego + "<br>" +
                                        "Data wydania dowodu rejestracyjnego: " + DataWydaniaDR + "<br>" +
                                        "Rodzaj: " + query.Rodzaj + "<br>" +
                                        "VIN: " + query.VIN + "<br>" +
                                        "Data rejestracji " + DataRejestracji + "<br>" +
                                        "Numer karty pojazdu: " + query.KartaPojazdu + "<br>" +
                                        "Moc: " + query.Moc + "<br>" +
                                        "Pojemność: " + query.Pojemnosc + "<br>" +
                                        "Rodzaj paliwa: " + query.RodzajPaliwa + "<br>" +
                                        "Masa własna: " + query.MasaWlasna + "<br>" +
                                        "Dopuszczalna masa całkowita: " + query.DMC + "<br>" +
                                        "Pozostałe dni do końca ubezpieczenia: " + query.DniDoUbezpieczenia + "<br>" +
                                        "<b>Pozostałe dni do końca przeglądu: " + query.DniDoPrzegladu + "</b><br><br>" +
                                        "Wiadomość wygenerowana automatycznie. Nie odpowiadaj na nią.";
                    var smtp = new SmtpClient("smtp.starpol.pl");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("infostarpol", "123qweINFO!");
                    smtp.EnableSsl = true;
                    smtp.Port = 587;
                    smtp.Send(message);
                    Console.WriteLine("Wysłano mail");
                }
                    
                }
            
        }
        public static void DniDoUbezpieczenia()
        {
            DataClasses1DataContext baza = new DataClasses1DataContext();
            int DniDoUbezpieczenia = -367;
            // Obliczanie dni do końca ubezpieczenia

            var ubezpieczenia = from u in baza.Ubezpieczenia
                                join s in baza.Samochody on u.ID_CAR_fk equals s.ID_CAR
                                where (u.Archiwalny == false)
                                select new
                                {
                                    u.ID_CAR_fk,
                                    u.DataZakonczenia,
                                    s.Marka,
                                    s.Model,
                                    s.Rocznik,
                                    s.Rejestracja,
                                   
                                };
            {
                foreach (var x in ubezpieczenia)
                {
                    var query = (from p in baza.Samochody
                                 where p.ID_CAR == x.ID_CAR_fk
                                 orderby p.ID_CAR
                                 select p).FirstOrDefault();
                    TimeSpan wynik = (DateTime)x.DataZakonczenia - DateTime.Today;
                    query.DniDoUbezpieczenia = Convert.ToInt32(wynik.TotalDays);   //procedurka liczenia daty  od x.DataZakonczenia
                    DniDoUbezpieczenia = (int)query.DniDoUbezpieczenia;
                    baza.SubmitChanges();
                    if(DniDoUbezpieczenia <= 10)
                    {
                        var message = new MailMessage();

                        string DataWydaniaDR = Convert.ToString(query.DataWydaniaDowoduRejestracyjnego);
                        DataWydaniaDR = DataWydaniaDR.Substring(0, DataWydaniaDR.Length - 9);

                        string DataRejestracji = Convert.ToString(query.DataRejestracji);
                        DataRejestracji = DataRejestracji.Substring(0, DataRejestracji.Length - 9);

                        var queryKontakty = from k in baza.Kontakty
                                            select new
                                            {
                                                k.Email
                                            };
                        foreach (var qK in queryKontakty)
                        {
                            message.To.Add(qK.Email);
                        }

                        message.IsBodyHtml = true;
                        message.From = new MailAddress("info@test.pl", "Flotapp");
                        
                        message.Subject = "Bliski koniec ubezpieczenia dla " + query.Rocznik + " " + query.Marka + " " + query.Model + " " + query.Rejestracja;
                        message.Body = "Zbliża się koniec ubezpieczenia dla auta" + query.Firma + "<br><br>" +
                                            "Pełne dane auta:" + "<br>" +
                                            "Marka: " + query.Marka + "<br>" +
                                            "Model: " + query.Model + "<br>" +
                                            "Rocznik: " + query.Rocznik + "<br>" +
                                            "Rejestracja: " + query.Rejestracja + "<br>" +
                                            "Numer dowodu rejestracyjnego: " + query.NrDowoduRejestracyjnego + "<br>" +
                                            "Data wydania dowodu rejestracyjnego: " + DataWydaniaDR + "<br>" +
                                            "Rodzaj: " + query.Rodzaj + "<br>" +
                                            "VIN: " + query.VIN + "<br>" +
                                            "Data rejestracji " + DataRejestracji + "<br>" +
                                            "Numer karty pojazdu: " + query.KartaPojazdu + "<br>" +
                                            "Moc: " + query.Moc + "<br>" +
                                            "Pojemność: " + query.Pojemnosc + "<br>" +
                                            "Rodzaj paliwa: " + query.RodzajPaliwa + "<br>" +
                                            "Masa własna: " + query.MasaWlasna + "<br>" +
                                            "Dopuszczalna masa całkowita: " + query.DMC + "<br>" +
                                            "<b>Pozostałe dni do końca ubezpieczenia: " + query.DniDoUbezpieczenia + "</b><br>" +
                                            "Pozostałe dni do końca przeglądu: " + query.DniDoPrzegladu + "<br><br>" +
                                            "Wiadomość wygenerowana automatycznie. Nie odpowiadaj na nią.";
                        var smtp = new SmtpClient("smpt.test.pl");
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential("pswd", "pswd");
                        smtp.EnableSsl = true;
                        smtp.Port = 587;
                        smtp.Send(message);
                        Console.WriteLine("Wysłano mail");
                    }
                }

            }
        }
    }
}
