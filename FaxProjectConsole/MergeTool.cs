using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FaxProjectConsole
{
    internal class MergeTool
    {
        private const string MediaExtension = ".pdf";
        private const string PdfSearch = "*" + MediaExtension;
        
        private static DirectoryInfo _AppDir = null;
        private static DirectoryInfo AppDir
            => _AppDir ??
            (_AppDir = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent);

        private string MassagedOutputPathFormat =>
            $"{OutputDirectory?.FullName ?? "."}\\{OutputNameBase} {{0}}{MediaExtension}";

        private DirectoryInfo OutputDirectory { get; set; }
        private IList<InputDocumentData> InputDocuments { get; set; }
        private string OutputNameBase { get; set; }
        private InputDocumentData CoverSheet { get; set; }
        private IEnumerable<FileInfo> SearchSpace { get; set; }
        
        public MergeTool(
            string inputDirectory = "input",
            string outputDirectory = "output",
            string outputNameBase = "merged",
            IList<InputDocumentData> inputDocuments = null)
        {
            OutputDirectory = AppDir.EnumerateDirectories(outputDirectory).FirstOrDefault();
            OutputNameBase = outputNameBase;
            InputDocuments = new List<InputDocumentData>(inputDocuments)
                .OrderBy(d => d.RelativeOrder)
                .ToList();
            CoverSheet = InputDocuments.FirstOrDefault(d => d.IsCoverSheet);
            SearchSpace = AppDir
                .EnumerateDirectories(inputDirectory)
                .FirstOrDefault()
                ?.EnumerateFiles(PdfSearch)
                .Where(f => InputDocuments.Any(d => 
                    string.Equals(
                        f.Name, d.FileName + MediaExtension, 
                        StringComparison.InvariantCultureIgnoreCase)))
                ?? Enumerable.Empty<FileInfo>();
        }

        internal void Merge()
        {
            var done = false;
            var outputDocCount = 1;
            var inputFileIdx = 0;
            var inputOffset = 0;
            var workingSet = InputDocuments.Where(d => !d.IsCoverSheet).ToArray();
            
            while (!done)
            {
                using (var merged = new MergedDocument(
                    string.Format(MassagedOutputPathFormat, outputDocCount.ToString("00")),
                    InputPath(CoverSheet?.FileName)))
                {
                    AppendResult result;

                    do
                    {
                        var inputDoc = workingSet[inputFileIdx];

                        result = merged.Append(
                            InputPath(inputDoc.FileName), inputOffset, inputDoc.ExcludedPages);
                        inputOffset = result.Offset;

                        if (result.Complete)
                            done = ++inputFileIdx >= workingSet.Length;                      
                    } while (result.Complete && !done);
                }

                outputDocCount++;
            }
        }

        private string InputPath(string fileName) 
            => string.IsNullOrWhiteSpace(fileName)
            ? null 
            : SearchSpace.FirstOrDefault(f => 
                string.Equals(
                    f.Name, fileName + MediaExtension, 
                    StringComparison.InvariantCultureIgnoreCase))
                .FullName;
    }
}
