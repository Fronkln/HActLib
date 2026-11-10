using HActLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CMNEdit
{
    internal static class AuthAssembler
    {
        public static void SegmentProcedure()
        {
            if (Form1.Instance.ResourceCutInfos == null)
                return;

            if (Form1.Instance.ResourceCutInfos.Length <= 1)
            {
                MessageBox.Show("Your HAct does not have more than one resource cut");
                return;
            }

            FolderBrowserDialog browser = new FolderBrowserDialog();

            if (browser.ShowDialog() != DialogResult.OK)
                return;

            string outputPath = browser.SelectedPath;

            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);



            float currentStart = 0;
            float lastCut = -1;

            for (int i = 0; i < Form1.Instance.ResourceCutInfos.Length; i++)
            {
                string resPrefix = i.ToString("D3");
                string folderDir = Path.Combine(outputPath, resPrefix + "_segment");

                if (!Directory.Exists(folderDir))
                    Directory.CreateDirectory(folderDir);

                float end = Form1.Instance.ResourceCutInfos[i];
                float length = end - currentStart;

                //create segmented CMN
                BaseCMN segmented = null;

                if (FileContext.IsOE)
                    segmented = new OECMN();
                else
                    segmented = new CMN();

                string resFile = null;


                if (!string.IsNullOrEmpty(FileContext.FilePath))
                {
                    string hactDir = new FileInfo(FileContext.FilePath).Directory.Parent.FullName;
                    string resDir = Path.Combine(hactDir, resPrefix);

                    if (Directory.Exists(resDir))
                    {
                        resFile = Path.Combine(resDir, "res.bin");

                        if (File.Exists(resFile))
                        {
                            RES res = RES.Read(resFile, false);

#warning TODO: Adjust res
                            foreach (var resource in res.Resources)
                            {
                                //These tend to have the full length of the hact. Wont touch
                                if (resource.Type != ResourceType.CustomModel && resource.Type != ResourceType.Character)
                                {
                                    resource.StartFrame -= currentStart;
                                    resource.EndFrame -= currentStart;
                                }

                            }

                            RES.Write(res, Path.Combine(folderDir, "res.bin"), CMN.IsDEGame(Form1.curGame));
                            // File.Copy(resFile, Path.Combine(folderDir, "res.bin"));
                        }

                        string[] files = Directory.GetFiles(resDir);

                        foreach (string str in files)
                        {
                            if (!str.EndsWith(".bin"))
                                File.Copy(str, Path.Combine(folderDir, Path.GetFileName(str)), true);
                        }
                    }

                }

                //create segmented CMN
                segmented.Version = Form1.Instance.Version;
                segmented.SetFlags(uint.Parse(Form1.Instance.flagsBox.Text));
                segmented.HActEnd = length;
                segmented.SetChainCameraIn(Utils.InvariantParse(Form1.Instance.cameraInBox.Text));
                segmented.SetChainCameraOut(Utils.InvariantParse(Form1.Instance.cameraOutBox.Text));
                segmented.ResourceCutInfo = new float[] { length };
                segmented.SetNodeDrawNum(Form1.Instance.NodeDrawNum);


                //filter cut info
                List<float> cutInfoSegment = new List<float>();

                foreach (float cutInfo in Form1.Instance.CutInfos)
                {
                    if (cutInfo >= currentStart && cutInfo <= end)
                        cutInfoSegment.Add(Utils.ConvertRange(cutInfo, currentStart, end, 0, length));
                }

                segmented.CutInfo = cutInfoSegment.ToArray();

                if (i > 0)
                    lastCut = currentStart;

                //filter nodes
                TreeViewItemNode result = (TreeViewItemNode)Sorting.FilterNodesByRange(Form1.Instance.nodesTree, lastCut, currentStart, end);
                segmented.Root = result.HActNode;

                if (i != Form1.Instance.ResourceCutInfos.Length - 1)
                    currentStart = end;

                OECMN.Write(segmented as OECMN, Path.Combine(folderDir, "cmn.bin"));
            }
        }
        public static void ReassembleProcedure()
        {
            FolderBrowserDialog browser = new FolderBrowserDialog();

            if (browser.ShowDialog() != DialogResult.OK)
                return;

            string inputPath = browser.SelectedPath;
            string outputPath = Path.Combine(browser.SelectedPath, "..", new DirectoryInfo(browser.SelectedPath).Name + "_reassembled");
            string outputCmnDir = Path.Combine(outputPath, "cmn");
            string outputCmnPath = Path.Combine(outputCmnDir, "cmn.bin");

            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            if (!Directory.Exists(outputCmnDir))
                Directory.CreateDirectory(outputCmnDir);

            var game = (Game)Form1.Instance.targetGameCombo.SelectedIndex;

            BaseCMN cmn = null;
            int curSegment = 0;

            float oldEnd = 0;

            List<float> resourceCuts = new List<float>();
            List<float> cutInfos = new List<float>();


            List<NodeElement> multiPartNodes = new List<NodeElement>();


            while (true)
            {
                string segmentPath = Path.Combine(inputPath, curSegment.ToString("D3") + "_segment");

                if (!Directory.Exists(segmentPath))
                    break;

                string cmnFile = Path.Combine(segmentPath, "cmn.bin");
                string resFile = Path.Combine(segmentPath, "res.bin");

                if (!File.Exists(cmnFile))
                    break;

                BaseCMN segment = BaseCMN.ReadGeneric(cmnFile, game);
                RES segmentedRes = null;

                if (File.Exists(resFile))
                    segmentedRes = RES.Read(resFile, CMN.IsDEGame(game)); ;

                //we will add on this segment to get the full thing
                if (cmn == null)
                {
                    resourceCuts = new List<float>(segment.ResourceCutInfo);
                    cutInfos = new List<float>(segment.CutInfo);
                    cmn = segment;
                }
                else
                {
                    foreach (float cutinfo in segment.CutInfo)
                    {
                        cutInfos.Add(cutinfo + cmn.HActEnd);
                    }

                    resourceCuts.Add(segment.ResourceCutInfo[0] + cmn.HActEnd);

                    oldEnd = cmn.HActEnd;
                    cmn.HActEnd += segment.HActEnd;

                    foreach (NodeElement element in segment.AllElements)
                    {
                        NodeElement originalNode = (NodeElement)cmn.FindNodeByGUID(element.Guid);

                        if (originalNode != null)
                        {
                            originalNode.End += element.Start;
                            originalNode.End += element.End;
                        }
                        else
                        {
                            Node originalParent = cmn.FindNodeByGUID(element.Parent.Guid);

                            //huhhh
                            if (originalParent != null)
                            {
                                originalParent.Children.Add(element);

                                element.Start += oldEnd;
                                element.End += oldEnd;
                            }
                        }
                    }

                    //jank but i dont fucking care
                    foreach (Node node in segment.GetNodes())
                    {
                        if (!(node is NodeMotionBase))
                            continue;

                        NodeMotionBase segmentedNode = (NodeMotionBase)node;
                        NodeMotionBase originalNode = (NodeMotionBase)cmn.FindNodeByGUID(node.Guid);

                        if (originalNode != null)
                        {
                            originalNode.End.Frame += segmentedNode.End.Frame;
                        }
                        else
                        {
                            Node originalParent = cmn.FindNodeByGUID(node.Parent.Guid);

                            //huhhh
                            if (originalParent != null)
                            {
                                originalParent.Children.Add(node);

                                segmentedNode.Start.Frame += oldEnd;
                                segmentedNode.End.Frame += oldEnd;
                            }
                        }
                    }

                }


                if (segmentedRes != null)
                {
                    foreach (Resource res in segmentedRes.Resources)
                    {
                        if (res.Type == ResourceType.CharacterMotion
                            || res.Type == ResourceType.PathMotion
                            || res.Type == ResourceType.CameraMotion || res.Type == ResourceType.AssetMotion)
                        {
                            res.StartFrame = oldEnd;
                            res.EndFrame = cmn.HActEnd;
                        }
                    }

                    string resDir = Path.Combine(new DirectoryInfo(outputCmnDir).Parent.FullName, curSegment.ToString("D3"));

                    if (!Directory.Exists(resDir))
                        Directory.CreateDirectory(resDir);

                    RES.Write(segmentedRes, Path.Combine(resDir, "res.bin"), CMN.IsDEGame(game));

                    string[] files = Directory.GetFiles(segmentPath);

                    foreach (string str in files)
                    {
                        if (!str.EndsWith(".bin"))
                            File.Copy(str, Path.Combine(resDir, Path.GetFileName(str)), true);
                    }
                }

                curSegment++;
            }

            //go back to 000 and correct any character/model lengths who typically are present for full hact duration..
            string res0Path = Path.Combine(outputPath, "000", "res.bin");

            if (File.Exists(res0Path))
            {
                RES res = RES.Read(res0Path, false);

                foreach (var resource in res.Resources)
                {
                    if (resource.Type == ResourceType.Character || resource.Type == ResourceType.CustomModel || resource.Type == ResourceType.Model)
                    {
                        resource.EndFrame = cmn.HActEnd;
                    }
                }

                RES.Write(res, res0Path, false);
            }

            cmn.CutInfo = cutInfos.Distinct().ToArray();
            cmn.ResourceCutInfo = resourceCuts.ToArray();

            BaseCMN.WriteGeneric(outputCmnPath, cmn);
        }
    }
}
