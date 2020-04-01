using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ACME.Models;
using System.Configuration;
using System.Data.SqlClient;



namespace ACME.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        
        
        public JsonResult GetCiudades(int idPais)
        {
            Procesos p = new Procesos();
            List<Ciudad> listaCiudades = new List<Ciudad>();
            listaCiudades = p.ObtenerCiudad(idPais);

            //  listaCiudades.Add(new Ciudad { IdCiudad = 1, NombreCiudad = "prueba", IdPais = 1});

            

           

            return Json(listaCiudades);
            
        }

        public JsonResult GetDistritos(int idCiudad, int idPais)
        {
            Procesos p = new Procesos();
            List<Distrito> listaDistritos = new List<Distrito>();
            listaDistritos = p.ObtenerDistrito(idCiudad,idPais);

            //  listaCiudades.Add(new Ciudad { IdCiudad = 1, NombreCiudad = "prueba", IdPais = 1});





            return Json(listaDistritos);

        }



        public ActionResult Registro()
        {
            Procesos p = new Procesos();
            List<Pais> listaPaises = new List<Pais>();
            listaPaises = p.ObtenerPais();
           

            
            ViewBag.paises = listaPaises;
            
            
            return View();
                
        }

        public ActionResult Token()
        {
            return View();
        }

       

        public ActionResult postregistro()
        {
            Encriptar e = new Encriptar();

            string nombre = e.encrip(Request.Form["name"].ToString());
            string apellido = e.encrip(Request.Form["lastname"].ToString());
            string email = e.encrip(Request.Form["email"].ToString());
            string pws = e.encrip(Request.Form["psw"].ToString());
            string pregunta = e.encrip(Request.Form["pregunta"].ToString());
            string respuesta = e.encrip(Request.Form["respuesta"].ToString());
            string pregunta2 = e.encrip(Request.Form["pregunta2"].ToString());
            string respuesta2 = e.encrip(Request.Form["respuesta2"].ToString());
            string pregunta3 = e.encrip(Request.Form["pregunta3"].ToString());
            string respuesta3 = e.encrip(Request.Form["respuesta3"].ToString());

            Procesos pr = new Procesos();
            
            pr.Registrar(nombre, apellido, email);
            pr.setearcontraseña(pws, email);
            pr.token0(email);
            pr.bloqueoinicio(email);
            pr.preguntas(email, pregunta, respuesta);
            pr.preguntas(email, pregunta2, respuesta2);
            pr.preguntas(email, pregunta3, respuesta3);

            return View();
        }

        public ActionResult Login()
        {

            return View();
        }   

        public ActionResult postLogin()
        {
            Encriptar e = new Encriptar();


            string correo = e.encrip(Request.Form["correo"].ToString());
            string contraseña = e.encrip(Request.Form["contraseña"].ToString());
            ViewData["correo"] = e.desencrip(correo);
            Procesos pr = new Procesos();
            pr.Login(correo, contraseña);
            
            

            if (pr.log == "entro")
            {
                pr.quitarbloqueo(correo);
                return View("postToken");
            }

            if (pr.log == "bloqueado") //esto
            {
                return View("Login"); // aca tiene que estar la pagina de bloqueado
            }
            else
            {
                return View("Login");
            }

            
        }

        public ActionResult postToken()
        {
            Encriptar e = new Encriptar();
            string correo1 = Request.Form["correo"].ToString();
            string correo = e.encrip(Request.Form["correo"].ToString());
            string Token = e.encrip(Request.Form["token"].ToString());

            Procesos pr = new Procesos();
            pr.ValidarToken(Token, correo);

            if (pr.val == "Correcto")
            {
                return View("Index");
            }
            else
            {
                return View("postToken");
            }

        }

        public ActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult Opciones()
        {
            return View();
        }

        public ActionResult postOpciones()
        {
            Encriptar e = new Encriptar();
            string correo = e.encrip(Request.Form["correo"].ToString());
            ViewData["correoRecuperacion"] = e.desencrip(correo);
            Procesos pr = new Procesos();
            pr.GenerarToken(e.desencrip(correo));
            return View("RecuperacionToken");

        }


        public ActionResult RecuperacionToken()
        {
            return View();

        }

        public ActionResult postRecuperacionToken()
        {
            Encriptar e = new Encriptar();
            string correo1 = Request.Form["correo"].ToString();
            string correo =e.encrip(Request.Form["correo"].ToString());
            string token =e.encrip(Request.Form["token"].ToString());

            Procesos pr = new Procesos();
            pr.ValidarToken(token, correo);

            if (pr.val == "Correcto")
            {
                return View("cambiarpass");
            }
            else
            {
                return View("RecuperacionToken");
            }
        }

        public ActionResult cambiarpass()
        {
            return View();
        }


        public ActionResult postcambiarpass()
        {
            Encriptar e = new Encriptar();
            string correo = e.encrip(Request.Form["correo"].ToString());
            string contra = e.encrip(Request.Form["contra"].ToString());
            string contra1 = e.encrip(Request.Form["contra2"].ToString());

            if (e.desencrip(contra) == e.desencrip(contra1))
            {
                Procesos pr = new Procesos();
                pr.newpass(correo, contra);
                pr.quitarbloqueo(correo);
                return View("Index");
            }
            else
            {
                return View(); //si no son iguales puede ir a otra pagina
            }
        }
    }
}
