# AzureKeyVault
Code samples for Azure KeyVault

The first (only!) sample (so far) is C# code that creates or reads an existing key ID in KeyVault and uses that to encrypt and decrypt a string. It's not complex.

You need to make sure you have these three assemblies installed:

Install-Package Azure.Security.KeyVault.Keys
Install-Package Azure.Identity

Can you read more about the SDK here https://docs.microsoft.com/en-us/azure/key-vault/quick-create-net. 
