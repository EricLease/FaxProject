using System;
using System.IO;

namespace PdfToolsLibrary
{
    public static class Constants
    {
        public const string MediaExtension = ".pdf";

        public static readonly DirectoryInfo AppDir = new DirectoryInfo(Environment.CurrentDirectory);
    }
}
