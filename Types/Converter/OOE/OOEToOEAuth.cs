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
    public static class OOEToOEAuth
    {
        public static void Convert(string directory, Game oeGame, bool convertAssets = true)
        {
            DirectoryInfo authCmnDir = GetDirectoryFiles(directory, "_cmn.par");
            Auth authFile = Auth.Read(Path.Combine(authCmnDir.FullName, "Auth.bin"));

            string outputDir = "converted_test2";
            string outputCmnDir = Path.Combine(outputDir, "cmn");

            if (!Directory.Exists(outputCmnDir))
                Directory.CreateDirectory(outputCmnDir);

            OECMN cmnFile = new OECMN();
            cmnFile.HActEnd = authFile.Length;
            cmnFile.Version = OECMN.GetCMNVersionForGame(oeGame);
            cmnFile.SetFlags(4);
            cmnFile.SetChainCameraIn(-1);
            cmnFile.SetChainCameraOut(-1);

            List<float> cutInfos = new List<float>();
            List<float> resourceCutInfos = new List<float>();

            foreach (AuthResourceCut cutInfo in authFile.CameraCuts)
            {
                cutInfos.Add(cutInfo.Start);
            }

            foreach (AuthResourceCut cutInfo in authFile.ResourceCuts)
            {
                resourceCutInfos.Add(cutInfo.Start);
            }

            cmnFile.CutInfo = cutInfos.ToArray();
            cmnFile.ResourceCutInfo = resourceCutInfos.ToArray();

            NodePathBase root = new NodePathBase();
            root.Category = AuthNodeCategory.Path;
            root.Name = "Root";
            root.Guid = Guid.NewGuid();

            cmnFile.Root = root;

            foreach (AuthNodeOOE ooeNode in authFile.Nodes)
            {
                Node node = Convert(ooeNode);
                root.Children.Add(node);
            }

            cmnFile.Root = root;

            OECMN.Write(cmnFile, Path.Combine(outputCmnDir, "cmn.bin"));
            RES.Write(new RES(), Path.Combine(outputCmnDir, "res.bin"), false);
            TEX.Write(new TEX(), Path.Combine(outputCmnDir, "tex.bin"), false);

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


                if (newRes != null)
                {
                    string resDirectory = Path.Combine(outputDir, cutName);

                    if (!Directory.Exists(resDirectory))
                        Directory.CreateDirectory(resDirectory);

                    RES.Write(newRes, Path.Combine(resDirectory, "res.bin"), false);

                    Thread convertAssetThread = new Thread(() => ConvertResThread(newRes, resDir, resDirectory, convertAssets));
                    convertAssetThread.Start();
                }

                current++;
            }
        }

        public static RES ConvertResource(AuthResOOE resource, OECMN cmn, AuthResourceCut cutInfo)
        {
            RES newRes = new RES();

            foreach (AuthResourceOOE ooeResource in resource.Resources)
            {
                Resource convertedResource = null;
                Guid guid = ooeResource.GUID;

                switch (ooeResource.Type)
                {
                    case AuthResourceOOEType.Character:
                        convertedResource = new Resource();
                        convertedResource.Type = ResourceType.Character;
                        convertedResource.Unk1 = 0;
                        convertedResource.Unk2 = 1;

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

                if (convertedResource != null)
                {
                    convertedResource.NodeGUID = guid;
                    convertedResource.StartFrame = cutInfo.Start;
                    convertedResource.EndFrame = cutInfo.End;
                    convertedResource.Name = ooeResource.Resource.Replace(".gmt", "").Replace(".cmt", "");

                    newRes.Resources.Add(convertedResource);
                }
            }

            return newRes;
        }

        private static void ConvertResThread(RES newRes, DirectoryInfo resDir, string outputDirectory, bool convertAssets)
        {
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

                
                if(resource.Type == ResourceType.CharacterMotion)
                {
                    origPath += ".gmt";

                    if (!File.Exists(origPath))
                        continue;

                    File.Copy(origPath, Path.Combine(outputDirectory, resource.Name + ".gmt"), true);
                }
            }
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

                foreach (EffectBase effect in node.Effects)
                {
                    var convertedEffect = OOEToDE.ConvertElementEffect(effect);

                    if (convertedEffect != null)
                        deNode.Children.Add(convertedEffect);
                }

                deNode.Guid = node.Guid;
                deNode.Name = node.Type.ToString();
            }
            return deNode;
        }

        public static NodePathBase GeneratePath()
        {
            NodePathBase root = new NodePathBase();
            root.Category = AuthNodeCategory.Path;
            root.Guid = Guid.NewGuid();
            root.Matrix = Matrix4x4.Default;
            root.Name = "Path";

            return root;
        }

        public static OENodeCharacter GenerateCharacter(AuthNodeOOE node)
        {
            OENodeCharacter chara = new OENodeCharacter();
            chara.Guid = Guid.NewGuid();
            chara.Name = node.Type.ToString();
            chara.Flag = 1;
            chara.Height = 185;

            OENodeCharacterMotion motion = new OENodeCharacterMotion();
            motion.Guid = node.AnimationData.Guid;
            motion.Start.Tick = new GameTick(node.AnimationData.StartFrame).Tick;
            motion.End.Tick = new GameTick(node.AnimationData.EndFrame).Tick;
            motion.Name = "Motion";
            motion.Priority = 1;

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
            motion.Priority = 1;

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
