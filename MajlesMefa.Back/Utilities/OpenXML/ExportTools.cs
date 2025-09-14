using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using IOFile = System.IO.File;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;

namespace MajlesMefa.Back.Utilities.OpenXML
{
    public static class ExportTools
    {
        public static Paragraph CreateBoldTitle(string text, string size, JustificationValues? justification = null)
        {
            justification ??= JustificationValues.Center;
            Paragraph title = new Paragraph(
                        new ParagraphProperties(
                        new RightToLeftText(),
                        new BiDi() { Val = true }, // Enable RTL
                        new Justification { Val = justification }
                    )
            );
            // Create run properties for font size and bold formatting
            RunProperties runProperties = new RunProperties();
            runProperties.FontSize = new FontSize { Val = size };
            runProperties.Bold = new Bold();
            Run run = new Run();
            run.Append(runProperties);
            run.Append(new Text(text));

            // Append the run to the paragraph
            title.Append(run);
            return title;
        }
        public static Paragraph CreateBoldNameParagraph(string label, string value)
        {
            Paragraph paragraph = new Paragraph(
                new ParagraphProperties(
                    new RightToLeftText(),
                    new BiDi() { Val = true }
                )
            );

            RunProperties boldProperties = new RunProperties();
            boldProperties.Bold = new Bold();

            Run boldRun = new Run();
            boldRun.Append(boldProperties);
            boldRun.Append(new Text(label));

            Run normalRun = new Run();
            normalRun.Append(new Text("\u00A0" + value)); // Non-breaking space

            paragraph.Append(boldRun);
            paragraph.Append(normalRun);

            return paragraph;
        }
        public static Paragraph CreateBoldMeetingParagraph(string label, string value)
        {
            Paragraph paragraph = new Paragraph(
                new ParagraphProperties(
                    new BiDi() { Val = true }
                )
            );

            RunProperties boldProperties = new RunProperties();
            boldProperties.Bold = new Bold();

            Run boldRun = new Run();
            boldRun.Append(boldProperties);
            boldRun.Append(new Text(label));

            Run normalRun = new Run();
            normalRun.Append(new Text("\u00A0" + value)); // Non-breaking space

            paragraph.Append(boldRun);
            paragraph.Append(normalRun);

            return paragraph;
        }
        public static Paragraph CreateRightAlignedParagraph(string text)
        {
            return new Paragraph(
                new ParagraphProperties(
                    new Justification() { Val = JustificationValues.Right }, // Align right
                    new RightToLeftText(),
                    new BiDi() { Val = true } // Enable RTL
                ),
                new Run(new Text(text))
            );
        }
        public static Paragraph CreateCenterAlignedParagraph(string text)
        {
            return new Paragraph(
                new ParagraphProperties(
                    new RightToLeftText(),
                    new Justification() { Val = JustificationValues.Center }, // Align center
                    new BiDi() { Val = true } // Enable RTL
                ),
                new Run(new Text(text))
            );
        }
        public static Table CreateTable<T>(string[] headers, List<Func<T, string>> dataExtractors, List<T> dataList)
        {
            // Initialize the table
            Table table = new Table();

            // Set table properties (e.g., borders)
            TableProperties tableProperties = new TableProperties(
                new TableBorders(
                    new TopBorder { Val = BorderValues.Single, Size = 4 },
                    new BottomBorder { Val = BorderValues.Single, Size = 4 },
                    new LeftBorder { Val = BorderValues.Single, Size = 4 },
                    new RightBorder { Val = BorderValues.Single, Size = 4 },
                    new InsideHorizontalBorder { Val = BorderValues.Single, Size = 4 },
                    new InsideVerticalBorder { Val = BorderValues.Single, Size = 4 }
                ),
                new BiDiVisual(),
                new TableWidth { Width = "5000", Type = TableWidthUnitValues.Pct } // Set table width to 100%
            );
            table.AppendChild(tableProperties);


            // Create the header row
            TableRow headerRow = new TableRow();
            int headerIndex = 0;  // Add this line to track the column index
            foreach (var headerText in headers)
            {

                TableCell headerCell = new TableCell(CreateCenterAlignedParagraph(headerText));
                var cellProperties = new TableCellProperties();


                // Apply gray background shading to header cell
                Shading headerShading = new Shading
                {
                    Val = ShadingPatternValues.Clear,
                    Color = "auto",
                    Fill = "D3D3D3" // Light gray color
                };
                TableCellVerticalAlignment verticalAlignment = new TableCellVerticalAlignment
                {
                    Val = TableVerticalAlignmentValues.Center
                };

                cellProperties.Append(headerShading, verticalAlignment);

                if (headerIndex == 0)
                {
                    cellProperties.Append(
                        new TableCellWidth { Width = "700", Type = TableWidthUnitValues.Dxa }
                    );
                }

                headerCell.TableCellProperties = cellProperties;
                headerRow.Append(headerCell);
                headerIndex++;  // Increment the counter
            }
            table.Append(headerRow);

            // Populate the table with data
            int index = 1;
            foreach (var item in dataList)
            {
                TableRow dataRow = new TableRow();
                string backgroundColor = (index % 2 == 0) ? "E0E0E0" : "FFFFFF"; // Gray for even rows, white for odd rows

                // Add index cell
                dataRow.Append(CreateIndexCell(index.ToString(), backgroundColor));

                // Add data cells
                foreach (var extractor in dataExtractors)
                {
                    string cellValue = extractor(item);
                    dataRow.Append(CreateStyledTableCell(cellValue, backgroundColor, false));
                }

                table.Append(dataRow);
                index++;
            }

            return table;
        }
        public static TableCell CreateIndexCell(string text, string backgroundColor)
        {
            return new TableCell(
                new TableCellProperties(
                    new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center },
                    new Shading { Val = ShadingPatternValues.Clear, Color = "auto", Fill = backgroundColor },
                    new TableCellWidth { Width = "700", Type = TableWidthUnitValues.Dxa }
                ),
                CreateCenterAlignedParagraph(text)
            );
        }
        public static TableCell CreateStyledTableCell(string text, string backgroundColor, bool centerAlign)
        {
            // Create paragraph with specified alignment
            Paragraph paragraph = centerAlign ? CreateCenterAlignedParagraph(text) : CreateRightAlignedParagraph(text);

            // Create table cell properties with background shading
            TableCellProperties cellProperties = new TableCellProperties();
            Shading shading = new Shading
            {
                Val = ShadingPatternValues.Clear,
                Color = "auto",
                Fill = backgroundColor
            };
            TableCellVerticalAlignment verticalAlignment = new TableCellVerticalAlignment
            {
                Val = TableVerticalAlignmentValues.Center
            };
            cellProperties.Append(shading, verticalAlignment);

            // Create table cell and append properties and paragraph
            TableCell cell = new TableCell();
            cell.Append(cellProperties);
            cell.Append(paragraph);

            return cell;
        }
        public static void AddImageToCell(OpenXmlElement parent, string relationshipId, int width, int height)
        {
            var drawing = new Drawing(
        new DW.Inline(
            new DW.Extent() { Cx = width * 9525, Cy = height * 9525 },
            new DW.EffectExtent()
            {
                LeftEdge = 0L,
                TopEdge = 0L,
                RightEdge = 0L,
                BottomEdge = 0L
            },
            new DW.DocProperties()
            {
                Id = 1U,
                Name = "Picture 1"
            },
            new DW.NonVisualGraphicFrameDrawingProperties(
                new A.GraphicFrameLocks() { NoChangeAspect = true }),
            new A.Graphic(
                new A.GraphicData(
                    new PIC.Picture(
                        new PIC.NonVisualPictureProperties(
                            new PIC.NonVisualDrawingProperties()
                            {
                                Id = 0U,
                                Name = "New Bitmap Image.jpg"
                            },
                            new PIC.NonVisualPictureDrawingProperties()),
                        new PIC.BlipFill(
                            new A.Blip()
                            {
                                Embed = relationshipId,
                                CompressionState = A.BlipCompressionValues.Print
                            },
                            new A.Stretch(new A.FillRectangle())),
                        new PIC.ShapeProperties(
                            new A.Transform2D(
                                new A.Offset() { X = 0L, Y = 0L },
                                new A.Extents() { Cx = width * 9525, Cy = height * 9525 }),
                            new A.PresetGeometry(
                                new A.AdjustValueList()
                            )
                            { Preset = A.ShapeTypeValues.Rectangle }))
                )
                { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
        )
        {
            DistanceFromTop = 0U,
            DistanceFromBottom = 0U,
            DistanceFromLeft = 0U,
            DistanceFromRight = 0U,
            EditId = "50D07946"
        });

            parent.Append(new Paragraph(new Run(drawing)));
        }
    }

}