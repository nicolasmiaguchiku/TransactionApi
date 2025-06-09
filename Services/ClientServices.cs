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

        public async Task<ResultData<Client>> AuthenticateUser(string email,string password)
        {
            var client = await _dataContext.Clients.SingleOrDefaultAsync(x => x.Email == email);

            ResultData<Client> result;

            if (client == null || string.IsNullOrEmpty(client.PasswordHash) || !_securityServices.VerifyPassword(password, client.PasswordHash))
            {
                result = ResultData<Client>.Error("Email ou senha incorreto");
            }
            else
            {
                result = ResultData<Client>.Success(client, "");
            }
            return result;

        }

        public async Task<Result> Register(RegisterViewModel clientRegister)
        {
            bool exists = await _dataContext.Clients.AnyAsync(c => c.Email == clientRegister.Email);
            ResultData<Client> result;

            if (exists)
            {
                string? message = "Email já registrado";

                result = ResultData<Client>.Error(message);
            }
            else 
            {
                var passwordEncrypted = _securityServices.EncryptPassword(clientRegister.Password);

                var newClient = new Client
                {
                    Name = clientRegister.Name,
                    Email = clientRegister.Email,
                    PasswordHash = passwordEncrypted
                };


                result = ResultData<Client>.Success(newClient, "Usuário registrado com sucesso");

                _dataContext.Clients.Add(newClient);
                await _dataContext.SaveChangesAsync();

            }

            return result;
        }
    }
}
