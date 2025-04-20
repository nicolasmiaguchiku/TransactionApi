using TransactionsApi.Interfaces;

namespace TransactionsApi.Services
{
    public class SecurityServices : ISecurityServices
    {
        public bool ComparePassword(string password, string ConfirmPassword)
            => password.Trim().Equals(ConfirmPassword.Trim()); 
         
        public string EncryptPassword(string password)
            => new (BCrypt.Net.BCrypt.HashPassword(password));

        public bool VerifyPassword(string password, string passwordHash)
            => BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}
