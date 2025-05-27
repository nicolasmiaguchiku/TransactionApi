using Microsoft.EntityFrameworkCore;
using TransactionsApi.Context;
using TransactionsApi.Interfaces;
using TransactionsApi.Models;
using TransactionsApi.Models.ModelsResquets;

namespace TransactionsApi.Services
{
    public class TransactionsServices : ITransactionsServices
    {
        private readonly DataContext _dataContext;

        public TransactionsServices(DataContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public async Task<ResultData<List<Transaction>>> GetTransactionsByUser(int clientId)
        {
            Wallet? wallet = await _dataContext.Wallets
                                           .Include(w => w.Transactions)
                                           .FirstOrDefaultAsync(w => w.ClientId == clientId);

            ResultData<List<Transaction>> result;

            if (wallet == null)
            {
                result = ResultData<List<Transaction>>.Error("Carteira não encontrada para o usuário.");
            }

            if (wallet?.Transactions == null || wallet.Transactions.Count == 0)
            {
                result = ResultData<List<Transaction>>.Error("Nenhuma transação encontrada para o usuário.");
            }
            else
            {
                var transactions = wallet.Transactions?.ToList();

                result = ResultData<List<Transaction>>.Success(transactions!, "");
            }

            return result;
        }

        public async Task<ResultData<Transaction>> AddTransacion(TransactionViewModel newTransaction, int clientId)
        {
            var wallet = await _dataContext.Wallets
                                   .FirstOrDefaultAsync(w => w.ClientId == clientId);

            ResultData<Transaction> result;

            if (wallet == null)
            {
                result = ResultData<Transaction>.Error("Carteira não encontrada");
            }
            else
            {
                Transaction transaction = new()
                {
                    Title = newTransaction.Title,
                    Amount = newTransaction.Amount,
                    Descriptor = newTransaction.Descriptor,
                    Type = newTransaction.Type.ToString(),
                    Date = (DateTime)newTransaction.Date,
                    WalletId = wallet.WalletId
                };

                _dataContext.Transactions.Add(transaction);
                await _dataContext.SaveChangesAsync();

                result = ResultData<Transaction>.Success(transaction, "Transação adicionada com sucesso");

            }
            return result;
        }

        public async Task<ResultData<List<Transaction>>> GetTransactionsIncome(int clientId)
        {
            Wallet? wallet = await _dataContext.Wallets.Include(w => w.Transactions)
                                                .FirstOrDefaultAsync(w => w.ClientId == clientId);

            var transactionsIncome = wallet?.Transactions?.Where(t => t.Type == "Entrada").ToList();

            ResultData<List<Transaction>> result;

            if (wallet == null)
            {
                result = ResultData<List<Transaction>>.Error("Carteira não encontrada para usuário");
            }
            else if (transactionsIncome?.Count == 0)
            {

                result = ResultData<List<Transaction>>.Success(transactionsIncome!, "Nenhuma transação de entrada encontrada nesta carteira");
            }
            else
            {
                result = ResultData<List<Transaction>>.Success(transactionsIncome!, "");
            }


            return result;
        }

        public async Task<ResultData<List<Transaction>>> GetTransactionExpense(int clientId)
        {
            var wallet = await _dataContext.Wallets.Include(w => w.Transactions)
                                                   .FirstOrDefaultAsync(w => w.ClientId == clientId);

            var transactionsExpense = wallet?.Transactions?.Where(t => t.Type == "Saida").ToList();

            ResultData<List<Transaction>> result;

            if (wallet == null)
            {
                result = ResultData<List<Transaction>>.Error("Carteira não encontrada para usuário");
            }
            else if (transactionsExpense?.Count == 0)
            {
                result = ResultData<List<Transaction>>.Success(transactionsExpense, "Nenhuma transação de saída encontrada nesta carteira");
            }
            else
            {
                result = ResultData<List<Transaction>>.Success(transactionsExpense!, "");
            }

            return result;
        }

        public async Task<ResultData<Transaction>> EditTransaction(TransactionViewModel editTransaction, int clientId, int transactionId)
        {
            var wallet = await _dataContext.Wallets
                                           .Include(w => w.Transactions)
                                           .FirstOrDefaultAsync(w => w.ClientId == clientId);

            var transactionBanco = wallet?.Transactions?.FirstOrDefault(t => t.TransactionId == transactionId);

            ResultData<Transaction> result;

            if (transactionBanco == null)
            {
                result = ResultData<Transaction>.Error("Transação não encontrada na carteira do cliente");
            }
            else
            {
                transactionBanco.Title = editTransaction.Title;
                transactionBanco.Amount = editTransaction.Amount;
                transactionBanco.Date = editTransaction.Date;
                transactionBanco.Type = editTransaction.Type.ToString();
                transactionBanco.Descriptor = editTransaction.Descriptor;

                _dataContext.Transactions.Update(transactionBanco);
                await _dataContext.SaveChangesAsync();

                result = ResultData<Transaction>.Success(transactionBanco, "Transação atualizada com sucesso");
            }

            return result;

        }

        public async Task<ResultData<Transaction>> DeleteTransaction(int transactionId, int clientId)
        {
            var wallet = await _dataContext.Wallets
                                           .Include(w => w.Transactions)
                                           .FirstOrDefaultAsync(c => c.ClientId == clientId);

            var transactionDelete = wallet?.Transactions?.FirstOrDefault(t => t.TransactionId == transactionId);

            ResultData<Transaction> result;

            if (transactionDelete == null)
            {
                result = ResultData<Transaction>.Error("Transação não encontrada na carteira do usuário");
            }
            else
            {
                result = ResultData<Transaction>.Success(transactionDelete!, "Transação deletada com sucesso");

                _dataContext.Transactions.Remove(transactionDelete!);
                await _dataContext.SaveChangesAsync();

            }


            return result;

        }
    }
}
