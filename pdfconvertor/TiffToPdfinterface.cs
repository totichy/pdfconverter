using ImageMagick;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace pdfconvertor;

public class TiffToPdfinterface : ITiffToPdfInterface
{
    public async Task<byte[]> ConvertTiffToPdfAsync(byte[] srcdata, int targetDpi)
    {
        using (var images = new MagickImageCollection(srcdata))
        {
            using (PdfDocument document = new PdfDocument())
            {
                foreach (var image in images)
                {
                    try
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            if (image.ColorSpace != ColorSpace.RGB)
                            {
                                Console.WriteLine($"⚠️ Převádím barevný model {image.ColorSpace} -> RGB");
                                image.ColorSpace = ColorSpace.RGB;
                            }
                            
                            image.Density = new Density(targetDpi);
                            
                            image.Format = MagickFormat.Jpeg;
                            image.Quality = 90;
                            await image.WriteAsync(memoryStream);
                            memoryStream.Position = 0;
                            
                            PdfPage page = document.AddPage();
                            XGraphics gfx = XGraphics.FromPdfPage(page);
                            XImage pdfImage = XImage.FromStream(memoryStream);
                            
                            //page.Width = XUnit.FromPoint(pdfImage.PixelWidth * 72.0 / targetDpi);
                            //page.Height = XUnit.FromPoint(pdfImage.PixelHeight * 72.0 / targetDpi);

                            gfx.DrawImage(pdfImage, 0, 0, page.Width.Point, page.Height.Point);
                            Console.WriteLine("✅ Stránka úspěšně přidána do PDF.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"⚠️ Chyba při zpracování stránky: {ex.Message}");
                    }
                }

                using (var outputStream = new MemoryStream())
                {
                    document.Save(outputStream);
                    return outputStream.ToArray();
                }
            }
        }
    }
}