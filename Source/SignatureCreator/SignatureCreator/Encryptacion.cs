using System;


namespace SignatureCreator
{
    internal class Encryptacion
    {
        /// <summary>
        /// Este método permite desencryptar un dato que se encuentra encryptado, utilizando un Key específico
        /// </summary>
        /// <param name="Firm">Datos que contiene la firma y se va encriptar</param>
        /// <param name="Key">Key que se utiliza para lograr la correcta desencriptación del dato</param>
        /// <returns>Dato Desencryptado; si no se logra desencryptar devuelve una cadena vacía</returns>
        public static string Encrypter(string Firm, string Key)
        {
            SymmCrypto encrypter = new SymmCrypto(SymmCrypto.SymmProvEnum.Rijndael);
            string EncrypterFirm = string.Empty;

            try
            {
                EncrypterFirm = encrypter.Encrypting(Firm, Key);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return EncrypterFirm;
        }

        /// <summary>
        /// Este método permite desencryptar un dato que se encuentra encryptado, utilizando un Key específico
        /// </summary>
        /// <param name="Firm">Firma encriptada y que se quiere desencriptar</param>
        /// <param name="Key">Key que se utiliza para lograr la correcta desencriptación de la firma</param>
        /// <returns>Dato Desencriptado; si no se logra desencriptar devuelve una cadena vacía</returns>
        public static string DisEncrypter(string Firm, string Key)
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
    }
}
