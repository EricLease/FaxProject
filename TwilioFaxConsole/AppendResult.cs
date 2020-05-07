namespace TwilioFaxConsole
{
    internal class AppendResult
    {
        internal bool Complete { get; private set; }
        internal int Offset { get; private set; }
        internal int Written { get; private set; }

        internal AppendResult(bool complete, int offset, int written)
        {
            Complete = complete;
            Offset = offset;
            Written = written;
        }
    }
}