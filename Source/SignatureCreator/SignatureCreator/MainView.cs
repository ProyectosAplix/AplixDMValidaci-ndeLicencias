using System;
using System.Windows.Forms;

namespace SignatureCreator
{
    public partial class MainView : Form
    {

        public MainView()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Encripta la firma utilizando la clave proporcionada y almacena el resultado en la variable Firm.
        /// </summary>
        /// <param name="firm">La cadena de firma a encriptar.</param>
        /// <param name="key">La clave utilizada para la encriptación.</param>
        /// <returns>True si la encriptación fue exitosa; de lo contrario, False.</returns>
        /// <remarks>
        /// La función intenta encriptar la cadena de firma utilizando la clave proporcionada.
        /// Si la encriptación es exitosa, almacena el resultado en la variable Firm y devuelve True.
        /// En caso de error, muestra un mensaje de error y devuelve False.
        /// </remarks>
        private bool FEncrypter(string firm, string key)
        {
            try
            {
                Firm = Encryptacion.Encrypter(firm, key);
              
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Se produjo un error al encryptar los datos.\nError: " + ex.Message, "Encryptando Datos...", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
            return true;
        }



        /// <summary>
        /// Desencripta la firma utilizando la clave proporcionada y almacena el resultado en la variable FirmDisencrypter.
        /// </summary>
        /// <param name="firm">La cadena de firma a desencriptar.</param>
        /// <param name="key">La clave utilizada para la desencriptación.</param>
        /// <returns>True si la desencriptación fue exitosa; de lo contrario, False.</returns>
        /// <remarks>
        /// La función intenta desencriptar la cadena de firma utilizando la clave proporcionada.
        /// Si la desencriptación es exitosa, almacena el resultado en la variable FirmDisencrypter y devuelve True.
        /// Si la firma no se puede desencriptar, muestra un mensaje de error y devuelve False.
        /// En caso de error, muestra un mensaje de error y devuelve False.
        /// </remarks>
        private bool FDisEncrypter(string firm,string key)
        {
            try
            {
                FirmDisencrypter = Encryptacion.DisEncrypter(firm, key);

                if(FirmDisencrypter == string.Empty)
                {
                    MessageBox.Show("La firma no se pudo desencryptar, verifique que la firma sea correcta y que la llave sea la correcta.", "Desencryptando Datos...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Se produjo un error al desencryptar los datos.\nError: " + ex.Message, "Desencryptando Datos...", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
            return true;
        }
    }
}
