using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LicenseVerifier
{
    public class Checker
    {


        public string FinalDate = string.Empty;



        /// <summary>
        /// Verifica la validez de la licencia utilizando la clave de desencriptación y la información de conexión a la base de datos.
        /// </summary>
        /// <param name="Key">Clave secreta utilizada para desencriptar la firma de la licencia.</param>
        /// <param name="Connection">Cadena de conexión a la base de datos.</param>
        /// <param name="BD">Nombre de la base de datos.</param>
        /// <param name="User">Nombre de usuario para la conexión a la base de datos.</param>
        /// <param name="Pass">Contraseña para la conexión a la base de datos.</param>
        /// <returns>Un array de cadenas que contiene el resultado de la verificación de la licencia.</returns>
        public static string[] Check(string Key, string Connection, string BD, string User, string Pass)
        {

            string urlXml = Path.Combine(Directory.GetCurrentDirectory(), "Licencia.xml");  //cambiar el path;
            string Firm = getFirm(urlXml);


            if (Firm == string.Empty)
            {
                return TupleToArray("LicenciaInvalida","No se pudo obtener la firma del archivo XML.");
            }

            else
            {
                string DisencrypterFirm = DisEncrypter(Firm, Key);
                if (DisencrypterFirm == string.Empty)
                {
                    
                    return TupleToArray("LicenciaInvalida", "No se pudo desencriptar la firma.");
                }
                else
                {

                    return ParityCheck(DisencrypterFirm, urlXml, Connection, BD, User, Pass);
                    
                }
            }



        }




        /// <summary>
        /// Obtiene la cadena de firma de un archivo XML de licencia.
        /// </summary>
        /// <param name="PathXml">Ruta del archivo XML de licencia.</param>
        /// <returns>La cadena de firma obtenida del archivo XML.</returns>
        private static string getFirm(string PathXml)
        {



            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(PathXml);

                XmlNode PathFirm = xmlDoc.SelectSingleNode("/Licencia/Firma");

                if (PathFirm != null)
                {

                    string Firm = PathFirm.InnerText;
                    return Firm;


                }
                else
                {
                    Console.WriteLine("Nodo 'Firm' no encontrado en el archivo XML.");
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }

        }


        /// <summary>
        /// Desencripta una cadena utilizando un algoritmo de cifrado simétrico.
        /// </summary>
        /// <param name="Firm">Cadena cifrada que se va a desencriptar.</param>
        /// <param name="Key">Clave secreta utilizada para la desencriptación.</param>
        /// <returns>La cadena desencriptada.</returns>
        private static string DisEncrypter(string Firm, string Key)
        {
            string DisencrypterFirm = string.Empty;

            SymmCrypto desencrip = new SymmCrypto(SymmCrypto.SymmProvEnum.Rijndael);

            try
            {
                DisencrypterFirm = desencrip.Decrypting(Firm, Key);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return DisencrypterFirm;
        }



        /// <summary>
        /// Realiza una verificación de paridad de licencia, comparando la información de la firma, el archivo XML y la base de datos.
        /// </summary>
        /// <param name="Firm">Información de la firma de la licencia.</param>
        /// <param name="PathXml">Ruta del archivo XML de licencia.</param>
        /// <param name="Connection">Cadena de conexión a la base de datos.</param>
        /// <param name="BD">Nombre de la base de datos.</param>
        /// <param name="User">Nombre de usuario para la conexión a la base de datos.</param>
        /// <param name="Pass">Contraseña para la conexión a la base de datos.</param>
        /// <returns>Un array de cadenas que contiene el resultado de la verificación de paridad.</returns>
        private static string[] ParityCheck(string Firm, string PathXml, string Connection, string BD, string User, string Pass)
        {
            string[] FirmParts = Firm.Split('|');
            string[] XmlParts = GetPartsXML(PathXml);
            string UniqueID = ConnectionBDChecker.GetUniqueID(Connection, BD, User, Pass);
            



            if (FirmParts[3] == "P")
            {
                
                return TupleToArray("LicenciaValida", "La licencia es permanente");
            }
            else if (FirmParts[0] == "I")
            {
                return TupleToArray("LicenciaInvalida", "Licencia inactiva");
            }
            else if (FirmParts[2] != UniqueID)
            {
                return TupleToArray("LicenciaInvalida", "Inconsistencia en la base de datos");
            }



            int length = FirmParts.Length - 2;

            for (int i = length - 1; i >= 0; i--)
            {

                if (FirmParts[i] != XmlParts[i])
                {
                    return TupleToArray("LicenciaInvalida", "Inconsistencia en los datos");
                    
                }

            }


            string DateLine = FirmParts[1];
            DateTime DateExpiration = DateTime.ParseExact(DateLine, "dd-MM-yyyy", null);
            DateExpiration = DateExpiration.AddDays(3);
            string IdUser = XmlParts[2];
            int RemainingDays = RemainingDaysToday(DateLine);
            if (RemainingDays >= 0 && RemainingDays <= 8)
            {
                return TupleToArray("LicenciaMensaje","La licencia " + IdUser + " vence dentro de " + RemainingDays + " días.Luego del " + FirmParts[1] + " no podrá ingresar al sistema.Le agradecemos se comunique tan pronto le sea posible con su ejecutivo de Aplix para obtener su correspondiente licencia y así evitarle mayores inconvenientes.");
            }
            else
            {
                if (RemainingDays < 0 && RemainingDays >= -3)
                {
                    return TupleToArray("LicenciaMensaje", "La licencia " + IdUser + " registrada para el uso del producto está vencida, le informamos que cuenta a partir de hoy con un período de 3 días para registrar la licencia correspondiente, luego del " + DateExpiration.ToString("dd-MM-yyyy") + " no podrá ingresar al sistema. Le agradecemos se comunique tan pronto le sea posible con su ejecutivo de Aplix para obtener su correspondiente licencia y así evitarle mayores inconvenientes.");

                }
                else if (RemainingDays < -3)
                {

                    string[] resultArray = { "LicenciaMensaje", "La licencia " + IdUser + " expiró el día " + DateExpiration.ToString("dd-MM-yyyy") + ". Para continuar utilizando la funcionalidad del mismo es necesario que se comunique con su ejecutivo de Aplix para gestionar la entrega de una nueva licencia de uso de este producto.","Expirada" };
                    return resultArray;
                    
                }
            }

            return TupleToArray("LicenciaValida", "Cumplio con todos los requerimientos");

        }



        /// <summary>
        /// Obtiene partes específicas de un archivo XML de licencia.
        /// </summary>
        /// <param name="PathXml">Ruta del archivo XML de licencia.</param>
        /// <returns>Un array de cadenas que contiene información específica de la licencia, como estado, fecha de vencimiento e ID.</returns>
        private static string[] GetPartsXML(string PathXml)
        {
            List<string> LicenseData = new List<string>();

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(PathXml);


                XmlNode PathNode = xmlDoc.SelectSingleNode("/Licencia");
                if (PathNode != null)
                {

                    LicenseData.Add(PathNode.SelectSingleNode("Estado")?.InnerText ?? "N/A");
                    LicenseData.Add(PathNode.SelectSingleNode("FechaVencimiento")?.InnerText ?? "N/A");
                    LicenseData.Add(PathNode.SelectSingleNode("ID")?.InnerText ?? "N/A");

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return LicenseData.ToArray();
        }



        /// <summary>
        /// Calcula los días restantes hasta una fecha específica a partir de la fecha actual.
        /// </summary>
        /// <param name="Date">Fecha de expiración en formato "dd-MM-yyyy".</param>
        /// <returns>El número de días restantes hasta la fecha especificada.</returns>
        private static int RemainingDaysToday(string Date)
        {
            DateTime Today = DateTime.Now;
            Console.WriteLine("Today: " + Date);
            DateTime DateExpiration = DateTime.ParseExact(Date, "dd-MM-yyyy", null);

            TimeSpan RemainingDays = DateExpiration - Today;

            return RemainingDays.Days;
        }


        /// <summary>
        /// Convierte dos mensajes de cadena en un array de cadenas.
        /// </summary>
        /// <param name="LicenseMessage">Mensaje de licencia a incluir en el array.</param>
        /// <param name="InformationMessage">Mensaje de información a incluir en el array.</param>
        /// <returns>Un array de cadenas que contiene los mensajes proporcionados.</returns>
        static string[] TupleToArray(string LicenseMessage, string InformationMessage)
        {
            
            string[] resultArray = { LicenseMessage, InformationMessage };
            return resultArray;
        }


    }
}

