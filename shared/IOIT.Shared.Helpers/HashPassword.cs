using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace IOIT.Shared.Helpers
{
    public class HashPassword
    {
        public static string Encrypt(string key, string data)
        {
            data = data.Trim();

            byte[] keydata = Encoding.ASCII.GetBytes(key);

            string md5String = BitConverter.ToString(new
                                                            MD5CryptoServiceProvider().ComputeHash(keydata)).Replace("-",
                                                                                                                    "").
                ToLower();

            byte[] tripleDesKey = Encoding.ASCII.GetBytes(md5String.Substring(0, 24));

            TripleDES tripdes = TripleDES.Create();

            tripdes.Mode = CipherMode.ECB;

            tripdes.Key = tripleDesKey;

            tripdes.GenerateIV();

            var ms = new MemoryStream();

            var encStream = new CryptoStream(ms, tripdes.CreateEncryptor(),
                                                CryptoStreamMode.Write);

            encStream.Write(Encoding.ASCII.GetBytes(data), 0, Encoding.ASCII.GetByteCount(data));

            encStream.FlushFinalBlock();

            byte[] cryptoByte = ms.ToArray();

            ms.Close();

            encStream.Close();

            return Convert.ToBase64String(cryptoByte, 0, cryptoByte.GetLength(0)).Trim();
        }

        public static string Decrypt(string key, string data)
        {
            try
            {
                byte[] keydata = Encoding.ASCII.GetBytes(key);

                string md5String = BitConverter.ToString(new
                                                                MD5CryptoServiceProvider().ComputeHash(keydata)).Replace("-",
                                                                                                                        "").
                    Replace(" ", "+").ToLower();

                byte[] tripleDesKey = Encoding.ASCII.GetBytes(md5String.Substring(0, 24));

                TripleDES tripdes = TripleDES.Create();

                tripdes.Mode = CipherMode.ECB;

                tripdes.Key = tripleDesKey;

                data = data.Trim();
                byte[] cryptByte = Convert.FromBase64String(data);

                var ms = new MemoryStream(cryptByte, 0, cryptByte.Length);

                ICryptoTransform cryptoTransform = tripdes.CreateDecryptor();

                var decStream = new CryptoStream(ms, cryptoTransform,
                                                    CryptoStreamMode.Read);

                var read = new StreamReader(decStream);

                return (read.ReadToEnd());
            } catch (Exception e)
            {
                return data;
            }
        }

        public static string EncryptRSA(string publickey, string data)
        {
            data = data.Trim();
            string encryptedValue = string.Empty;
            var csp = new CspParameters(1);

            var rsa = new RSACryptoServiceProvider(csp);
            rsa.FromXmlString(publickey);

            byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(data);
            byte[] bytesEncrypted = rsa.Encrypt(bytesToEncrypt, false);
            encryptedValue = Convert.ToBase64String(bytesEncrypted);
            return encryptedValue;
        }

        public static string DecryptRSA(string privateKey, string data)
        {
            data = data.Trim();
            string decryptedValue = string.Empty;
            var csp = new CspParameters(1);

            var rsa = new RSACryptoServiceProvider(csp);
            rsa.FromXmlString(privateKey);
            byte[] valueToDecrypt = Convert.FromBase64String(data);
            byte[] plainTextValue = rsa.Decrypt(valueToDecrypt, false);

            // Extract our decrypted byte array into a string value to return to our user
            decryptedValue = Encoding.UTF8.GetString(plainTextValue);
            return decryptedValue;
        }

        public static string CreateSignRSA(string data, string privateKey)
        {
            //RSACryptoServiceProvider rsaCryptoIPT = new RSACryptoServiceProvider(1024);

            CspParameters _cpsParameter;
            RSACryptoServiceProvider rsaCryptoIPT;
            _cpsParameter = new CspParameters();
            _cpsParameter.Flags = CspProviderFlags.UseMachineKeyStore;
            rsaCryptoIPT = new RSACryptoServiceProvider(1024, _cpsParameter);

            rsaCryptoIPT.FromXmlString(privateKey);
            return
                Convert.ToBase64String(rsaCryptoIPT.SignData(new ASCIIEncoding().GetBytes(data),
                                                                new SHA1CryptoServiceProvider()));
        }

        public static bool CheckSignRSA(string data, string sign, string publicKey)
        {
            try
            {
                var rsacp = new RSACryptoServiceProvider();
                rsacp.FromXmlString(publicKey);
                return rsacp.VerifyData(Encoding.UTF8.GetBytes(data), "SHA1", Convert.FromBase64String(sign));
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
