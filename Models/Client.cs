using System.ComponentModel.DataAnnotations;

namespace TransactionsApi.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public Wallet Wallet { get; set; } = new Wallet();

    }
}
