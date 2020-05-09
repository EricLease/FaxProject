using PdfSharp.Pdf.IO;

namespace PdfToolsLibrary
{
    public static class PdfExtensionMethods
    {
        public static bool IsValidPdf(this string filePath)
            => !string.IsNullOrWhiteSpace(filePath) && 
            PdfReader.TestPdfFile(filePath) != 0;
    }
}
