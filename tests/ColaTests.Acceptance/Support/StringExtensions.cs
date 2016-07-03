// Copyright (c) Adam Ralph. (adam@adamralph.com)
namespace ColaTests.Acceptance.Support
{
    using System.IO;
    using System.Linq;

    internal static class StringExtensions
    {
        public static string ToPath(this string text)
        {
            foreach (var chr in Path.GetInvalidPathChars().Concat(Path.GetInvalidFileNameChars()))
            {
                text = text?.Replace(chr, '_');
            }

            return text;
        }
    }
}
