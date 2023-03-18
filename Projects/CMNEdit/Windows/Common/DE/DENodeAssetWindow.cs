using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMNEdit;
using CMNEdit.Windows.Common.DE;
using HActLib;


namespace CMNEdit.Windows.Common.DE
{
    internal static class DENodeAssetWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            NodeAsset inf = node as NodeAsset;

            form.CreateSpace(25);
            form.CreateHeader("Asset Info");

            form.CreateInput("Asset ID", inf.AssetID.ToString(), delegate (string val) { inf.AssetID = (uint)int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateComboBox("Replace ID", (int)inf.ReplaceID, Enum.GetNames<AssetReplaceID>(), delegate(int idx) { inf.ReplaceID = (uint)idx; });

            form.CreateInput("Offset Type", inf.OffsetType.ToString(), delegate (string val) { inf.OffsetType = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateInput("Allow Asset Position Use", inf.EnableAssetPositionUse.ToString(), delegate (string val) { inf.EnableAssetPositionUse = int.Parse(val); }, NumberBox.NumberMode.Byte);
            form.CreateInput("Init Calc Matrix", inf.InitCalcMatrix.ToString(), delegate (string val) { inf.InitCalcMatrix = int.Parse(val); }, NumberBox.NumberMode.Byte);

            form.CreateInput("Is Keep Pose", inf.IsKeepPose.ToString(), delegate (string val) { inf.IsKeepPose = int.Parse(val); }, NumberBox.NumberMode.Byte);

            form.CreateInput("Is Use Trans", inf.IsUseTrans.ToString(), delegate (string val) { inf.IsUseTrans = int.Parse(val); }, NumberBox.NumberMode.Byte);
            form.CreateInput("Is Unuse Trans", inf.IsUnUseTrans.ToString(), delegate (string val) { inf.IsUnUseTrans = int.Parse(val); }, NumberBox.NumberMode.Byte);

            form.CreateInput("Position X", inf.ReplacePosX.ToString(), delegate(string val) { inf.ReplacePosX = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Position Y", inf.ReplacePosY.ToString(), delegate (string val) { inf.ReplacePosY = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Position Z", inf.ReplacePosZ.ToString(), delegate (string val) { inf.ReplacePosZ = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
        }
    }
}
