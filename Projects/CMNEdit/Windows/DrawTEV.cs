using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CMNEdit.Windows
{
    internal class DrawTEV
    {
        public static void Draw(TreeNodeOOEAuthNode node)
        {
            TreeNodeOOEAuthNode authNode = (TreeNodeOOEAuthNode)node;

            PropertyEditorBuilder.CreateHeader("Auth Node");

            PropertyEditorBuilder.CreateInput("GUID", authNode.Node.Guid.ToString(), delegate (string val) { authNode.Node.Guid = new Guid(val); });
            PropertyEditorBuilder.CreateInput("Type", authNode.Node.Type.ToString(), null, readOnly: true);

            PropertyEditorBuilder.CreateHeader("Animation Data");
            PropertyEditorBuilder.CreateInput("Type", authNode.Node.AnimationData.Type.ToString(), delegate (string val) { authNode.Node.AnimationData.Type = int.Parse(val); }, NumberBox.NumberMode.Int);
            PropertyEditorBuilder.CreateInput("Unknown", authNode.Node.AnimationData.Unknown2.ToString(), delegate (string val) { authNode.Node.AnimationData.Unknown2 = int.Parse(val); }, NumberBox.NumberMode.Int);
            PropertyEditorBuilder.CreateInput("GUID", authNode.Node.AnimationData.Guid.ToString(), delegate (string val) { authNode.Node.AnimationData.Guid = new Guid(val); });
            PropertyEditorBuilder.CreateInput("Start", authNode.Node.AnimationData.StartFrame.ToString(CultureInfo.InvariantCulture), delegate (string val) { authNode.Node.AnimationData.StartFrame = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("End", authNode.Node.AnimationData.EndFrame.ToString(CultureInfo.InvariantCulture), delegate (string val) { authNode.Node.AnimationData.EndFrame = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
        }
    }
}
