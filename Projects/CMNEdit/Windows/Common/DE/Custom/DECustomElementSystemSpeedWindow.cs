using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class DECustomElementSystemSpeedWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DECustomElementSystemSpeed speed = node as DECustomElementSystemSpeed;

            if (speed.Type > 5)
                speed.Type = 5;

            form.CreateHeader("System Speed (Custom)");

            string[] speedTypes = Enum.GetNames<SpeedType>();

            form.CreateComboBox("Speed Type", (int)speed.Type, speedTypes, delegate (int index) { speed.Type = (uint)index; });
            form.CreateInput("Speed", speed.Speed.ToString(), delegate (string val) { speed.Speed = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
        }
    }
}
