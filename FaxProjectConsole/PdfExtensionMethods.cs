using PdfSharp.Pdf.IO;

namespace FaxProjectConsole
{
    internal static class PdfExtensionMethods
    {
        internal static bool IsValidPdf(this string filePath)
            => !string.IsNullOrWhiteSpace(filePath) && 
            PdfReader.TestPdfFile(filePath) != 0;
    }
}
