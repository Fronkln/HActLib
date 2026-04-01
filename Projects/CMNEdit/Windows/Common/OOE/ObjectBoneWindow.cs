using HActLib;
using HActLib.OOE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMNEdit
{
    internal static class ObjectBoneWindow
    {
        public static void Draw(Form1 form, ObjectBone obj)
        {
            form.CreateHeader("Bone");

            form.CreateInput("Bone ID ", obj.BoneID.ToString(), delegate (string val) { obj.BoneID = int.Parse(val); }, NumberBox.NumberMode.Int);
        }
    }
}
