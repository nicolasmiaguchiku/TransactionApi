using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace TransactionsApi.Models
{
    public class Wallet
    {
        [Key]
        public int WalletId { get; set; }
        public int ClientId { get; set; }
        [JsonIgnore]
        public Client? Client { get; set; }
        public List<Transaction>? Transactions { get; set; }

    }
}
