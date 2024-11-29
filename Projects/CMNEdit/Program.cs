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

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppRegistry.Init();
            Application.Run(new Form1());

        }
    }
}
