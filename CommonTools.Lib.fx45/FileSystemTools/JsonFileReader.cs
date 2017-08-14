using CommonTools.Lib.ns11.StringTools;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace CommonTools.Lib.fx45.FileSystemTools
{
    public class JsonFileReader
    {
    }


    public static partial class JsonFile
    {
        public static T Read<T>(string filepathOrName)
        {
            if (!TryFindFile(filepathOrName, out string absolutePath))
                throw new FileNotFoundException($"Missing: {filepathOrName}");

            var json = File.ReadAllText(absolutePath, Encoding.UTF8);
            return JsonConvert.DeserializeObject<T>(json);
        }


        private static bool TryFindFile(string filepath, out string absolutePath)
        {
            absolutePath = null;
            if (filepath.IsBlank()) return false;

            if (File.Exists(filepath))
            {
                absolutePath = filepath;
                return true;
            }

            var exeDir = CurrentExe.GetDirectory();
            absolutePath = Path.Combine(exeDir, filepath);
            return File.Exists(absolutePath);
        }
    }
}
