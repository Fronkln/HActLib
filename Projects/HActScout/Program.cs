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
        private static Dictionary<uint, string> foundUnkNodes = new Dictionary<uint, string>();

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
            int id = int.Parse(args[1]);
            string game = args[2];

            bool recursive = false;

            if (args.Length > 3)
                recursive = args[3] == "recursive";

            bool findUnknownMode = id == -1;

            Game gameEnum = CMN.GetGameFromString(game);

            string extraMode = "";

            if(args.Length > 3)
                extraMode = args[3];
            bool bepMode = extraMode == "bep";
            bool pibMode = extraMode == "pibdump";

            if (!Directory.Exists(dir))
            {
                Console.WriteLine("Invalid directory.");
                return;
            }

            Console.OutputEncoding = Encoding.GetEncoding("Shift-JIS");

            
            if(pibMode)
            {
                ProcessPibsNames(Directory.GetFiles(dir, "*.par", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly), gameEnum, id);
                Console.WriteLine("Done");
                Console.ReadKey();
                return;
            }

            if(!bepMode)
                Process(Directory.GetFiles(dir, "*.par", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly), gameEnum, id, bepMode);
            else
                Process(Directory.GetFiles(dir, "*.bep", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly), gameEnum, id, bepMode);

            Console.OutputEncoding = Encoding.Unicode;

            Console.WriteLine("Amount of pars the specified node ID was found: " + m_foundPars.Count);

            foreach (string str in m_foundPars)
                Console.WriteLine(str);

            if (findUnknownMode)
            {
                Console.WriteLine("Unknown node IDs list: ");

                foreach (var kv in foundUnkNodes.OrderBy(x=> x.Key))
                    Console.WriteLine($"{kv.Key} {kv.Value}");
            }
        }

        private static void Process(string[] files, Game game, int id, bool bepMode)
        {
            bool isDE = CMN.IsDEGame(game);
            bool findUnknownMode = id == -1;

            uint[] values = Enum.GetValues(HActLib.Internal.Reflection.GetElementEnumFromGame(game)).Cast<uint>().ToArray();

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
                catch(Exception ex)
                {
                    WriteLineColor("Par reading failed! " + fileName + " Error: " + ex.Message + " " + ex.InnerException, ConsoleColor.Red);
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

                IEnumerable<NodeElement> filtered = null;

                if(!findUnknownMode)
                    filtered = nodes.Where(x => x.ElementKind == id);
                else
                    filtered = nodes.Where(x => !values.Contains(x.ElementKind) && !foundUnkNodes.ContainsKey(x.ElementKind));

                if (filtered.Count() > 0)
                {
                    WriteLineColor("Found node in " + fileName + "\n", ConsoleColor.Green);
                    m_foundPars.Add(fileName);

                    foreach (NodeElement element in filtered)
                    {
                        Console.WriteLine(element.Name + $" ({element.ElementKind})" + " " + element.Guid.ToString());

                        if(!foundUnkNodes.ContainsKey(element.ElementKind))
                            foundUnkNodes.Add(element.ElementKind, element.Name);
                    }

                    Console.WriteLine();
                }


                curInf.Par?.Dispose();

                GC.Collect();
            }
        }

        private static void ProcessPibsNames(string[] files, Game game, int id)
        {
            HashSet<uint> foundPibs = new HashSet<uint>();
            uint pibElementID = HActLib.Internal.Reflection.GetElementIDByName("e_auth_element_particle", game);

            StreamWriter writer = new StreamWriter("out_pib_list.txt");

            foreach (string file in files)
            {
                HActInfo curInf;
                byte[] buf = null;

                string fileName = Path.GetFileName(file);

                curInf = new HActInfo(file);
                buf = curInf.GetCmnBuffer();

                if (buf == null || buf.Length <= 0)
                    continue;


                NodeElement[] nodes = (CMN.IsDEGame(game) ? CMN.Read(buf, game).AllElements : OECMN.Read(buf).AllElements);


                foreach(var element in nodes)
                    if(element.ElementKind == pibElementID)
                    {
                        var ptc = element as DEElementParticle;

                        if (foundPibs.Contains(ptc.ParticleID))
                            continue;


                        foundPibs.Add(ptc.ParticleID);
                        Console.WriteLine(ptc.Name);
                        writer.WriteLine(ptc.Name);
                    }
            }
        }
    }
}
