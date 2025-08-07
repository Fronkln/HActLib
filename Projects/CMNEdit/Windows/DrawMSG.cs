using MsgLib;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMNEdit
{
    internal static class DrawMSG
    {
        public static void DrawEvent(Form1 form, TreeNodeMsgEvent evt)
        {
            form.CreateHeader("Event");
            form.CreateInput("Value", evt.Event.Value.ToString(), delegate (string value) { evt.Event.Value = value; evt.Text = value; });
        }

        public static void DrawProperty(Form1 form, TreeNodeMsgProperty prt)
        {
            form.CreateHeader("Property");
            form.CreateInput("Type 1", prt.Property.Type1.ToString(), delegate { }, readOnly: true);
            form.CreateInput("Type 2", ((byte)prt.Property.PropType).ToString(), delegate { }, readOnly: true);

            switch (prt.Property.GetType().Name)
            {
                case "MsgPropSetSpeaker":
                    form.CreateHeader("Set Speaker");
                    var setSpeaker = prt.Property as MsgPropSetSpeaker;
                    form.CreateInput("Speaker Name", setSpeaker.SpeakerName, delegate(string val) {setSpeaker.SpeakerName = val; });
                    form.CreateInput("Entity UID", setSpeaker.EntityUID.ToString(), delegate(string val) { setSpeaker.EntityUID = int.Parse(val); }, NumberBox.NumberMode.Int);
                    break;

                case "MsgPropPlayAnimation":
                    form.CreateHeader("Play Animation");
                    var anim = prt.Property as MsgPropPlayAnimation;

                    form.CreateInput("Animation", anim.Animation1.ToString(), delegate (string val) { anim.Animation1 = val; });
                    form.CreateInput("Entity UID", anim.EntityUID.ToString(), delegate (string val) { anim.EntityUID = int.Parse(val); }, NumberBox.NumberMode.Int);

                    break;

                case "MsgPropPlayAnimationLoop":
                    form.CreateHeader("Play Looping Animation");
                    var loopAnim = prt.Property as MsgPropPlayAnimationLoop;

                    form.CreateInput("Animation", loopAnim.Animation1.ToString(), delegate(string val) { loopAnim.Animation1 = val; });
                    form.CreateInput("Entity UID", loopAnim.EntityUID.ToString(), delegate (string val) { loopAnim.EntityUID = int.Parse(val); }, NumberBox.NumberMode.Int);
                    break;

                case "MsgPropPlayActionSetAnimation":
                    form.CreateHeader("Play Actionset Animation");
                    var casAnim = prt.Property as MsgPropPlayActionSetAnimation;

                    form.CreateInput("Animation", casAnim.Animation1.ToString(), delegate (string val) { casAnim.Animation1 = val; });
                    form.CreateInput("Unknown", casAnim.Unk1.ToString(), delegate (string val) { casAnim.Unk1 = byte.Parse(val); }, NumberBox.NumberMode.Byte);
                    form.CreateInput("Talk Animation", casAnim.TalkSound.ToString(), delegate (string val) { casAnim.TalkSound = short.Parse(val); }, NumberBox.NumberMode.Short);
                    form.CreateInput("Unknown", casAnim.Unk3.ToString(), delegate (string val) { casAnim.Unk3 = byte.Parse(val); }, NumberBox.NumberMode.Byte);
                    form.CreateInput("Unknown", casAnim.Unk4.ToString(), delegate (string val) { casAnim.Unk4 = short.Parse(val); }, NumberBox.NumberMode.Short);
                    form.CreateInput("Entity UID", casAnim.EntityUID.ToString(), delegate (string val) { casAnim.EntityUID = int.Parse(val); }, NumberBox.NumberMode.Int);
                    break;

                case "MsgPropChoice":
                    form.CreateHeader("Choice");
                    var choice = prt.Property as MsgPropChoice;

                    form.CreateInput("Choice", choice.Choice, delegate(string val) { choice.Choice = val; });
                    form.CreateInput("Unknown", choice.Unk1.ToString(), delegate(string val) { choice.Unk1 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
                    form.CreateInput("Unknown", choice.Unk2.ToString(), delegate (string val) { choice.Unk2 = int.Parse(val); }, NumberBox.NumberMode.Int);
                    form.CreateInput("Unknown", choice.Unk3.ToString(), delegate (string val) { choice.Unk3 = short.Parse(val); }, NumberBox.NumberMode.Short);
                    form.CreateInput("Unknown", choice.Unk4.ToString(), delegate (string val) { choice.Unk4 = int.Parse(val); }, NumberBox.NumberMode.Int);
                    break;

                case "MsgPropMove":
                    form.CreateHeader("Move");
                    var move = prt.Property as MsgPropMove;

                    form.CreateInput("Entity UID To Walk To", move.EntityToWalkTo.ToString(), delegate (string val) { move.EntityToWalkTo = int.Parse(val); }, NumberBox.NumberMode.Int);
                    form.CreateInput("Entity UID", move.EntityUID.ToString(), delegate (string val) { move.EntityUID = int.Parse(val); }, NumberBox.NumberMode.Int);
                    break;

                case "MsgPropFaceEntity":
                    form.CreateHeader("Face Entity");
                    var faceTarget = prt.Property as MsgPropFaceEntity;

                    form.CreateInput("Unknown", faceTarget.Unk1.ToString(), delegate (string val) { faceTarget.Unk1 = short.Parse(val); }, NumberBox.NumberMode.Short);
                    form.CreateInput("Unknown", faceTarget.Unk2.ToString(), delegate (string val) { faceTarget.Unk2 = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    form.CreateInput("Target Entity UID To Face", faceTarget.EntityToFaceUID.ToString(), delegate (string val) { faceTarget.EntityToFaceUID = int.Parse(val); }, NumberBox.NumberMode.Int);
                    form.CreateInput("Entity UID", faceTarget.Unk3.ToString(), delegate (string val) { faceTarget.Unk3 = int.Parse(val); }, NumberBox.NumberMode.Int);
                    break;

                case "MsgPropTeleport":
                    form.CreateHeader("Teleport");
                    var teleport = prt.Property as MsgPropTeleport;

                    form.CreateInput("Entity UID To Teleport To", teleport.EntityUIDToTeleportTo.ToString(), delegate (string val) { teleport.EntityUIDToTeleportTo= int.Parse(val); }, NumberBox.NumberMode.Int);
                    form.CreateInput("Entity UID", teleport.EntityUID.ToString(), delegate (string val) { teleport.EntityUID = int.Parse(val); }, NumberBox.NumberMode.Int);
                    break;

                case "MsgPropSetWindowType":
                    form.CreateHeader("Set Talk Window Type");
                    var setWindowType = prt.Property as MsgPropSetWindowType;

                    form.CreateInput("Window Type", setWindowType.WindowType.ToString(), delegate (string val) { setWindowType.WindowType = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
                    break;

                case "MsgPropFadeIn":
                    form.CreateHeader("Fade In");
                    var fadeIn = prt.Property as MsgPropFadeIn;

                    form.CreateInput("Time (Frames)", fadeIn.Time.ToString(CultureInfo.InvariantCulture), delegate (string val) { fadeIn.Time = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    break;

                case "MsgPropFadeOut":
                    form.CreateHeader("Fade Out");
                    var fadeOut = prt.Property as MsgPropFadeOut;

                    form.CreateInput("Time (Frames)", fadeOut.Time.ToString(CultureInfo.InvariantCulture), delegate (string val) { fadeOut.Time = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    break;

                case "MsgPropSetInvisible":
                    form.CreateHeader("Set Invisible");
                    var setInvis = prt.Property as MsgPropSetInvisible;

                    form.CreateInput("Entity UID", setInvis.EntityUID.ToString(), delegate (string val) { setInvis.EntityUID = int.Parse(val); }, NumberBox.NumberMode.Int);
                    break;
            }
        }
    }
}
