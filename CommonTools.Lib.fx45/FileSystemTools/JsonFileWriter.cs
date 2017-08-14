using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace CommonTools.Lib.fx45.FileSystemTools
{
    class JsonFileWriter
    {
    }


    public static partial class JsonFile
    {
        public static void Write<T> (T @object, string filepathOrName, bool indented = true)
        {
            var frmt = indented ? Formatting.Indented : Formatting.None;
            var json = JsonConvert.SerializeObject(@object, frmt);
            var path = MakeAbsolute(filepathOrName);
            File.WriteAllText(path, json, Encoding.UTF8);
        }


        private static string MakeAbsolute(string filepath)
        {
            if (Path.IsPathRooted(filepath))
                return filepath;
            else
                return Path.Combine(CurrentExe.GetDirectory(), filepath);
        }
    }
}
