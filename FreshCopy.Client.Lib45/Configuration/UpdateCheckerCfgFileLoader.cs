using CommonTools.Lib.fx45.Cryptography;
using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.ns11.ExceptionTools;
using CommonTools.Lib.ns11.StringTools;
using FreshCopy.Common.API.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using static System.Environment;

namespace FreshCopy.Client.Lib45.Configuration
{
    public class UpdateCheckerCfgFile
    {
        public const string FILE_NAME = "FC.UpdateChecker.cfg";


        public static UpdateCheckerSettings LoadOrDefault()
        {
            UpdateCheckerSettings cfg;
            try
            {
                cfg = DeserializeEncryptedCfg();
            }
            catch (FileNotFoundException)
            {
                return WriteDefaultSettingsFile();
            }
            SetDefaults(ref cfg);
            return cfg;
        }


        private static UpdateCheckerSettings DeserializeEncryptedCfg()
        {
            var jsonCfg  = "";
            var savedCfg = jsonCfg = ReadSavedFile();

            if (!jsonCfg.TrimStart().StartsWith("{"))
            {
                jsonCfg = Decrypt(jsonCfg, ReadEncryptKey());
                if (jsonCfg == null) throw Fault.Intruder();
            }

            var obj = JsonConvert.DeserializeObject<UpdateCheckerSettings>(jsonCfg);
            obj.SetSavedFile(savedCfg);
            return obj;
        }


        public static string ReadSavedFile()
            => File.ReadAllText(FILE_NAME.MakeAbsolute(), Encoding.UTF8);


        internal static void RewriteWith(string encryptedDTO, string key)
        {
            WriteEncryptKey(key);

            var decryptd = Decrypt(encryptedDTO, key);
            var path     = FILE_NAME.MakeAbsolute();
            File.WriteAllText(path, decryptd, Encoding.UTF8);
        }


        private static void WriteEncryptKey(string sharedKey)
        {
            var path     = GetEncryptKeyPath(false);
            var pwd      = GetEncryptKeyDecryptor();
            var encryptd = Encrypt(sharedKey, pwd);
            File.WriteAllText(path, encryptd);
        }


        private static string ReadEncryptKey()
        {
            var path     = GetEncryptKeyPath(true);
            var pwd      = GetEncryptKeyDecryptor();
            var encryptd = File.ReadAllText(path);
            return Decrypt(encryptd, pwd);
        }


        private static string GetEncryptKeyPath(bool errorIfMissing)
        {
            var dir  = GetFolderPath(SpecialFolder.LocalApplicationData);
            var hash = FILE_NAME.SHA1ForUTF8();
            var path = Path.Combine(dir, hash);

            if (errorIfMissing && !File.Exists(path))
                throw new InvalidOperationException("Missing Cfg cypher.");

            return path;
        }


        private static string GetEncryptKeyDecryptor()
            => FILE_NAME.SHA1ForUTF8().SHA1ForUTF8();


        private static string Encrypt(string content, string password)
            => AESThenHMAC.SimpleEncryptWithPassword(content, password);

        private static string Decrypt(string content, string password)
            => AESThenHMAC.SimpleDecryptWithPassword(content, password);


        private static void SetDefaults(ref UpdateCheckerSettings cfg)
        {
            if (!cfg.UpdateSelf.HasValue) cfg.UpdateSelf = true;
            if (!cfg.CanExitApp.HasValue) cfg.CanExitApp = false;
        }


        private static UpdateCheckerSettings WriteDefaultSettingsFile()
        {
            var cfg = UpdateCheckerSettings.CreateDefault();
            JsonFile.Write(cfg, FILE_NAME);
            return cfg;
        }
    }
}
