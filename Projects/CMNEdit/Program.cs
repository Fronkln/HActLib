using HActLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMNEdit
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string directoryDir = "config";

            if(Directory.Exists(directoryDir))
            {
                string gamesFile = Path.Combine(directoryDir, "games.txt");
                
                if(File.Exists(gamesFile))
                {
                    string[] gameDirs = File.ReadAllLines(gamesFile);

                    foreach(string str in gameDirs)
                    {
                        string gameDir = Path.Combine(directoryDir, "game", str);

                        if(Directory.Exists(gameDir))
                        {
                            string nodesDir = Path.Combine(gameDir, "nodes");

                            if(Directory.Exists(nodesDir))
                            {
                                foreach(string nodeFile in Directory.GetFiles(nodesDir, "*.txt"))
                                {
                                    UserElementFile file = UserElementFile.Read(nodeFile);
                                    HActLib.Internal.Reflection.RegisterUserNode(file.TargetGame, file.Data);
                                }
                            }
                        }
                    }
                }
            }

            if (!File.Exists("config.ini"))
                SaveINIFile();
            else
                ReadINIFile();

            Application.SetHighDpiMode(HighDpiMode.SystemAware);

            if(INISettings.DarkMode)
                Application.SetColorMode(SystemColorMode.Dark);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppRegistry.Init();
            Application.Run(new Form1());

        }

        public static void ReadINIFile()
        {
            Ini ini = new Ini("config.ini");
            ini.Load();

            INISettings.DarkMode = ini.GetValue("DarkMode", "APP") == "1";
            INISettings.Y3CsvPath = ini.GetValue("Y3_CSV_PATH", "OOE");
            INISettings.Y4CsvPath = ini.GetValue("Y4_CSV_PATH", "OOE");
        }

        public static void SaveINIFile()
        {
            Ini ini = new Ini("config.ini");
            ini.WriteValue("DarkMode", "APP", Convert.ToInt32(INISettings.DarkMode).ToString());
            ini.WriteValue("Y3_CSV_PATH", "OOE", INISettings.Y3CsvPath.Replace(@"\\", ""));
            ini.WriteValue("Y4_CSV_PATH", "OOE", INISettings.Y4CsvPath);
            ini.Save();
        }
    }
}
