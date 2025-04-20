using System.ComponentModel.DataAnnotations;

namespace TransactionsApi.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public Wallet Wallet { get; set; } = new Wallet();
        
    }
}
