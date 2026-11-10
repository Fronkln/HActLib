using HActLib;
using PIBLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CMNEdit
{
    internal static class MepToBepConverter
    {
        public static void Procedure()
        {
            PibVersion GetVersionForGame(Game game)
            {
                switch (game)
                {
                    default:
                        return PibVersion.Y0;
                    case Game.Y5:
                        return PibVersion.Y5;
                    case Game.Ishin:
                        return PibVersion.Ishin;
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


            Game selectedDEGame = Form1.curGame = (Game)Form1.Instance.targetGameCombo.SelectedIndex;

            if (!CMN.IsDEGame(selectedDEGame))
            {
                MessageBox.Show("Select a DE game first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MEPVersion mepVer;
            Game game;
            Dictionary<string, Node> m_createdBones = new Dictionary<string, Node>();

            Node ConvertMepNodeToBep(MepEffectOE mepNode)
            {
                Node node = null;
                node = OE2DECmn.Convert(mepNode.Effect, OECMN.GetCMNVersionForGame(game), selectedDEGame, game);

                return node;
            }

            OpenFileDialog dialog = new OpenFileDialog();
            // dialog.Filter = ".mep|.MEP file";
            dialog.CheckFileExists = true;

            DialogResult res = dialog.ShowDialog();

            if (res != DialogResult.OK)
                return;

            FileContext.FilePath = null;

            MEP mep = MEP.Read(dialog.FileName);
            mepVer = mep.Version;
            game = mepVer == MEPVersion.Y5 ? Game.Y5 : Game.Y0;

            Form1.Instance.ClearEverything();
            Form1.Instance.SetBEPMode();

            foreach (MepEffectOE mepOe in mep.Effects.Cast<MepEffectOE>())
            {
                Node result = ConvertMepNodeToBep(mepOe);
                TreeViewItemNode resultNode = null;

                if (result != null)
                {
                    result.BEPDat.Bone = mepOe.BoneName;
                    resultNode = new TreeViewItemNode(result);
                    Form1.Instance.nodesTree.Nodes.Add(resultNode);
                }
            }

            if (MessageBox.Show("Convert particle files?", "Conversion", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                FolderBrowserDialog pibDir = new FolderBrowserDialog();

                if (pibDir.ShowDialog() != DialogResult.OK)
                    return;

                string pibSearchDir = pibDir.SelectedPath;

                FolderBrowserDialog pibOutputDir = new FolderBrowserDialog();

                if (pibOutputDir.ShowDialog() != DialogResult.OK)
                    return;

                string pibOutputPath = pibOutputDir.SelectedPath;


                OEParticle[] mepNodes = mep.Effects.Cast<MepEffectOE>().Where(x => x.Effect.ElementKind == 2).Select(x => x.Effect).Cast<OEParticle>().ToArray();

                uint puidStartID = uint.Parse(Microsoft.VisualBasic.Interaction.InputBox("Enter the starting ID the converted pibs will have",
                "Adjust PUID ID",
                "",
                0,
                0));

                uint[] pibIds = mepNodes.Select(x => x.ParticleID).GroupBy(x => x).Select(y => y.First()).ToArray();
                string[] pibFiles = Directory.GetFiles(pibSearchDir, "*.pib", SearchOption.AllDirectories);

                Dictionary<uint, uint> newPibIDs = new Dictionary<uint, uint>();

                List<string> foundPibPaths = new List<string>();
                List<BasePib> mepPibs = new List<BasePib>();

                foreach (string str in pibFiles)
                {
                    if (pibIds.Length == pibFiles.Length)
                        break;

                    try
                    {
                        BasePib pib = PIB.Read(str);

                        if (pibIds.Contains(pib.ParticleID))
                        {
                            foundPibPaths.Add(str);
                            mepPibs.Add(pib);
                        }
                    }
                    catch
                    {

                    }
                }

                foreach (BasePib pib in mepPibs)
                {
                    BasePib converted = PIB.Convert(pib, GetVersionForGame(selectedDEGame));

                    if (converted == null)
                        continue;

                    uint oldID = converted.ParticleID;
                    uint newID = puidStartID;
                    puidStartID++;

                    converted.ParticleID = newID;
                    newPibIDs[oldID] = newID;

                    PIB.Write(converted, Path.Combine(pibOutputPath, converted.Name + ".pib"));
                }

                foreach (DEElementParticle ptc in Form1.Instance.GetAllNodesTreeView().Where(x => x.HActNode is DEElementParticle).Select(x => x.HActNode).Cast<DEElementParticle>())
                {
                    if (newPibIDs.ContainsKey(ptc.ParticleID))
                        ptc.ParticleID = newPibIDs[ptc.ParticleID];
                }
            }
        }
    }
}
