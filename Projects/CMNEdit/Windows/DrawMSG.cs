using YakuzaDataTypes.MSG;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CMNEdit
{
    internal static class DrawMSG
    {
        public static void DrawEvent(Form1 form, TreeNodeMsgEvent evt)
        {
            form.CreateHeader("Event");
            form.CreateMultilineInput("Value", evt.Event.Value.ToString(), delegate (string value) { evt.Event.Value = value; evt.Text = value; }, 45);

            form.CreateButton("Auto Generate Pauses Y5", delegate
            {
                for (int i = 0; i < evt.Event.Value.Length; i++)
                {
                    MsgPropSpeechPause pause = null;


                    if (i == evt.Event.Value.Length - 1)
                    {
                        pause = new MsgPropSpeechPause();
                        pause.PauseLength = 20;
                    }
                    else
                    {
                        switch(evt.Event.Value[i])
                        {
                            case '!':
                                pause = new MsgPropSpeechPause();
                                pause.PauseLength = 12;
                                break;

                            case '?':
                                pause = new MsgPropSpeechPause();
                                pause.PauseLength = 12;
                                break;

                            case ',':
                                pause = new MsgPropSpeechPause();
                                pause.PauseLength = 10;
                                break;
                            case '.':
                                pause = new MsgPropSpeechPause();
                                pause.PauseLength = 12;
                                break;
                        }
                    }


                    if (pause != null)
                    {
                        pause.PropType = (ushort)Msg.GetIDForPropType("SpeechPause", (YakuzaDataTypes.Game)Enum.Parse(typeof(YakuzaDataTypes.Game), Form1.curGame.ToString())); 
                        pause.PausePosition = (ushort)(i + 1);

                        evt.Nodes.Add(new TreeNodeMsgProperty(pause));
                    }
                }
            });
        }

        public static void DrawProperty(Form1 form, TreeNodeMsgProperty prt)
        {
            form.CreateHeader("Property");
            form.CreateInput("Type", ((ushort)prt.Property.PropType).ToString("X"), delegate (string val) { prt.Property.PropType = ushort.Parse(val, NumberStyles.HexNumber); });

            bool isY0 = Form1.curGame >= HActLib.Game.Ishin;
            var propEnumType = isY0 ? typeof(MsgPropTypeY0) : typeof(MsgPropTypeY5);

            string name = Enum.GetName(propEnumType, (ushort)prt.Property.PropType);

            switch (name)
            {
                case "DialogueSettings":
                    form.CreateHeader("Dialogue Settings");
                    var dialogSettings = prt.Property as MsgPropDialogueSettings;

                    form.CreateInput("Unk1", dialogSettings.Unk1.ToString(), delegate (string val) { dialogSettings.Unk1 = short.Parse(val); });
                    form.CreateInput("Unk2", dialogSettings.Unk2.ToString(), delegate (string val) { dialogSettings.Unk2 = short.Parse(val); });
                    form.CreateInput("Unk3", dialogSettings.Unk3.ToString(), delegate (string val) { dialogSettings.Unk3 = short.Parse(val); });
                    form.CreateInput("Duration", dialogSettings.Duration.ToString(), delegate (string val) { dialogSettings.Duration = short.Parse(val); });
                    var lengthBox = form.CreateInput("Text Length", dialogSettings.TextLength.ToString(), delegate (string val) { dialogSettings.TextLength = short.Parse(val); }, NumberBox.NumberMode.Short);
                    form.CreateInput("Entity UID", dialogSettings.EntityUID.ToString(), delegate (string val) { dialogSettings.EntityUID = int.Parse(val); }, NumberBox.NumberMode.Int);


                    form.CreateButton("Set length to text length", delegate
                    {
                        string eventVal = ((TreeNodeMsgEvent)form.nodesTree.SelectedNode.Parent).Event.Value;
                        int length = 0;

                        if (!string.IsNullOrEmpty(eventVal))
                            length = eventVal.Length;

                        lengthBox.Text = length.ToString();
                        dialogSettings.TextLength = (short)length;
                    });

                    break;

                case "ScenarioBranching":
                    form.CreateHeader("Scenario Branching");
                    var branchingScenario = prt.Property as MsgPropScenarioBranching;
                    form.CreateInput("Unk1", branchingScenario.Unk1.ToString(), delegate (string val) { branchingScenario.Unk1 = byte.Parse(val); });
                    form.CreateInput("Unk2", branchingScenario.Unk2.ToString(), delegate (string val) { branchingScenario.Unk2 = byte.Parse(val); });

                    var parentGroupScenario = prt.Parent.Parent as TreeNodeMsgGroup;
                    var eventsScenario = parentGroupScenario.Nodes.Cast<TreeNodeMsgEvent>().ToList();

                    var branchEventScenario = eventsScenario.FirstOrDefault(x => x.Event == branchingScenario.BranchingEvent);
                    form.CreateComboBox("Branching Event", eventsScenario.IndexOf(branchEventScenario), eventsScenario.Select(x => x.Text).ToArray(), delegate (int idx) { branchingScenario.BranchingEvent = eventsScenario[idx].Event; });
                    break;
                case "ChoiceBranching":
                case "ChoiceBranching2":
                case "UnknownBranching":
                    form.CreateHeader("Choice Branching");
                    var branching = prt.Property as MsgPropBranching;
                    form.CreateInput("Unk1", branching.Unk1.ToString(), delegate (string val) { branching.Unk1 = byte.Parse(val); });

                    var parentGroup = prt.Parent.Parent as TreeNodeMsgGroup;
                    var events = parentGroup.Nodes.Cast<TreeNodeMsgEvent>().ToList();

                    var branchEvent = events.FirstOrDefault(x => x.Event == branching.BranchingEvent);
                    form.CreateComboBox("Branching Event", events.IndexOf(branchEvent), events.Select(x => x.Text).ToArray(), delegate (int idx) { branching.BranchingEvent = events[idx].Event; });

                    // form.CreateInput("Event ID To Branch", branching.BranchEventID.ToString(), delegate (string val) { branching.BranchEventID = byte.Parse(val); });
                    break;

                case "SetSpeaker":
                    form.CreateHeader("Set Speaker");
                    var setSpeaker = prt.Property as MsgPropSetSpeaker;
                    form.CreateInput("Speaker Name", setSpeaker.SpeakerName, delegate (string val) { setSpeaker.SpeakerName = val; });
                    form.CreateInput("Entity UID", setSpeaker.EntityUID.ToString(), delegate (string val) { setSpeaker.EntityUID = int.Parse(val); }, NumberBox.NumberMode.Int);
                    break;

                case "SpeechPause":
                    form.CreateHeader("Speech Pause");
                    var speechPause = prt.Property as MsgPropSpeechPause;

                    var pauseParentEvent = prt.Parent as TreeNodeMsgEvent;

                    form.CreateInput("Pause Length", speechPause.PauseLength.ToString(), delegate (string val) { speechPause.PauseLength = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
                    var pausePosBox = form.CreateInput("Pause Position", speechPause.PausePosition.ToString(), delegate (string val) { speechPause.PausePosition = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);


                    string pauseText = pauseParentEvent.Event.Value;
                    string pausePrefix = "<--(PAUSE)";

                    if (speechPause.PausePosition >= pauseText.Length)
                        pauseText = pauseParentEvent.Event.Value + pausePrefix;
                    else
                        pauseText = pauseText.Insert(speechPause.PausePosition, pausePrefix);

                    var pauseTextEditBox = form.CreateMultilineInput("Pause", pauseText, delegate (string val)
                    {
                        int pauseIdx = val.IndexOf(pausePrefix);

                        if (pauseIdx < 0)
                            return;

                        pausePosBox.Text = pauseIdx.ToString();
                        speechPause.PausePosition = (ushort)pauseIdx;
                    }, 40);
                    break;

                case "PlayAnimation":
                    form.CreateHeader("Play Animation");
                    var anim = prt.Property as MsgPropPlayAnimation;

                    form.CreateInput("Animation", anim.Animation1.ToString(), delegate (string val) { anim.Animation1 = val; });
                    form.CreateInput("Entity UID", anim.EntityUID.ToString(), delegate (string val) { anim.EntityUID = int.Parse(val); }, NumberBox.NumberMode.Int);
                    break;

                case "PlayAnimationLoop":
                    form.CreateHeader("Play Looping Animation");
                    var loopAnim = prt.Property as MsgPropPlayAnimationLoop;

                    form.CreateInput("Animation", loopAnim.Animation1.ToString(), delegate (string val) { loopAnim.Animation1 = val; });
                    form.CreateInput("Entity UID", loopAnim.EntityUID.ToString(), delegate (string val) { loopAnim.EntityUID = int.Parse(val); }, NumberBox.NumberMode.Int);
                    break;

                case "SetIdleAnimation":
                    form.CreateHeader("Set Idle Animation");
                    var setIdleAnim = prt.Property as MsgPropSetIdleAnimation;

                    form.CreateInput("Animation", setIdleAnim.Animation1.ToString(), delegate (string val) { setIdleAnim.Animation1 = val; });
                    form.CreateInput("Entity UID", setIdleAnim.EntityUID.ToString(), delegate (string val) { setIdleAnim.EntityUID = int.Parse(val); }, NumberBox.NumberMode.Int);
                    break;

                case "PlayActionSetAnimation":
                    form.CreateHeader("Play Actionset Animation");
                    var casAnim = prt.Property as MsgPropPlayActionSetAnimation;

                    form.CreateInput("Animation", casAnim.Animation1.ToString(), delegate (string val) { casAnim.Animation1 = val; });
                    form.CreateInput("Unknown", casAnim.Unk1.ToString(), delegate (string val) { casAnim.Unk1 = byte.Parse(val); }, NumberBox.NumberMode.Byte);
                    form.CreateInput("Talk Animation", casAnim.TalkSound.ToString(), delegate (string val) { casAnim.TalkSound = short.Parse(val); }, NumberBox.NumberMode.Short);
                    form.CreateInput("Unknown", casAnim.Unk3.ToString(), delegate (string val) { casAnim.Unk3 = byte.Parse(val); }, NumberBox.NumberMode.Byte);
                    form.CreateInput("Unknown", casAnim.Unk4.ToString(), delegate (string val) { casAnim.Unk4 = short.Parse(val); }, NumberBox.NumberMode.Short);
                    form.CreateInput("Entity UID", casAnim.EntityUID.ToString(), delegate (string val) { casAnim.EntityUID = int.Parse(val); }, NumberBox.NumberMode.Int);
                    break;

                case "Choice":
                    form.CreateHeader("Choice");
                    var choice = prt.Property as MsgPropChoice;

                    form.CreateInput("Choice", choice.Choice, delegate (string val) { choice.Choice = val; });
                    form.CreateInput("Unknown", choice.Unk1.ToString(), delegate (string val) { choice.Unk1 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
                    form.CreateInput("Unknown", choice.Unk2.ToString(), delegate (string val) { choice.Unk2 = int.Parse(val); }, NumberBox.NumberMode.Int);
                    form.CreateInput("Unknown", choice.Unk3.ToString(), delegate (string val) { choice.Unk3 = short.Parse(val); }, NumberBox.NumberMode.Short);
                    form.CreateInput("Unknown", choice.Unk4.ToString(), delegate (string val) { choice.Unk4 = int.Parse(val); }, NumberBox.NumberMode.Int);
                    break;

                case "MoveToEntity":
                    form.CreateHeader("Move");
                    var move = prt.Property as MsgPropMoveToEntity;

                    form.CreateInput("Entity UID To Walk To", move.EntityToWalkTo.ToString("x"), delegate (string val) { move.EntityToWalkTo = int.Parse(val, NumberStyles.HexNumber); }, NumberBox.NumberMode.Int, hex: true);
                    form.CreateInput("Entity UID", move.EntityUID.ToString("x"), delegate (string val) { move.EntityUID = int.Parse(val, NumberStyles.HexNumber); }, NumberBox.NumberMode.Int, hex:true);
                    break;

                case "TeleportToEntity":
                    form.CreateHeader("Teleport");
                    var teleport = prt.Property as MsgPropTeleportToEntity;

                    form.CreateInput("Entity UID To Teleport To", teleport.EntityUIDToTeleportTo.ToString("x"), delegate (string val) { teleport.EntityUIDToTeleportTo = int.Parse(val, NumberStyles.HexNumber); }, NumberBox.NumberMode.Int, hex: true);
                    form.CreateInput("Entity UID", teleport.EntityUID.ToString("x"), delegate (string val) { teleport.EntityUID = int.Parse(val, NumberStyles.HexNumber); }, NumberBox.NumberMode.Int, hex: true);
                    break;

                case "TeleportToMsgCoordinate":
                    var teleportToCoord = prt.Property as MsgPropTeleportToCoordinate;

                    form.CreateHeader("Teleport (MSG Coordinate)");
                    form.CreateInput("Unknown 1", teleportToCoord.Unk1.ToString(), delegate (string val) { teleportToCoord.Unk1 = int.Parse(val); }, NumberBox.NumberMode.Int);
                    form.CreateInput("Unknown 2", teleportToCoord.Unk2.ToString(), delegate (string val) { teleportToCoord.Unk2 = short.Parse(val); }, NumberBox.NumberMode.Short);
                    form.CreateInput("Coordinate ID", teleportToCoord.CoordinateID.ToString(), delegate (string val) { teleportToCoord.CoordinateID = int.Parse(val); });
                    form.CreateInput("Entity UID", teleportToCoord.EntityUID.ToString(), delegate (string val) { teleportToCoord.EntityUID = int.Parse(val); });

                    break;

                case "FaceEntity":
                    form.CreateHeader("Face Target");
                    var faceEntity = prt.Property as MsgPropFaceEntity;

                    form.CreateInput("Angle Limit? (Rot Y)", faceEntity.Unk1.ToString(), delegate (string val) { faceEntity.Unk1 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
                    form.CreateInput("Angle", OERotationY.ToAngle(faceEntity.Unk1).ToString(), delegate (string val) { faceEntity.Unk1 = OERotationY.ToOERotation(Utils.InvariantParse(val)); }, NumberBox.NumberMode.Ushort);
                    form.CreateInput("Unknown", faceEntity.Unk2.ToString(CultureInfo.InvariantCulture), delegate (string val) { faceEntity.Unk2 = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                    form.CreateInput("Facing Entity UID", faceEntity.Unk3.ToString(), delegate (string val) { faceEntity.Unk3 = int.Parse(val); }, NumberBox.NumberMode.Int);
                    form.CreateInput("Entity To Face UID", faceEntity.EntityToFaceUID.ToString(), delegate (string val) { faceEntity.EntityToFaceUID = int.Parse(val); }, NumberBox.NumberMode.Int);
                    break;

                case "FaceExpression":
                    form.CreateHeader("Face Expression");
                    var faceExp = prt.Property as MsgPropFaceExpression;

                    form.CreateInput("IFA ID", faceExp.IfaID.ToString(), delegate (string val) { faceExp.IfaID = short.Parse(val); }, NumberBox.NumberMode.Ushort);
                    form.CreateInput("IFA ID 2", faceExp.IfaID2.ToString(), delegate (string val) { faceExp.IfaID2 = short.Parse(val); }, NumberBox.NumberMode.Ushort);

                    form.CreateInput("Unknown 1", faceExp.Unk1.ToString(), delegate (string val) { faceExp.Unk1 = int.Parse(val); }, NumberBox.NumberMode.Int);
                    form.CreateInput("Unknown 2", faceExp.Unk2.ToString(), delegate (string val) { faceExp.Unk2 = short.Parse(val); }, NumberBox.NumberMode.Short);

                    form.CreateInput("Entity UID", faceExp.EntityUID.ToString(), delegate (string val) { faceExp.EntityUID = int.Parse(val); }, NumberBox.NumberMode.Int);
                    break;


                case "CameraParam":
                    form.CreateHeader("Camera Param");
                    var cParam = prt.Property as MsgPropCameraParam;

                    form.CreateInput("Flags", cParam.Flags1.ToString(), delegate (string val) { cParam.Flags1 = (MsgPropCameraParam.Flag1)ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
                    form.CreateInput("Flags", cParam.Flags2.ToString(), delegate (string val) { cParam.Flags2 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
                    form.CreateInput("Unknown", cParam.Unk1.ToString(), delegate (string val) { cParam.Unk1 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);

                    if (((ushort)cParam.Flags1 & 49152) != 0)
                    {
                        if ((cParam.Flags2 & 25600) != 0)
                        {
                            form.CreateInput("Entity UID 1", cParam.IntVal1.ToString(), delegate (string val) { cParam.IntVal1 = int.Parse(val); }, NumberBox.NumberMode.Int);
                            form.CreateInput("Entity UID 2", cParam.IntVal2.ToString(), delegate (string val) { cParam.IntVal2 = int.Parse(val); }, NumberBox.NumberMode.Int);
                        }
                        else
                        {
                            form.CreateInput("Integer Value 1", cParam.IntVal1.ToString(), delegate (string val) { cParam.IntVal1 = int.Parse(val); }, NumberBox.NumberMode.Int);
                            form.CreateInput("Integer Value 2", cParam.IntVal2.ToString(), delegate (string val) { cParam.IntVal2 = int.Parse(val); }, NumberBox.NumberMode.Int);
                        }
                    }
                    else
                    {
                        if (cParam.Flags1.HasFlag(MsgPropCameraParam.Flag1.MoveCameraInterpolated) || cParam.Flags1.HasFlag(MsgPropCameraParam.Flag1.MoveCamera))
                        {
                            form.CreateInput("Lookat Coordinate ID", cParam.IntVal1.ToString(), delegate (string val) { cParam.IntVal1 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
                            form.CreateInput("Position Coordinate ID", cParam.IntVal2.ToString(), delegate (string val) { cParam.IntVal2 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);


                            var nodePos = (form.nodesTree.Nodes[1].Nodes[cParam.IntVal2] as TreeNodeMsgCoordinate);
                            var nodeLookAt = (form.nodesTree.Nodes[1].Nodes[cParam.IntVal1] as TreeNodeMsgCoordinate); 

                            MsgPositionData posDat = nodePos.Position;
                            MsgPositionData lookDat = nodeLookAt.Position;

                            form.CreateInput("Position X", posDat.Position.x.ToString(CultureInfo.InvariantCulture), delegate (string val) { posDat.Position.x = Utils.InvariantParse(val); nodePos.Position = posDat; }, NumberBox.NumberMode.Float);
                            form.CreateInput("Position Y", posDat.Position.y.ToString(CultureInfo.InvariantCulture), delegate (string val) { posDat.Position.y = Utils.InvariantParse(val); nodePos.Position = posDat; }, NumberBox.NumberMode.Float);
                            form.CreateInput("Position Z", posDat.Position.z.ToString(CultureInfo.InvariantCulture), delegate (string val) { posDat.Position.z = Utils.InvariantParse(val); nodePos.Position = posDat; }, NumberBox.NumberMode.Float);

                            form.CreateSpace(10);

                            form.CreateInput("Lookat X", lookDat.Position.x.ToString(CultureInfo.InvariantCulture), delegate (string val) { lookDat.Position.x = Utils.InvariantParse(val); nodeLookAt.Position = lookDat; }, NumberBox.NumberMode.Float);
                            form.CreateInput("Lookat Y", lookDat.Position.y.ToString(CultureInfo.InvariantCulture), delegate (string val) { lookDat.Position.y = Utils.InvariantParse(val); nodeLookAt.Position = lookDat; }, NumberBox.NumberMode.Float);
                            form.CreateInput("Lookat Z", lookDat.Position.z.ToString(CultureInfo.InvariantCulture), delegate (string val) { lookDat.Position.z = Utils.InvariantParse(val); nodeLookAt.Position = lookDat; }, NumberBox.NumberMode.Float);
                        }
                        else
                        {
                            form.CreateInput("UShort Value 1", cParam.IntVal1.ToString(), delegate (string val) { cParam.IntVal1 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
                            form.CreateInput("UShort Value 2", cParam.IntVal2.ToString(), delegate (string val) { cParam.IntVal2 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
                        }
                    }


                    if (cParam.Flags1.HasFlag(MsgPropCameraParam.Flag1.MoveCameraInterpolated))
                    {
                        form.CreateInput("Interpolation Duration", cParam.FloatVal1.ToString(CultureInfo.InvariantCulture), delegate (string val) { cParam.FloatVal1 = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    }

                    break;

                case "SetWindowType":
                    form.CreateHeader("Set Talk Window Type");
                    var setWindowType = prt.Property as MsgPropSetWindowType;

                    form.CreateInput("Window Type", setWindowType.WindowType.ToString(), delegate (string val) { setWindowType.WindowType = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
                    break;

                case "ScreenFadeIn":
                    form.CreateHeader("Screen Fade In");
                    var fadeIn = prt.Property as MsgPropScreenFadeIn;

                    form.CreateInput("Time (Frames)", fadeIn.Time.ToString(CultureInfo.InvariantCulture), delegate (string val) { fadeIn.Time = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    break;
                case "ScreenFadeOut":
                    form.CreateHeader("Screen Fade Out");
                    var fadeOut = prt.Property as MsgPropScreenFadeOut;

                    form.CreateInput("Time (Frames)", fadeOut.Time.ToString(CultureInfo.InvariantCulture), delegate (string val) { fadeOut.Time = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    break;

                case "SetVisibility":
                    form.CreateHeader("Set Invisible");
                    var setInvis = prt.Property as MsgPropSetVisibility;

                    form.CreateInput("Invisible", setInvis.Invisible.ToString(), delegate (string val) { setInvis.Invisible = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
                    form.CreateInput("Entity UID", setInvis.EntityUID.ToString(), delegate (string val) { setInvis.EntityUID = int.Parse(val); }, NumberBox.NumberMode.Int);
                    break;

                case "PlayBgm":
                    form.CreateHeader("Play BGM");
                    var playBgm = prt.Property as MsgPropPlayBGM;

                    form.CreateInput("BGM", playBgm.BGM, delegate (string val) { playBgm.BGM = val; });
                    form.CreateInput("Unknown 1", playBgm.Unk1.ToString(), delegate (string val) { playBgm.Unk1 = int.Parse(val); }, NumberBox.NumberMode.Int);
                    form.CreateInput("Unknown 2", playBgm.Unk2.ToString(), delegate (string val) { playBgm.Unk2 = int.Parse(val); }, NumberBox.NumberMode.Int);
                    form.CreateInput("Unknown 3", playBgm.Unk3.ToString(CultureInfo.InvariantCulture), delegate (string val) { playBgm.Unk3 = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    break;

                case "CharacterSpeech":
                    form.CreateHeader("Character Speech");
                    var charaSpeech = prt.Property as MsgPropCharacterSpeech;

                    form.CreateInput("Unknown", charaSpeech.Unknown.ToString(), delegate (string val) { charaSpeech.Unknown = int.Parse(val); });
                    form.CreateInput("Unknown", charaSpeech.Unknown2.ToString(), delegate (string val) { charaSpeech.Unknown2 = short.Parse(val); });
                    form.CreateInput("Speech", charaSpeech.Speech, delegate (string val) { charaSpeech.Speech = val; });
                    form.CreateInput("Entity UID", charaSpeech.EntityUID.ToString(), delegate (string val) { charaSpeech.EntityUID = int.Parse(val); }, NumberBox.NumberMode.Int);
                    break;

                case "PlayDialogueSound":
                    form.CreateHeader("Dialogue Sound");
                    var diagSound = prt.Property as MsgPropPlayDialogueSound;

                    form.CreateInput("Unknown 1", diagSound.Unk1.ToString(), delegate (string val) { diagSound.Unk1 = byte.Parse(val); }, NumberBox.NumberMode.Byte);
                    form.CreateInput("Sound", diagSound.Sound, delegate (string val) { diagSound.Sound = val; });

                    break;

                case "UnknownPropWithString1":
                    form.CreateHeader("Unknown Prop");
                    var unkPropStr1 = prt.Property as MsgPropUnknownPropWString;

                    form.CreateInput("Unknown 1", unkPropStr1.Unk1.ToString(), delegate (string val) { unkPropStr1.Unk1 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
                    form.CreateInput("Unknown 2", unkPropStr1.Unk2.ToString(), delegate (string val) { unkPropStr1.Unk2 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
                    form.CreateInput("Unknown 3", unkPropStr1.Unk3.ToString(), delegate (string val) { unkPropStr1.Unk3 = int.Parse(val); }, NumberBox.NumberMode.Int);

                    form.CreateInput("Unknown String", unkPropStr1.UnknownString.ToString(), delegate (string val) { unkPropStr1.UnknownString = val; });
                    break;

                case "UnknownPropWithString2":
                    form.CreateHeader("Unknown Prop");
                    var unkPropStr2 = prt.Property as MsgPropUnknownPropWString2;

                    form.CreateInput("Unknown 1", unkPropStr2.Unk1.ToString(), delegate (string val) { unkPropStr2.Unk1 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
                    form.CreateInput("Unknown 2", unkPropStr2.Unk2.ToString(), delegate (string val) { unkPropStr2.Unk2 = int.Parse(val); }, NumberBox.NumberMode.Int);

                    form.CreateInput("Unknown String", unkPropStr2.UnknownString.ToString(), delegate (string val) { unkPropStr2.UnknownString = val; });
                    break;

            }
        }

        public static void DrawCoordinate(Form1 form, TreeNodeMsgCoordinate coord)
        {
            form.CreateHeader("Coordinate");

            form.CreateInput("X", coord.Position.Position.x.ToString(CultureInfo.InvariantCulture), delegate (string val) { coord.Position.Position.x = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Y", coord.Position.Position.y.ToString(CultureInfo.InvariantCulture), delegate (string val) { coord.Position.Position.y = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Z", coord.Position.Position.z.ToString(CultureInfo.InvariantCulture), delegate (string val) { coord.Position.Position.z = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Angle", OERotationY.ToAngle((ushort)coord.Position.Angle).ToString(), delegate (string val) { coord.Position.Angle = OERotationY.ToOERotation(Utils.InvariantParse(val)); }, NumberBox.NumberMode.Ushort);
            form.CreateInput("Angle (Rot Y)", coord.Position.Angle.ToString(), delegate (string val) { coord.Position.Angle = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
        }
    }
}
