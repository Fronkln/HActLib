using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMNEdit;
using CMNEdit.Windows.Common.DE;
using HActLib;


namespace CMNEdit
{
    internal static class DENodeElementSEWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementSE inf = node as DEElementSE;

            form.CreateSpace(25);
            form.CreateHeader("Sound Effect");

            if (CMN.VersionGreater(Form1.curVer, GameVersion.Yakuza6Demo))
                form.CreateComboBox("Version", inf.SEVer, Enum.GetNames(typeof(DEElementSE.Versions)), delegate (int val) { inf.SEVer = val; });

            form.CreateInput("Cuesheet ID", inf.CueSheet.ToString(), delegate (string val) { inf.CueSheet = ushort.Parse(val); Form1.EditingNode.Update(); }, NumberBox.NumberMode.Ushort);
            form.CreateInput("Sound ID", inf.SoundIndex.ToString(), delegate (string val) { inf.SoundIndex = byte.Parse(val); Form1.EditingNode.Update(); }, NumberBox.NumberMode.Byte);
            form.CreateInput("Flag?", inf.Flags.ToString(), delegate (string val) { inf.Flags = byte.Parse(val); }, NumberBox.NumberMode.Byte);

            form.CreateInput("Near Decay Distance", inf.CustomDecayNearDist.ToString(), delegate (string val) { inf.CustomDecayNearDist = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Near Decay Volume", inf.CustomDecayNearVol.ToString(), delegate (string val) { inf.CustomDecayNearVol = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Far Decay Distance", inf.CustomDecayFarDist.ToString(), delegate (string val) { inf.CustomDecayFarDist = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Far Decay Volume", inf.CustomDecayFarVol.ToString(), delegate (string val) { inf.CustomDecayFarVol = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Volume", inf.Volume.ToString(), delegate (string val) { inf.Volume = int.Parse(val); }, NumberBox.NumberMode.Int);
        }
    }
}
