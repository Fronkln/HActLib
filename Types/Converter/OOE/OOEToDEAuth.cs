using HActLib.OOE;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HActLib
{
    public static class OOEToDEAuth
    {
        public static void Convert(string directory, Game deGame, bool convertAssets = true)
        {
            DirectoryInfo authCmnDir = GetDirectoryFiles(directory, "_cmn.par");
            Auth authFile = Auth.Read(Path.Combine(authCmnDir.FullName, "Auth.bin"));

            string outputDir = "converted_test";
            string outputCmnDir = Path.Combine(outputDir, "cmn");

            if(!Directory.Exists(outputCmnDir))
                Directory.CreateDirectory(outputCmnDir);

            CMN cmnFile = new CMN();
            cmnFile.HActEnd = authFile.Length;
            cmnFile.GameVersion = CMN.GetVersionForGame(deGame);
            cmnFile.Header.Version = OECMN.GetCMNVersionForGame(deGame);
            cmnFile.SetFlags(12);

            List<float> cutInfos = new List<float>();
            List<float> resourceCutInfos = new List<float>();

            foreach(AuthResourceCut cutInfo in authFile.CameraCuts)
            {
                cutInfos.Add(cutInfo.Start);
            }

            foreach (AuthResourceCut cutInfo in authFile.ResourceCuts)
            {
                resourceCutInfos.Add(cutInfo.Start);
            }

            cmnFile.CutInfo = cutInfos.ToArray();
            cmnFile.ResourceCutInfo = resourceCutInfos.ToArray();

            DENodePath root = new DENodePath();
            root.Category = AuthNodeCategory.Path;
            root.Name = "Root";
            root.Guid = Guid.NewGuid();

            cmnFile.Root = root;

            foreach(AuthNodeOOE ooeNode in authFile.Nodes)
            {
                Node node = Convert(ooeNode);
                root.Children.Add(node);
            }

            cmnFile.Root = root;

            CMN.Write(cmnFile, Path.Combine(outputCmnDir, "cmn.bin"));
            RES.Write(new RES(), Path.Combine(outputCmnDir, "res.bin"), true);
            TEX.Write(new TEX(), Path.Combine(outputCmnDir, "tex.bin"), true);

            string GMTConverterPath = @"GMT_Converter.exe";
            bool gmtExporterExists = File.Exists(GMTConverterPath);

            if (!gmtExporterExists)
            {
                Console.WriteLine("GMT Converter does not exist. The tool will not convert GMTs to DE.\n Please put GMT_Converter.exe in the directory of this exe.");
                System.Threading.Thread.Sleep(1500);
            }

            int current = 0;
            while (true)
            {
                string cutName = current.ToString("D3");
                DirectoryInfo resDir = GetDirectoryFiles(directory, "res_" + cutName);

                if (resDir == null)
                    break;

                string resFile = Path.Combine(resDir.FullName, "Resource.bin");

                if (!File.Exists(resFile))
                    continue;

                AuthResOOE res = AuthResOOE.Read(resFile);
                RES newRes = ConvertResource(res, cmnFile, authFile.ResourceCuts[current]);


                if(newRes != null)
                {
                    string resDirectory = Path.Combine(outputDir, cutName);

                    if(!Directory.Exists(resDirectory))
                        Directory.CreateDirectory(resDirectory);

                    RES.Write(newRes, Path.Combine(resDirectory, "res.bin"), true);
                    RES.Write(newRes, Path.Combine(resDirectory, "res_vo.bin"), true);
                    RES.Write(newRes, Path.Combine(resDirectory, "res_zh.bin"), true);

                    Thread convertAssetThread = new Thread(() => ConvertResThread(newRes, resDir, resDirectory, convertAssets));
                    convertAssetThread.Start();

                    /*
                    foreach(var resource in  newRes.Resources)
                    {
                        string origPath = Path.Combine(resDir.FullName, resource.Name);

                        if (!File.Exists(origPath))
                            continue;

                        //Just copy CMT
                        if (resource.Type == ResourceType.CameraMotion)
                            File.Copy(origPath, Path.Combine(resDirectory, resource.Name), true);

                        if(resource.Type == ResourceType.CharacterMotion)
                        {
                            if (gmtExporterExists)
                            {
                                string dir = Path.Combine(resDirectory, resource.Name);

                                string args = $"-ig y3 -og yk2 -i {'\u0022'}{origPath}{'\u0022'} -o {'\u0022'}{dir}{'\u0022'}";

                                ProcessStartInfo start = new ProcessStartInfo(GMTConverterPath);
                                start.Arguments = args;
                                start.WindowStyle = ProcessWindowStyle.Hidden;
                                start.CreateNoWindow = true;

                                Process.Start(start).WaitForExit();
                            }
                        }
                    }
                    */
                }

                current++;
            }
        }

        private static  void ConvertResThread(RES newRes, DirectoryInfo resDir, string outputDirectory, bool convertAssets)
        {
            string GMTConverterPath = @"GMT_Converter.exe";
            bool gmtExporterExists = File.Exists(GMTConverterPath);

            foreach (var resource in newRes.Resources)
            {
                string origPath = Path.Combine(resDir.FullName, resource.Name);

                //Just copy CMT
                if (resource.Type == ResourceType.CameraMotion)
                {
                    origPath += ".cmt";

                    if (!File.Exists(origPath))
                        continue;

                    File.Copy(origPath, Path.Combine(outputDirectory, resource.Name + ".cmt"), true);
                }

                if (convertAssets)
                {
                    if (resource.Type == ResourceType.CharacterMotion)
                    {
                        if (gmtExporterExists)
                        {
                            origPath += ".gmt";

                            if (!File.Exists(origPath))
                                continue;

                            string dir = Path.Combine(outputDirectory, resource.Name + ".gmt");

                            string args = $"-ig y3 -og yk2 -i {'\u0022'}{origPath}{'\u0022'} -o {'\u0022'}{dir}{'\u0022'}";

                            ProcessStartInfo start = new ProcessStartInfo(GMTConverterPath);
                            start.Arguments = args;
                            start.WindowStyle = ProcessWindowStyle.Hidden;
                            start.CreateNoWindow = true;

                            Process.Start(start).WaitForExit();
                        }
                    }
                }
            }
        }

        public static RES ConvertResource(AuthResOOE resource, CMN cmn, AuthResourceCut cutInfo)
        {
            RES newRes = new RES();

            foreach(AuthResourceOOE ooeResource in resource.Resources)
            {
                Resource convertedResource = null;
                Guid guid = ooeResource.GUID;

                switch(ooeResource.Type)
                {
                    case AuthResourceOOEType.Character:
                        convertedResource = new Resource();
                        convertedResource.Type = ResourceType.Character;
                        convertedResource.Unk1 = 2;
                        convertedResource.Unk2 = 1;

                        DENodeCharacter characterNode = (DENodeCharacter)cmn.FindNodeByGUID(guid);

                        if(characterNode != null)
                        {
                            string scaleLookup = "height_" + ooeResource.UnknownData[3].ToString();
                            DECharacterScaleID scale;
                            try
                            {
                                scale = (DECharacterScaleID)Enum.Parse(typeof(DECharacterScaleID), Enum.GetNames<DECharacterScaleID>().FirstOrDefault(x => x == scaleLookup));
                            }
                            catch
                            {
                                scale = DECharacterScaleID.invalid;
                            }

                            characterNode.ScaleID = (uint)scale;
                        }

                        //Height is also stored on OOE res. do that later
                        break;
                    case AuthResourceOOEType.CameraMotion:
                        convertedResource = new Resource();
                        convertedResource.Type = ResourceType.CameraMotion;
                        convertedResource.Unk1 = 0;
                        convertedResource.Unk2 = 1;
                        break;

                     //TODO: Seperate
                    case AuthResourceOOEType.ObjectMotion:
                        convertedResource = new Resource();
                        convertedResource.Type = ResourceType.CharacterMotion;
                        convertedResource.Unk1 = 0;
                        convertedResource.Unk2 = 1;
                        break;
                }

                if(convertedResource != null)
                {
                    convertedResource.NodeGUID = guid;

                    if(convertedResource.Type == ResourceType.Character)
                    {
                        convertedResource.StartFrame = 0;
                        convertedResource.EndFrame = cmn.HActEnd;
                    }
                    else
                    {
                        convertedResource.StartFrame = cutInfo.Start;
                        convertedResource.EndFrame = cutInfo.End;
                    }
                    convertedResource.Name = ooeResource.Resource.Replace(".gmt", "").Replace(".cmt", "");

                    newRes.Resources.Add(convertedResource);
                }
            }

            return newRes;
        }

        public static Node Convert(AuthNodeOOE node)
        {
            Node deNode = null;

            switch (node.Type)
            {
                case AuthNodeTypeOOE.Path:
                    deNode = GeneratePath();
                    break;

                case AuthNodeTypeOOE.Character:
                    deNode = GenerateCharacter(node);
                    break;

                case AuthNodeTypeOOE.Camera:
                    deNode = GenerateCamera(node);
                    break;
            }

            if (deNode != null)
            {
                foreach (AuthNodeOOE child in node.Children)
                {
                    var result = Convert(child);

                    if (result != null)
                        deNode.Children.Add(result);
                }

                foreach(EffectBase effect in node.Effects)
                {
                    var convertedEffect = OOEToDE.ConvertElementEffect(effect);

                    if(convertedEffect != null)
                        deNode.Children.Add(convertedEffect);
                }

                deNode.Guid = node.Guid;
                deNode.Name = node.Type.ToString();
            }
            return deNode;
        }

        public static DENodePath GeneratePath()
        {
            DENodePath root = new DENodePath();
            root.Category = AuthNodeCategory.Path;
            root.Guid = Guid.NewGuid();
            root.Matrix = Matrix4x4.Default;
            root.Name = "Path";

            return root;
        }
        public static DENodeCharacter GenerateCharacter(AuthNodeOOE node)
        {
            DENodeCharacter chara = new DENodeCharacter();
            chara.Guid = Guid.NewGuid();
            chara.Name = node.Type.ToString();

            chara.CharacterID = 1;
            chara.ScaleID = 22;

            DENodeCharacterMotion motion = new DENodeCharacterMotion();
            motion.Guid = node.AnimationData.Guid;
            motion.Start.Tick = new GameTick(node.AnimationData.StartFrame).Tick;
            motion.End.Tick = new GameTick(node.AnimationData.EndFrame).Tick;
            motion.Name = "Motion";

            chara.Children.Add(motion);

            return chara;
        }

        public static NodeCamera GenerateCamera(AuthNodeOOE cam)
        {
            NodeCamera camera = new NodeCamera();

            camera.Name = cam.Type.ToString();
            camera.Guid = cam.Guid;

            NodeCameraMotion motion = new NodeCameraMotion();
            motion.Guid = cam.AnimationData.Guid;
            motion.Name = "Motion";
            motion.Start.Frame = cam.AnimationData.StartFrame;
            motion.End.Frame = cam.AnimationData.EndFrame;

            camera.Children.Add(motion);

            return camera;
        }



        private static DirectoryInfo GetDirectoryFiles(string root, string dirToFind)
        {
            DirectoryInfo[] dirs = new DirectoryInfo(root).GetDirectories();

            DirectoryInfo dir = dirs.FirstOrDefault(x => x.Name.Contains(dirToFind));

            if (dir == null)
                return null;

            if (dir.GetFiles().Length > 0)
                return dir;
            else
            {
                var dirs2 = dir.GetDirectories();

                if (dirs2.Length <= 0)
                    return null;
                return dirs2[0];
            }
        }
    }
}
