using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TransactionsApi.Models.ModelsResquets;
using TransactionsApi.Interfaces;
using Microsoft.AspNetCore.Authorization;


namespace TransactionsApi.Controllers
{
    [ApiController]
    [Route("ApiController")]
    public class TransactionsController : ControllerBase
    {

        private readonly ITransactionsServices _transactionServices;

        public TransactionsController(ITransactionsServices transactionServices)
        {
            _transactionServices = transactionServices ?? throw new ArgumentNullException(nameof(transactionServices));
        }

        [Authorize]
        [HttpPost]
        [Route("AddTransaction")]
        public async Task<IActionResult> AddTransaction([FromBody] TransactionViewModel transaction)
        {
            var clientClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (clientClaim == null || !int.TryParse(clientClaim, out int clientId))
            {
                return Unauthorized(new { error = "Usuário não autenticado ou ID inválido." });
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            try
            {
                var newTransaction = await _transactionServices.AddTransacion(transaction, clientId);

                return StatusCode(200, newTransaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro ao adicionar transação", details = ex.Message });
            }


        }

        [Authorize]
        [HttpGet]
        [Route("GetTransactions")]
        public async Task<IActionResult> GetransactionsClient()
        {
            string? userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            int userId = int.Parse(userIdClaim!);

            try
            {
                var transactions = await _transactionServices.GetTransactionsByUser(userId);
                return Ok(transactions);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro inesperado ao buscar transações.", details = ex.Message });
            }

        }

        [Authorize]
        [HttpGet]
        [Route("GetInboundTransactions")]
        public async Task<IActionResult> GetTransactionsIncome()
        {
            var userClaim = User.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            int userId = int.Parse(userClaim!);

            try
            {
                var transactionIncome = await _transactionServices.GetTransactionsIncome(userId);

                return Ok(transactionIncome);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro inesperado ao buscar transações.", details = ex.Message });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetOutgoingTransactions")]
        public async Task<IActionResult> GetTransactionsExpense()
        {
            string? clientClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            int clientId = int.Parse(clientClaim!);

            try
            {
                var transactionExpense = await _transactionServices.GetTransactionExpense(clientId);

                return Ok(transactionExpense);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { error = "Erro inesperado ao buscar transações.", details = ex.Message });
            }
        }

    }
}
