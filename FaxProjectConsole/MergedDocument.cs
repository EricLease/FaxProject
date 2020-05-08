using System;
using System.Collections.Generic;
using System.Linq;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace FaxProjectConsole
{
    public class MergedDocument : IDisposable
    {
        private const int MaxPageCount = 60;
        private const bool BreakForLargeDocuments = true;

        private string OutputPath { get; set; }
        private string CoverSheetPath { get; set; }
        private bool _disposed = false;

        private PdfDocument Document { get; set; }
        private int PageCount { get; set; } = 0;

        public MergedDocument(string outputPath, string coverSheetPath = null)
        {
            OutputPath = outputPath;
            CoverSheetPath = coverSheetPath;
            Init();
        }

        ~MergedDocument() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                // Dispose managed resources.
                if (disposing) ReleaseDocument();

                // Dispose unmanaged resources
                
                _disposed = true;
            }
        }

        private void ReleaseDocument()
        {
            if (Document == null) return;

            var message = "";

            if (!Document.CanSave(ref message))
                Console.Error.WriteLine($"May not have saved output document: {message}");

            Document.Close();
            Document.Dispose();
            Document = null;
        }

        private void Init()
        {
            Document = OutputPath.IsValidPdf()
                ? PdfReader.Open(OutputPath, PdfDocumentOpenMode.Import)
                : new PdfDocument(OutputPath);
            AddCoverSheet();
        }

        private void AddCoverSheet()
        {
            if (Document.IsImported || string.IsNullOrEmpty(CoverSheetPath)) return;

            if (!CoverSheetPath.IsValidPdf())
                throw new ApplicationException(
                    $"Cover sheet ({CoverSheetPath}) is not a valid PDF file");

            using (var input = PdfReader.Open(CoverSheetPath, PdfDocumentOpenMode.Import))
            {
                if (input.PageCount > MaxPageCount)
                    throw new ApplicationException(
                        $"Cover sheet's page count ({input.PageCount}) exceeds maximum page count ({MaxPageCount})");

                if (!Append(input).Complete)
                    throw new ApplicationException(
                        $"Failed to add cover sheet to merged document");
            }
        }

        internal AppendResult Append(string inputFilePath, int inputOffset = 0, IList<int> removals = null)
        {
            if (!inputFilePath.IsValidPdf())
            {
                Console.Error.WriteLine(
                    $"Input file ({inputFilePath}) is not a valid PDF.  Merge failed.");

                return new AppendResult(false, inputOffset, 0);
            }

            using (var input = PdfReader.Open(inputFilePath, PdfDocumentOpenMode.Import))
                return Append(input, inputOffset, removals);
        }

        private AppendResult Append(PdfDocument input, int inputOffset = 0, IList<int> removals = null)
        {
            var remaining = MaxPageCount - PageCount;
            var inputPages = Enumerable
                .Range(1, input.PageCount)
                .Where(p => !(removals?.Contains(p) ?? false))
                .Skip(inputOffset)
                .ToArray();
            var inputPageCount = inputPages.Length;

            if (!Document.IsImported &&
                ((inputPageCount > MaxPageCount && BreakForLargeDocuments) ||
                 inputPageCount / 2 > remaining))
                return new AppendResult(false, 0, 0);

            var idx = 0;

            for (; idx < remaining && idx < inputPageCount; idx++)
            {
                Document.AddPage(input.Pages[inputPages[idx] - 1]);
            }

            var complete = idx >= inputPageCount;

            inputOffset = complete ? 0 : idx;

            Console.WriteLine(
                $"Merged pages ({string.Join(", ", inputPageCount)}) from {input.FullPath} into output document");
            Console.WriteLine(
                $"Complete: {(complete ? "Yes" : "No")}\tNumber of pages written: {idx}\tResulting offset: {inputOffset}");

            PageCount = Document.PageCount;

            return new AppendResult(complete, inputOffset, idx);
        }
    }
}
