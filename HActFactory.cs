using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Yarhl.FileFormat;



namespace HActLib
{
    public static class HActFactory
    {
        internal const string GMTConverterPath = @"GMT_Converter.exe";
        internal const string GMTConverterPathV1 = @"gmt_converter_v1.exe";

        private static string GetCMNPath(string path)
        {
            string def = Path.Combine(path, "cmn");
            string alt2 = Path.Combine(path, "cmn.par.unpack", "cmn");
            string alt1 = Path.Combine(path, "cmn.par.unpack");

            if (Directory.Exists(def))
                return def;

            if (Directory.Exists(alt2))
                return alt2;

            if (Directory.Exists(alt1))
                return alt1;

            return null;
        }

        private static string GetRESPath(string path)
        {
            string def = Path.Combine(path, "000");
            string alt2 = Path.Combine(path, "000.par.unpack", "000");
            string alt1 = Path.Combine(path, "000.par.unpack");

            if (Directory.Exists(def))
                return def;

            if (Directory.Exists(alt2))
                return alt2;

            if (Directory.Exists(alt1))
                return alt1;

            return null;
        }

        private static string GetTEVPath(string path)
        {
            string def = Path.Combine(path, "tmp");

            if (Directory.Exists(def))
                return def;

            return null;
        }

        public static bool ConvertOOEToOE(string tevDir, string outputDir, uint outputOEVer, string csvPath)
        {
            if (!Directory.Exists(tevDir) && !File.Exists(tevDir))
                return false;

            if (File.Exists(tevDir))
                tevDir = new DirectoryInfo(tevDir).Parent.FullName;

            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            string hactIDFolder = null;
            DirectoryInfo tevDir2 = new DirectoryInfo(tevDir);

            if (tevDir2.Name == "tmp")
                hactIDFolder = tevDir2.Parent.Name;
            else
                hactIDFolder = tevDir2.Name;

            uint ooeHactId = uint.Parse(hactIDFolder.Substring(0, 4));

            CSV csv = CSV.Read(csvPath);

            if (csv == null)
                throw new Exception("CSV file is missing/invalid.");

            CSVHAct csvData = csv.TryGetEntry(ooeHactId);

            HActDir hactInf = new HActDir();
            hactInf.Open(tevDir);
            byte[] tevBuf = hactInf.FindFile("hact_tev.bin").Read();


            if (tevBuf == null)
                throw new Exception("TEV not found\nPath: " + tevDir);

            bool gmtExporterExists = File.Exists(AppDomain.CurrentDomain.BaseDirectory + "GMT_Converter.exe");

            if (!gmtExporterExists)
            {
                Console.WriteLine("GMT Converter does not exist. The tool will not convert GMTs to DE.\n Please put GMT_Converter.exe in the directory of this exe.");
                System.Threading.Thread.Sleep(1500);
            }

            TEV tev = TEV.Read(tevBuf);
            RES res = new RES();
            OECMN convertedTEV = TEV.ToOE(tev, csvData, outputOEVer);

            string outputCMNDir = Path.Combine(outputDir, "cmn");
            string outputRESDir = Path.Combine(outputDir, "000");


            if (!Directory.Exists(outputCMNDir))
                Directory.CreateDirectory(outputCMNDir);
            if (!Directory.Exists(outputRESDir))
                Directory.CreateDirectory(outputRESDir);

            HActDir ptc = hactInf.GetParticle();

            foreach (OENodeCharacter chara in convertedTEV.AllCharacters)
            {
                Resource resEntry = new Resource();
                resEntry.NodeGUID = chara.Guid;
                resEntry.Type = ResourceType.Character;
                resEntry.EndFrame = convertedTEV.CMNHeader.End;
                resEntry.Unk1 = 1;
                resEntry.Unk2 = 1;
                resEntry.Name = chara.Name;

                res.Resources.Add(resEntry);
            }

            foreach(OENodeCharacter chara in convertedTEV.AllCharacters) 
            {
                NodeMotionBase[] motions = chara.GetChildsOfType<NodeMotionBase>();

                foreach(NodeMotionBase motion in motions)
                {
                    Resource motionRes = new Resource();
                    motionRes.NodeGUID = motion.Guid;
                    motionRes.Type = ResourceType.CharacterMotion;
                    motionRes.Name = motion.Name.Replace(".gmt", "");
                    motionRes.StartFrame = motion.Start;
                    motionRes.EndFrame = motion.End;
                    motionRes.Unk1 = 0;
                    motionRes.Unk2 = 1;

                    res.Resources.Add(motionRes);

                    string inputGmtDir = Path.Combine(tevDir, motion.Name);
                    string outputGmtDir = Path.Combine(outputRESDir, motion.Name);

                    //Y5, just copy anim
                    if (outputOEVer < 15)
                        File.Copy(inputGmtDir, outputGmtDir, true);
                    else
                    {
                        if (gmtExporterExists)
                            System.Diagnostics.Process.Start(GMTConverterPath, $"-ig y3 -og y0 -i {'\u0022'}{inputGmtDir}{'\u0022'} -o {'\u0022'}{outputGmtDir}{'\u0022'}");
                    }
                }
            }

            foreach(NodeCamera cam in convertedTEV.AllCameras)
            {
                NodeCameraMotion[] motions = cam.GetChildsOfType<NodeCameraMotion>();

                foreach(NodeCameraMotion motion in motions)
                {
                    Resource camRes = new Resource();
                    camRes.NodeGUID = motion.Guid;
                    camRes.Type = ResourceType.CameraMotion;
                    camRes.Name = motion.Name.Replace(".cmt", "");
                    camRes.StartFrame = motion.Start;
                    camRes.EndFrame = motion.End;
                    camRes.Unk1 = 0;
                    camRes.Unk2 = 1;

                    res.Resources.Add(camRes);
                    File.Copy(Path.Combine(tevDir, motion.Name), Path.Combine(outputRESDir, motion.Name), true);
                }
            }


            OECMN.Write(convertedTEV, Path.Combine(outputCMNDir, "cmn.bin"));
            RES.Write(new RES(), Path.Combine(outputCMNDir, "res.bin"), false);
            TEX.Write(new TEX(), Path.Combine(outputCMNDir, "tex.bin"), false);
            RES.Write(res, Path.Combine(outputRESDir, "res.bin"), false);

            return true;
        }

