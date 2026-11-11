using HActLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CMNEdit
{
    internal class MEPFormatEditor : FormatEditorBase
    {
        public virtual bool HasHierarchy()
        {
            return false;
        }

        public override void Save(string filePath)
        {
            base.Save(filePath);

            Form1.curGame = (Game)Form1.Instance.targetGameCombo.SelectedIndex;
            ConvertCurrentMep();
            MEP.Write(FileContext.Mep, filePath);
        }

        public void ConvertCurrentMep()
        {
            var curGame = (Game)Form1.Instance.targetGameCombo.SelectedIndex;
            MepEffect[] mepNodes = Form1.Instance.nodesTree.Nodes.Cast<TreeViewItemMepNode>().Select(x => x.Node).Cast<MepEffect>().ToArray();

            if (FileContext.Mep.Version >= MEPVersion.Y5)
            {

                if (FileContext.Mep.Version == MEPVersion.Y5)
                {
                    if (curGame >= Game.Y0)
                    {
                        foreach (MepEffectOE oe in mepNodes)
                        {
                            oe.OE_ConvertToY0();
                            oe.Effect.OE_ConvertToY0();
                        }

                    }
                }
                else
                {
                    if (curGame == Game.Y5)
                        foreach (MepEffectOE oe in mepNodes)
                        {
                            oe.OE_ConvertToY5();
                            oe.Effect.OE_ConvertToY5();
                        }
                }
            }

            FileContext.Mep.Effects = Form1.Instance.nodesTree.Nodes.Cast<TreeViewItemMepNode>().Select(x => x.Node).ToList();

            if (curGame >= Game.Y0)
                FileContext.Mep.Version = MEPVersion.Y0;
            else if (curGame == Game.Y5)
                FileContext.Mep.Version = MEPVersion.Y5;
            else if (curGame <= Game.Y4)
                FileContext.Mep.Version = MEPVersion.Y3;
            else
                FileContext.Mep.Version = MEPVersion.Y0;
        }
    }
}
