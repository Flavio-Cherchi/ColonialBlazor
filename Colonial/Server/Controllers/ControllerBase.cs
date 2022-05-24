using Colonial.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Colonial.Server.Controllers
{
    
    [ApiController]
    public class ControllerBase : Microsoft.AspNetCore.Mvc.Controller
    {
        public static string _conn { get; set; }
        public ControllerBase(IConfiguration configuration)
        {
            _conn = configuration.GetConnectionString("ColonialDBConnection");
        }

        public static DataSet Read(string queryString)
        {
            
            using (SqlConnection connection = new SqlConnection(_conn))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(queryString, connection);
                DataSet dt = new DataSet();
                try
                {
                    connection.Open();

                    // Using sqlDataAdapter, we process the query string and fill the data into the dataset

                    adapter.Fill(dt);
                }
                catch (SqlException E)
                {
                    Console.WriteLine(E.ToString());
                }
                finally
                {
                    connection.Close();
                }
                return dt;
            }
        }

        public static void Exec(string queryString)
        {
            using (SqlConnection Con = new SqlConnection(_conn))
            {
                try
                {
                    Con.Open();
                    SqlCommand Cmd = new SqlCommand(queryString, Con);
                    Cmd.ExecuteNonQuery();
                }
                catch (SqlException E)
                {
                    Console.WriteLine(E.ToString());
                }
                finally
                {
                    Con.Close();
                }
            }
        }
    }
}