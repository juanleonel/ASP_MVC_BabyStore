using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Domain
{
    public sealed class DapperBase
    {
        private static SqlConnection Conn = null;

        private static readonly string SqlConfig = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

        public static SqlConnection GetConnection()
        {
            try
            {
                if (Conn == null)
                {
                    Conn = new SqlConnection(SqlConfig);
                    Conn.Open();                    
                }
                return Conn;
            }
            catch (Exception)
            {
                throw new Exception("Error connect!") ;
            }
        }


    }
}
