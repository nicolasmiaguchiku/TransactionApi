using TransactionsApi.Models;
using TransactionsApi.Models.ModelsResquets;

namespace TransactionsApi.Interfaces
{
    public interface IClientServices
    {
        public Task<Result> Register(RegisterViewModel clientResgister);

        public Task<ResultData<Client>> AuthenticateUser(string email,string password);
    }
}
