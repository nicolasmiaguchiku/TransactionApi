using TransactionsApi.Interfaces;
using TransactionsApi.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace TransactionsApi.Services
{
    public class TransactionPdfService : ITransactionPdfService
    {
        public byte[] GenerateStatementPdf(List<Transaction> transactions, string userName)
        {
            var br = new System.Globalization.CultureInfo("pt-BR");

            var sortedTransactions = transactions
                .OrderByDescending(t => t.Date)
                .ToList();

            var totalIncome = transactions
                .Where(t => t.Type == "Entrada")
                .Sum(t => t.Amount);

            var totalExpense = transactions
                .Where(t => t.Type == "Saida")
                .Sum(t => t.Amount);

            var saldoFinal = totalIncome - totalExpense;

            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);
                    page.Size(PageSizes.A4);

                    page.Header().Text($"Extrato de Transações - {userName}")
                                 .FontSize(20)
                                 .Bold()
                                 .AlignCenter();

                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        col.Spacing(15);

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Título").Bold();
                                header.Cell().Text("Tipo").Bold();
                                header.Cell().Text("Valor").Bold();
                                header.Cell().Text("Data").Bold();
                            });

                            foreach (var t in sortedTransactions)
                            {
                                table.Cell().Text(t.Title);
                                table.Cell().Text(t.Type);
                                table.Cell().Text(t.Amount.ToString("C", br))
                                    .FontColor(t.Type == "Saida" ? Colors.Red.Medium : Colors.Green.Darken2);
                                table.Cell().Text(t.Date.ToString("dd/MM/yyyy"));
                            }
                        });

                        col.Item().PaddingTop(15).Text($"Total de Entrada: {totalIncome.ToString("C", br)}")
                            .Bold().FontSize(12).FontColor(Colors.Green.Darken2);

                        col.Item().Text($"Total de Saida: {totalExpense.ToString("C", br)}")
                            .Bold().FontSize(12).FontColor(Colors.Red.Medium);

                        col.Item().Text($"Saldo Final: {saldoFinal.ToString("C", br)}")
                            .Bold().FontSize(14)
                            .FontColor(saldoFinal < 0 ? Colors.Red.Darken2 : Colors.Green.Darken2);
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Gerado em: ").SemiBold();
                        text.Span($"{DateTime.Now:dd/MM/yyyy HH:mm}");
                    });
                });
            }).GeneratePdf();

            return pdfBytes;
        }
    }
}
