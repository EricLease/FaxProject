using System.Collections.Generic;

namespace FaxProjectConsole
{
    internal class InputDocumentData
    {
        internal string FileName { get; private set; }
        internal bool IsCoverSheet { get; private set; }
        internal IList<int> ExcludedPages { get; private set; }
        internal int RelativeOrder { get; set; }
        
        internal InputDocumentData(
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
