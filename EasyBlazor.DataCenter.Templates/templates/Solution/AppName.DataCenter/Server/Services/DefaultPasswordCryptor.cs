using AppName.DataCenter.Server.Services.Abstractions;
using System.Security.Cryptography;
using System.Text;

namespace AppName.DataCenter.Server.Services
{
    internal class DefaultPasswordCryptor : IPasswordCryptor
    {
        public string Encrypt(string sourceText, string salt)
        {
            return string.Empty;
        }
    }
}