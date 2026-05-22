using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace AliHaydarBase.Api.HelperFunctions
{
    public static class ExportHelper
    {
        public static byte[] ToPdf<T>(IEnumerable<T> data)
        {
            var items = data.ToList();
            var properties = typeof(T).GetProperties();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(20);
                    page.Size(PageSizes.A4);

                    page.Header().Text("Print History Export").FontSize(20).Bold();

                    page.Content().Table(table =>
                    {
                        // Columns
                        table.ColumnsDefinition(columns =>
                        {
                            foreach (var _ in properties)
                                columns.RelativeColumn();
                        });

                        // Header row
                        table.Header(header =>
                        {
                            foreach (var prop in properties)
                                header.Cell().Text(prop.Name).Bold();
                        });

                        // Data rows
                        foreach (var item in items)
                        {
                            foreach (var prop in properties)
                            {
                                var value = prop.GetValue(item)?.ToString() ?? "";
                                table.Cell().Text(value);
                            }
                        }
                    });

                    page.Footer().AlignRight().Text($"Generated at {DateTime.UtcNow}");
                });
            });

            return document.GeneratePdf();
        }

        public static string ToCsv<T>(IEnumerable<T> data)
        {
            var sb = new StringBuilder();
            var properties = typeof(T).GetProperties();

            // Header
            sb.AppendLine(string.Join(",", properties.Select(p => p.Name)));

            // Rows
            foreach (var item in data)
            {
                var values = properties.Select(p =>
                {
                    var value = p.GetValue(item)?.ToString() ?? "";
                    value = value.Replace("\"", "\"\"");
                    return $"\"{value}\"";
                });

                sb.AppendLine(string.Join(",", values));
            }

            return sb.ToString();
        }
    }

}