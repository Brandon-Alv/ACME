using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ACME.Models
{
    public class Procesos
    {
        public void ValidarToken(String token, String correo)
        {
            Encriptar e = new Encriptar();
            Conexion c = new Conexion();
            SqlConnection con = new SqlConnection();
            con = c.ObtenerConexion();

            try
            {
                using (con)
                {
                    SqlCommand cmd = new SqlCommand("LlamadaToken", con);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@correo", e.desencrip(correo));



                    SqlDataReader reader = cmd.ExecuteReader();

                    String Tokken = "";

                    while (reader.Read())
                    {
                        Tokken = reader["Token"].ToString();
                    }
                    con.Close();


                    if (e.desencrip(token).Equals(Tokken))
                    {
                        val = "Correcto";
                    }
                    else
                    {
                        val = "Incorrecto";
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String val = " ";

        public void Registrar(String nombre, String apellido, String email)
        {
            Encriptar e = new Encriptar();
            


            Conexion c = new Conexion();
            SqlConnection con = new SqlConnection();
            con = c.ObtenerConexion();

            try
            {
                using (con)
                {
                    SqlCommand cmd = new SqlCommand("AgregarUsuario", con);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@nom", e.desencrip(nombre));
                    cmd.Parameters.AddWithValue("@ap", e.desencrip(apellido));
                    cmd.Parameters.AddWithValue("@correo", e.desencrip(email));


                    cmd.ExecuteScalar();

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public void token0(String correo)
        {

            Encriptar e = new Encriptar();



            Conexion c = new Conexion();
            SqlConnection con = new SqlConnection();
            con = c.ObtenerConexion();

            try
            {
                using (con)
                {
                    SqlCommand cmd = new SqlCommand("AlmacenarToken", con);

                    cmd.CommandType = CommandType.StoredProcedure;

                    
                    cmd.Parameters.AddWithValue("@correo", e.desencrip(correo));
                    cmd.Parameters.AddWithValue("@Token", 0000);


                    cmd.ExecuteScalar();

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void setearcontraseña(String contraseña, String correo)
        {
            Encriptar e = new Encriptar();


 
            Conexion c = new Conexion();
            SqlConnection con = new SqlConnection();
            con = c.ObtenerConexion();

            try
            {
                using (con)
                {
                    SqlCommand cmd = new SqlCommand("dbo.SetearContraseña", con);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@contraseña", e.desencrip(contraseña));
                    cmd.Parameters.AddWithValue("@correo", e.desencrip(correo));



                    cmd.ExecuteScalar();

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public string[] llenarbox()
        {
            string[] preguntas = new string[10];
            int i = 0;
            Conexion c = new Conexion();
            SqlConnection con = new SqlConnection();
            con = c.ObtenerConexion();

            using (con)
            {


                SqlCommand cmd = new SqlCommand("obtenerpreguntas", con);
                cmd.CommandType = CommandType.StoredProcedure;


                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    preguntas[i] = reader["Pregunta"].ToString();
                    i++;
                }

                con.Close();
            }

            return preguntas;
        }

        public void preguntas(String correo,String pregunta, String Respuesta)
        {
            Encriptar e = new Encriptar();



            Conexion c = new Conexion();
            SqlConnection con = new SqlConnection();
            con = c.ObtenerConexion();

            try
            {
                using (con)
                {
                    SqlCommand cmd = new SqlCommand("AlmacenarRespuestas", con);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@correo", e.desencrip(correo));
                    cmd.Parameters.AddWithValue("@respuestas", e.desencrip(Respuesta));
                    cmd.Parameters.AddWithValue("@pregunta", e.desencrip(pregunta));


                    cmd.ExecuteScalar();

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Login(String correo, String contraseña)
        {
            
            Encriptar e = new Encriptar();
            EnviarCorreo ev = new EnviarCorreo();
            Conexion c = new Conexion();
            SqlConnection con = new SqlConnection();
            con = c.ObtenerConexion();

            try
            {
                using (con)
                {

                    SqlCommand cmd = new SqlCommand("LlamarUsuario", con);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@correo", e.desencrip(correo));
                    cmd.Parameters.AddWithValue("@contra", e.desencrip(contraseña));

                    SqlDataReader reader = cmd.ExecuteReader();

                    String corrreo = "";
                    String contra = "";

                    while (reader.Read())
                    {
                        corrreo = reader["Correo"].ToString();
                        contra = reader["Contraseña"].ToString();
                    }
                    con.Close();

                    
                    if (e.desencrip(correo).Equals(corrreo) & (e.desencrip(contraseña).Equals(contra)))
                    {
                        log = "entro";
                        GenerarToken(e.desencrip(correo));

                       maxbloqueo(e.desencrip(correo));

                       // int maxb = 0;
                        //maxb = Convert.ToInt32(nbloqueos);

                        if (nbloqueos >= 3)
                        {
                           log = "bloqueado";
                        }
                        
                    }

                    else
                    {
                        bloqueo(e.desencrip(correo));
                        log = "noentro";
                        //esto

                        maxbloqueo(e.desencrip(correo));

                       // int maxb = 0;
                        //maxb = Convert.ToInt32(nbloqueos);

                        if (nbloqueos >= 3)
                        {
                            log = "bloqueado";
                        }
                        

                    }
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //esto
        public void bloqueo(String correo)
        {
            Conexion c = new Conexion();
            SqlConnection con = new SqlConnection();
            con = c.ObtenerConexion();
            

            try
            {
                using (con)
                {

                    SqlCommand cmd = new SqlCommand("bloqueo", con);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@correo", correo);
                    

                    cmd.ExecuteScalar();

                    con.Close();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void bloqueoinicio(String correo) //esto
        {
            Conexion c = new Conexion();
            SqlConnection con = new SqlConnection();
            con = c.ObtenerConexion();
            Encriptar e = new Encriptar();

            try
            {
                using (con)
                {

                    SqlCommand cmd = new SqlCommand("bloqueoinicio", con);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@correo", e.desencrip(correo));


                    cmd.ExecuteScalar();

                    con.Close();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //esto
        public void maxbloqueo(String correo)
        {
            Encriptar e = new Encriptar();
            Conexion c = new Conexion();
            SqlConnection con = new SqlConnection();
            con = c.ObtenerConexion();

            try
            {
                using (con)
                {
                    SqlCommand cmd = new SqlCommand("maxbloqueo", con);

                    // cmd.CommandType = new CommandType();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@correo", correo);

                    SqlDataReader reader = cmd.ExecuteReader();

                    
                    

                    while (reader.Read())
                    {
                        nbloqueos = Convert.ToInt32 (reader["nbloqueo"].ToString());
                    }
                    con.Close();

                   
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        int nbloqueos =0;

        public void quitarbloqueo(String correo)
        {
            Conexion c = new Conexion();
            SqlConnection con = new SqlConnection();
            con = c.ObtenerConexion();
            Encriptar e = new Encriptar();


            try
            {
                using (con)
                {

                    SqlCommand cmd = new SqlCommand("quitarbloqueo", con);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@correo", e.desencrip(correo));


                    cmd.ExecuteScalar();

                    con.Close();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public string log = "";

        public void GenerarToken(String correo)
        {
            Conexion c = new Conexion();
            SqlConnection con = new SqlConnection();
            con = c.ObtenerConexion();

            Random r = new Random();
            int num = r.Next(0, 9999);

           

            EnviarCorreo ev = new EnviarCorreo();
            ev.EnviarToken(correo, num);
            //ev.EnviarToken2(correo, num);


            try
            {
                using (con)
                {

                    SqlCommand cmd = new SqlCommand("ActualizarToken", con);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@correo", correo);
                    cmd.Parameters.AddWithValue("@token", num);

                    cmd.ExecuteScalar();

                    con.Close();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void newpass(String correo, String contraseña)
        {
            Conexion c = new Conexion();
            SqlConnection con = new SqlConnection();
            con = c.ObtenerConexion();
            Encriptar e = new Encriptar();


            try
            {
                using (con)
                {

                    SqlCommand cmd = new SqlCommand("newpass", con);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@correo", e.desencrip(correo));
                    cmd.Parameters.AddWithValue("@contraseña", e.desencrip(contraseña));


                    cmd.ExecuteScalar();

                    con.Close();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // AJAX

        public List<Pais> ObtenerPais()
        {
            Encriptar e = new Encriptar();
            Conexion c = new Conexion();
            SqlConnection con = new SqlConnection();
            con = c.ObtenerConexion();
            List<Pais> listaPaises = new List<Pais>();

            try
            {
                using (con)
                {
                    SqlCommand cmd = new SqlCommand("ObtenerPaises", con);

                    // cmd.CommandType = new CommandType();

                    cmd.CommandType = CommandType.StoredProcedure;

                    

                    SqlDataReader reader = cmd.ExecuteReader();




                    while (reader.Read())
                    {
                      listaPaises.Add(new Pais { IdPais = Convert.ToInt32( reader["id_pais"].ToString()), NombrePais = reader["nombre"].ToString() });
                    }
                    con.Close();

                    return listaPaises;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Ciudad> ObtenerCiudad(int idpais)
        {
            Encriptar e = new Encriptar();
            Conexion c = new Conexion();
            SqlConnection con = new SqlConnection();
            con = c.ObtenerConexion();
            List<Ciudad> listaCiudad = new List<Ciudad>();

            try
            {
                using (con)
                {
                    SqlCommand cmd = new SqlCommand("CiudadesFiltradas", con);

                    // cmd.CommandType = new CommandType();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@id_pais", idpais);

                    SqlDataReader reader = cmd.ExecuteReader();

                    int prueba_idciudad;
                    int prueba_idpais;
                    string prueba_nombre = "";

                    while (reader.Read())
                    {
                        prueba_idciudad = Convert.ToInt32(reader["id_ciudad"].ToString());
                        prueba_idpais = Convert.ToInt32(reader["id_pais"].ToString());
                        prueba_nombre = reader["nombre"].ToString();

                        listaCiudad.Add(new Ciudad { IdCiudad = prueba_idciudad, IdPais = prueba_idpais, NombreCiudad = prueba_nombre });

                  /*      listaCiudad.Add(new Ciudad { IdCiudad = Convert.ToInt32(reader["id_ciudad"].ToString()), NombreCiudad = reader["nombre"].ToString(),IdPais= Convert.ToInt32( reader["id_pais"].ToString()) });
                  */
                        
                    }
                    con.Close();

                    

                    return listaCiudad;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Distrito> ObtenerDistrito(int idciudad,int idpais)
        {
            Encriptar e = new Encriptar();
            Conexion c = new Conexion();
            SqlConnection con = new SqlConnection();
            con = c.ObtenerConexion();
            List<Distrito> listaDistrito = new List<Distrito>();

            try
            {
                using (con)
                {
                    SqlCommand cmd = new SqlCommand("DistritosFiltrados", con);

                    // cmd.CommandType = new CommandType();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@id_ciudad", idciudad);
                    cmd.Parameters.AddWithValue("@id_pais", idpais);

                    SqlDataReader reader = cmd.ExecuteReader();

                    int prueba_idciudad;
                    int prueba_iddistrito;
                    string prueba_nombre = "";

                    while (reader.Read())
                    {
                        prueba_idciudad = Convert.ToInt32(reader["id_ciudad"].ToString());
                        prueba_iddistrito = Convert.ToInt32(reader["id_distrito1"].ToString());
                        prueba_nombre = reader["nombre"].ToString();

                        listaDistrito.Add(new Distrito { IdCiudad = prueba_idciudad, Iddistrito = prueba_iddistrito, Nombredistrito = prueba_nombre });

                        /*      listaCiudad.Add(new Ciudad { IdCiudad = Convert.ToInt32(reader["id_ciudad"].ToString()), NombreCiudad = reader["nombre"].ToString(),IdPais= Convert.ToInt32( reader["id_pais"].ToString()) });
                        */

                    }
                    con.Close();



                    return listaDistrito;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

    public class Pais
    {
        public int IdPais { get; set; }
        public string NombrePais { get; set; }
    }

    public class Ciudad
    {
        public int IdCiudad { get; set; }
        public string NombreCiudad { get; set; }
        public int IdPais { get; set; }
    }

    public class Distrito
    {
        public int Iddistrito { get; set; }
        public string Nombredistrito { get; set; }
        public int IdCiudad { get; set; }
    }
}
