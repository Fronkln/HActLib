using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class DENodeCharacterBehaviorWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DENodeCharacterBehavior bhv = node as DENodeCharacterBehavior;

            PropertyEditorBuilder.CreateHeader("Character Behavior");

            PropertyEditorBuilder.CreateInput("Flags", bhv.Flags.ToString(), delegate (string val) { bhv.Flags = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            PropertyEditorBuilder.CreateInput("Start", bhv.InFrame.Frame.ToString(), delegate (string val) { bhv.InFrame.Frame = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("End", bhv.OutFrame.Frame.ToString(), delegate (string val) { bhv.OutFrame.Frame = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            PropertyEditorBuilder.CreateInput("Behavior Set", bhv.BehaviorSet.ToString(), delegate (string val) { bhv.BehaviorSet = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            PropertyEditorBuilder.CreateInput("Behavior Group", bhv.BehaviorGroup.ToString(), delegate (string val) { bhv.BehaviorGroup = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            PropertyEditorBuilder.CreateInput("Behavior Action", bhv.BehaviorAction.ToString(), delegate (string val) { bhv.BehaviorAction = uint.Parse(val); }, NumberBox.NumberMode.UInt);
        }
    }
}
