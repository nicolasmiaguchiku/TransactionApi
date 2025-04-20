using TransactionsApi.Models;

namespace TransactionsApi.Interfaces
{
    public interface ITransactionPdfService
    {
        byte[] GenerateStatementPdf(List<Transaction> transactions, string userName);
    }
}
