using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace SikkerhedOgTest.Codes;


public class AsymmetriskEncryptionHandler
{
    private string _publicKey;
    private string _privateKey;
    private readonly HttpClient _httpClient;

    public AsymmetriskEncryptionHandler(HttpClient httpClient)
    {
        _httpClient = httpClient;
        LoadKeysFromXmlFiles();

        //using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        //{
        //    //gør persistent og gem i en fil 
        //    //_privateKey = rsa.ToXmlString(true);
        //    //_publicKey = rsa.ToXmlString(false);
        //}


    }

    public void LoadKeysFromXmlFiles()
    {
        string privateKeyPath = "privateKey.xml";
        string publicKeyPath = "publicKey.xml";

        if (File.Exists(privateKeyPath) && File.Exists(publicKeyPath))
        {
            _privateKey = File.ReadAllText(privateKeyPath);
            _publicKey = File.ReadAllText(publicKeyPath);
        }
        else
        {
            SaveKeysToXmlFiles();
        }
    }

    public void SaveKeysToXmlFiles()
    {
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            // Generate keys if they don't exist
            string privateKeyPath = "privateKey.xml";
            string publicKeyPath = "publicKey.xml";

            if (!File.Exists(privateKeyPath) || !File.Exists(publicKeyPath))
            {
                // Save the private key to a file
                string privateKeyXml = rsa.ToXmlString(true);
                File.WriteAllText(privateKeyPath, privateKeyXml);

                // Save the public key to a file
                string publicKeyXml = rsa.ToXmlString(false);
                File.WriteAllText(publicKeyPath, publicKeyXml);
                LoadKeysFromXmlFiles();
            }
        }
    }

    public async Task<string> AsymmetriskEncrypt(string textToEncrypt)
    {
        string[] param = new string[] {textToEncrypt, _publicKey};
        string serializeObject = JsonConvert.SerializeObject(param);
        StringContent stringContent = new StringContent(serializeObject, System.Text.Encoding.UTF8, "application/json");
        var encryptedValue = await _httpClient.PostAsync("https://localhost:7277/api/Encryption", stringContent);
        string encryptedValueAsString = encryptedValue.Content.ReadAsStringAsync().Result;
        return encryptedValueAsString;
    }

    public async Task<string> AsymmetriskDecrypt(string textToDecrypt)
    {

        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(_privateKey);
            byte[] byteArrayTextToDecrypt = Convert.FromBase64String(textToDecrypt);
            var decryptedValue = rsa.Decrypt(byteArrayTextToDecrypt, true);
            string decryptedValueAsString = System.Text.Encoding.UTF8.GetString(decryptedValue);

            return decryptedValueAsString;
        }

        
    }


}
