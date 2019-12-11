using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Gbr.Dynamics.Online.Utilities.Cryptography
{
    public static class CryptographyOperations
    {
        #region Variables

        private static readonly string defaultKey = "ENCRYPTIONKEY";
        private static readonly string defaultIv = "ENCRYPTIONIV";
        private static readonly TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();

        #endregion

        #region Public Methods

        public static string Encrypt(this string input)
        {
            return input.Encrypt(defaultKey, defaultIv);
        }

        public static string Encrypt(this string plaintext, string key, string iv)
        {
            string encryptedInput;
            byte[] aKey = StringToByteArray(key.PadLeft(24, '0'));
            byte[] aIV = StringToByteArray(iv);
            using (MemoryStream encryptedStream = new MemoryStream())
            {
                using (CryptoStream encryptorStream = new CryptoStream(encryptedStream, tripleDESCryptoServiceProvider.CreateEncryptor(aKey, aIV), CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(encryptorStream))
                    {
                        sw.Write(plaintext);
                    }
                    encryptedInput = "";
                    foreach (var item in encryptedStream.ToArray().Select(q => q.ToString("000")))
                    {
                        encryptedInput += item;
                    }
                }
            }
            return encryptedInput;
        }

        public static string Decrypt(this string input)
        {
            return input.Decrypt(defaultKey, defaultIv, StringToByteArray);
        }

        public static string Decrypt(this string cyphertext, string key, string iv, Func<string, byte[]> encryptedStringToByteArray)
        {

            string[] byteChunksStr = cyphertext.Chunk(3).ToArray();
            List<byte> arr = new List<byte>();
            foreach (var item in byteChunksStr)
            {
                arr.Add(byte.Parse(item));
            }
            cyphertext = ByteArrayToString(arr.ToArray());
            string decryptedInput;
            byte[] aKey = StringToByteArray(key.PadLeft(24, '0'));
            byte[] aIV = StringToByteArray(iv);
            byte[] encrypted = encryptedStringToByteArray(cyphertext);
            using (MemoryStream encryptedStream = new MemoryStream(encrypted))
            {
                using (CryptoStream decryptorStream = new CryptoStream(encryptedStream, tripleDESCryptoServiceProvider.CreateDecryptor(aKey, aIV), CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(decryptorStream))
                    {
                        decryptedInput = sr.ReadToEnd();
                    }
                }
            }
            return decryptedInput;
        }

        #endregion

        #region Private Methods

        private static string ByteArrayToString(byte[] encrypted)
        {
            char[] ret = new char[encrypted.Length];
            for (int i = 0; i < encrypted.Length; i++)
            {
                ret[i] = Convert.ToChar(encrypted[i]);
            }
            return new string(ret);
        }

        private static byte[] StringToByteArray(string input)
        {
            byte[] ret = new byte[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                ret[i] = Convert.ToByte(input[i]);
            }
            return ret;
        }

        private static IEnumerable<string> Chunk(this string str, int chunkSize)
        {
            for (int i = 0; i < str.Length; i += chunkSize)
            {
                yield return str.Substring(i, chunkSize);
            }
        }

        #endregion
    }
}
