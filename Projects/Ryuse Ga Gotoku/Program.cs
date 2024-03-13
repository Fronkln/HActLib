using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using HActLib;
using HActLib.Types.DE.Enum;


namespace RyuseGaGotoku
{
    class Program
    {

        public static Type inputDEGameEnum;
        public static Type outputDEGameEnum;

        private static List<NodeCamera> NodeCameras = new List<NodeCamera>();

        public static CMN HAct = null;
        public static BEP Bep = null;

        public static string GetNameFromEnum(Type _enum)
        {
            if (_enum == typeof(YLADNodeIDs))
                return "ylad";
            else if (_enum == typeof(YK2NodeIDs))
                return "yk2";
            else if (_enum == typeof(Y6NodeIDs))
                return "y6";
            else
                return "unknown";
        }


        public static Type GetElementEnumFromName(string name)
        {
            string nameLower = name.ToLowerInvariant();

            if (nameLower == "y7" || nameLower == "ylad" || nameLower == "bestgame")
                return typeof(YLADNodeIDs);

            if (nameLower == "y6" || nameLower == "songoflife" || nameLower == "y6demo")
                return typeof(Y6NodeIDs);

            if (nameLower == "yk2" || nameLower == "y2")
                return typeof(YK2NodeIDs);

            if (nameLower == "lj")
                return typeof(YLADNodeIDs);

            if (nameLower == "je")
                return typeof(YK2NodeIDs);

            throw new Exception("Unknown Game: " + name);
        }

        static void PrintUsage()
        {
            Console.WriteLine("\n\nUsage:");
            Console.WriteLine("RyuseGaGotoku.exe inputgame outputgame\n");
            Console.WriteLine("Example:");
            Console.WriteLine("RyuseGaGotoku.exe ylad k2  ---->  will convert a Yakuza 7's HAct structure ID's to Kiwami 2");
        }

        static void Main(string[] args)
        {
            string path = AppContext.BaseDirectory;
            Console.WriteLine("Ryuse Ga Gotoku: Reusing HActs, RGGS style!");

            if (args.Length < 3)
            {
                if (args.Length == 0)
                    Console.WriteLine("No arguments were specified.");
                else if (args.Length == 2)
                    Console.WriteLine("No output game was specified.");

                PrintUsage();
                System.Threading.Thread.Sleep(3000);
                return;
            }

#if !DEBUG
            try
            {
#endif
            string file = args[0];

            string inputDEGame = "";
            string outputDEGame = "";
            bool bepMode = false;

            if (args.Length > 1)
            {
                inputDEGame = args[1];
                outputDEGame = args[2];

                if(args.Length > 3)
                bepMode = args[3] == "bep";
            }
            else
            {
                Ini ini = new Ini(Path.Combine(path, "defaultsettings.ini"));

                inputDEGame = ini.GetValue("input");

                if (string.IsNullOrEmpty(inputDEGame))
                    inputDEGame = "yk2";

                outputDEGame = ini.GetValue("output");

                if (string.IsNullOrEmpty(outputDEGame))
                    outputDEGame = "ylad";
            }


            if (!file.EndsWith(".bin") && !file.EndsWith(".bep"))
            {
                Console.WriteLine("This program only works with Dragon Engine CMN bins or BEPs.");
                return;
            }

            inputDEGameEnum = GetElementEnumFromName(inputDEGame);
            outputDEGameEnum = GetElementEnumFromName(outputDEGame);


            if (inputDEGameEnum == outputDEGameEnum)
                throw new Exception("Input and output game is the same.");


            bool deToOldDe = CMN.GetVersionForGame(CMN.GetGameFromString(outputDEGame)) == GameVersion.DE1 && CMN.GetVersionForGame(CMN.GetGameFromString(inputDEGame)) == GameVersion.DE2;

            if (inputDEGame == "y6" || outputDEGame == "y6")
                Console.WriteLine("WARNING: Conversions from and to Yakuza 6 are limited due to differences in auth pages.");

            Console.WriteLine("\nConverting " + inputDEGame + " to " + outputDEGame);

#if !DEBUG
                try
                {
#endif
            if (!bepMode)
                HAct = CMN.Read(file, CMN.GetGameFromString(inputDEGame));
            else
                Bep = BEP.Read(file, CMN.GetGameFromString(inputDEGame));
#if !DEBUG
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error reading CMN: " + ex.Message);
                    return;
                }
#endif


            if(Bep != null)
               Bep.Nodes = Conversion.Convert(Bep.Nodes, inputDEGameEnum, outputDEGameEnum);
            else
               HAct.Root = Conversion.Convert(HAct.AllNodes.ToList(), inputDEGameEnum, outputDEGameEnum)[0];
           
            Console.WriteLine();


            Conversion.ConvertedTypes = Conversion.ConvertedTypes.OrderBy(x => x.Length).ToHashSet();

            Console.WriteLine("Conversions");
            Console.WriteLine("-----------------");

            foreach (string str in Conversion.ConvertedTypes)
            {
                uint originalID = (uint)Enum.Parse(inputDEGameEnum, str);
                uint outputID = (uint)Enum.Parse(outputDEGameEnum, str);

                Console.WriteLine($"{str} ID {originalID} ------------> ID {outputID}");
            }

            if (Conversion.FailedTypes.Count > 0)
            {
                Conversion.FailedTypes = Conversion.FailedTypes.OrderBy(x => x.Length).ToHashSet();

                Console.WriteLine("\n\nFailures (these elements have been set to dummy ID and will not work)");
                Console.WriteLine("-----------------");

                foreach (string str in Conversion.FailedTypes)
                {
                    uint originalID = (uint)Enum.Parse(inputDEGameEnum, str);
                    Console.WriteLine($"{str} ID {originalID}");
                }
            }

            if (Conversion.BlackListedTypes.Count > 0)
            {
                Console.WriteLine("\nBlacklisted (these have been known to be incompatible across different engines or games and didn't get converted)");
                Console.WriteLine("-----------------");

                Conversion.BlackListedTypes = Conversion.BlackListedTypes.OrderBy(x => x.Length).ToHashSet();

                foreach (string str in Conversion.BlackListedTypes)
                {
                    uint originalID = (uint)Enum.Parse(inputDEGameEnum, str);
                    Console.WriteLine($"{str} ID {originalID}");
                }
            }





            string fileName = Path.GetFileName(file);
            string outputPath = Path.GetFullPath(Path.GetDirectoryName(file) + @"\");

            if(!bepMode)
                outputPath += fileName.Replace(".bin", "_converted.bin");
            else
                outputPath += fileName.Replace(".bep", "_converted.bep");

            if (!bepMode)
            {
                Conversion.ProcessSpecificConversion(HAct, GetNameFromEnum(inputDEGameEnum), GetNameFromEnum(outputDEGameEnum));
                HAct.GameVersion = CMN.GetVersionForGame(CMN.GetGameFromString(outputDEGame));

                CMN.Write(HAct, outputPath);
            }
            else
                BEP.Write(Bep, outputPath, CMN.GetVersionForGame(CMN.GetGameFromString(outputDEGame)));

            System.Threading.Thread.Sleep(5000);
#if !DEBUG
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error converting ID's: " + ex.Message);
            }
#endif
        }

    }
}
