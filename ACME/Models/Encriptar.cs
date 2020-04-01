using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACME.Models
{
    public class Encriptar
    {
        public string encrip (String cadena)
        {
            string result = string.Empty;
            byte[] encryted = System.Text.ASCIIEncoding.Unicode.GetBytes(cadena);
            result = Convert.ToBase64String(encryted);

            return result;
        }

        public string desencrip(String cadena)
        {
            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(cadena);
            result = System.Text.ASCIIEncoding.Unicode.GetString(decryted);
            return result;


        }
    }

   
}
