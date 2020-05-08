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

        private string OutputDirectoryPath
            => AppDir.EnumerateDirectories(OutputDirectory).FirstOrDefault()?.FullName ?? ".";
        private string MassagedOutputPathFormat =>
            $"{OutputDirectoryPath}\\{OutputNameBase} {{0}}{MediaExtension}";

        private bool CoverSheetPresent { get; set; }
        private string InputDirectory { get; set; }
        private string OutputDirectory { get; set; }
        private string OutputNameBase { get; set; }
        private int[][] Removals { get; set; }

        private string[] FilePaths
            => AppDir
            .EnumerateDirectories(InputDirectory)
            .FirstOrDefault()
            ?.EnumerateFiles(PdfSearch)
            .OrderBy(f => f.Name)
            .Select(f => f.FullName)
            .ToArray();

        public MergeTool(
            bool coverSheetPresent = false, 
            string inputDirectory = "input",
            string outputDirectory = "output",
            string outputNameBase = "merged",
            int[][] removals = null)
        {
            CoverSheetPresent = coverSheetPresent;
            InputDirectory = inputDirectory;
            OutputDirectory = outputDirectory;
            OutputNameBase = outputNameBase;
            Removals = (int[][])removals.Clone();
        }

        internal void PrepareDocuments()
        {
            var done = false;
            var outputDocCount = 1;
            var inputFileIdx = CoverSheetPresent ? 1 : 0;
            var coverSheetPath = CoverSheetPresent ? FilePaths[0] : null;
            var inputOffset = 0;

            while (!done)
            {
                using (var merged = new MergedDocument(
                    string.Format(MassagedOutputPathFormat, outputDocCount.ToString("00")),
                    coverSheetPath))
                {
                    AppendResult result;

                    do
                    {
                        result = merged.AppendToOutput(
                            FilePaths[inputFileIdx],
                            inputOffset,
                            Removals[inputFileIdx]);
                        inputOffset = result.Offset;

                        if (result.Complete)
                            done = ++inputFileIdx >= FilePaths.Length;
                    } while (result.Complete && !done);
                }

                outputDocCount++;
            }
        }
    }
}
