using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Scente.API.Entity;

namespace Scente.API.Documents;

// WEEK 3 — PDF invoice using QuestPDF.
// Package already installed: QuestPDF
// Program.cs already has: QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
public static class InvoiceDocument
{
    public static byte[] Generate(Order order, User user)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(11).FontColor(Colors.Black));

                // ---- Header ----
                page.Header().Column(col =>
                {
                    col.Item().Text("SCENTÉ").FontSize(26).Bold();
                    col.Item().Text("Invoice").FontSize(14).FontColor(Colors.Grey.Darken1);
                });

                // ---- Content ----
                page.Content().PaddingVertical(20).Column(col =>
                {
                    col.Spacing(10);

                    // Order info + customer
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text($"Order: {order.OrderNumber}").Bold();
                            c.Item().Text($"Date: {order.Date:dd MMM yyyy}");
                            c.Item().Text($"Status: {order.Status}");
                        });
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text("Billed to").Bold();
                            c.Item().Text($"{user.FirstName} {user.LastName}");
                            c.Item().Text(user.Email);
                        });
                    });

                    // Items table
                    col.Item().PaddingTop(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(4); // Product
                            columns.RelativeColumn(1); // Size
                            columns.RelativeColumn(1); // Qty
                            columns.RelativeColumn(2); // Price
                            columns.RelativeColumn(2); // Subtotal
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Product").Bold();
                            header.Cell().Text("Size").Bold();
                            header.Cell().Text("Qty").Bold();
                            header.Cell().AlignRight().Text("Price").Bold();
                            header.Cell().AlignRight().Text("Subtotal").Bold();
                        });

                        foreach (var item in order.Items)
                        {
                            table.Cell().Text(item.ProductName);
                            table.Cell().Text(item.Size);
                            table.Cell().Text(item.Quantity.ToString());
                            table.Cell().AlignRight().Text($"${item.Price:0.00}");
                            table.Cell().AlignRight().Text($"${item.Price * item.Quantity:0.00}");
                        }
                    });

                    // Total
                    col.Item().AlignRight().PaddingTop(10)
                        .Text($"Total Paid: ${order.TotalPaid:0.00}")
                        .FontSize(14).Bold();
                });

                // ---- Footer ----
                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Thank you for shopping with Scenté  •  ");
                    x.Span($"Generated {DateTime.UtcNow:dd MMM yyyy}").FontColor(Colors.Grey.Medium);
                });
            });
        });

        return document.GeneratePdf();
    }
}
