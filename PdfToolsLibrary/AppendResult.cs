namespace PdfToolsLibrary
{
    public class AppendResult
    {
        public bool Complete { get; private set; }
        public int Offset { get; private set; }
        public int Written { get; private set; }

        public AppendResult(bool complete, int offset, int written)
        {
            Complete = complete;
            Offset = offset;
            Written = written;
        }
    }
}