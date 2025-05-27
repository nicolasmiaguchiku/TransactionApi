using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TransactionsApi.Models.ModelsResquets;
using TransactionsApi.Interfaces;
using Microsoft.AspNetCore.Authorization;


namespace TransactionsApi.Controllers
{
    [ApiController]
    [Route("ApiController")]
    [Authorize]
    public class TransactionsController : ControllerBase
    {

        private readonly ITransactionsServices _transactionServices;

        public TransactionsController(ITransactionsServices transactionServices)
        {
            _transactionServices = transactionServices ?? throw new ArgumentNullException(nameof(transactionServices));
        }


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

        [HttpGet]
        [Route("GetTransactions")]
        public async Task<IActionResult> GetransactionsClient()
        {
            string? userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            int userId = int.Parse(userIdClaim!);

            try
            {
                var transactionsResult = await _transactionServices.GetTransactionsByUser(userId);

                if (!transactionsResult.IsSuccess || transactionsResult.Data == null || transactionsResult.Data.Count == 0)
                {

                    return StatusCode(200, transactionsResult.Message);
                }

                return Ok(transactionsResult.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro inesperado ao buscar transações.", details = ex.Message });
            }

        }


        [HttpGet]
        [Route("GetInboundTransactions")]
        public async Task<IActionResult> GetTransactionsIncome()
        {
            var userClaim = User.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            int userId = int.Parse(userClaim!);

            try
            {
                var transactionIncome = await _transactionServices.GetTransactionsIncome(userId);

                if (transactionIncome.Data?.Count == 0)
                {
                    return StatusCode(200, transactionIncome?.Message);
                }
                    return Ok(transactionIncome.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro inesperado ao buscar transações.", details = ex.Message });
            }
        }


        [HttpGet]
        [Route("GetOutgoingTransactions")]
        public async Task<IActionResult> GetTransactionsExpense()
        {
            string? clientClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            int clientId = int.Parse(clientClaim!);

            try
            {
                var transactionExpense = await _transactionServices.GetTransactionExpense(clientId);

                if(transactionExpense.Data?.Count == 0)
                {
                    return StatusCode(200, transactionExpense?.Message);
                }

                return Ok(transactionExpense.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro inesperado ao buscar transações.", details = ex.Message });
            }
        }

        [HttpPut]
        [Route("EditTransaction/{id}")]
        public async Task<IActionResult> EditTransaction([FromBody] TransactionViewModel editTransaction, Guid id)
        {
            var clientClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var clientId = int.Parse(clientClaim!);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var editedtransaction = await _transactionServices.EditTransaction(editTransaction, clientId, id);
                if (!editedtransaction.IsSuccess || editedtransaction.Data == null)
                {
                    return StatusCode(404, editedtransaction.Message);
                }
                return Ok(editedtransaction.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro inesperado ao editar essa transação", details = ex.Message });
            }
        }

        [HttpDelete]
        [Route("DeleteTransaction/{TransactionId}")]
        public async Task<IActionResult> DeleteTransaction(Guid TransactionId)
        {
            var clientClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var clientId = int.Parse(clientClaim!);

            try
            {
                var deleteTransaction = await _transactionServices.DeleteTransaction(TransactionId, clientId);

                if (!deleteTransaction.IsSuccess || deleteTransaction.Data == null)
                {
                    return StatusCode(404, deleteTransaction.Message);
                }
                return Ok(deleteTransaction.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro inesperado ao deletar essa transação", details = ex.Message });
            }
        }

    }
}
