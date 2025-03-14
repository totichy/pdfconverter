﻿using pdfconvertor;

class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Použití: TiffToPdfConverter <vstupní TIFF> <výstupní PDF> <DPI>");
            return;
        }

        string inputTiff = args[0];
        string outputPdf = args[1];
        if (!int.TryParse(args[2], out int targetDpi))
        {
            Console.WriteLine("❌ Chyba: Neplatná hodnota DPI.");
            return;
        }

        Console.WriteLine($"🔄 Zpracovávám soubor: {inputTiff} s DPI: {targetDpi}");

        if (!File.Exists(inputTiff))
        {
            Console.WriteLine("❌ Chyba: Vstupní soubor neexistuje.");
            return;
        }

        try
        {
            ITiffToPdfInterface service = new TiffToPdfinterface();
            byte[] tiffData = await File.ReadAllBytesAsync(inputTiff);
            byte[] pdfData = await service.ConvertTiffToPdfAsync(tiffData, targetDpi);
            await File.WriteAllBytesAsync(outputPdf, pdfData);
            Console.WriteLine($"✅ Konverze dokončena: {outputPdf}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Chyba při konverzi: {ex}");
        }
    }
}

