using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class DEElementCharacterChangeWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementCharacterChange charChange = node as DEElementCharacterChange;

            form.CreateHeader("Character Change");

            form.CreateInput("Character ID", charChange.CharacterID.ToString(), delegate (string val) { charChange.CharacterID = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateInput("Flag", charChange.Flags.ToString(), delegate (string val) { charChange.Flags = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateInput("Mask", charChange.Mask.ToString(), delegate (string val) { charChange.Mask = byte.Parse(val); }, NumberBox.NumberMode.Byte);
        }
    }
}
