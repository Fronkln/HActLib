﻿using System;
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

            form.CreateHeader("Character Behavior");

            form.CreateInput("Flags", bhv.Flags.ToString(), delegate (string val) { bhv.Flags = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateInput("Start", bhv.InFrame.Frame.ToString(), delegate (string val) { bhv.InFrame.Frame = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("End", bhv.OutFrame.Frame.ToString(), delegate (string val) { bhv.OutFrame.Frame = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Behavior Set", bhv.BehaviorSet.ToString(), delegate (string val) { bhv.BehaviorSet = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateInput("Behavior Group", bhv.BehaviorGroup.ToString(), delegate (string val) { bhv.BehaviorGroup = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateInput("Behavior Action", bhv.BehaviorAction.ToString(), delegate (string val) { bhv.BehaviorAction = uint.Parse(val); }, NumberBox.NumberMode.UInt);
        }
    }
}
