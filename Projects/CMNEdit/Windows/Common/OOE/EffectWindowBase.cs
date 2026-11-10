using HActLib.OOE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMNEdit
{
    internal static class EffectWindowBase
    {
        public static void Draw(Form1 form, EffectBase effect)
        {
            PropertyEditorBuilder.CreateHeader("Effect");

            PropertyEditorBuilder.CreateSpace(25);

            PropertyEditorBuilder.CreateInput("GUID", effect.Guid.ToString(), null, readOnly: true);
            PropertyEditorBuilder.CreateInput("Type", effect.ElementKind.ToString(), null, readOnly: true);
            PropertyEditorBuilder.CreateInput("Bone ID", effect.BoneID.ToString(), delegate(string val) { effect.BoneID = int.Parse(val); }, NumberBox.NumberMode.Int);
        }
    }
}
