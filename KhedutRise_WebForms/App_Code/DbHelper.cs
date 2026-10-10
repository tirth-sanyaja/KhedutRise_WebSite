using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace KhedutRise
{
    // Helper class to handle common database operations using ADO.NET
    public static class DbHelper
    {
        // Get the active connection string from Web.config
        public static string GetConnectionString()
        {
            if (ConfigurationManager.ConnectionStrings["KhedutRiseDB"] != null)
            {
                return ConfigurationManager.ConnectionStrings["KhedutRiseDB"].ConnectionString;
            }
            // Fallback connection to LocalDB
            return @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=KhedutRiseDB;Integrated Security=True;";
        }

        // Execute a SELECT query and return results as a DataTable (used for GridViews and Repeaters)
        public static DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }
            return dt;
        }

        // Execute an INSERT, UPDATE, or DELETE query and return the number of affected rows
        public static int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    con.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
