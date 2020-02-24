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
            var keyName = "keyTest2";

            // pop a UI to logon to Azure
            // this account must have the RBAC policy to create/read a key and encrypt
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
                // If the KeyVault is hardware backed (ie; Premier SKU) then you could use RsaHsm
                try
                {
                    key = client.CreateKey(keyName, KeyType.Rsa);
                } 
                catch (Exception e)
                {
                    throw e;
                }
            }

            // this is the client to use to perform crypto operations again KeyVault
            var cryptoClient = new CryptographyClient(key.Id, creds);

            // only three options, all RSA, for encryption
            EncryptionAlgorithm alg = EncryptionAlgorithm.RsaOaep;

            // encrypt in the Key Vault and get the ciphertext
            byte[] plaintext = Encoding.UTF8.GetBytes("Just one block of plaintext!");
            EncryptResult encryptResult = cryptoClient.Encrypt(alg, plaintext);
            var cipher = Convert.ToBase64String(encryptResult.Ciphertext);

            // take the ciphertext and decrypt it in the Key Vault
            DecryptResult decryptResult = cryptoClient.Decrypt(alg, encryptResult.Ciphertext);
            var plain = Encoding.Default.GetString(decryptResult.Plaintext);

            //DeleteKeyOperation operation = client.StartDeleteKey(keyName);

            Console.ReadKey();
        }
    }
}
