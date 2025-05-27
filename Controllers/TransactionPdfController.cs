using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TransactionsApi.Interfaces;
using System.Collections.Generic;

namespace TransactionsApi.Controllers
{
    public class TransactionPdfController : ControllerBase
    {
        private readonly ITransactionPdfService _pdfService;
        private readonly ITransactionsServices _transactionsServices;

        public TransactionPdfController(ITransactionsServices transactionServices, ITransactionPdfService pdfService)
        {
            _pdfService = pdfService ?? throw new ArgumentNullException(nameof(pdfService));
            _transactionsServices = transactionServices ?? throw new ArgumentNullException(nameof(transactionServices));
        }

        [Authorize]
        [HttpGet]
        [Route("DownloadStatement")]
        public async Task<IActionResult> DownloadStatement()
        {
            var userClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userClaim == null || !int.TryParse(userClaim, out int userId))
                return Unauthorized(new { error = "Usuário não autenticado." });

            try
            {
                var transactions = await _transactionsServices.GetTransactionsByUser(userId);
                string userName = User.Identity?.Name ?? $"Usuário_{userId}";

                if (!transactions.IsSuccess || transactions.Data == null)
                {
                    return StatusCode(200, transactions.Message );
                }

                var pdfBytes = _pdfService.GenerateStatementPdf(transactions.Data, userName);

                return File(pdfBytes, "application/pdf", "ExtratoTransacoes.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro ao gerar o PDF.", details = ex.Message });
            }
        }
    }
}
