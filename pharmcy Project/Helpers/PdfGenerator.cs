using DinkToPdf;
using Pharmcy.Core.Entities;

namespace pharmcy_Project.Helpers
{
    public class PdfGenerator
    {
        public byte[] GenerateInvoicePdf(Invoice invoice)
        {
            var htmlContent = $"<h1>Invoice #{invoice.Id}</h1><p>Date: {invoice.IssueDate}</p>";

            var converter = new BasicConverter(new PdfTools());
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
            },
                Objects = {
                new ObjectSettings() {
                    HtmlContent = htmlContent,
                    WebSettings = { DefaultEncoding = "utf-8" }
                }
            }
            };

            return converter.Convert(doc);
        }
    }

}

    