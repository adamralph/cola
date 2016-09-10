// Copyright (c) Adam Ralph. (adam@adamralph.com)
namespace Cola
{
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    internal static class FileEx
    {
        public static async Task<string> ReadAllTextAsync(string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
            {
                var contents = new StringBuilder();
                var buffer = new byte[4096];
                int byteCount;
                while ((byteCount = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    contents.Append(Encoding.UTF8.GetString(buffer, 0, byteCount));
                }

                return contents.ToString();
            }
        }
    }
}
