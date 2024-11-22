using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;


namespace LicenseVerifier
{
    internal class ConnectionBDChecker
    {
        private readonly string _connectionString;


        /// <summary>
        /// Constructor de la clase ConnectionBDChecker.
        /// Inicializa una nueva instancia de la clase y establece la cadena de conexión utilizando la configuración predeterminada.
        /// Debe existir una cadena de conexión con el nombre "DefaultConnection" en el archivo de configuración.
        /// Ejemplo:
        /// <connectionStrings>
		///<add name = "DefaultConnection" connectionString="Data Source=localhost;Initial Catalog=master;User ID=pruebas;Password=pruebas" />
	    ///</connectionStrings>
        /// </summary>
        public ConnectionBDChecker(string Connection, string BD, string User, string Pass)
        {
            _connectionString = "Data Source="+Connection+";Initial Catalog="+BD+";User ID="+User+";Password="+Pass;
        }


        /// <summary>
        /// Obtiene y abre una nueva conexión a la base de datos utilizando la cadena de conexión especificada en el constructor.
        /// </summary>
        /// <returns>Una instancia de SqlConnection abierta y lista para su uso.</returns>
        public SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }



        /// <summary>
        /// Obtiene un identificador único de la base de datos mediante una consulta SQL.
        /// </summary>
        /// <returns>Una cadena que representa el identificador único, o una cadena vacía si no se pudo obtener.</returns>
        public static string GetUniqueID(string Connection, string BD, string User, string Pass)
        {   
            SqlConnection connection = null;
            try
            {
                ConnectionBDChecker connectionBD = new ConnectionBDChecker( Connection,  BD,  User,  Pass);
                connection = connectionBD.GetConnection();
                

                // Ejecutar el SELECT
                string selectQuery = "DECLARE @UniqueIdentifier NVARCHAR(100) " +
                                     "SET @UniqueIdentifier = @@SERVERNAME + '_' + " +
                                     "CAST(SERVERPROPERTY('ProductVersion') AS NVARCHAR(50)) + '_aplix' " +
                                     "SELECT @UniqueIdentifier AS UniqueIdentifier";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader["UniqueIdentifier"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error de conexión: " + ex.Message);
            }
            finally
            {
                // Asegúrate de cerrar la conexión en caso de éxito o error
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return "";
        }



    }

}
