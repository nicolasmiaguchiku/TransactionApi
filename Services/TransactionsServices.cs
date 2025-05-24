
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public async Task<IReadOnlyList<Transaction>> GetTransactionsByUser(int clientId)
        {
            Wallet? wallet = await _dataContext.Wallets
                                           .Include(w => w.Transactions)
                                           .FirstOrDefaultAsync(w => w.ClientId == clientId);

            if (wallet == null)
            {
                throw new KeyNotFoundException("Carteira não encontrada para o usuário.");
            }

            if(wallet.Transactions == null || wallet.Transactions.Count == 0)
            {
                throw new InvalidOperationException("Nenhuma transação encontrada para o usuário.");
            }

            return wallet.Transactions;
            
        }

        public async Task<ResultData<Transaction>> AddTransacion(TransactionViewModel newTransaction, int Id)
        {
            var wallet = await _dataContext.Wallets
                                   .FirstOrDefaultAsync(w => w.ClientId == Id);

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

                result = ResultData<Transaction>.Success(transaction);

            }
                return result;
        }

        public async Task<List<Transaction>> GetTransactionsIncome(int clientId)
        {
            Wallet? wallet = await _dataContext.Wallets.Include(w => w.Transactions)
                                                .FirstOrDefaultAsync(w => w.ClientId == clientId);

            var TransactionsIncome = wallet?.Transactions?.Where(t => t.Type == "Entrada");


            return (List<Transaction>)TransactionsIncome!;
        }

        public async Task<List<Transaction>> GetTransactionExpense(int clientId)
        {
            var wallet = await _dataContext.Wallets.Include(w => w.Transactions)
                                                   .FirstOrDefaultAsync(w => w.ClientId == clientId);

            var transactionsExpense = wallet?.Transactions?.Where(t => t.Type == "Saida");

            return (List<Transaction>)transactionsExpense!;
        }
    }
}
