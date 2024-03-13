using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class DECustomElementLAB8CharacterSelectionWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DECustomElementLAB8CharacterSelection selection = node as DECustomElementLAB8CharacterSelection;

            form.CreateHeader("Character Selection");
            form.CreateInput("Player ID", selection.PlayerID.ToString(), delegate (string val) { selection.PlayerID = uint.Parse(val); }, NumberBox.NumberMode.UInt);
        }
    }
}
