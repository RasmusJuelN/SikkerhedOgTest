using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Kryptering.Hashing
{
    public class HashingHandler
    {
        public string BCryptHashing(string textToHash)
        {
            int workFactor = 100;
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            //bool enhancedEntropy = true;
            //HashType hashType = HashType.SHA256;
            return BCrypt.Net.BCrypt.HashPassword(textToHash, salt, true, HashType.SHA256);

            //int workFactor = 100;
            //string salt = BCrypt.Net.BCrypt.GenerateSalt();
            //bool enhancedEntropy = true;

            //return BCrypt.Net.BCrypt.HashPassword(textToHash, salt, enhancedEntropy);

            //BCrypt.Net.BCrypt.HashPassword(textToHash);
        }


        public bool BCryptVerify(string textToVerify, string hashedValueFromDb)
        {
            return BCrypt.Net.BCrypt.Verify(textToVerify, hashedValueFromDb, true, HashType.SHA256);
            //return BCrypt.Net.BCrypt.Verify(textToVerify, hashedValueFromDb, true);

            //BCrypt.Net.BCrypt.Verify(textToVerify, hashedValueFromDb);
        }

        public string MD5Hashing(string textToHash)
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(textToHash);
            MD5 md5 = MD5.Create();
            byte[] hashedValue = md5.ComputeHash(inputBytes);
            return Convert.ToBase64String(hashedValue);
        }

        public string SHA256Hashing(string textToHash)
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(textToHash);
            SHA256 sha256 = SHA256.Create();
            byte[] hashedValue = sha256.ComputeHash(inputBytes);
            return Convert.ToBase64String(hashedValue);
        }

        public string HMACHashing(string textToHash)
        {
            byte[] myKey = Encoding.ASCII.GetBytes("VERYSECRETKEYHAHAHAHA");
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(textToHash);
            HMACSHA256 hmacsha256 = new HMACSHA256();
            hmacsha256.Key = myKey;
            byte[] hashedValue = hmacsha256.ComputeHash(inputBytes);
            return Convert.ToBase64String(hashedValue);

        }

        public string PBKDF2Hashing(string textToHash)
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(textToHash);
            byte[] salt = Encoding.ASCII.GetBytes("SALTYRAT");
            var hashAlgorithm = new HashAlgorithmName("SHA256");
            byte[] hashedValue = Rfc2898DeriveBytes.Pbkdf2(inputBytes, salt, 100, hashAlgorithm, 32);
            return Convert.ToBase64String(hashedValue);
        }
    }
}
