namespace AppName.DataCenter.Server.Services.Abstractions
{
    public interface IPasswordCryptor
    {
        string Encrypt(string sourceText, string salt);
    }
}