using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class OENodeElementSEWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            OEElementSE inf = node as OEElementSE;

            PropertyEditorBuilder.CreateSpace(25);
            PropertyEditorBuilder.CreateHeader("Sound Effect");

            PropertyEditorBuilder.CreateInput("Cuesheet ID", inf.Cuesheet.ToString(), delegate (string val) { inf.Cuesheet = ushort.Parse(val); if (Form1.EditingNode != null) Form1.EditingNode.Update(); }, NumberBox.NumberMode.Ushort);
            PropertyEditorBuilder.CreateInput("Sound ID", inf.Sound.ToString(), delegate (string val) { inf.Sound = ushort.Parse(val); if(Form1.EditingNode != null) Form1.EditingNode.Update(); }, NumberBox.NumberMode.Ushort);
            PropertyEditorBuilder.CreateInput("Flag?", inf.Flags.ToString(), delegate (string val) { inf.Flags = uint.Parse(val); }, NumberBox.NumberMode.UInt);


            PropertyEditorBuilder.CreateInput("Volume?", inf.Volume.ToString(), delegate (string val) { inf.Volume = int.Parse(val); }, NumberBox.NumberMode.Int);
           
            PropertyEditorBuilder.CreateInput("Decay Near Distance", inf.CustomDecayNearDist.ToString(), delegate (string val) { inf.CustomDecayNearDist = float.Parse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Decay Near Volume", inf.CustomDecayNearVol.ToString(), delegate (string val) { inf.CustomDecayNearVol = float.Parse(val); }, NumberBox.NumberMode.Float);

            PropertyEditorBuilder.CreateInput("Decay Far Distance", inf.CustomDecayFarDist.ToString(), delegate (string val) { inf.CustomDecayFarDist = float.Parse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Decay Far Volume", inf.CustomDecayFarVol.ToString(), delegate (string val) { inf.CustomDecayFarVol = float.Parse(val); }, NumberBox.NumberMode.Float);
        }
    }
}
