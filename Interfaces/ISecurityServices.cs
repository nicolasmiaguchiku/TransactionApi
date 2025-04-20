namespace TransactionsApi.Interfaces
{
    public interface ISecurityServices
    {
        public string EncryptPassword(string password);

        public bool ComparePassword(string password, string ConfirmPassword);
        public bool VerifyPassword(string password, string PasswordHash);
    }
}
