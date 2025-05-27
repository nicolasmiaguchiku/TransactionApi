using System.ComponentModel.DataAnnotations;

namespace TransactionsApi.Models.ModelsResquets
{
    public enum TransactionType
    {
        Saida,
        Entrada,
        Investimento,
    }
    public class TransactionViewModel
    {
        [Required(ErrorMessage = "O título da transação é obrigatório.")]
        public string Title { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "O valor da transação deve ser maior que zero.")]
        public double Amount { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public TransactionType Type { get; set; } = TransactionType.Saida;

        public string Descriptor { get; set; } = string.Empty;

        public TransactionViewModel(string title, double amount, DateTime date, TransactionType type, string descriptor)
        {
            Title = title;
            Amount = amount;
            Date = date;
            Type = type;
            Descriptor = descriptor;
        }
    }
}
