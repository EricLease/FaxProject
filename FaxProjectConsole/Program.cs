using System;

namespace FaxProjectConsole
{
    class Program
    {        
        static void Main(string[] args)
        {
            PrepareDocuments();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        #region Document (PDF) Prep

        // Directories relative to app's source directory
        private const string InputDirectory = "input";
        private const string OutputDirectory = "output";

        // Base filename to use for output, file number will be appended
        private const string OutputNameBase = "MergeTest";

        // Use, as cover sheet, first document in InputDirectory, after sort
        private const bool CoverSheetPresent = true;

        // Arrays of page numbers to be removed, relative to each document
        private static readonly int[][] RemoveFromMerged =
        {
            null, // 0 - Cover sheet
            null,
            new int[] { 1, 2, 3, 4, 15, 16, 17, 18 },
            new int[] { 1, 2, 3, 13 },
            new int[] { 1, 11 },
            new int[] { 2, 3, 4 },
            null,
            null,
            null,
            new int[] { 39 },
            null
        };

        private static void PrepareDocuments()
        {
            new MergeTool(
                coverSheetPresent: CoverSheetPresent,
                inputDirectory: InputDirectory,
                outputDirectory: OutputDirectory,
                outputNameBase: OutputNameBase,
                removals: RemoveFromMerged)
                .PrepareDocuments();
        }

        #endregion Document (PDF) Prep
    }
}
