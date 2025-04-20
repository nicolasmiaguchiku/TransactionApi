using TransactionsApi.Models;
using TransactionsApi.Models.ModelsResquets;

namespace TransactionsApi.Interfaces
{
    public interface ITransactionsServices
    {
        public Task<Transaction> AddTransacion(TransactionViewModel newTransaction, int userId);
        public Task<IReadOnlyList<Transaction>> GetTransactionsByUser(int clientId);
        public Task<List<Transaction>> GetTransactionsIncome(int clientId);
        public Task<List<Transaction>> GetTransactionExpense(int clientId);
     
    }
}
