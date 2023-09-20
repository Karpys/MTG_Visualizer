using System.IO;

namespace Script.Helper
{
    public static class StringHelper
    {
        public static string ToFileName(this string path)
        {
            return Path.GetFileName(path);
        }
    }
}