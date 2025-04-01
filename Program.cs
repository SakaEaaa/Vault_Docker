﻿using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.Commons;


var EndPoint = "https://localhost:8201/";
var httpClientHandler = new HttpClientHandler();
httpClientHandler.ServerCertificateCustomValidationCallback =
(message, cert, chain, sslPolicyErrors) => { return true; };


// Initialize one of the several auth methods.
IAuthMethodInfo authMethod =
new TokenAuthMethodInfo("00000000-0000-0000-0000-000000000000");
// Initialize settings. You can also set proxies, custom delegates etc. here.
var vaultClientSettings = new VaultClientSettings(EndPoint, authMethod)
{
    Namespace = "",
    MyHttpClientProviderFunc = handler
    => new HttpClient(httpClientHandler)
    {
        BaseAddress = new Uri(EndPoint)
    }
};
IVaultClient vaultClient = new VaultClient(vaultClientSettings);



// Use client to read a key-value secret.
try
{
    Secret<SecretData> kv2Secret = await vaultClient.V1.Secrets.KeyValue.V2
    .ReadSecretAsync(path: "passwords", mountPoint: "secret");

    Secret<SecretData> kv2Secret1 = await vaultClient.V1.Secrets.KeyValue.V2
   .ReadSecretAsync(path: "Brugernavn", mountPoint: "secret");

    var minkode = kv2Secret.Data.Data["hnrk"];
    var minkode1 = kv2Secret1.Data.Data["saka"];
    Console.WriteLine($"Henriks password er: {minkode}");
    Console.WriteLine($"Sakariyes brugernavn er: {minkode1}");
}
catch (Exception e)
{
    Console.WriteLine("Noget gik galt: " + e.InnerException.Message);
}

