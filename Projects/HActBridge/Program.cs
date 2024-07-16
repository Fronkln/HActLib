using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HActLib;


using PIBLib;

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
                if (CMN.IsDEGame(InputGame))
                {
                    if (outputGameVer == GameVersion.OOE)
                        success = HActFactory.ConvertDEToOOE(args[0], args[0] + "_" + outputGameVer.ToString().ToLowerInvariant() + "_" + OutputGame.ToString().ToLowerInvariant(), "", InputGame);
                    else if (outputGameVer == GameVersion.Y0_K1)
                        success = FromDE(args, outputGameVer);
                }
                else
                {
                    if (outputGameVer == GameVersion.Y0_K1)
                    {
                        string outputDir = args[0] + "_" + outputGameVer.ToString().ToLowerInvariant() + "_" + OutputGame.ToString().ToLowerInvariant();

                        bool to_y5 = OutputGame == Game.Y5;

                        success = HActFactory.ConvertOEToOE(args[0], outputDir, to_y5);

                        HActDir hactDir = new HActDir();
                        hactDir.Open(args[0]);
                        HActDir ptcDir = hactDir.GetParticle();

                        HActFile[] pibs = ptcDir.FindFilesOfType(".pib");
                        HActFile[] tex = ptcDir.FindFilesOfType(".dds");

                        if (pibs.Length > 0)
                        {
                            string inputPibDir = new FileInfo(pibs[0].Path).FullName;
                            string pibDir = Path.Combine(outputDir, "ptc");

                            if (!Directory.Exists(pibDir))
                                Directory.CreateDirectory(pibDir);


                            foreach (var file in pibs)
                            {
                                BasePib pib = PIB.Read(file.Path);

                                Console.WriteLine("Converting " + pib.Name);
                                PIB.Write(PIB.Convert(pib, GetVersionForGame(OutputGame)), Path.Combine(pibDir, Path.GetFileName(file.Path)));
                            }

                            foreach (var file in tex)
                            {
                                File.Copy(file.Path, Path.Combine(pibDir, Path.GetFileName(file.Path)), true);
                            }
                        }
                    }
                    if (outputGameVer == GameVersion.OOE)
                    {
                        string outputDir = args[0] + "_" + outputGameVer.ToString().ToLowerInvariant() + "_" + OutputGame.ToString().ToLowerInvariant();
                        success = HActFactory.ConvertOEToOOE(args[0], outputDir, "");

                        HActDir hactDir = new HActDir();
                        hactDir.Open(args[0]);
                        HActDir ptcDir = hactDir.GetParticle();

                        HActFile[] pibs = ptcDir.FindFilesOfType(".pib");
                        HActFile[] tex = ptcDir.FindFilesOfType(".dds");

                        if (pibs.Length > 0)
                        {
                            string inputPibDir = new FileInfo(pibs[0].Path).FullName;
                            string pibDir = Path.Combine(outputDir, "ptc");

                            if (!Directory.Exists(pibDir))
                                Directory.CreateDirectory(pibDir);

                            foreach (var file in pibs)
                            {
                                BasePib pib = PIB.Read(file.Path);

                                Console.WriteLine("Converting " + pib.Name);
                                PIB.Write(PIB.Convert(pib, PibVersion.Y3), Path.Combine(pibDir, Path.GetFileName(file.Path)));
                            }

                            foreach (var file in tex)
                            {
                                File.Copy(file.Path, Path.Combine(pibDir, Path.GetFileName(file.Path)), true);
                            }
                        }
                    }
                    else if (outputGameVer >= GameVersion.Yakuza6Demo)
                        success = HActFactory.ConvertOEToDE(args[0], args[0] + "_" + outputGameVer.ToString().ToLowerInvariant() + "_" + OutputGame.ToString().ToLowerInvariant(), OutputGame);
                }

            }

            if (success)
            {
                Console.WriteLine("Converted HAct for " + outputGameVer + " " + OutputGame);

                if (outputGameVer == GameVersion.Y0_K1 && CMN.IsDEGame(InputGame))
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

        static bool FromDE(string[] args, GameVersion outputGameVer)
        {
            string outputDir = args[0] + "_" + outputGameVer.ToString().ToLowerInvariant() + "_" + OutputGame.ToString().ToLowerInvariant();
            bool success = HActFactory.ConvertDEToOE(args[0], outputDir, InputGame, OECMN.GetCMNVersionForGame(OutputGame));



            if(InputGame == Game.Y6 && OECMN.GetCMNVersionForGame(OutputGame) == 16)
            {
                string outputCMNDir = Path.Combine(outputDir, "cmn");
                string outputPTCDir = Path.Combine(outputDir, "ptc");
                HActInfo hactInf = new HActInfo(args[0]);

                DirectoryInfo hactDir = new DirectoryInfo(hactInf.MainPath);


                if(DEParticleConverter.Convert(InputGame, hactInf, outputPTCDir))
                {
                    OECMN cmn = OECMN.Read(Path.Combine(outputCMNDir, "cmn.bin"));
                    cmn.SetFlags(1); //Use PTC

                    OECMN.Write(cmn, Path.Combine(outputCMNDir, "cmn.bin"));
                }
            }

            return success;
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
            bool isDE = CMN.IsDE(outputGameVer);

            bool success = false;

            string outputDir = args[0] + "_" + outputGameVer.ToString().ToLowerInvariant() + "_" + OutputGame.ToString().ToLowerInvariant();


            if (isDE)
                success = HActFactory.ConvertOOEToDE(new FileInfo(inf.MainPath).Directory.FullName, outputDir, OutputGame, HActCsvPath);
            else
            {
                if (outputGameVer == GameVersion.Y0_K1)
                {
                    success = HActFactory.ConvertOOEToOE(args[0], outputDir, OECMN.GetCMNVersionForGame(OutputGame), HActCsvPath);

                    HActDir hactDir = new HActDir();
                    hactDir.Open(args[0]);
                    HActDir ptcDir = hactDir.GetParticle();

                    HActFile[] pibs = ptcDir.FindFilesOfType(".pib");
                    HActFile[] tex = ptcDir.FindFilesOfType(".dds");

                    if (pibs.Length > 0)
                    {
                        string inputPibDir = new FileInfo(pibs[0].Path).FullName;
                        string pibDir = Path.Combine(outputDir, "ptc");

                        if (!Directory.Exists(pibDir))
                            Directory.CreateDirectory(pibDir);


                        foreach (var file in pibs)
                        {
                            BasePib pib = PIB.Read(file.Path);
                            PIB.Write(PIB.Convert(pib, GetVersionForGame(OutputGame)), Path.Combine(pibDir, Path.GetFileName(file.Path)));
                        }

                        foreach (var file in tex)
                        {
                            File.Copy(file.Path, Path.Combine(pibDir, Path.GetFileName(file.Path)), true);
                        }
                    }
                }
            }

            return success;
        }


        static PibVersion GetVersionForGame(Game game)
        {
            switch (game)
            {
                default:
                    return PibVersion.LJ;
                case Game.Kenzan:
                    return PibVersion.Kenzan;
                case Game.Y3:
                    return PibVersion.Y3;
                case Game.Y4:
                    return PibVersion.Y3;
                case Game.Y5:
                    return PibVersion.Y5;
                case Game.Ishin:
                    return PibVersion.Ishin;
                case Game.Y0:
                    return PibVersion.Y0;
                case Game.Y6:
                    return PibVersion.Y6;
                case Game.YK2:
                    return PibVersion.YK2;
                case Game.JE:
                    return PibVersion.JE;
                case Game.YLAD:
                    return PibVersion.YLAD;
                case Game.LJ:
                    return PibVersion.LJ;
            }
        }
    }
}
