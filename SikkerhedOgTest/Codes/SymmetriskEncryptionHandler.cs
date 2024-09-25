using Microsoft.AspNetCore.DataProtection;

namespace SikkerhedOgTest.Codes
{
    public class SymmetriskEncryptionHandler
    {
        private readonly IDataProtector _dataProtector; //key

            public SymmetriskEncryptionHandler(IDataProtectionProvider dataProtectionProvider)
            {
                _dataProtector = dataProtectionProvider.CreateProtector("SymmetricEncryption");
            }

            public string Protect(string textToEncrypt)
            {
                return _dataProtector.Protect(textToEncrypt);
            }

            public string Unprotect(string textToDecrypt)
            {
                return _dataProtector.Unprotect(textToDecrypt);
            }
        }
}
