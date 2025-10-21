namespace pdfconvertor;

public interface ITiffToPdfInterface
{
    Task<byte[]> ConvertTiffToPdfAsync(byte[] srcdata, int targetDpi);
    
    Task<byte[]> EncryptExistingPdf(byte[] pdfBytes, string password);
}
