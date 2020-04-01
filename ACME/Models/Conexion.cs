using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ACME.Models
{
    public class Conexion
    {

        public SqlConnection con = new SqlConnection();

        public SqlConnection ObtenerConexion()
        {
            con = new SqlConnection("Password=progra;Persist Security Info=True;User ID=sa;Initial Catalog=LoginACME2;Data Source=DESKTOP-OTPQGFQ");
            try
            {
                con.Open();
                return con;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DescargarConexion()
        {
            con.Dispose();
            return true;
        }
    }
}

