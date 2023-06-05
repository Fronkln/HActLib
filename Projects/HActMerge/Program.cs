using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using HActLib;


namespace HActMerge
{
    //(game) (hact1) (hact1 end) (hact2) (hact2 end)
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Game game = CMN.GetGameFromString(args[0]);
            GameVersion version = CMN.GetVersionForGame(game);

            Console.WriteLine(args[1]);
            Console.WriteLine(args[3]);

            CMN sourceHact = CMN.Read(args[1], game);
            float sourceHactEnd = float.Parse(args[2]);
            CMN destinationHact = CMN.Read(args[3], game);
            float destinationHactEnd = float.Parse(args[4]);
            bool dontFilter = false;


            if (args.Length > 5)
                dontFilter = args[5] == "nofilter";

            string basePath = AppDomain.CurrentDomain.BaseDirectory;


            if (Directory.Exists(Path.Combine(basePath, "output")))
                Directory.Delete(Path.Combine(basePath, "output"), true);

            Directory.CreateDirectory(Path.Combine(basePath, "output"));
            Directory.CreateDirectory(Path.Combine(basePath, "output/000"));
            Directory.CreateDirectory(Path.Combine(basePath, "output/cmn"));


            string sourceResDir = Path.Combine(new FileInfo(args[1]).Directory.FullName, @"..\", "000");
            string destResDir = Path.Combine(new FileInfo(args[3]).Directory.FullName, @"..\", "000");

            RES sourceHActRes = RES.Read(Path.Combine(sourceResDir, "res.bin"), CMN.IsDEGame(game));
            RES destinationHActRes = RES.Read(Path.Combine(destResDir, "res.bin"), CMN.IsDEGame(game));

            RES sourceCmnRes = RES.Read(Path.Combine(new FileInfo(args[1]).Directory.FullName, "res.bin"), CMN.IsDEGame(game));
            RES destCmnRes = RES.Read(Path.Combine(new FileInfo(args[3]).Directory.FullName, "res.bin"), CMN.IsDEGame(game));

            TEX sourceCmnTex = TEX.Read(Path.Combine(new FileInfo(args[1]).Directory.FullName, "tex.bin"), CMN.IsDEGame(game));
            TEX destCmnTex = TEX.Read(Path.Combine(new FileInfo(args[3]).Directory.FullName, "tex.bin"), CMN.IsDEGame(game));

            sourceCmnTex.Textures.AddRange(destCmnTex.Textures);

            MergeRES(sourceCmnRes, destCmnRes);


            if (!dontFilter)
            {
                FilterNode(sourceHact, 0, sourceHactEnd);
                FilterNode(destinationHact, sourceHactEnd, destinationHactEnd);
            }
            else
            {
                NodeCamera sourceCamera = (NodeCamera)sourceHact.AllNodes.FirstOrDefault(x => x.Category == AuthNodeCategory.Camera);
                NodeCamera destCamera = (NodeCamera)destinationHact.AllNodes.FirstOrDefault(x => x.Category == AuthNodeCategory.Camera);

                if(sourceCamera != null && destCamera != null)
                {
                    sourceCamera.FrameProgression = destCamera.FrameProgression;
                    sourceCamera.FrameProgressionSpeed = destCamera.FrameProgressionSpeed;
                }
            }

            CopyRESFiles(sourceResDir, sourceHActRes);
            CopyRESFiles(destResDir, destinationHActRes);

            MergeHAct(sourceHact, destinationHact, dontFilter);
            MergeRES(sourceHActRes, destinationHActRes);

            if (destinationHact.AuthPages != null)
            {
                if(!dontFilter)
                    AdjustPages(destinationHact, sourceHactEnd);

                sourceHact.AuthPages.AddRange(destinationHact.AuthPages);
            }

            foreach (Resource res in sourceHActRes.Resources)
                res.EndFrame = 99999;

            sourceHact.HActEnd = sourceHactEnd + destinationHactEnd;


            CMN.Write(sourceHact, Path.Combine(basePath, "output/cmn/cmn.bin"));
            RES.Write(sourceHActRes, Path.Combine(basePath, "output/000/res.bin"), CMN.IsDEGame(game));
            RES.Write(sourceCmnRes, Path.Combine(basePath, "output/cmn/res.bin"), CMN.IsDEGame(game));
            TEX.Write(sourceCmnTex, Path.Combine(basePath, "output/cmn/tex.bin"), CMN.IsDEGame(game));

            if (game >= Game.JE)
                CMN.Write(sourceHact, Path.Combine(basePath, "output/cmn/cmn_vo.bin"));

            if (game >= Game.LJ)
                RES.Write(sourceHActRes, Path.Combine(basePath, "output/000/res_vo.bin"), CMN.IsDEGame(game));


            Console.WriteLine("Output: " + new FileInfo(Path.Combine(basePath, "output/cmn/cmn.bin")).FullName);

            Thread.Sleep(5000);
        }

        private static void FilterNode(CMN hact, float start, float startLimit)
        {
            foreach (NodeElement element in hact.AllElements)
            {
                element.Start += start;
                element.End += start;

                if (element.Start > start + startLimit)
                {
                    if (element.Parent != null)
                        element.Parent.Children.Remove(element);
                }

                if (element.End > start + startLimit)
                    element.End = start + startLimit;
            }

            foreach (NodeCamera camera in hact.AllCameras)
            {
                if (camera.FrameProgression.Length == 0)
                {
                    camera.FrameProgression = new float[(int)hact.HActEnd + 1];
                    camera.FrameProgressionSpeed = new float[camera.FrameProgression.Length];

                    for (int i = 0; i < camera.FrameProgression.Length; i++)
                    {
                        camera.FrameProgression[i] = i + 1;
                        camera.FrameProgressionSpeed[i] = 1f;
                    }
                }

                for (int i = 0; i < camera.FrameProgression.Length; i++)
                    camera.FrameProgression[i] += start;


                camera.FrameProgression = camera.FrameProgression.Where(x => x <= start + startLimit).ToArray();
                Array.Resize(ref camera.FrameProgressionSpeed, camera.FrameProgression.Length);
            }


            foreach (NodeMotionBase motion in hact.AllNodes.Where(x => x is NodeMotionBase))
            {
                motion.Start.Frame += start;
                motion.End.Frame += start;

                if (motion.End.Frame > start + startLimit)
                    motion.End.Frame = start + startLimit;
            }

            for (int i = 0; i < hact.CutInfo.Length; i++)
                hact.CutInfo[i] += start;

            for (int i = 0; i < hact.DisableFrameInfo.Length; i++)
            {
                hact.DisableFrameInfo[i].Start += start;
                hact.DisableFrameInfo[i].End += start;
            }

            hact.DisableFrameInfo = new DisableFrameInfo[0]; //hact.DisableFrameInfo.Where(x => x.Start < start + startLimit).ToArray();
        }

        private static void MergeHAct(CMN hact1, CMN hact2, bool nofilter)
        {
            //We only process this for one camera, i dont care
            NodeCamera sourceCamera = (NodeCamera)hact1.AllNodes.FirstOrDefault(x => x.Category == AuthNodeCategory.Camera);
            NodeCamera destCamera = (NodeCamera)hact2.AllNodes.FirstOrDefault(x => x.Category == AuthNodeCategory.Camera);

            NodeCameraMotion[] destCameraMotion = destCamera.GetChildsOfType<NodeCameraMotion>();

            foreach (NodeCameraMotion motion in destCameraMotion)
            {
                motion.Parent.Children.Remove(motion);
                sourceCamera.Children.Add(motion);
            }

            Node[] destRootNodes = hact2.AllNodes.Where(x => x.Parent == hact2.Root).ToArray();

            foreach (Node node in destRootNodes)
            {
                if (node.Category != AuthNodeCategory.Camera)
                    hact1.Root.Children.Add(node);
                else
                {
                    foreach (Node child in node.Children)
                        sourceCamera.Children.Add(child);
                }
            }

            if (!nofilter)
            {
                List<float> frameProgression = new List<float>(sourceCamera.FrameProgression);
                List<float> frameProgressionSpeed = new List<float>(sourceCamera.FrameProgressionSpeed);

                frameProgression.AddRange(destCamera.FrameProgression);
                frameProgressionSpeed.AddRange(destCamera.FrameProgressionSpeed);

                sourceCamera.FrameProgression = frameProgression.ToArray();
                sourceCamera.FrameProgressionSpeed = frameProgressionSpeed.ToArray();
            }
        }

        private static void AdjustPages(CMN hact, float adjustment)
        {
            foreach (AuthPage page in hact.AuthPages)
            {
                page.Start.Frame += adjustment;
                page.End.Frame += adjustment;
                page.SkipTick.Frame += adjustment;

                if (page.TalkInfo != null)
                {
                    foreach (TalkInfo inf in page.TalkInfo)
                    {
                        GameTick startTick = new GameTick(inf.StartTick);
                        GameTick endTick = new GameTick(inf.EndTick);

                        startTick.Frame += adjustment;
                        endTick.Frame += adjustment;

                        inf.StartTick = startTick.Tick;
                        inf.EndTick = endTick.Tick;
                    }
                }
            }
        }

        private static void MergeRES(RES res1, RES res2)
        {
            res1.Resources.AddRange(res2.Resources);
        }

        private static void CopyRESFiles(string dir, RES res)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            foreach (Resource resFile in res.Resources)
            {
                string extension = null;

                if (resFile.Type == ResourceType.CameraMotion)
                    extension = ".cmt";
                else if (resFile.Type == ResourceType.CharacterMotion || resFile.Type == ResourceType.AssetMotion)
                    extension = ".gmt";

                if (extension != null)
                {
                    string file = Path.Combine(dir, resFile.Name + extension);

                    if (File.Exists(file))
                    {
                        string fileName = Path.GetFileName(file);
                        File.Copy(file, Path.Combine(basePath, "output/000/", fileName), true);
                    }
                }
            }
        }
    }
}
