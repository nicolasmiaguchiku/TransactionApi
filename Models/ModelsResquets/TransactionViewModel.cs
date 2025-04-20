

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
        public string Title { get; set; } = string.Empty;

        public double Amount { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public TransactionType Type { get; set; } = TransactionType.Saida;

        public string Descriptor { get; set; } = string.Empty;

    }
}
