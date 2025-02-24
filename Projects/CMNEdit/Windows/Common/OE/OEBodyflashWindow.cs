using System;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal class OEBodyflashWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            OEBodyflash flash = node as OEBodyflash;

            form.CreateHeader("Bodyflash");
            form.CreateInput("Bone ID 1", flash.BoneID.ToString(), delegate (string val) { flash.BoneID = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Bone ID 2", flash.BoneID2.ToString(), delegate (string val) { flash.BoneID2 = int.Parse(val); }, NumberBox.NumberMode.Int);
        }
    }
}
