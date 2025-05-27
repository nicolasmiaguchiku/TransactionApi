using TransactionsApi.Models;
using TransactionsApi.Models.ModelsResquets;

namespace TransactionsApi.Interfaces
{
    public interface ITransactionsServices
    {
        public Task<ResultData<Transaction>> AddTransacion(TransactionViewModel newTransaction, int clientId);
        public Task<ResultData<List<Transaction>>> GetTransactionsByUser(int clientId);
        public Task<ResultData<List<Transaction>>> GetTransactionsIncome(int clientId);
        public Task<ResultData<List<Transaction>>> GetTransactionExpense(int clientId);
        public Task<ResultData<Transaction>> EditTransaction(TransactionViewModel editTransaction, int clientId, Guid transactionId);
        public Task<ResultData<Transaction>> DeleteTransaction(Guid transactionId, int clientId);
     
    }
}
