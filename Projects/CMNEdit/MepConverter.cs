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
    internal static class MepConverter
    {
        public static void Procedure()
        {
            if (!FileContext.IsMep)
                return;

            MessageBox.Show("(SLOW) Convert the current open mep file to the selected OE game including pibs. Input the paths carefully.");

            string input = Microsoft.VisualBasic.Interaction.InputBox("Directory where the mep's pibs are",
           "MEP Pib Path",
           "",
           0,
           0);

            if (string.IsNullOrEmpty(input))
                return;

            string output = Microsoft.VisualBasic.Interaction.InputBox("Directory to output to",
           "MEP Export Path",
           "",
           0,
           0);

            (((MEPFormatEditor)FileContext.ActiveFormatEditor)).ConvertCurrentMep();

            OEParticle[] mepNodes = Form1.Instance.nodesTree.Nodes.Cast<TreeViewItemMepNode>().Select(x => x.Node).Cast<MepEffectOE>().Where(x => x.Effect.ElementKind == 2).Select(x => x.Effect).Cast<OEParticle>().ToArray();

            uint[] pibIds = mepNodes.Select(x => x.ParticleID).ToArray();
            string[] pibFiles = Directory.GetFiles(input, "*.pib", SearchOption.AllDirectories);


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

            PibVersion targetVer = PibVersion.Y5;

            switch (Form1.curGame)
            {
                case Game.Ishin:
                    targetVer = PibVersion.Ishin;
                    break;
                case Game.Y5:
                    targetVer = PibVersion.Y5;
                    break;
                case Game.Y0:
                    targetVer = PibVersion.Y0;
                    break;
                case Game.YK1:
                    targetVer = PibVersion.Y0;
                    break;
            }

            for (int i = 0; i < mepPibs.Count; i++)
            {
                BasePib converted = PIB.Convert(mepPibs[i], targetVer);

                foreach (BasePibEmitter emitter in converted.Emitters)
                {
                    string dir = new FileInfo(foundPibPaths[i]).Directory.FullName;

                    foreach (string tex in emitter.Textures)
                    {
                        string texPath = Path.Combine(dir, tex);

                        if (File.Exists(texPath))
                            File.Copy(texPath, Path.Combine(output, tex), true);
                    }
                }

                PIB.Write(converted, Path.Combine(output, converted.Name + ".pib"));
            }


            MEP.Write(FileContext.Mep, Path.Combine(output, Path.GetFileName(FileContext.FilePath)));
        }
    }
}
