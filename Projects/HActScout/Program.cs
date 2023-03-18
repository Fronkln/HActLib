using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using HActLib;
using System.Text;

namespace HActScout
{
    internal class Program
    {
        public static void WriteColor(string text, ConsoleColor col)
        {
            System.Console.ForegroundColor = col;
            System.Console.Write(text);
            System.Console.ForegroundColor = ConsoleColor.Gray; //return to normal color or it will do all upcoming writes in this color
        }

        public static void WriteLineColor(string text, ConsoleColor col)
        {
            WriteColor(text + "\n", col);
        }

        private static List<string> m_foundPars = new List<string>();

        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (args.Length < 3)
            {
                Console.WriteLine("Invalid args.\nUsage: HActScout.exe (directory of pars) (element ID) (game)\nE.G: HActScout.exe \"C:/hacts\" 25 (sound effect ID in Y7) ylad\n");

                Console.WriteLine("Available Games:\n");

                foreach (string str in Enum.GetNames<Game>())
                    Console.WriteLine(str);

                return;
            }

            string dir = args[0];
            uint id = uint.Parse(args[1]);
            string game = args[2];
            Game gameEnum = CMN.GetGameFromString(game);

            bool bepMode = args.Length > 3 && args[3] == "bep";

            if (!Directory.Exists(dir))
            {
                Console.WriteLine("Invalid directory.");
                return;
            }

            Console.OutputEncoding = Encoding.GetEncoding("Shift-JIS");

            if(!bepMode)
                Process(Directory.GetFiles(dir, "*.par"), gameEnum, id, bepMode);
            else
                Process(Directory.GetFiles(dir, "*.bep"), gameEnum, id, bepMode);

            Console.OutputEncoding = Encoding.Unicode;

            Console.WriteLine("Amonut of pars the specified node ID was found: " + m_foundPars.Count);

            foreach (string str in m_foundPars)
                Console.WriteLine(str);
        }

        private static void Process(string[] files, Game game, uint id, bool bepMode)
        {
            bool isDE = CMN.IsDEGame(game);

            foreach(string file in files)
            {
                HActInfo curInf;
                byte[] buf = null;

                string fileName = Path.GetFileName(file);

        //        if (!file.Contains("feeltheheat"))
        //    continue;

                try
                {
                    if (!bepMode)
                    {
                        curInf = new HActInfo(file);
                        buf = curInf.GetCmnBuffer();
                    }
                    else
                    {
                        buf = File.ReadAllBytes(file);
                        curInf.Par = null;
                    }
                }
                catch
                {
                    WriteLineColor("Par reading failed! " + fileName, ConsoleColor.Red);
                    Console.WriteLine();
                    continue;
                }

                if(buf.Length <= 0)
                {
                    WriteLineColor(file + " failed. Couldnt get CMN", ConsoleColor.Red);
                    Console.WriteLine();
                    continue;
                }

                NodeElement[] nodes = null;

                //  try
                //{
                if (!bepMode)
                    nodes = (isDE ? CMN.Read(buf, game).AllElements : OECMN.Read(buf).AllElements);
                else
                   nodes = (BEP.Read(buf, game).AllElements);
               // }
               // catch
                //{
                   // Console.WriteLine("Cmn reading failed! " + fileName);
                   // curInf.Par?.Dispose();
                   // Console.WriteLine();
                  //  continue;
                //}
                IEnumerable<NodeElement> filtered = nodes.Where(x => x.ElementKind == id);

                if (filtered.Count() > 0)
                {
                    WriteLineColor("Found node in " + fileName + "\n", ConsoleColor.Green);
                    m_foundPars.Add(fileName);

                    foreach (NodeElement element in filtered)
                        Console.WriteLine(element.Name + " " + element.Guid.ToString());

                    Console.WriteLine();
                }

                curInf.Par?.Dispose();

                GC.Collect();
            }
        }
    }
}
