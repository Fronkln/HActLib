using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace CMNEdit
{
    internal static class DrawPS2Prop
    {
        public static void Draw(TreeNode node)
        {
            Be.Windows.Forms.DynamicByteProvider provider = null;

            if (node is TreeNodePS2PropertyData1)
            {
                var dat1Node = node as TreeNodePS2PropertyData1;
                PropertyEditorBuilder.CreateHeader("Property (Data1)");
                PropertyEditorBuilder.CreateInput("Type", dat1Node.Data1.Type.ToString(), delegate (string val) { }, NumberBox.NumberMode.Int, true);
                PropertyEditorBuilder.CreateInput("Start", dat1Node.Data1.Start.ToString(CultureInfo.InvariantCulture), delegate (string val) { dat1Node.Data1.Start = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                PropertyEditorBuilder.CreateInput("End", dat1Node.Data1.End.ToString(CultureInfo.InvariantCulture), delegate (string val) { dat1Node.Data1.End = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                if (dat1Node.Data1.UnknownData != null)
                {
                    provider = new Be.Windows.Forms.DynamicByteProvider(dat1Node.Data1.UnknownData);
                    provider.Changed += delegate
                    {
                        if (provider.Bytes.Count == dat1Node.Data1.UnknownData.Length)
                            dat1Node.Data1.UnknownData = provider.Bytes.ToArray();
                    };
                }
            }
            else if (node is TreeNodePS2PropertyData2)
            {
                var dat2Node = node as TreeNodePS2PropertyData2;
                PropertyEditorBuilder.CreateHeader("Property (Data2)");
                PropertyEditorBuilder.CreateInput("Type?", dat2Node.Data2.Type.ToString(), delegate (string val) { }, NumberBox.NumberMode.Int, true);
                PropertyEditorBuilder.CreateInput("Start", dat2Node.Data2.Start.ToString(CultureInfo.InvariantCulture), delegate (string val) { dat2Node.Data2.Start = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                PropertyEditorBuilder.CreateInput("End", dat2Node.Data2.End.ToString(CultureInfo.InvariantCulture), delegate (string val) { dat2Node.Data2.End = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                if (dat2Node.Data2.UnknownData != null)
                {
                    provider = new Be.Windows.Forms.DynamicByteProvider(dat2Node.Data2.UnknownData);
                    provider.Changed += delegate
                    {
                        if (provider.Bytes.Count == dat2Node.Data2.UnknownData.Length)
                            dat2Node.Data2.UnknownData = provider.Bytes.ToArray();
                    };
                }
            }

            PropertyEditorBuilder.CreateHeader("");
            Form1.Instance.unkBytesBox.ByteProvider = provider;
            Form1.Instance.varPanel.ResumeLayout();
        }
    }
}