        public static bool ConvertOOEToDE(string ooePath, string outputDir, Game game, string csvPath)
        {
            if (!Directory.Exists(ooePath) && !File.Exists(ooePath))
                return false;

            if (File.Exists(ooePath))
                ooePath = new DirectoryInfo(ooePath).Parent.FullName;

            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);


            string tevPath = ooePath;
            TEV tev = TEV.Read(Path.Combine(tevPath, "hact_tev.bin"));

            string hactIDFolder = null;
            DirectoryInfo tevDir = new DirectoryInfo(ooePath);

            if (tevDir.Name == "tmp")
                hactIDFolder = tevDir.Parent.Name;
            else
                hactIDFolder = tevDir.Name;

            uint ooeHactId = uint.Parse(hactIDFolder.Substring(0, 4));

            CSV csv = CSV.Read(csvPath);

            if (csv == null)
                throw new Exception("CSV Not Found");

            CSVHAct csvData = csv.TryGetEntry(ooeHactId);

            //write the output here
            string outputCMNDir = Path.Combine(outputDir, "cmn");
            string outputRESDir = Path.Combine(outputDir, "000");

            if (!Directory.Exists(outputCMNDir))
                Directory.CreateDirectory(outputCMNDir);

            if (!Directory.Exists(outputRESDir))
                Directory.CreateDirectory(outputRESDir);

            GameVersion ver = CMN.GetVersionForGame(game);

