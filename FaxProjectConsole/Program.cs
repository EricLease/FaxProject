using System;
using System.Collections.Generic;

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

        private static readonly IList<InputDocumentData> InputDocuments = new List<InputDocumentData>
        {
            new InputDocumentData(fileName: "00", isCoverSheet: true),
            new InputDocumentData(fileName: "01"),
            new InputDocumentData(fileName: "02", relativeOrder: 1, excludedPages: new List<int>{ 1, 2, 3, 4, 15, 16, 17, 18 }),
            new InputDocumentData(fileName: "03", relativeOrder: 2, excludedPages: new List<int>{ 1, 2, 3, 13 }),
            new InputDocumentData(fileName: "04", relativeOrder: 3, excludedPages: new List<int>{ 1, 11 }),
            new InputDocumentData(fileName: "05", relativeOrder: 4, excludedPages: new List<int>{ 2, 3, 4 }),
            new InputDocumentData(fileName: "06", relativeOrder: 5),
            new InputDocumentData(fileName: "07", relativeOrder: 6),
            new InputDocumentData(fileName: "08", relativeOrder: 7),
            new InputDocumentData(fileName: "09", relativeOrder: 8, excludedPages: new List<int>{ 39 }),
            new InputDocumentData(fileName: "10", relativeOrder: 9),
        };

        private static void PrepareDocuments()
            => new MergeTool(
                inputDirectory: InputDirectory,
                inputDocuments: InputDocuments,
                outputDirectory: OutputDirectory,
                outputNameBase: OutputNameBase)
                .Merge();

        #endregion Document (PDF) Prep
    }
}
