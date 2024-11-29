using HActLib;
using ParLibrary;
using ParLibrary.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Yarhl.FileSystem;
using System.IO;
using Node = HActLib.Node;
using System.DirectoryServices.ActiveDirectory;
using PIBLib;

namespace CMNEdit.Windows
{
    public partial class RyuseWindow : Form
    {
        public RyuseWindow()
        {
            InitializeComponent();
        }

        private void particleOutputBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void RyuseWindow_Load(object sender, EventArgs e)
        {
            Form1.Instance.Enabled = false;

            foreach (Game game in CMN.GetDEGames())
                targetGameBox.Items.Add(HActLib.Internal.Reflection.GetGamePrefixes(game)[0]);

            targetGameBox.SelectedIndex = 0;
        }

        private void RyuseWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1.Instance.Enabled = true;
        }

        private PibVersion GetPibVersionForGame(Game game)
        {
            switch (game)
            {
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
                case Game.LAD7Gaiden:
                    return PibVersion.LJ;
                case Game.LADIW:
                    return PibVersion.LJ;
            }

            return PibVersion.LJ;
        }

        private void convertButton_Click(object sender, EventArgs e)
        {
            Node[] nodes = null;

            Game oldGame = Form1.curGame;
            Game prefixGame = CMN.GetGameFromString(targetGameBox.Items[targetGameBox.SelectedIndex].ToString());
            GameVersion targetVer = CMN.GetVersionForGame(prefixGame);

            bool is58Game = prefixGame == Game.LJ || prefixGame == Game.LADIW || prefixGame == Game.LAD7Gaiden;

            if (Form1.IsBep || Form1.IsMep)
                nodes = Form1.Instance.GetAllNodes();
            else
            {
                BaseCMN genHact = null;

                if (Form1.IsOE)
                    genHact = Form1.Instance.GenerateOEHAct();
                else
                    genHact = Form1.Instance.GenerateHAct();

                Form1.Instance.GenerateBaseInfo(genHact);
                nodes = genHact.GetNodes();


                for (int i = 0; i < Form1.Instance.AuthPagesDE.Length; i++)
                {
                    AuthPage page = Form1.Instance.AuthPagesDE[i];

                    //Convert between old and new formats
                    if ((page.Format == 0 && targetVer > GameVersion.Yakuza6) || (page.Format > 0 && targetVer <= GameVersion.Yakuza6))
                    {
                        page.PageIndex = i;

                        foreach (Transition transition in page.Transitions)
                            foreach (Condition condition in transition.Conditions)
                            {
                                string name = ConditionConvert.GetName(condition.ConditionID, Form1.curGame);
                                uint newID = ConditionConvert.GetID(name, prefixGame);
                                condition.ConditionID = newID;
                            }
                    }

                    if (targetVer <= GameVersion.Yakuza6)
                        page.Format = 0;
                    else if (targetVer == GameVersion.DE1)
                        page.Format = 1;
                    else
                        page.Format = 2;

                    if (page.IsTalkPage())
                    {
                        if (Form1.curVer <= GameVersion.DE1 && targetVer >= GameVersion.DE2)
                        {
                            if ((page.Flag & 0x40) == 0)
                                page.Flag |= 0x40;
                        }
                    }
                    else
                    {
                        if (Form1.curVer <= GameVersion.DE1)
                            page.Flag = 0;
                    }
                }

            }

            Node[] outputNodes;

            if (!Form1.IsOE)
            {
                RyuseModule.ConversionInformation inf = RyuseModule.ConvertNodes(nodes, Form1.curGame, prefixGame);
                outputNodes = inf.OutputNodes;
            }
            else
            {
                RyuseOEModule.ConversionInformation inf = RyuseOEModule.ConvertNodes(nodes, Form1.curGame, prefixGame);
                outputNodes = inf.OutputNodes;
            }

            Form1.curGame = prefixGame;
            Form1.curVer = CMN.GetVersionForGame(Form1.curGame);
            CMN.LastHActDEGame = Form1.curGame;

            Form1.Instance.targetGameCombo.SelectedIndex = (int)Form1.curGame;

            Form1.Instance.ClearNodes();

            if (!Form1.IsBep)
            {
                Form1.Instance.nodesTree.Nodes.Add(new TreeViewItemNode(outputNodes[0]));
            }
            else
            {
                foreach (Node node in outputNodes)
                    Form1.Instance.nodesTree.Nodes.Add(new TreeViewItemNode(node));

                Form1.Instance.ProcessBEPHierarchy();
            }

            if (convertParticleCheck.Checked)
            {
                if (!File.Exists(particleParBox.Text))
                {
                    MessageBox.Show("Please input the particle.par path");
                    return;
                }

                if (!Directory.Exists(particleOutputBox.Text))
                {
                    MessageBox.Show("Please input output directory");
                    return;
                }


                using (var Par = NodeFactory.FromFile(particleParBox.Text, "par"))
                {
                    Par.TransformWith<ParArchiveReader, ParArchiveReaderParameters>(new ParArchiveReaderParameters() { Recursive = true });

                    string[] ptcs = null;

                    if (oldGame >= Game.YK2)
                        ptcs = Form1.Instance.GetAllNodes().Where(x => x is DEElementParticle).Cast<DEElementParticle>().Select(x => (x.ParticleName + ".pib").ToLowerInvariant()).Distinct().ToArray();
                    else
                        ptcs = Form1.Instance.GetAllNodes().Where(x => x is DEElementParticle).Cast<DEElementParticle>().Select(x => (x.Name.Substring(0, 7) + ".pib").ToLowerInvariant()).Distinct().ToArray();

                    foreach (string str in ptcs)
                    {
                        foreach (var node in Navigator.IterateNodes(Par))
                            if (ptcs.Contains(node.Name.ToLowerInvariant()))
                            {
                                var ptcFile = node.GetFormatAs<ParFile>();

                                if (ptcFile.IsCompressed)
                                    node.TransformWith<ParLibrary.Sllz.Decompressor>();
                                if (node.Stream.Length > 0)
                                {
                                    byte[] ptcBuf = node.Stream.ToArray();
                                    try
                                    {
                                        PibVersion target = GetPibVersionForGame(prefixGame);
                                        BasePib pibFile = PIB.Read(ptcBuf, node.Name);

                                        if (pibFile.Version == PibVersion.LJ)
                                        {
                                            foreach (var emitter in pibFile.Emitters)
                                            {
                                                if (prefixGame == Game.LAD7Gaiden)
                                                    (emitter as PibEmitterv58).ToGaidenRevision();
                                                else
                                                    (emitter as PibEmitterv58).ToLJRevision();
                                            }
                                        }

                                        BasePib newPib = PIB.Convert(pibFile, target);

                                        if ((pibFile.Version == newPib.Version) && (pibFile.Version != PibVersion.LJ && !is58Game))
                                        {
                                            MessageBox.Show($"Don't know how to convert pib version {pibFile.Version} to {target}");
                                            return;
                                        }

                                        foreach (var emitter in newPib.Emitters)
                                        {
                                            foreach (string tex in emitter.Textures)
                                            {
                                                string name = FixTexName(tex).ToLowerInvariant();

                                                var texNode = TryFetchTexture(node.Parent.Parent, name);

                                                if (texNode != null)
                                                {
                                                    var texFile = texNode.GetFormatAs<ParFile>();

                                                    if (texFile.IsCompressed)
                                                        texNode.TransformWith<ParLibrary.Sllz.Decompressor>();

                                                    File.WriteAllBytes(Path.Combine(particleOutputBox.Text, name), texNode.Stream.ToArray());
                                                }
                                            }
                                        }

                                        PIB.Write(newPib, Path.Combine(particleOutputBox.Text, node.Name));
                                    }
                                    catch (Exception ex)
                                    {
                                        System.Diagnostics.Debug.WriteLine("PIB: " + node.Name + " ERR:" + ex + "\n");
                                        MessageBox.Show("PIB: " + node.Name + " ERR:" + ex + "\n", "Error");
                                    }
                                }
                            }
                    }
                }

            }

            Close();
        }

        public static string FixTexName(string textureName)
        {
            if (!textureName.EndsWith(".dds"))
                textureName += ".dds";

            return textureName;
        }

        public static Yarhl.FileSystem.Node TryFetchTexture(Yarhl.FileSystem.Node node, string textureName)
        {
            foreach (var node2 in Navigator.IterateNodes(node))
            {
                if (node2.Name.ToLowerInvariant() == textureName)
                    return node2;
            }

            return null;
        }

        private void convertParticleCheck_CheckedChanged(object sender, EventArgs e)
        {
            particleParBox.ReadOnly = !convertParticleCheck.Checked;
            particleOutputBox.ReadOnly = !convertParticleCheck.Checked;
            particleParBrowseDir.Enabled = convertParticleCheck.Checked;
            outputBrowseDir.Enabled = convertParticleCheck.Checked;
        }

        private void particleParBrowseDir_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            particleParBox.Text = dialog.FileName;
        }

        private void outputBrowseDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            particleOutputBox.Text = dialog.SelectedPath;
        }
    }
}