            CMN convertedTEV = TEV.ToDE(tev, game, csvData, Path.Combine(ooePath, "ptc"));
            RES res = new RES();

            foreach(DENodeCharacter chara in convertedTEV.AllCharacters)
            {
                Resource resEntry = new Resource();
                resEntry.NodeGUID = chara.Guid;
                resEntry.Type = ResourceType.Character;
                resEntry.EndFrame = convertedTEV.Header.End;
                resEntry.Unk1 = 0;
                resEntry.Unk2 = 1;
                resEntry.Name = "character_" + (uint)chara.ReplaceID;

                res.Resources.Add(resEntry);
            }

            foreach (NodeCameraMotion camMot in convertedTEV.AllNodes.Where(x => x.Category == AuthNodeCategory.CameraMotion).ToArray())
            {
                if (string.IsNullOrEmpty(camMot.Name))
                    continue;

                Resource resEntry = new Resource();
                resEntry.NodeGUID = camMot.Guid;
                resEntry.Type = ResourceType.CameraMotion;
                resEntry.EndFrame = camMot.End;
                resEntry.Unk1 = 0;
                resEntry.Unk2 = 1;
                resEntry.Name = camMot.Name.Replace(".cmt", "");

                res.Resources.Add(resEntry);
            }

            foreach (DENodeCharacterMotion charMot in convertedTEV.AllNodes.Where(x => x.Category == AuthNodeCategory.CharacterMotion).ToArray())
            {
                if (string.IsNullOrEmpty(charMot.Name))
                    continue;

                Resource resEntry = new Resource();
                resEntry.NodeGUID = charMot.Guid;
                resEntry.Type = ResourceType.CharacterMotion;
                resEntry.EndFrame = convertedTEV.Header.End;
                resEntry.Unk1 = 0;
                resEntry.Unk2 = 1;
                resEntry.Name = charMot.Name.Replace(".gmt", "");

                res.Resources.Add(resEntry);
            }

            bool gmtExporterExists = File.Exists(AppDomain.CurrentDomain.BaseDirectory +  "GMT_Converter.exe");

            if (!gmtExporterExists)
            {
                Console.WriteLine("GMT Converter does not exist. The tool will not convert GMTs to DE.\n Please put GMT_Converter.exe in the directory of this exe.");
                System.Threading.Thread.Sleep(1500);
            }

            foreach (Resource resInf in res.Resources)
            {

                if (gmtExporterExists && resInf.Type == ResourceType.CharacterMotion)
                {
                   

                    string inp = Path.Combine(tevPath, resInf.Name.Replace("\0", "") + ".gmt");
                    string dir = Path.Combine(outputRESDir, resInf.Name.Replace("\0", "") + ".gmt");

                    var proc = System.Diagnostics.Process.Start(GMTConverterPath, $"-ig y3 -og yk2 -i {'\u0022'}{inp}{'\u0022'} -o {'\u0022'}{dir}{'\u0022'}");
                    proc.WaitForExit();
                }
                else if (resInf.Type == ResourceType.CameraMotion)
                {
                    string inp = Path.Combine(tevPath, resInf.Name.Replace("\0", "") + ".cmt");
                    string dir = Path.Combine(outputRESDir, resInf.Name.Replace("\0", "") + ".cmt");

                    //this is an actual problem. 6080 richardson
                    if(!string.IsNullOrEmpty(resInf.Name))
                        File.Copy(inp, dir, true);
                }
            }

            CMN.Write(convertedTEV, Path.Combine(outputCMNDir, "cmn.bin"));

            //also write vo and res
            if (game >= Game.LJ)
            {
                CMN.Write(convertedTEV, Path.Combine(outputCMNDir, "cmn_vo.bin"));
                RES.Write(res, Path.Combine(outputRESDir, "res_vo.bin"), true);
            }

