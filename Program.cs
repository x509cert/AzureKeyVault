using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using System;
using System.Text;

namespace ConsoleApp5
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // add your KeyVault URL here, this is the DNS field in the Azure Portal for the KeyVault
            var keyVaultUrl = $"https://xyzzy.vault.azure.net/";

            // this key will be created if it does not already exist
            var keyName = "keyTest";

            // pop a UI to logon to Azure
            // this account must have the RBAC policy to create/read a key and encrypt/decrypt
            var creds = new DefaultAzureCredential(true);

            // this is the KeyVault client
            var client = new KeyClient(new Uri(keyVaultUrl), creds);

            // see if the key exists first
            KeyVaultKey key;
            try
            {
                key = client.GetKey(keyName);
            }
            catch (Azure.RequestFailedException)
            {
                // if the KeyVault is hardware backed (ie; Premier SKU) then you could use RsaHsm
                try
                {
                    key = client.CreateKey(keyName, KeyType.Rsa);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            // this is the client that performs crypto operations against KeyVault
            var cryptoClient = new CryptographyClient(key.Id, creds);

            // only three options, all RSA, for encryption
            var alg = EncryptionAlgorithm.RsaOaep;

            var starttext = "Just one block of plaintext!";
            Console.WriteLine($"Plaintext: {starttext}");

            // encrypt using the Key Vault API and get the ciphertext
            var starttextBytes = Encoding.UTF8.GetBytes(starttext);
            var encryptResult = cryptoClient.Encrypt(alg, starttextBytes);
            var cipherText = Convert.ToBase64String(encryptResult.Ciphertext);
            Console.WriteLine($"Ciphertext: {cipherText}");

            // decrypt using the Key Vault API to get the plaintext back
            var decryptResult = cryptoClient.Decrypt(alg, encryptResult.Ciphertext);
            var plainText = Encoding.Default.GetString(decryptResult.Plaintext);
            Console.WriteLine($"Plaintext: {plainText}");

            // not needed, but now you know :)
            //DeleteKeyOperation operation = client.StartDeleteKey(keyName);

            Console.ReadKey();
        }
    }
}
