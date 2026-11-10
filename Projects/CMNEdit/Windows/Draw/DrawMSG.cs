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
            PropertyEditorBuilder.CreateHeader("Event");
            PropertyEditorBuilder.CreateMultilineInput("Value", evt.Event.Value.ToString(), delegate (string value) { evt.Event.Value = value; evt.Text = value; }, 45);

            PropertyEditorBuilder.CreateButton("Auto Generate Pauses Y5", delegate
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
            PropertyEditorBuilder.CreateHeader("Property");
            PropertyEditorBuilder.CreateInput("Type", ((ushort)prt.Property.PropType).ToString("X"), delegate (string val) { prt.Property.PropType = ushort.Parse(val, NumberStyles.HexNumber); });

            bool isY0 = Form1.curGame >= HActLib.Game.Ishin;
            var propEnumType = isY0 ? typeof(MsgPropTypeY0) : typeof(MsgPropTypeY5);

            string name = Enum.GetName(propEnumType, (ushort)prt.Property.PropType);

            switch (name)
            {
                case "DialogueSettings":
                    PropertyEditorBuilder.CreateHeader("Dialogue Settings");
                    var dialogSettings = prt.Property as MsgPropDialogueSettings;

                    PropertyEditorBuilder.CreateInput("Unk1", dialogSettings.Unk1.ToString(), delegate (string val) { dialogSettings.Unk1 = short.Parse(val); });
                    PropertyEditorBuilder.CreateInput("Unk2", dialogSettings.Unk2.ToString(), delegate (string val) { dialogSettings.Unk2 = short.Parse(val); });
                    PropertyEditorBuilder.CreateInput("Unk3", dialogSettings.Unk3.ToString(), delegate (string val) { dialogSettings.Unk3 = short.Parse(val); });
                    PropertyEditorBuilder.CreateInput("Duration", dialogSettings.Duration.ToString(), delegate (string val) { dialogSettings.Duration = short.Parse(val); });
                    var lengthBox = PropertyEditorBuilder.CreateInput("Text Length", dialogSettings.TextLength.ToString(), delegate (string val) { dialogSettings.TextLength = short.Parse(val); }, NumberBox.NumberMode.Short);
                    PropertyEditorBuilder.CreateInput("Entity UID", dialogSettings.EntityUID.ToString(), delegate (string val) { dialogSettings.EntityUID = int.Parse(val); }, NumberBox.NumberMode.Int);


                    PropertyEditorBuilder.CreateButton("Set length to text length", delegate
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
                    PropertyEditorBuilder.CreateHeader("Scenario Branching");
                    var branchingScenario = prt.Property as MsgPropScenarioBranching;
                    PropertyEditorBuilder.CreateInput("Unk1", branchingScenario.Unk1.ToString(), delegate (string val) { branchingScenario.Unk1 = byte.Parse(val); });
                    PropertyEditorBuilder.CreateInput("Unk2", branchingScenario.Unk2.ToString(), delegate (string val) { branchingScenario.Unk2 = byte.Parse(val); });

                    var parentGroupScenario = prt.Parent.Parent as TreeNodeMsgGroup;
                    var eventsScenario = parentGroupScenario.Nodes.Cast<TreeNodeMsgEvent>().ToList();

                    var branchEventScenario = eventsScenario.FirstOrDefault(x => x.Event == branchingScenario.BranchingEvent);
                    PropertyEditorBuilder.CreateComboBox("Branching Event", eventsScenario.IndexOf(branchEventScenario), eventsScenario.Select(x => x.Text).ToArray(), delegate (int idx) { branchingScenario.BranchingEvent = eventsScenario[idx].Event; });
                    break;
                case "ChoiceBranching":
                case "ChoiceBranching2":
                case "UnknownBranching":
                    PropertyEditorBuilder.CreateHeader("Choice Branching");
                    var branching = prt.Property as MsgPropBranching;
                    PropertyEditorBuilder.CreateInput("Unk1", branching.Unk1.ToString(), delegate (string val) { branching.Unk1 = byte.Parse(val); });

                    var parentGroup = prt.Parent.Parent as TreeNodeMsgGroup;
                    var events = parentGroup.Nodes.Cast<TreeNodeMsgEvent>().ToList();

                    var branchEvent = events.FirstOrDefault(x => x.Event == branching.BranchingEvent);
                    PropertyEditorBuilder.CreateComboBox("Branching Event", events.IndexOf(branchEvent), events.Select(x => x.Text).ToArray(), delegate (int idx) { branching.BranchingEvent = events[idx].Event; });

                    // PropertyEditorBuilder.CreateInput("Event ID To Branch", branching.BranchEventID.ToString(), delegate (string val) { branching.BranchEventID = byte.Parse(val); });
                    break;

                case "SetSpeaker":
                    PropertyEditorBuilder.CreateHeader("Set Speaker");
                    var setSpeaker = prt.Property as MsgPropSetSpeaker;
                    PropertyEditorBuilder.CreateInput("Speaker Name", setSpeaker.SpeakerName, delegate (string val) { setSpeaker.SpeakerName = val; });
                    PropertyEditorBuilder.CreateInput("Entity UID", setSpeaker.EntityUID.ToString(), delegate (string val) { setSpeaker.EntityUID = int.Parse(val); }, NumberBox.NumberMode.Int);
                    break;

                case "SpeechPause":
                    PropertyEditorBuilder.CreateHeader("Speech Pause");
                    var speechPause = prt.Property as MsgPropSpeechPause;

                    var pauseParentEvent = prt.Parent as TreeNodeMsgEvent;

                    PropertyEditorBuilder.CreateInput("Pause Length", speechPause.PauseLength.ToString(), delegate (string val) { speechPause.PauseLength = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
                    var pausePosBox = PropertyEditorBuilder.CreateInput("Pause Position", speechPause.PausePosition.ToString(), delegate (string val) { speechPause.PausePosition = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);


                    string pauseText = pauseParentEvent.Event.Value;
                    string pausePrefix = "<--(PAUSE)";

                    if (speechPause.PausePosition >= pauseText.Length)
                        pauseText = pauseParentEvent.Event.Value + pausePrefix;
                    else
                        pauseText = pauseText.Insert(speechPause.PausePosition, pausePrefix);

                    var pauseTextEditBox = PropertyEditorBuilder.CreateMultilineInput("Pause", pauseText, delegate (string val)
                    {
                        int pauseIdx = val.IndexOf(pausePrefix);

                        if (pauseIdx < 0)
                            return;

                        pausePosBox.Text = pauseIdx.ToString();
                        speechPause.PausePosition = (ushort)pauseIdx;
                    }, 40);
                    break;

                case "PlayAnimation":
                case "PlayAnimationRelated":
                    PropertyEditorBuilder.CreateHeader("Play Animation");
                    var anim = prt.Property as MsgPropPlayAnimation;

                    PropertyEditorBuilder.CreateInput("Animation", anim.Animation1.ToString(), delegate (string val) { anim.Animation1 = val; });
                    PropertyEditorBuilder.CreateInput("Entity UID", anim.EntityUID.ToString(), delegate (string val) { anim.EntityUID = int.Parse(val); }, NumberBox.NumberMode.Int);
                    break;

                case "PlayAnimationLoop":
                    PropertyEditorBuilder.CreateHeader("Play Looping Animation");
                    var loopAnim = prt.Property as MsgPropPlayAnimationLoop;

                    PropertyEditorBuilder.CreateInput("Animation", loopAnim.Animation1.ToString(), delegate (string val) { loopAnim.Animation1 = val; });
                    PropertyEditorBuilder.CreateInput("Entity UID", loopAnim.EntityUID.ToString(), delegate (string val) { loopAnim.EntityUID = int.Parse(val); }, NumberBox.NumberMode.Int);
                    break;

                case "SetIdleAnimation":
                    PropertyEditorBuilder.CreateHeader("Set Idle Animation");
                    var setIdleAnim = prt.Property as MsgPropSetIdleAnimation;

                    PropertyEditorBuilder.CreateInput("Animation", setIdleAnim.Animation1.ToString(), delegate (string val) { setIdleAnim.Animation1 = val; });
                    PropertyEditorBuilder.CreateInput("Entity UID", setIdleAnim.EntityUID.ToString(), delegate (string val) { setIdleAnim.EntityUID = int.Parse(val); }, NumberBox.NumberMode.Int);
                    break;

                case "PlayActionSetAnimation":
                    PropertyEditorBuilder.CreateHeader("Play Actionset Animation");
                    var casAnim = prt.Property as MsgPropPlayActionSetAnimation;

                    PropertyEditorBuilder.CreateInput("Animation", casAnim.Animation1.ToString(), delegate (string val) { casAnim.Animation1 = val; });
                    PropertyEditorBuilder.CreateInput("Unknown", casAnim.Unk1.ToString(), delegate (string val) { casAnim.Unk1 = byte.Parse(val); }, NumberBox.NumberMode.Byte);
                    PropertyEditorBuilder.CreateInput("Talk Animation", casAnim.TalkSound.ToString(), delegate (string val) { casAnim.TalkSound = short.Parse(val); }, NumberBox.NumberMode.Short);
                    PropertyEditorBuilder.CreateInput("Unknown", casAnim.Unk3.ToString(), delegate (string val) { casAnim.Unk3 = byte.Parse(val); }, NumberBox.NumberMode.Byte);
                    PropertyEditorBuilder.CreateInput("Unknown", casAnim.Unk4.ToString(), delegate (string val) { casAnim.Unk4 = short.Parse(val); }, NumberBox.NumberMode.Short);
                    PropertyEditorBuilder.CreateInput("Entity UID", casAnim.EntityUID.ToString(), delegate (string val) { casAnim.EntityUID = int.Parse(val); }, NumberBox.NumberMode.Int);
                    break;


                case "Reactor":
                    PropertyEditorBuilder.CreateHeader("Reactor");
                    var reactor = prt.Property as MsgPropReactor;

                    PropertyEditorBuilder.CreateInput("Animation", reactor.Reactor, delegate (string val) { reactor.Reactor = val; });
                    PropertyEditorBuilder.CreateInput("Entity UID", reactor.EntityUID.ToString(), delegate (string val) { reactor.EntityUID = int.Parse(val); }, NumberBox.NumberMode.Int);
                    PropertyEditorBuilder.CreateInput("Unknown", reactor.Unk1.ToString(), delegate (string val) { reactor.Unk1 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
                    PropertyEditorBuilder.CreateInput("Unknown", reactor.Unk2.ToString(), delegate (string val) { reactor.Unk2 = int.Parse(val); }, NumberBox.NumberMode.Int);
                    PropertyEditorBuilder.CreateInput("Unknown", reactor.Unk3.ToString(), delegate (string val) { reactor.Unk3 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);

                    break;

                case "Choice":
                    PropertyEditorBuilder.CreateHeader("Choice");
                    var choice = prt.Property as MsgPropChoice;

                    PropertyEditorBuilder.CreateInput("Choice", choice.Choice, delegate (string val) { choice.Choice = val; });
                    PropertyEditorBuilder.CreateInput("Unknown", choice.Unk1.ToString(), delegate (string val) { choice.Unk1 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
                    PropertyEditorBuilder.CreateInput("Unknown", choice.Unk2.ToString(), delegate (string val) { choice.Unk2 = int.Parse(val); }, NumberBox.NumberMode.Int);
                    PropertyEditorBuilder.CreateInput("Unknown", choice.Unk3.ToString(), delegate (string val) { choice.Unk3 = short.Parse(val); }, NumberBox.NumberMode.Short);
                    PropertyEditorBuilder.CreateInput("Unknown", choice.Unk4.ToString(), delegate (string val) { choice.Unk4 = int.Parse(val); }, NumberBox.NumberMode.Int);
                    break;

                case "MoveToEntity":
                    PropertyEditorBuilder.CreateHeader("Move");
                    var move = prt.Property as MsgPropMoveToEntity;

                    PropertyEditorBuilder.CreateInput("Entity UID To Walk To", move.EntityToWalkTo.ToString("x"), delegate (string val) { move.EntityToWalkTo = int.Parse(val, NumberStyles.HexNumber); }, NumberBox.NumberMode.Int, hex: true);
                    PropertyEditorBuilder.CreateInput("Entity UID", move.EntityUID.ToString("x"), delegate (string val) { move.EntityUID = int.Parse(val, NumberStyles.HexNumber); }, NumberBox.NumberMode.Int, hex:true);
                    break;

                case "TeleportToEntity":
                    PropertyEditorBuilder.CreateHeader("Teleport");
                    var teleport = prt.Property as MsgPropTeleportToEntity;

                    PropertyEditorBuilder.CreateInput("Entity UID To Teleport To", teleport.EntityUIDToTeleportTo.ToString("x"), delegate (string val) { teleport.EntityUIDToTeleportTo = int.Parse(val, NumberStyles.HexNumber); }, NumberBox.NumberMode.Int, hex: true);
                    PropertyEditorBuilder.CreateInput("Entity UID", teleport.EntityUID.ToString("x"), delegate (string val) { teleport.EntityUID = int.Parse(val, NumberStyles.HexNumber); }, NumberBox.NumberMode.Int, hex: true);
                    break;

                case "TeleportToMsgCoordinate":
                    var teleportToCoord = prt.Property as MsgPropTeleportToCoordinate;

                    PropertyEditorBuilder.CreateHeader("Teleport (MSG Coordinate)");
                    PropertyEditorBuilder.CreateInput("Unknown 1", teleportToCoord.Unk1.ToString(), delegate (string val) { teleportToCoord.Unk1 = int.Parse(val); }, NumberBox.NumberMode.Int);
                    PropertyEditorBuilder.CreateInput("Unknown 2", teleportToCoord.Unk2.ToString(), delegate (string val) { teleportToCoord.Unk2 = short.Parse(val); }, NumberBox.NumberMode.Short);
                    PropertyEditorBuilder.CreateInput("Coordinate ID", teleportToCoord.CoordinateID.ToString(), delegate (string val) { teleportToCoord.CoordinateID = int.Parse(val); });
                    PropertyEditorBuilder.CreateInput("Entity UID", teleportToCoord.EntityUID.ToString(), delegate (string val) { teleportToCoord.EntityUID = int.Parse(val); });

                    break;

                case "FaceEntity":
                    PropertyEditorBuilder.CreateHeader("Face Target");
                    var faceEntity = prt.Property as MsgPropFaceEntity;

                    PropertyEditorBuilder.CreateInput("Angle Limit? (Rot Y)", faceEntity.Unk1.ToString(), delegate (string val) { faceEntity.Unk1 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
                    PropertyEditorBuilder.CreateInput("Angle", OERotationY.ToAngle(faceEntity.Unk1).ToString(), delegate (string val) { faceEntity.Unk1 = OERotationY.ToOERotation(Utils.InvariantParse(val)); }, NumberBox.NumberMode.Ushort);
                    PropertyEditorBuilder.CreateInput("Unknown", faceEntity.Unk2.ToString(CultureInfo.InvariantCulture), delegate (string val) { faceEntity.Unk2 = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                    PropertyEditorBuilder.CreateInput("Facing Entity UID", faceEntity.Unk3.ToString(), delegate (string val) { faceEntity.Unk3 = int.Parse(val); }, NumberBox.NumberMode.Int);
                    PropertyEditorBuilder.CreateInput("Entity To Face UID", faceEntity.EntityToFaceUID.ToString(), delegate (string val) { faceEntity.EntityToFaceUID = int.Parse(val); }, NumberBox.NumberMode.Int);
                    break;

                case "FaceExpression":
                    PropertyEditorBuilder.CreateHeader("Face Expression");
                    var faceExp = prt.Property as MsgPropFaceExpression;

                    PropertyEditorBuilder.CreateInput("IFA ID", faceExp.IfaID.ToString(), delegate (string val) { faceExp.IfaID = short.Parse(val); }, NumberBox.NumberMode.Ushort);
                    PropertyEditorBuilder.CreateInput("IFA ID 2", faceExp.IfaID2.ToString(), delegate (string val) { faceExp.IfaID2 = short.Parse(val); }, NumberBox.NumberMode.Ushort);

                    PropertyEditorBuilder.CreateInput("Unknown 1", faceExp.Unk1.ToString(), delegate (string val) { faceExp.Unk1 = int.Parse(val); }, NumberBox.NumberMode.Int);
                    PropertyEditorBuilder.CreateInput("Unknown 2", faceExp.Unk2.ToString(), delegate (string val) { faceExp.Unk2 = short.Parse(val); }, NumberBox.NumberMode.Short);

                    PropertyEditorBuilder.CreateInput("Entity UID", faceExp.EntityUID.ToString(), delegate (string val) { faceExp.EntityUID = int.Parse(val); }, NumberBox.NumberMode.Int);
                    break;


                case "CameraParam":
                    PropertyEditorBuilder.CreateHeader("Camera Param");
                    var cParam = prt.Property as MsgPropCameraParam;

                    PropertyEditorBuilder.CreateInput("Flags", cParam.Flags1.ToString(), delegate (string val) { cParam.Flags1 = (MsgPropCameraParam.Flag1)ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
                    PropertyEditorBuilder.CreateInput("Flags", cParam.Flags2.ToString(), delegate (string val) { cParam.Flags2 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
                    PropertyEditorBuilder.CreateInput("Unknown", cParam.Unk1.ToString(), delegate (string val) { cParam.Unk1 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);

                    if (((ushort)cParam.Flags1 & 49152) != 0)
                    {
                        if ((cParam.Flags2 & 25600) != 0)
                        {
                            PropertyEditorBuilder.CreateInput("Entity UID 1", cParam.IntVal1.ToString(), delegate (string val) { cParam.IntVal1 = int.Parse(val); }, NumberBox.NumberMode.Int);
                            PropertyEditorBuilder.CreateInput("Entity UID 2", cParam.IntVal2.ToString(), delegate (string val) { cParam.IntVal2 = int.Parse(val); }, NumberBox.NumberMode.Int);
                        }
                        else
                        {
                            PropertyEditorBuilder.CreateInput("Integer Value 1", cParam.IntVal1.ToString(), delegate (string val) { cParam.IntVal1 = int.Parse(val); }, NumberBox.NumberMode.Int);
                            PropertyEditorBuilder.CreateInput("Integer Value 2", cParam.IntVal2.ToString(), delegate (string val) { cParam.IntVal2 = int.Parse(val); }, NumberBox.NumberMode.Int);
                        }
                    }
                    else
                    {
                        if (cParam.Flags1.HasFlag(MsgPropCameraParam.Flag1.MoveCameraInterpolated) || cParam.Flags1.HasFlag(MsgPropCameraParam.Flag1.MoveCamera))
                        {
                            PropertyEditorBuilder.CreateInput("Lookat Coordinate ID", cParam.IntVal1.ToString(), delegate (string val) { cParam.IntVal1 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
                            PropertyEditorBuilder.CreateInput("Position Coordinate ID", cParam.IntVal2.ToString(), delegate (string val) { cParam.IntVal2 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);


                            var nodePos = (form.nodesTree.Nodes[1].Nodes[cParam.IntVal2] as TreeNodeMsgCoordinate);
                            var nodeLookAt = (form.nodesTree.Nodes[1].Nodes[cParam.IntVal1] as TreeNodeMsgCoordinate); 

                            MsgPositionData posDat = nodePos.Position;
                            MsgPositionData lookDat = nodeLookAt.Position;

                            PropertyEditorBuilder.CreateInput("Position X", posDat.Position.x.ToString(CultureInfo.InvariantCulture), delegate (string val) { posDat.Position.x = Utils.InvariantParse(val); nodePos.Position = posDat; }, NumberBox.NumberMode.Float);
                            PropertyEditorBuilder.CreateInput("Position Y", posDat.Position.y.ToString(CultureInfo.InvariantCulture), delegate (string val) { posDat.Position.y = Utils.InvariantParse(val); nodePos.Position = posDat; }, NumberBox.NumberMode.Float);
                            PropertyEditorBuilder.CreateInput("Position Z", posDat.Position.z.ToString(CultureInfo.InvariantCulture), delegate (string val) { posDat.Position.z = Utils.InvariantParse(val); nodePos.Position = posDat; }, NumberBox.NumberMode.Float);

                            PropertyEditorBuilder.CreateSpace(10);

                            PropertyEditorBuilder.CreateInput("Lookat X", lookDat.Position.x.ToString(CultureInfo.InvariantCulture), delegate (string val) { lookDat.Position.x = Utils.InvariantParse(val); nodeLookAt.Position = lookDat; }, NumberBox.NumberMode.Float);
                            PropertyEditorBuilder.CreateInput("Lookat Y", lookDat.Position.y.ToString(CultureInfo.InvariantCulture), delegate (string val) { lookDat.Position.y = Utils.InvariantParse(val); nodeLookAt.Position = lookDat; }, NumberBox.NumberMode.Float);
                            PropertyEditorBuilder.CreateInput("Lookat Z", lookDat.Position.z.ToString(CultureInfo.InvariantCulture), delegate (string val) { lookDat.Position.z = Utils.InvariantParse(val); nodeLookAt.Position = lookDat; }, NumberBox.NumberMode.Float);
                        }
                        else
                        {
                            PropertyEditorBuilder.CreateInput("UShort Value 1", cParam.IntVal1.ToString(), delegate (string val) { cParam.IntVal1 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
                            PropertyEditorBuilder.CreateInput("UShort Value 2", cParam.IntVal2.ToString(), delegate (string val) { cParam.IntVal2 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
                        }
                    }


                    if (cParam.Flags1.HasFlag(MsgPropCameraParam.Flag1.MoveCameraInterpolated))
                    {
                        PropertyEditorBuilder.CreateInput("Interpolation Duration", cParam.FloatVal1.ToString(CultureInfo.InvariantCulture), delegate (string val) { cParam.FloatVal1 = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    }

                    break;

                case "SetWindowType":
                    PropertyEditorBuilder.CreateHeader("Set Talk Window Type");
                    var setWindowType = prt.Property as MsgPropSetWindowType;

                    PropertyEditorBuilder.CreateInput("Window Type", setWindowType.WindowType.ToString(), delegate (string val) { setWindowType.WindowType = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
                    break;

                case "ScreenFadeIn":
                    PropertyEditorBuilder.CreateHeader("Screen Fade In");
                    var fadeIn = prt.Property as MsgPropScreenFadeIn;

                    PropertyEditorBuilder.CreateInput("Time (Frames)", fadeIn.Time.ToString(CultureInfo.InvariantCulture), delegate (string val) { fadeIn.Time = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    break;
                case "ScreenFadeOut":
                    PropertyEditorBuilder.CreateHeader("Screen Fade Out");
                    var fadeOut = prt.Property as MsgPropScreenFadeOut;

                    PropertyEditorBuilder.CreateInput("Time (Frames)", fadeOut.Time.ToString(CultureInfo.InvariantCulture), delegate (string val) { fadeOut.Time = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    break;

                case "SetVisibility":
                    PropertyEditorBuilder.CreateHeader("Set Invisible");
                    var setInvis = prt.Property as MsgPropSetVisibility;

                    PropertyEditorBuilder.CreateInput("Invisible", setInvis.Invisible.ToString(), delegate (string val) { setInvis.Invisible = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
                    PropertyEditorBuilder.CreateInput("Entity UID", setInvis.EntityUID.ToString(), delegate (string val) { setInvis.EntityUID = int.Parse(val); }, NumberBox.NumberMode.Int);
                    break;

                case "PlayBgm":
                    PropertyEditorBuilder.CreateHeader("Play BGM");
                    var playBgm = prt.Property as MsgPropPlayBGM;

                    PropertyEditorBuilder.CreateInput("BGM", playBgm.BGM, delegate (string val) { playBgm.BGM = val; });
                    PropertyEditorBuilder.CreateInput("Unknown 1", playBgm.Unk1.ToString(), delegate (string val) { playBgm.Unk1 = int.Parse(val); }, NumberBox.NumberMode.Int);
                    PropertyEditorBuilder.CreateInput("Unknown 2", playBgm.Unk2.ToString(), delegate (string val) { playBgm.Unk2 = int.Parse(val); }, NumberBox.NumberMode.Int);
                    PropertyEditorBuilder.CreateInput("Unknown 3", playBgm.Unk3.ToString(CultureInfo.InvariantCulture), delegate (string val) { playBgm.Unk3 = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    break;

                case "CharacterSpeech":
                    PropertyEditorBuilder.CreateHeader("Character Speech");
                    var charaSpeech = prt.Property as MsgPropCharacterSpeech;

                    PropertyEditorBuilder.CreateInput("Unknown", charaSpeech.Unknown.ToString(), delegate (string val) { charaSpeech.Unknown = int.Parse(val); });
                    PropertyEditorBuilder.CreateInput("Unknown", charaSpeech.Unknown2.ToString(), delegate (string val) { charaSpeech.Unknown2 = short.Parse(val); });
                    PropertyEditorBuilder.CreateInput("Speech", charaSpeech.Speech, delegate (string val) { charaSpeech.Speech = val; });
                    PropertyEditorBuilder.CreateInput("Entity UID", charaSpeech.EntityUID.ToString(), delegate (string val) { charaSpeech.EntityUID = int.Parse(val); }, NumberBox.NumberMode.Int);
                    break;

                case "PlayDialogueSound":
                    PropertyEditorBuilder.CreateHeader("Dialogue Sound");
                    var diagSound = prt.Property as MsgPropPlayDialogueSound;

                    PropertyEditorBuilder.CreateInput("Unknown 1", diagSound.Unk1.ToString(), delegate (string val) { diagSound.Unk1 = byte.Parse(val); }, NumberBox.NumberMode.Byte);
                    PropertyEditorBuilder.CreateInput("Sound", diagSound.Sound, delegate (string val) { diagSound.Sound = val; });

                    break;

                case "UnknownPropWithString1":
                    PropertyEditorBuilder.CreateHeader("Unknown Prop");
                    var unkPropStr1 = prt.Property as MsgPropUnknownPropWString;

                    PropertyEditorBuilder.CreateInput("Unknown 1", unkPropStr1.Unk1.ToString(), delegate (string val) { unkPropStr1.Unk1 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
                    PropertyEditorBuilder.CreateInput("Unknown 2", unkPropStr1.Unk2.ToString(), delegate (string val) { unkPropStr1.Unk2 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
                    PropertyEditorBuilder.CreateInput("Unknown 3", unkPropStr1.Unk3.ToString(), delegate (string val) { unkPropStr1.Unk3 = int.Parse(val); }, NumberBox.NumberMode.Int);

                    PropertyEditorBuilder.CreateInput("Unknown String", unkPropStr1.UnknownString.ToString(), delegate (string val) { unkPropStr1.UnknownString = val; });
                    break;

                case "UnknownPropWithString2":
                    PropertyEditorBuilder.CreateHeader("Unknown Prop");
                    var unkPropStr2 = prt.Property as MsgPropUnknownPropWString2;

                    PropertyEditorBuilder.CreateInput("Unknown 1", unkPropStr2.Unk1.ToString(), delegate (string val) { unkPropStr2.Unk1 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
                    PropertyEditorBuilder.CreateInput("Unknown 2", unkPropStr2.Unk2.ToString(), delegate (string val) { unkPropStr2.Unk2 = int.Parse(val); }, NumberBox.NumberMode.Int);

                    PropertyEditorBuilder.CreateInput("Unknown String", unkPropStr2.UnknownString.ToString(), delegate (string val) { unkPropStr2.UnknownString = val; });
                    break;

                case "PlayHAct":
                    PropertyEditorBuilder.CreateHeader("Play HAct");
                    var playHAct = prt.Property as MsgPropPlayHAct;

                    PropertyEditorBuilder.CreateInput("HAct", playHAct.HAct, delegate(string val) { playHAct.HAct = val; });
                    PropertyEditorBuilder.CreateInput("Unknown 1", playHAct.Unk1.ToString(), delegate (string val) { playHAct.Unk1 = short.Parse(val); }, NumberBox.NumberMode.Short);
                    PropertyEditorBuilder.CreateInput("Unknown 2", playHAct.Unk2.ToString(), delegate (string val) { playHAct.Unk2 = short.Parse(val); }, NumberBox.NumberMode.Int);
                    PropertyEditorBuilder.CreateInput("Unknown 3", playHAct.Unk3.ToString(), delegate (string val) { playHAct.Unk3 = short.Parse(val); }, NumberBox.NumberMode.Short);
                    break;

            }
        }

        public static void DrawCoordinate(Form1 form, TreeNodeMsgCoordinate coord)
        {
            PropertyEditorBuilder.CreateHeader("Coordinate");

            PropertyEditorBuilder.CreateInput("X", coord.Position.Position.x.ToString(CultureInfo.InvariantCulture), delegate (string val) { coord.Position.Position.x = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Y", coord.Position.Position.y.ToString(CultureInfo.InvariantCulture), delegate (string val) { coord.Position.Position.y = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Z", coord.Position.Position.z.ToString(CultureInfo.InvariantCulture), delegate (string val) { coord.Position.Position.z = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            PropertyEditorBuilder.CreateInput("Angle", OERotationY.ToAngle((ushort)coord.Position.Angle).ToString(), delegate (string val) { coord.Position.Angle = OERotationY.ToOERotation(Utils.InvariantParse(val)); }, NumberBox.NumberMode.Ushort);
            PropertyEditorBuilder.CreateInput("Angle (Rot Y)", coord.Position.Angle.ToString(), delegate (string val) { coord.Position.Angle = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
        }
    }
}
