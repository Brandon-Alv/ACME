using System;
using System.Net.Mail;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACME.Models
{
    public class EnviarCorreo
    {

        public void EnviarToken(String Correo, int token)
        {
            String Usuario, contraseña, destinatario, asunto,mensaje;

            Usuario = "acmeempresa2019@gmail.com";
            contraseña = "ACME2019";
            destinatario = Correo;
            asunto = "Validacion de Cuenta";
            mensaje = token.ToString();
            

            MailMessage enviar = new MailMessage(Usuario,destinatario,asunto,mensaje);
            SmtpClient servidor = new SmtpClient("smtp.gmail.com");
            NetworkCredential credenciales = new NetworkCredential(Usuario,contraseña);
            servidor.Credentials = credenciales;
            servidor.EnableSsl = true; 

            try
            {
                servidor.Send(enviar);
                enviar.Dispose();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            

        }

        public void EnviarToken2(String Correo, int token)
        {
            String Usuario, contraseña, destinatario, asunto, mensaje;

            Usuario = "acmeempresa2019@gmail.com";
            contraseña = "ACME2019";
            destinatario = Correo;
            asunto = "Validacion de Cuenta";
            mensaje = token.ToString();


            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress(Correo));
            email.From = new MailAddress(Usuario);
            email.Subject = asunto;
            email.Body = mensaje;
            email.IsBodyHtml = true;
            email.Priority = MailPriority.Normal;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 25;
            smtp.EnableSsl = false;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(Usuario, contraseña);

            try
            {
                smtp.Send(email);
                email.Dispose();
               
            }
            catch (Exception ex)
            {
               
            }




        }
    }
}
