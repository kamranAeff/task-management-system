using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DockerizeTaskManagementApi.AppCode.Extensions
{
    static public partial class Extension
    {
        const string salt = "@mon-p!ace-r@ce";

        public static string Encrypt(this string data)
        {
            return data.Crypto(SymmetricCryptoMethods.Encrypt, "6ac3fed0a2396f86c6938bd6ec2d8a20cccf65d3").ToString();
        }

        public static string Decrypt(this string data)
        {
            return data.Crypto(SymmetricCryptoMethods.Decrypt, "6ac3fed0a2396f86c6938bd6ec2d8a20cccf65d3").ToString();
        }

        public static object Crypto(this object input, SymmetricCryptoMethods method, string key)
        {
            key = key + string.Join("", salt.ToCharArray().Reverse());
            try
            {
                SymmetricAlgorithm symmetricAlgorithm = new TripleDESCryptoServiceProvider();

                var keyBuffer = new byte[symmetricAlgorithm.Key.Length];
                var bytes = Encoding.UTF8.GetBytes(key);
                Array.Copy(bytes, 0, keyBuffer, 0, bytes.Length > keyBuffer.Length ? keyBuffer.Length : bytes.Length);
                symmetricAlgorithm.Key = keyBuffer;
                symmetricAlgorithm.Padding = PaddingMode.PKCS7;
                symmetricAlgorithm.Mode = CipherMode.ECB;
                ICryptoTransform cryptoTransform = null;
                switch (method)
                {
                    case SymmetricCryptoMethods.Encrypt:
                        cryptoTransform = symmetricAlgorithm.CreateEncryptor();
                        break;
                    case SymmetricCryptoMethods.Decrypt:
                        cryptoTransform = symmetricAlgorithm.CreateDecryptor();
                        break;
                }
                if (input is string)
                {
                    var inputBuffer = method == SymmetricCryptoMethods.Encrypt ? Encoding.UTF8.GetBytes(input as string) : Convert.FromBase64String(input as string);
                    var finalBlock = cryptoTransform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
                    symmetricAlgorithm.Clear();
                    return method == SymmetricCryptoMethods.Encrypt ? Convert.ToBase64String(finalBlock) : Encoding.UTF8.GetString(finalBlock);
                }
                else if (input is byte[])
                {
                    var finalBlock = cryptoTransform.TransformFinalBlock(input as byte[], 0, (input as byte[]).Length);
                    symmetricAlgorithm.Clear();
                    return finalBlock;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }
            return false;
        }

        public enum SymmetricCryptoMethods
        {
            Encrypt,
            Decrypt,
        }
    }
}
