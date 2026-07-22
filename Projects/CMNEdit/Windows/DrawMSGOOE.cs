using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YakuzaDataTypes.MSG;

namespace CMNEdit.Windows
{
    internal static class DrawMSGOOE
    {
        public static void DrawEvent(Form1 form, TreeNodeMsgEventOOE evt)
        {
            form.CreateHeader("Event");
            form.CreateMultilineInput("Value", evt.Event.Text.ToString(), delegate (string value) { evt.Event.Text = value; evt.Text = value; }, 100);
        }

        public static void DrawProperty(Form1 form, TreeNodeMsgPropertyOOE prt)
        {
            form.CreateHeader("Property");
            form.CreateInput("Type", ((ushort)prt.Property.Type).ToString("X"), delegate (string val) { prt.Property.Type = (MsgPropTypeOOE)ushort.Parse(val, NumberStyles.HexNumber); });

            switch (prt.Property.Type)
            {
                case MsgPropTypeOOE.PlayAnimation:
                    {
                        var propAnim = (MsgPropertyOOEPlayAnim)prt.Property;
                        form.CreateHeader("Play Animation");

                        form.CreateInput("Unknown 1", propAnim.Unknown1.ToString(), delegate (string val) { propAnim.Unknown1 = short.Parse(val); }, NumberBox.NumberMode.Short);
                        form.CreateInput("Unknown 2", propAnim.Unknown2.ToString(), delegate (string val) { propAnim.Unknown2 = short.Parse(val); }, NumberBox.NumberMode.Short);
                        form.CreateInput("Unknown 3", propAnim.Unknown3.ToString(), delegate (string val) { propAnim.Unknown3 = short.Parse(val); }, NumberBox.NumberMode.Short);

                        form.CreateInput("Entity UID", propAnim.EntityUID.ToString("X"), delegate (string val) { propAnim.EntityUID = uint.Parse(val, NumberStyles.HexNumber); }, NumberBox.NumberMode.UInt);
                        form.CreateInput("Animation", propAnim.Animation, delegate (string val) { propAnim.Animation = val; }, NumberBox.NumberMode.Text);
                        break;
                    }

                case MsgPropTypeOOE.PlayActionSetAnimation:
                    {
                        var propAnim = (MsgPropertyOOEPlayActionsetAnim)prt.Property;
                        form.CreateHeader("Play Actionset Animation");

                        form.CreateInput("Unknown 1", propAnim.Unknown1.ToString(), delegate (string val) { propAnim.Unknown1 = short.Parse(val); }, NumberBox.NumberMode.Short);
                        form.CreateInput("Unknown 2", propAnim.Unknown2.ToString(), delegate (string val) { propAnim.Unknown2 = short.Parse(val); }, NumberBox.NumberMode.Short);
                        form.CreateInput("Unknown 3", propAnim.Unknown3.ToString(), delegate (string val) { propAnim.Unknown3 = short.Parse(val); }, NumberBox.NumberMode.Short);

                        form.CreateInput("Entity UID", propAnim.EntityUID.ToString("X"), delegate (string val) { propAnim.EntityUID = uint.Parse(val, NumberStyles.HexNumber); }, NumberBox.NumberMode.UInt);
                        form.CreateInput("Animation", propAnim.Animation, delegate (string val) { propAnim.Animation = val; }, NumberBox.NumberMode.Text);
                        break;
                    }

                case MsgPropTypeOOE.SetSpeaker:
                    form.CreateHeader("Set Speaker");
                    var speakerProp = prt.Property as MsgPropertyOOESetSpeaker;
                    form.CreateInput("Speaker", speakerProp.Speaker, delegate (string val) { speakerProp.Speaker = val; }, NumberBox.NumberMode.Text);
                    form.CreateInput("Unknown 1", speakerProp.Unknown1.ToString("X"), delegate (string val) { speakerProp.Unknown1 = int.Parse(val, NumberStyles.HexNumber); }, NumberBox.NumberMode.Int);
                    form.CreateInput("Unknown 2", speakerProp.Unknown2.ToString(), delegate (string val) { speakerProp.Unknown2 = short.Parse(val); }, NumberBox.NumberMode.Ushort);
                    form.CreateInput("Unknown 3", speakerProp.Unknown3.ToString(), delegate (string val) { speakerProp.Unknown3 = short.Parse(val); }, NumberBox.NumberMode.Ushort);
                    form.CreateInput("Entity UID", speakerProp.EntityUID.ToString("X"), delegate (string val) { speakerProp.EntityUID = uint.Parse(val, NumberStyles.HexNumber); }, NumberBox.NumberMode.UInt);
                    break;

                case MsgPropTypeOOE.SpeechPause:
                    form.CreateHeader("Speech Pause");
                    var speechPause = prt.Property as MSGPropertyOOESpeechPause;

                    var pauseParentEvent = prt.Parent as TreeNodeMsgEventOOE;

                    form.CreateInput("Pause Length", speechPause.PauseLength.ToString(), delegate (string val) { speechPause.PauseLength = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
                    var pausePosBox = form.CreateInput("Pause Position", speechPause.PausePosition.ToString(), delegate (string val) { speechPause.PausePosition = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);


                    string pauseText = pauseParentEvent.Event.Text;
                    string pausePrefix = "<--(PAUSE)";

                    if (speechPause.PausePosition >= pauseText.Length)
                        pauseText = pauseParentEvent.Event.Text + pausePrefix;
                    else
                        pauseText = pauseText.Insert(speechPause.PausePosition, pausePrefix);

                    var pauseTextEditBox = form.CreateMultilineInput("Pause", pauseText, delegate (string val)
                    {
                        int pauseIdx = val.IndexOf(pausePrefix);

                        if (pauseIdx < 0)
                            return;

                        pausePosBox.Text = pauseIdx.ToString();
                        speechPause.PausePosition = (ushort)pauseIdx;
                    }, 100);
                    break;

                case MsgPropTypeOOE.DialogueSettings:

                    var dialogueSettings = (MsgPropertyOOEDialogueSettings)prt.Property;
                    form.CreateHeader("Dialogue Settings");

                    form.CreateInput("Unknown 1", dialogueSettings.Unknown1.ToString(), delegate (string val) { dialogueSettings.Unknown1 = int.Parse(val); }, NumberBox.NumberMode.Int);
                    form.CreateInput("Length", dialogueSettings.Length.ToString(), delegate (string val) { dialogueSettings.Length = short.Parse(val); }, NumberBox.NumberMode.Short);
                    var textLengthBox =  form.CreateInput("Text Length", dialogueSettings.TextLength.ToString(), delegate (string val) { dialogueSettings.TextLength = short.Parse(val); }, NumberBox.NumberMode.Short);

                    form.CreateButton("Set length to text length", delegate
                    {
                        string eventVal = ((TreeNodeMsgEventOOE)prt.Parent).Event.Text;
                        int length = 0;

                        if (!string.IsNullOrEmpty(eventVal))
                            length = eventVal.Length;

                        textLengthBox.Text = length.ToString();
                        dialogueSettings.TextLength = (short)length;
                    });
                    break;
            }
        }
    }
}
