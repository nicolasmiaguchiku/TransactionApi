using Microsoft.AspNetCore.Mvc;
using TransactionsApi.Models.ModelsResquets;
using TransactionsApi.Interfaces;

using TransactionsApi.Services;

namespace TransactionsApi.Controllers
{
    [Route("api[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientServices _clientServices;


        public ClientController(IClientServices clientServices)
        {
            _clientServices = clientServices;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterClient(RegisterViewModel registerClient)
        {
            var client = await _clientServices.Register(registerClient);
            return Ok(client);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginViewModel client)
        {

            var user = await _clientServices!.AuthenticateUser(client.Email, client.Password);

            var token = TokenService.GenerateToken(user!);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(2)
            };

            Response.Cookies.Append("auth_token", token, cookieOptions);

            return Ok(token);

        }




    }
}
