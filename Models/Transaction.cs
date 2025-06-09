using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TransactionsApi.Models
{
 
    public class Transaction
    {
        [Key]
        public Guid TransactionId { get; set; }
        public int WalletId { get; set; }
        [JsonIgnore]
        public Wallet? Wallet { get; set; }  
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Descriptor { get; set; }



    }
}