            RES.Write(res, Path.Combine(outputRESDir, "res.bin"), true);
            RES.Write(new RES(), Path.Combine(outputCMNDir, "res.bin"), true);
            RES.Write(new RES(), Path.Combine(outputCMNDir, "tex.bin"), true);

            return true;
        }

        public static bool ConvertOEToDE(string oePath, string outputDir, Game game)
        {
            if (!Directory.Exists(oePath))
                return false;

            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);
          
            HActInfo hactInf = new HActInfo(oePath);     
            string cmnPath = hactInf.MainPath;

            if (string.IsNullOrEmpty(cmnPath))
                throw new Exception("CMN not found\nPath: " + cmnPath);

            if (hactInf.ResourcesPaths.Length <= 0)
                throw new Exception("RES not found\nPath: " + oePath);

            OECMN cmn = OECMN.Read(cmnPath);

            CMN.LastHActDEGame = game;
            CMN.LastGameVersion = CMN.GetVersionForGame(game);
            CMN convertedOE = OECMN.ToDE(cmn, CMN.LastGameVersion);

            string cmnDir = new DirectoryInfo(cmnPath).Parent.FullName;
            string outputCMNDir = Path.Combine(outputDir, "cmn");

            if (!Directory.Exists(outputCMNDir))
                Directory.CreateDirectory(outputCMNDir);

            ConvertResourceAssets(Path.Combine(outputDir, "res"), hactInf, false, cmn.Version, 18);

            RES res = RES.Read(Path.Combine(cmnDir, "res.bin"), false);
            TEX tex = TEX.Read(Path.Combine(cmnDir, "tex.bin"), false);         

            IEnumerable<string> texStrings = tex.Textures.Select(x => x.TextureName);

            foreach (DEElementUITexture uiTex in convertedOE.AllElements.Where(x => x is DEElementUITexture))
                for(int i = 0; i < tex.Textures.Count; i++)
                {
                    TEX.Texture texture = tex.Textures[i];

                    if (uiTex.TextureName == texture.TextureName.Replace(".dds", ""))
                    {
                        texture.GUID = uiTex.Guid;
                        tex.Textures[i] = texture;
                        break;
                    }
                }

            List<DisableFrameInfo> disableFramesToRemove = new List<DisableFrameInfo>();

            foreach (DisableFrameInfo inf in convertedOE.DisableFrameInfo)
                if (inf.Start == cmn.HActEnd && inf.End == 0)
                    disableFramesToRemove.Add(inf);

            foreach(var frame in disableFramesToRemove)
                cmn.DisableFrameInfo.Remove(frame);

            CMN.Write(convertedOE, Path.Combine(outputCMNDir, "cmn.bin"));
            RES.Write(res, Path.Combine(outputCMNDir, "res.bin"), true);
            TEX.Write(tex, Path.Combine(outputCMNDir, "tex.bin"), true);

            //also write VO and res
            if (CMN.LastGameVersion >= GameVersion.DE2)
                CMN.Write(convertedOE, Path.Combine(outputCMNDir, "cmn_vo.bin"));


