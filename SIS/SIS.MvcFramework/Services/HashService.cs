namespace SIS.MvcFramework.Services
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using Logger;

    public class HashService : IHashService
    {
        private readonly ILogger logger;

        public HashService(ILogger logger)
        {
            this.logger = logger;
        }

        public string Hash(string stringToHash)
        {
            
            stringToHash = stringToHash + "myAppSalt1234235251234#";
            // SHA256 is disposable by inheritance.  
            using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));
                // Get the hashed string.  
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                this.logger.Log(hash);
                return hash;
            }

        }
    }
}
