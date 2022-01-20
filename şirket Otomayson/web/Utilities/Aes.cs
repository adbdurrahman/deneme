﻿using System;
using System.IO;
using System.Security.Cryptography;

namespace BitirmeProjesiWeb.Utilities
{
    public class Aes : IEncryptor
    {
        readonly byte[] key = Convert.FromBase64String("S1j8IsQ97D0lcQuDb46B9nHK40Ooa4Bme28b8CQlpCg=");
        readonly byte[] iv = Convert.FromBase64String("fOEDMGNWpDzgwBe4QnOy9w==");

        public string Encrypt(string text)
        {
            string encrypted = string.Empty;
            try
            {
                using (RijndaelManaged myRijndael = new RijndaelManaged())
                {
                    myRijndael.Key = key;
                    myRijndael.IV = iv;

                    // Encrypt the string to an array of bytes. 
                    byte[] bytes = EncryptStringToBytes(text, myRijndael.Key, myRijndael.IV);

                    // Convert encrypted bytes to string
                    encrypted = Convert.ToBase64String(bytes);
                }
            }
            catch (Exception exp)
            {
                throw new Exception("AES Encryption Error: " + exp.Message);
            }
            return encrypted;
        }
        public string Decrypt(string text)
        {
            string decrypted = string.Empty;
            try
            {
                using (RijndaelManaged myRijndael = new RijndaelManaged())
                {
                    myRijndael.Key = key;
                    myRijndael.IV = iv;

                    // Convert string to bytes
                    byte[] decrypted_bytes = Convert.FromBase64String(text);

                    // Decrypt the bytes to a string. 
                    decrypted = DecryptStringFromBytes(decrypted_bytes, myRijndael.Key, myRijndael.IV);

                }
            }
            catch (Exception exp)
            {
                throw new Exception("AES Decryption Error: " + exp.Message);
            }
            return decrypted;
        }

        static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            // Create an RijndaelManaged object 
            // with the specified key and IV. 
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption. 
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream. 
            return encrypted;

        }
        static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an RijndaelManaged object 
            // with the specified key and IV. 
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption. 
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream 
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;
        }
    }
}