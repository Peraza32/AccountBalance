using CardAPI.Domain.Entities.DTO;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CardAPI.Reports
{
    public class AccountStatementDocument : IDocument
    {
        private readonly AccountBalanceResponseDTO _data;

        public AccountStatementDocument(AccountBalanceResponseDTO data)
        {
            _data = data;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header().Text("Estado de Cuenta - Tarjeta de Crédito")
                    .SemiBold().FontSize(18);

                page.Content().Column(column =>
                {
                    column.Spacing(10);

                    column.Item().Text($"Titular: {_data.cardHolderName}");
                    column.Item().Text($"Tarjeta: **** **** **** {_data.cardNumber}");

                    column.Item().PaddingTop(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        AddRow(table, "Saldo Total", _data.currentCredit.ToString("C"));
                        AddRow(table, "Límite de Crédito", _data.creditLimit.ToString("C"));
                        AddRow(table, "Saldo Disponible", _data.availableCredit.ToString("C"));
                        AddRow(table, "Interés Bonificable", _data.bonificationInterest.ToString("C"));
                        AddRow(table, "Cuota Mínima a Pagar", _data.minimumPayment.ToString("C"));
                        AddRow(table, "Monto Total a Pagar", _data.totalWithInterest.ToString("C"));
                        AddRow(table, "Contado con Intereses", _data.totalWithInterest.ToString("C"));
                    });

                    column.Item().PaddingTop(15).Text("Compras del mes").Bold();

                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(4);
                            columns.RelativeColumn(2);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Fecha").Bold();
                            header.Cell().Text("Descripción").Bold();
                            header.Cell().Text("Monto").Bold();
                        });

                        foreach (var compra in _data.purchases ?? new())
                        {
                            table.Cell().Text(compra.purchaseDate.ToString("dd/MM/yyyy"));
                            table.Cell().Text(compra.description);
                            table.Cell().Text(compra.price.ToString("C"));
                        }
                    });
                });

                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Generado el ");
                    x.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                });
            });
        }

        private void AddRow(TableDescriptor table, string label, string value)
        {
            table.Cell().Text(label);
            table.Cell().AlignRight().Text(value);
        }
    }
}