            return true;
        }

        public static bool ConvertDEToOE(string dePath, string outputDir, Game inputGame, uint outputOEVer)
        {
            if (!Directory.Exists(dePath))
                return false;

            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            HActInfo hactInf = new HActInfo(dePath);
            string cmnDir = hactInf.MainPath;

            if (string.IsNullOrEmpty(cmnDir))
                throw new Exception("CMN not found\nPath: " + cmnDir);

            if (hactInf.ResourcesPaths.Length <= 0)
                throw new Exception("RES not found\nPath: " + dePath);

            CMN deCmn = CMN.Read(hactInf.MainPath, inputGame);
            string outputCMNDir = Path.Combine(outputDir, "cmn");

            if (!Directory.Exists(outputCMNDir))
                Directory.CreateDirectory(outputCMNDir);

            ConvertResourceAssets(Path.Combine(outputDir, "res"), hactInf, true, 18, outputOEVer);

            OECMN convertedDE = CMN.ToOE(deCmn, outputOEVer);
            OECMN.Write(convertedDE, Path.Combine(outputCMNDir, "cmn.bin"));

            RES.Write(new RES(), Path.Combine(outputCMNDir, "res.bin"), false);
            TEX.Write(new TEX(), Path.Combine(outputCMNDir, "tex.bin"), false);

            return true;
        }

        public static bool ConvertOEToOE(string oePath, string outputDir, bool to_y5)
        {
            if (!Directory.Exists(oePath))
                return false;

            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            HActInfo hactInf = new HActInfo(oePath);
            string cmnDir = hactInf.MainPath;

            if (string.IsNullOrEmpty(cmnDir))
                throw new Exception("CMN not found\nPath: " + cmnDir);

            if (hactInf.ResourcesPaths.Length <= 0)
                throw new Exception("RES not found\nPath: " + oePath);

            Game inputGame;
            Game outputGame;

            string inputGmt;
            string outputGmt;

            if(to_y5)
            {
                inputGame = Game.Y0;
                outputGame = Game.Y5;

                inputGmt = "y0";
                outputGmt = "y5";
            }
            else
            {
                inputGame = Game.Y5;
                outputGame = Game.Y0;

                inputGmt = "y5";
                outputGmt = "y0";
            }

            OECMN cmn = OECMN.Read(cmnDir);
            cmn.AllNodes = RyuseOEModule.ConvertNodes(cmn.AllNodes, inputGame, outputGame).OutputNodes;

            uint version = inputGame == Game.Y5 ? (uint)16 : (uint)10;

            cmn.Version = version;
            cmn.CMNHeader.Version = version;

            RES resFile = RES.Read(hactInf.ResourcesPaths[0], false);


            string outputRESDir = Path.Combine(new DirectoryInfo(outputDir).FullName, "000");
            string outputCMNDir = Path.Combine(new DirectoryInfo(outputDir).FullName, "cmn");

            if (!Directory.Exists(outputRESDir))
                Directory.CreateDirectory(outputRESDir);

            if (!Directory.Exists(outputCMNDir))
                Directory.CreateDirectory(outputCMNDir);

            File.Copy(new FileInfo(cmnDir).Directory.FullName + "/tex.bin", Path.Combine(outputCMNDir, "tex.bin"), true);
            File.Copy(new FileInfo(cmnDir).Directory.FullName + "/res.bin", Path.Combine(outputCMNDir, "res.bin"), true);

            foreach (Resource res in resFile.Resources)
            {
                string inputDir = new DirectoryInfo(hactInf.ResourcesPaths[0]).Parent.FullName;

                if (res.Type == ResourceType.CharacterMotion)
                {
                    string inp = Path.Combine(inputDir, res.Name.Replace("\0", "") + ".gmt");
                    string dir = Path.Combine(outputRESDir, res.Name.Replace("\0", "") + ".gmt");

                    var proc = System.Diagnostics.Process.Start(GMTConverterPath, $"-ig {inputGmt} -og {outputGmt} -i {'\u0022'}{inp}{'\u0022'} -o {'\u0022'}{dir}{'\u0022'}");
                }
                else if (res.Type == ResourceType.CameraMotion)
                {
                    string inp = Path.Combine(inputDir, res.Name.Replace("\0", "") + ".cmt");
                    string dir = Path.Combine(outputRESDir, res.Name.Replace("\0", "") + ".cmt");

                    File.Copy(inp, dir, true);
                }

            }

            RES.Write(resFile, Path.Combine(outputDir, "000", "res.bin"), false);
            OECMN.Write(cmn, Path.Combine(outputCMNDir, "cmn.bin"));


            HActDir hactDir = new HActDir();
            hactDir.Open(oePath);

            HActDir ptc = hactDir.GetParticle();

            return true;
        }

        public static bool ConvertOEToOOE(string oePath, string outputDir, string csvPath)
        {
            if (!Directory.Exists(oePath))
                return false;

            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            HActInfo hactInf = new HActInfo(oePath);
            string cmnDir = hactInf.MainPath;

            if (string.IsNullOrEmpty(cmnDir))
                throw new Exception("CMN not found\nPath: " + cmnDir);

            if (hactInf.ResourcesPaths.Length <= 0)
                throw new Exception("RES not found\nPath: " + oePath);

            OECMN cmn = OECMN.Read(cmnDir);

            TEV tev = (TEV)ConvertFormat.With<OEToOOE>(new OEToOOEConversionInfo { Cmn = cmn, CMNPath = cmnDir });
            RES resFile = RES.Read(hactInf.ResourcesPaths[0], false);

            foreach(Resource res in resFile.Resources)
            {
                string inputDir = new DirectoryInfo(hactInf.ResourcesPaths[0]).Parent.FullName;
                string outputRESDir = Path.Combine(new DirectoryInfo(outputDir).Parent.FullName);

                if (res.Type == ResourceType.CharacterMotion)
                {
                    string inp = Path.Combine(inputDir, res.Name.Replace("\0", "") + ".gmt");
                    string dir = Path.Combine(outputDir, res.Name.Replace("\0", "") + ".gmt");

                   var proc = System.Diagnostics.Process.Start(GMTConverterPath, $"-ig y0 -og y3 -i {'\u0022'}{inp}{'\u0022'} -o {'\u0022'}{dir}{'\u0022'}");
                }
                else if(res.Type == ResourceType.CameraMotion)
                {
                    string inp = Path.Combine(inputDir, res.Name.Replace("\0", "") + ".cmt");
                    string dir = Path.Combine(outputDir, res.Name.Replace("\0", "") + ".cmt");

                    File.Copy(inp, dir, true);
                }

            }
            

            TEV.Write(tev, Path.Combine(outputDir, "hact_tev.bin"));

            return true;
        }

        public static bool ConvertDEToOOE(string oePath, string outputDir, string csvPath, Game game)
        {
            if (!Directory.Exists(oePath))
                return false;

            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            HActInfo hactInf = new HActInfo(oePath);
            string cmnDir = hactInf.MainPath;

            if (string.IsNullOrEmpty(cmnDir))
                throw new Exception("CMN not found\nPath: " + cmnDir);

            if (hactInf.ResourcesPaths.Length <= 0)
                throw new Exception("RES not found\nPath: " + oePath);

            CMN cmn = CMN.Read(cmnDir, game);

            TEV tev = (TEV)ConvertFormat.With<DEToOOE>(new DEToOOEConversionInfo { Cmn = cmn, CMNPath = cmnDir, Game = game });
            RES resFile = RES.Read(hactInf.ResourcesPaths[0], true);

            foreach (Resource res in resFile.Resources)
            {
                string inputDir = new DirectoryInfo(hactInf.ResourcesPaths[0]).Parent.FullName;
                string outputRESDir = Path.Combine(new DirectoryInfo(outputDir).Parent.FullName);

                if (res.Type == ResourceType.CharacterMotion)
                {
                    string inp = Path.Combine(inputDir, res.Name.Replace("\0", "") + ".gmt");
                    string dir = Path.Combine(outputDir, res.Name.Replace("\0", "") + ".gmt");

                    var proc = System.Diagnostics.Process.Start(GMTConverterPath, $"-ig yk2 -og y3 -i {'\u0022'}{inp}{'\u0022'} -o {'\u0022'}{dir}{'\u0022'}");
                }
                else if (res.Type == ResourceType.CameraMotion)
                {
                    string inp = Path.Combine(inputDir, res.Name.Replace("\0", "") + ".cmt");
                    string dir = Path.Combine(outputDir, res.Name.Replace("\0", "") + ".cmt");

                    File.Copy(inp, dir, true);
                }

            }

            TEV.Write(tev, Path.Combine(outputDir, "hact_tev.bin"));

            return true;
        }

        public static void ConvertResourceAssets(string outputDir, HActInfo hactInf, bool isDE, uint inputHactVersion, uint targetHactVersion)
        {
            bool gmtExporterExists = File.Exists(AppDomain.CurrentDomain.BaseDirectory + "GMT_Converter.exe");

            if (!gmtExporterExists)
            {
                Console.WriteLine("GMT Converter does not exist. The tool will not convert GMTs.\n Please put GMT_Converter.exe in the directory of this exe.");
                System.Threading.Thread.Sleep(500);
            }

            //Process res.bin
            for(int i = 0; i < hactInf.ResourcesPaths.Length; i++)
            {
                string str = hactInf.ResourcesPaths[i];

                RES res = RES.Read(str, isDE);
                string inputDir = new DirectoryInfo(str).Parent.FullName;
                string outputRESDir = Path.Combine(new DirectoryInfo(outputDir).Parent.FullName, i.ToString("D3"));

                if (!Directory.Exists(outputRESDir))
                    Directory.CreateDirectory(outputRESDir);

                RES filteredRes = new RES() { Resources = res.Resources };


                foreach (Resource resInf in filteredRes.Resources)
                {
                    if (resInf.Type == ResourceType.CharacterMotion || resInf.Type == ResourceType.CameraMotion)
                        resInf.Unk1 = 0;
                    else if(resInf.Type == ResourceType.Character)
                        resInf.Unk1 = 1;

                    if (gmtExporterExists && resInf.Type == ResourceType.CharacterMotion)
                    {
                        string inp = Path.Combine(inputDir, resInf.Name.Replace("\0", "") + ".gmt");
                        string dir = Path.Combine(outputRESDir, resInf.Name.Replace("\0", "") + ".gmt");

                        string inputGame = "";
                        string outputGame = "";

                        if (targetHactVersion >= 18)
                            outputGame = "yk2"; 
                        else
                            outputGame = (targetHactVersion <= 10 ? "y5" : "yk1");

                        if (inputHactVersion >= 18)
                            inputGame = "yk2";
                        else if (inputHactVersion >= 15)
                            inputGame = "y0";
                        else
                            inputGame = "y5";

                        string path = $"{'\u0022'}{inp}{'\u0022'} -o {'\u0022'}{dir}{'\u0022'}";
                        string outputPath = $"{'\u0022'}{dir}{'\u0022'}";

                        var proc = System.Diagnostics.Process.Start(GMTConverterPath, $"-ig {inputGame} -og {outputGame} -i {path} -o {outputPath}");
                        //proc.WaitForExit();
                    }
                    else if (resInf.Type == ResourceType.CameraMotion)
                    {
                        string inp = Path.Combine(inputDir, resInf.Name + ".cmt");
                        string dir = Path.Combine(outputRESDir, resInf.Name.Replace("\0", "") + ".cmt");

                        File.Copy(inp, dir, true);
                    }
                    else if(resInf.Type == ResourceType.AssetMotion)
                    {
                        string inp = Path.Combine(inputDir, resInf.Name + ".gmt");
                        string dir = Path.Combine(outputRESDir, resInf.Name.Replace("\0", "") + ".gmt");

                        File.Copy(inp, dir, true);
                    }
                    else if(resInf.Type == ResourceType.PathMotion)
                    {
                        string inp = Path.Combine(inputDir, resInf.Name + ".gmt");
                        string dir = Path.Combine(outputRESDir, resInf.Name.Replace("\0", "") + ".gmt");

                        File.Copy(inp, dir, true);
                    }
                }

                RES.Write(filteredRes, Path.Combine(outputRESDir, "res.bin"), !isDE);

                if (targetHactVersion >= 18 && CMN.LastHActDEGame >= Game.LJ)
                    RES.Write(filteredRes, Path.Combine(outputRESDir, "res_vo.bin"), true);
            }

        }

    }
}
