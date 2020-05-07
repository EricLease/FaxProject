using PdfSharp.Pdf.IO;

namespace TwilioFaxConsole
{
    internal static class PdfExtensionMethods
    {
        internal static bool IsValidPdf(this string filePath)
            => !string.IsNullOrWhiteSpace(filePath) && 
            PdfReader.TestPdfFile(filePath) != 0;
    }
}
