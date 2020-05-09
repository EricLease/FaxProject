using System.Collections.Generic;

namespace PdfToolsLibrary
{
    public class InputDocumentData
    {
        public string FileName { get; private set; }
        public bool IsCoverSheet { get; private set; }
        public IList<int> ExcludedPages { get; private set; }
        public int RelativeOrder { get; set; }
        
        public InputDocumentData(
            string fileName,
            bool isCoverSheet = false,
            IList<int> excludedPages = null, 
            int relativeOrder = 0)
        {
            FileName = fileName;
            IsCoverSheet = isCoverSheet;
            ExcludedPages = excludedPages == null 
                ? new List<int>() 
                : new List<int>(excludedPages);
            RelativeOrder = relativeOrder;
        }
    }
}
