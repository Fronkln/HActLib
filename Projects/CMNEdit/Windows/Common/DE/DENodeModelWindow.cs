using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class DENodeModelWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            NodeModel mdl = node as NodeModel;

            PropertyEditorBuilder.CreateHeader("Bone");
            PropertyEditorBuilder.CreateInput("Bone Name", mdl.BoneName.Text, delegate (string val) { mdl.BoneName.Set(val); });
         
            if(Form1.curVer == GameVersion.Y0_K1)
            {
                PropertyEditorBuilder.CreateInput("Bone ID", mdl.BoneID.ToString(), delegate (string val) { mdl.BoneID = int.Parse(val); }, NumberBox.NumberMode.Int);
            }
        }
    }
}
