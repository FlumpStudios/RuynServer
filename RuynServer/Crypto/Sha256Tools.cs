using System.Security.Cryptography;
using System.Text;

namespace RuynServer.Crypto
{
    public static class Sha256Tools
    {
        public static string GenerateHashString(in byte[] dataBytes)
        {
            if (dataBytes is null) { return string.Empty; }

            using SHA256 sha256Hash = SHA256.Create();
            byte[] hashBytes = sha256Hash.ComputeHash(dataBytes);

            StringBuilder hashString = new();
            foreach (byte b in hashBytes)
            {
                hashString.Append(b.ToString("x2"));
            }

            // Output the hash
            return hashString.ToString();
        }
    }
}
