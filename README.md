# AzureKeyVault
Code samples for Azure KeyVault

The first (only!) sample (so far) is C# code that creates or reads an existing key ID in KeyVault and uses that to encrypt and decrypt a string. It's not complex.

# Pre-reqs
You need to make sure you have these three assemblies installed:
- Install-Package Azure.Security.KeyVault.Keys
- Install-Package Azure.Identity

You need to create a KeyVault in Azure and grant the account you'll use the ability to create and read a key as well as encrypt. If you're a subscription owner or contributor, then you'll be fine.

This code was created and used .NET 4.7.2. I have not tested it with other versions. 

You can read more about the SDK here https://docs.microsoft.com/en-us/azure/key-vault/quick-create-net. 
