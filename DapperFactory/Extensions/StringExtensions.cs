using System.IO;
using System.Threading.Tasks;

namespace Chambersoft.DapperFactory.Extensions
{
    public static class StringExtensions
    {
        public static string ReadFileContents(this string path)
        {
            using (StreamReader sr = File.OpenText(path))
            {
                return sr.ReadToEnd();
            }
        }

        public static async Task<string> ReadFileContentsAsync(this string path)
        {
            using (StreamReader sr = File.OpenText(path))
            {
                return await sr.ReadToEndAsync();
            }
        } 
    }
}
