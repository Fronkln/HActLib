using HActLib.OOE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TevView
{
    internal static class EffectWindowBase
    {
        public static void Draw(Form1 form, EffectBase effect)
        {
            form.CreateHeader("Effect");

            form.CreateSpace(25);

            form.CreateInput("GUID", effect.Guid.ToString(), null, readOnly: true);
            form.CreateInput("Type", effect.ElementKind.ToString(), null, readOnly: true);
        }
    }
}
