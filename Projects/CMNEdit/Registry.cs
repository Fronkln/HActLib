using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMNEdit
{
    internal class AppRegistry
    {
        public static RegistryKey Root;

        public static void Init()
        {
            try
            {
                Root = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AuthEdit");
            }
            catch
            {

            }
        }

        public static string GetFileOpenPath()
        {
            return (string)Root.GetValue("LastFileDir");
        }

        public static string GetFileExtractOpenPath()
        {
            return (string)Root.GetValue("LastHActAssetExportDir");
        }
    }
}
