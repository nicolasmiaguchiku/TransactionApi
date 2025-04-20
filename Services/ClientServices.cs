using Microsoft.EntityFrameworkCore;
using TransactionsApi.Context;
using TransactionsApi.Interfaces;
using TransactionsApi.Models;
using TransactionsApi.Models.ModelsResquets;

namespace TransactionsApi.Services
{
    public class ClientServices : IClientServices
    {
        private readonly DataContext _dataContext;
        private readonly SecurityServices _securityServices;

        public ClientServices(DataContext dataContext, SecurityServices securityServices)
        {
            _dataContext = dataContext;
            _securityServices = securityServices;
        }

        public async Task<Client?> AuthenticateUser(string email,string password)
        {
            var client = await _dataContext.Clients.SingleOrDefaultAsync(x => x.Email == email);


            if (client == null || string.IsNullOrEmpty(client.PasswordHash) || !_securityServices.VerifyPassword(password, client.PasswordHash))
            {
                throw new InvalidOperationException("Email ou senha incorreto");
            }

            return client;

        }

        public async Task<Client> Register(RegisterViewModel clientRegister)
        {
            bool exists = await _dataContext.Clients.AnyAsync(c => c.Email == clientRegister.Email);

            if (exists)
                throw new InvalidOperationException("Email já registrado");

            var passwordEncrypted = _securityServices.EncryptPassword(clientRegister.Password);

            var newClient = new Client
            {
                Name = clientRegister.Name,
                Email = clientRegister.Email,
                PasswordHash = passwordEncrypted
            };

            _dataContext.Clients.Add(newClient);
            await _dataContext.SaveChangesAsync();

            return newClient;
        }
    }
}
