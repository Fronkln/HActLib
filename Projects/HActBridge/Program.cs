using System;
using System.IO;
using System.Linq;
using HActLib;

namespace HActBridge
{
    internal class Program
    {
        public static GameVersion Default = GameVersion.DE1;
        public static Game InputGame = Game.YK2;
        public static Game OutputGame = Game.YK2;
        public static string HActCsvPath = null;

        static Ini Ini = null;

        private static HActInfo inf;
        static void Main(string[] args)
        {
            Console.WriteLine("HActBridge by Jhrino");
            Ini = new Ini(Path.Combine(AppContext.BaseDirectory, "hactbridge_config.ini"));

            if (args.Length <= 0)
            {
                Console.WriteLine("Usage: Drag and drop a OOE/OE HAct folder to this exe after configuring hactbridge_config.ini.");
                Console.WriteLine("You can also use it in commandline with arguments.\n");

                Console.WriteLine("Arguments for converting FROM OOE: (filepath) (inputgame) (outputgame) (hact_csv.bin filepath)");
                Console.WriteLine("Arguments for converting between OE and DE: (filepath) (inputgame) (outputgame)\n");

                Console.WriteLine(@"Example conversion from OOE to OE: HActBridge.exe ""C:/Games/Yakuza 3/hact_all/hact/1350.par.unpack"" y3 yk1 ""C:/Games/Yakuza 3/hact_csv/hact_csv/hact_csv.bin""");
                Console.WriteLine(@"Example conversion from OE to DE: HActBridge.exe ""C:/Games/Yakuza 0/hact_all/hact/m51000_disco_st_a.par.unpack"" yk1 ylad");

                System.Threading.Thread.Sleep(6000);
                return;
            }

            if (args.Length == 1)
            {
                InputGame = CMN.GetGameFromString(Ini.GetValue("input", "Settings", "y0"));
                OutputGame = CMN.GetGameFromString(Ini.GetValue("output", "Settings", "yk2"));
                HActCsvPath = Ini.GetValue("hact_csv_bin_path", "FromOOESettings", "");
            }
            else
            {
                InputGame = CMN.GetGameFromString(args[1]);
                OutputGame = CMN.GetGameFromString(args[2]);
            }

            inf = new HActInfo(args[0]);
            bool success = false;

            GameVersion outputGameVer = CMN.GetVersionForGame(OutputGame);
            bool isDE = CMN.IsDE(outputGameVer);

            if (inf.IsTEV)
                success = FromOOE(args);
            else
            {
                if (isDE)
                    success = HActFactory.ConvertOEToDE(args[0], args[0] + "_" + outputGameVer.ToString().ToLowerInvariant() + "_" + OutputGame.ToString().ToLowerInvariant(), OutputGame);
                else
                    if (outputGameVer == GameVersion.Y0_K1)
                    success = HActFactory.ConvertDEToOE(args[0], args[0] + "_" + outputGameVer.ToString().ToLowerInvariant() + "_" + OutputGame.ToString().ToLowerInvariant(), InputGame, OECMN.GetCMNVersionForGame(OutputGame));

            }

            if (success)
            {
                Console.WriteLine("Converted HAct for " + outputGameVer + " " + OutputGame);

                if(outputGameVer == GameVersion.Y0_K1 && CMN.IsDEGame(InputGame))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("GMT CONVERTER BREAKS DE ANIMATIONS CONVERTING TO OE, YOU MUST RETARGET MANUALLY ON BLENDER!");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
            else
                Console.WriteLine("Fail");

            System.Threading.Thread.Sleep(2500);
        }

        static bool FromOOE(string[] args)
        {
            CSV csv = null;
            uint ooeHactID = 0;

            if (inf.IsTEV)
            {
                if (args.Length > 3)
                    HActCsvPath = args[3];
                else
                    HActCsvPath = Ini.GetValue("hact_csv_bin_path", "FromOOESettings", "");

                string hactIDFolder = null;
                FileInfo tevDir = new FileInfo(inf.MainPath);

                if (tevDir.Directory.Name == "tmp")
                    hactIDFolder = tevDir.Directory.Parent.Name;
                else
                    hactIDFolder = tevDir.Directory.Name;

                ooeHactID = uint.Parse(hactIDFolder.Substring(0, 4));
            }

            GameVersion outputGameVer = CMN.GetVersionForGame(OutputGame);
            bool isDE = outputGameVer == GameVersion.DE1 || outputGameVer == GameVersion.DE2;

            bool success = false;

            string outputDir = args[0] + "_" + outputGameVer.ToString().ToLowerInvariant() + "_" + OutputGame.ToString().ToLowerInvariant();

            if (isDE)
                success = HActFactory.ConvertOOEToDE(new FileInfo(inf.MainPath).Directory.FullName, outputDir, OutputGame, HActCsvPath);
            else
            {
                if (outputGameVer == GameVersion.Y0_K1)
                    success = HActFactory.ConvertOOEToOE(args[0], args[0] + "_" + outputGameVer.ToString().ToLowerInvariant() + "_" + OutputGame.ToString().ToLowerInvariant(), OECMN.GetCMNVersionForGame(OutputGame), HActCsvPath);
            }

            return success;
        }
    }
}
