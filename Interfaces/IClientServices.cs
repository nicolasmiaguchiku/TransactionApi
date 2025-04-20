using TransactionsApi.Models;
using TransactionsApi.Models.ModelsResquets;

namespace TransactionsApi.Interfaces
{
    public interface IClientServices
    {
        public Task<Client> Register(RegisterViewModel clientResgister);

        public Task<Client?> AuthenticateUser(string email,string password);
    }
}
