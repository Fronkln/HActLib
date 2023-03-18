using System;
using CMNEdit;
using CMNEdit.Windows.Common.DE;
using HActLib;

namespace CMNEdit.Windows.Common.DE
{
    public static class DEElementTimingInfoWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DETimingInfoMuteki muteki = node as DETimingInfoMuteki;

            if (node is DETimingInfoMuteki)
            {
                form.CreateHeader("Invincibility");
                form.CreateComboBox("Mode:", (int)muteki.Param, Enum.GetNames(typeof(InvulnerabilityParam)), delegate (int val) { muteki.Param = (uint)val; });
            }
        }
    }
}
