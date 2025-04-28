using System.Security.Cryptography;
using System.Text;

namespace Projekat_A_Prodavnica_racunarske_opreme.Util
{
    public class PasswordHash
    {
        //ovo mi pravi hash koji cu unijeti u bazu za svakog korisnika
        public static string HashPassword(string password)
        {
            //treba dodati salt ali trebam onda to cuvati u bazi, a ne ispravlja mi se baza
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytePassword = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(bytePassword);

                StringBuilder hexString = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hexString.Append(b.ToString("x2"));
                }
                return hexString.ToString();
            }
        }
    }
}
