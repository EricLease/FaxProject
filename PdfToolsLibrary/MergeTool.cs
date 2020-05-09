using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PdfToolsLibrary
{
    public class MergeTool
    {
        private string MassagedOutputPathFormat =>
            $"{Constants.AppDir.FullName}\\{{0}} {OutputNameBase} {{1}}{Constants.MediaExtension}";

        private IList<InputDocumentData> InputDocuments { get; set; }
        private InputDocumentData CoverSheet { get; set; }
        private string OutputNameBase { get; set; }
        
        public MergeTool(
            IList<InputDocumentData> inputDocuments = null, 
            string outputNameBase = "merge")
        {
            InputDocuments = new List<InputDocumentData>(inputDocuments)
                .OrderBy(d => d.RelativeOrder)
                .ToList();
            CoverSheet = InputDocuments.FirstOrDefault(d => d.IsCoverSheet);
            OutputNameBase = outputNameBase;
        }

        public List<string> Merge()
        {
            var done = false;
            var outputDocCount = 1;
            var inputFileIdx = 0;
            var inputOffset = 0;
            var workingSet = InputDocuments.Where(d => !d.IsCoverSheet).ToArray();
            var dtStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var outputFiles = new List<string>();
            
            while (!done)
            {
                var outputFileName = string.Format(
                    MassagedOutputPathFormat, 
                    dtStamp, outputDocCount.ToString("00"));

                using (var merged = new MergedDocument(
                    outputFileName, CoverSheet?.FileName))
                {
                    AppendResult result;

                    do
                    {
                        var inputDoc = workingSet[inputFileIdx];

                        result = merged.Append(
                            inputDoc.FileName, inputOffset, inputDoc.ExcludedPages);
                        inputOffset = result.Offset;

                        if (result.Complete)
                            done = ++inputFileIdx >= workingSet.Length;                      
                    } while (result.Complete && !done);
                }

                outputFiles.Add(outputFileName.Split('\\').Last());
                outputDocCount++;
            }

            return outputFiles;
        }
    }
}
