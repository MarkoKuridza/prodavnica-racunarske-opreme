using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
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
using Projekat_A_Prodavnica_racunarske_opreme.Util;

namespace Projekat_A_Prodavnica_racunarske_opreme
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window, ILocalizable
    {
        public static zaposleni current;
        public LogInWindow()
        {
            InitializeComponent();
            ApplyInternationalization();
        }

        private void BtnPrijava_Click(object sender, RoutedEventArgs e)
        {           
            string username = TbIme.Text.Trim();
            string password = PbPass.Password.Trim();
            string role = "";
            try
            {
                role = UserAuthentication(username, password);
            }
            catch(InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            if (role == "Prodavac")
            {
                SellerWindow sellerWindow = new SellerWindow();
                
                sellerWindow.Show();
                this.Close();
            }
            else if (role == "Menadzer")
            {
                ManagerWindow managerWindow = new ManagerWindow();
                managerWindow.Show();
                this.Close();
            }
        }
        //ovo vraca role za ispravno uneseno korisnicko ime i lozinka
        //lozinka se provjerava na drugaciji nacin, provjerava se hash vrijednost sto znaci da moram proslijediti
        //hash vrijednost, odnosno za unijetu lozinku u plain text-u koji heshiram, to proslijedjujem na autentifikaciju
        //ali prije svega toga moram kreirati hash vrijednosti i unijeti u bazu
        public string UserAuthentication(string username, string password)
        {
            //ovdje ide hesiranje
            string hashedpassword = Projekat_A_Prodavnica_racunarske_opreme.Util.PasswordHash.HashPassword(password);
            //mozda staticki negdje kreirati ovaj objekat da ga stalno ne pravim i brisem, al o tom po tom
            using (ProdavnicaDb prodavnica = new ProdavnicaDb())
            {
                
                var zap = (
                    from c in prodavnica.zaposlenis
                    where c.Ime == username && c.Lozinka == hashedpassword
                    select c).FirstOrDefault();

                // ako je prazan da ne vraca null, tj da ne baca izuzetak
                // ako neko nije aktivan izbaci poruku!

                if(zap == null)
                {
                    throw new InvalidOperationException(LanguageController.Instance.ResourceManager.GetString("LogInError1", CultureInfo.CurrentCulture));
                }
                
                else if(zap.Aktivan != 1)
                {
                    throw new InvalidOperationException(LanguageController.Instance.ResourceManager.GetString("LogInError2", CultureInfo.CurrentCulture));
                }
                else
                {
                    current = new zaposleni();
                    current = zap;
                    return zap.Uloga;
                }
                
            }
        }

        private void English_Click(object sender, RoutedEventArgs e)
        {
            LanguageController.Instance.ChangeLanguage("en");
        }

        private void Serbian_Click(object sender, RoutedEventArgs e)
        {
            LanguageController.Instance.ChangeLanguage("sr");
        }
        

        public void ApplyInternationalization()
        {
            Title = LanguageController.Instance.ResourceManager.GetString("Title_LogInWindow", CultureInfo.CurrentCulture);
            LbUser.Content = LanguageController.Instance.ResourceManager.GetString("LbUser", CultureInfo.CurrentCulture);
            LbPass.Content = LanguageController.Instance.ResourceManager.GetString("LbPass", CultureInfo.CurrentCulture);
            BtnLogIn.Content = LanguageController.Instance.ResourceManager.GetString("BtnLogIn", CultureInfo.CurrentCulture);
            MenuItemLang.Header = LanguageController.Instance.ResourceManager.GetString("MenuLang", CultureInfo.CurrentCulture);
            MenuItemEn.Header = LanguageController.Instance.ResourceManager.GetString("MenuLangEn", CultureInfo.CurrentCulture);
            MenuItemSr.Header = LanguageController.Instance.ResourceManager.GetString("MenuLangSr", CultureInfo.CurrentCulture);
            LbTitle.Content = LanguageController.Instance.ResourceManager.GetString("LbTitle", CultureInfo.CurrentCulture);
        } 
    }
}
