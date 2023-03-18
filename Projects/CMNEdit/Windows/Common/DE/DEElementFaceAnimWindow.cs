using System;
using HActLib;


namespace CMNEdit
{
    internal static class DEElementFaceAnimWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementFaceAnim anim = node as DEElementFaceAnim;

            Type enumType = null;

            switch(Form1.curGame)
            {
                default:
                    enumType = typeof(FacePatternLJ);
                    break;
                case Game.Y6: case Game.Y6Demo:
                    enumType = typeof(FacePatternY6);
                    break;
                case Game.YK2:
                    enumType = typeof(FacePatternY6);
                    break;
                case Game.YLAD:
                    enumType = typeof(FacePatternYLAD);
                    break;
                case Game.LJ:
                    enumType = typeof(FacePatternLJ);
                    break;
            }

            form.CreateHeader("Face Animation");
            form.CreateComboBox("Pattern", (int)anim.PatternID, Enum.GetNames(enumType), delegate (int val) { anim.PatternID = (uint)val; });
        }
    }
}
