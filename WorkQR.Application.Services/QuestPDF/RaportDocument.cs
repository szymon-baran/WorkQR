using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace WorkQR.Application
{
    public class RaportDocument : IDocument
    {
        private RaportDocumentVM _model { get; }
        private List<RaportEmployeeDTO> _employees { get; }

        public RaportDocument(RaportDocumentVM model, List<RaportEmployeeDTO> employees)
        {
            _model = model;
            _employees = employees;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.Margin(50);

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);


                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
        }

        void ComposeHeader(IContainer container)
        {
            var headerStyle = TextStyle.Default.FontSize(22).SemiBold().FontColor("#d8904d").LetterSpacing(1);
            var titleStyle = TextStyle.Default.FontSize(18).SemiBold();

            container.BorderBottom(1).BorderColor(Colors.Black).Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().PaddingBottom(10).Text($"workQR").Style(headerStyle);
                    column.Item().PaddingBottom(5).Text($"Raport").Style(titleStyle);

                    column.Item().Text(text =>
                    {
                        text.Span("Wygenerowano: ").SemiBold();
                        text.Span($"{DateTime.Now}");
                    });

                    column.Item().Text(text =>
                    {
                        text.Span("Przedział czasowy: ").SemiBold();
                        text.Span($"{_model.DateFrom.ToShortDateString()} - {_model.DateTo.ToShortDateString()}");
                    });
                });

            });
        }

        void ComposeContent(IContainer container)
        {
            container.PaddingVertical(40).Column(column =>
            {
                column.Spacing(5);

                column.Item().PaddingTop(15).Element(ComposeTable);
                column.Item().PaddingTop(10).Element(ComposeDisclaimer);
            });
        }

        void ComposeTable(IContainer container)
        {
            container.Table(table =>
            {
                // step 1
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(25);
                    columns.RelativeColumn(3);
                    columns.RelativeColumn();
                });

                // step 2
                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("#");
                    header.Cell().Element(CellStyle).Text("Pracownik");
                    header.Cell().Element(CellStyle).AlignRight().Text("Przepracowany czas");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });

                // step 3
                foreach (var employee in _employees)
                {
                    table.Cell().Element(CellStyle).Text(_employees.IndexOf(employee) + 1);
                    table.Cell().Element(CellStyle).Text($"{employee.LastName} {employee.FirstName}");
                    table.Cell().Element(CellStyle).AlignRight().Text($"{employee.WorkedHours}h");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                    }
                }
            });
        }

        void ComposeDisclaimer(IContainer container)
        {
            container.PaddingTop(10).Column(column =>
            {
                column.Item().Text("Raport został wygenerowany z wykorzystaniem aplikacji workQR.");
            });
        }


    }
}