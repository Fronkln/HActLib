using System;
using System.Linq;
using System.Collections.Generic;
using Yarhl.FileFormat;
using Yarhl.IO;
using HActLib.Internal;

namespace HActLib
{
    public class DEToOEConversionInfo
    {
        public CMN Cmn;
        public uint TargetVer;
    }

    public class DE2OECmn : IConverter<DEToOEConversionInfo, OECMN>
    {
        public OECMN Convert(DEToOEConversionInfo inf)
        {
            CMN deCmn = inf.Cmn;

            Game oeGame;

            switch(inf.TargetVer)
            {
                default:
                    oeGame = Game.Y0;
                    break;
                case 10:
                    oeGame = Game.Y5;
                    break;
                case 15:
                    oeGame = Game.Ishin;
                    break;
            }

            OECMN converted = new OECMN();
            converted.CMNHeader.Version = inf.TargetVer;

            ConvertHeader(deCmn, converted);
            converted.Root = Convert(deCmn.Root, deCmn.GameVersion, CMN.LastHActDEGame, oeGame);


            return converted;
        }


        private static void ConvertHeader(CMN deCMN, OECMN converted)
        {
            converted.CMNHeader.NodeDrawNum = deCMN.Header.NodeDrawNum;
            converted.CMNHeader.Start = deCMN.Header.Start;
            converted.CMNHeader.End = deCMN.Header.End;

            converted.CutInfo = deCMN.CutInfo;
            converted.ResourceCutInfo = deCMN.ResourceCutInfo;
            converted.DisableFrameInfo = deCMN.DisableFrameInfo;

            converted.CMNHeader.ChainCameraIn = -1;
            converted.CMNHeader.ChainCameraOut = -1;
        }

        private static Node Convert(Node deNode, GameVersion deVer, Game game, Game oeGame)
        {
            //Come, my child. Let's venture to Old Engine!
            List<Node> convertedChild = new List<Node>();
            Node oeNode = null;

            switch (deNode.Category)
            {
                case AuthNodeCategory.Path:
                    oeNode = new NodePathBase { Matrix = (deNode as NodePathBase).Matrix};
                    break;

                case AuthNodeCategory.Model_node:
                    oeNode = deNode; //No conversion needed
                    break;

                case AuthNodeCategory.Camera:
                    oeNode = deNode; //No conversion needed
                    break;

                case AuthNodeCategory.Character:
                    DENodeCharacter deCharacter = (DENodeCharacter)deNode;
                    OENodeCharacter oeCharacter = new OENodeCharacter();
                    
                    //temp
                    oeCharacter.Height = 185;
                    oeNode = oeCharacter;

                    break;

                case AuthNodeCategory.CameraMotion:
                    oeNode = deNode; //No conversion needed
                    break;

                case AuthNodeCategory.CharacterMotion:
                    NodeMotionBase characterMot = new NodeMotionBase();

                    DENodeCharacterMotion deNodeCharacterMotion = deNode as DENodeCharacterMotion;

                    characterMot.Start.Frame = new GameTick(deNodeCharacterMotion.Start).Frame;
                    characterMot.End.Frame = new GameTick(deNodeCharacterMotion.End).Frame;
                    characterMot.Flags = deNodeCharacterMotion.Flags;

                    oeNode = characterMot;

                    break;

                case AuthNodeCategory.Element:
                    NodeElement deElementNode = deNode as NodeElement;

                    oeNode = TryConvertElement(deElementNode, deVer, game, oeGame);
                    break;
            }

            if (oeNode != null)
            {
                Node.CopyBaseInfo(deNode, oeNode);

                if (oeNode is NodeElement)
                {
                    NodeElement elem = oeNode as NodeElement;
                    Node.CopyBaseElementInfo(deNode as NodeElement, elem);
                }
                else if(oeNode is OENodeCharacter)
                {
                    string convName = GetReplaceName((deNode as DENodeCharacter).ReplaceID);
                    oeNode.Name = (string.IsNullOrEmpty(convName) ? deNode.Name : convName);
                   
                }

                if(deNode.Children.Count > 0)
                    foreach (Node child in deNode.Children)
                        convertedChild.Add(Convert(child, deVer, game, oeGame));

                oeNode.Children = convertedChild.Where(x => x != null).ToList();
            }

            return oeNode;
        }
        private static Node TryConvertElement(NodeElement node, GameVersion deVer, Game game, Game oeGame)
        {
            string deNodeName = Reflection.GetElementNameByID(node.ElementKind, game);

            switch(deNodeName)
            {
                case "e_auth_element_battle_damage":
                    NodeBattleDamage deDmg = node as NodeBattleDamage;
                    OEDamage oeDmg = new OEDamage();
                    oeDmg.Damage = (int)deDmg.Damage;
                    oeDmg.ElementKind = Reflection.GetElementIDByName("e_auth_element_damage", oeGame);
                    return oeDmg;
                case "e_auth_element_battle_heat":
                    DEElementBattleHeat deHeat = node as DEElementBattleHeat;
                    OEHeat oeHeat = new OEHeat();
                    oeHeat.HeatChange = deHeat.HeatChange;
                    oeHeat.ElementKind = Reflection.GetElementIDByName("e_auth_element_heat_change", oeGame);
                    return oeHeat;
                case "e_auth_element_hact_input":
                    DEHActInput deInput = node as DEHActInput;
                    OEHActInput oeInput = new OEHActInput();
                    oeInput.Timing = deInput.DecideTick;
                    oeInput.ElementKind = Reflection.GetElementIDByName("e_auth_element_hact_input", oeGame);
                    return oeInput;
                case "e_auth_element_hact_input_barrage":
                    DEHActInputBarrage deInputBarrage = node as DEHActInputBarrage;
                    OEHActInputBarrage oeInputBarrage = new OEHActInputBarrage();

                    oeInputBarrage.Presses = deInputBarrage.BarrageCount;
                    oeInputBarrage.ElementKind = Reflection.GetElementIDByName("e_auth_element_hact_input_barrage", oeGame);
                    return oeInputBarrage;
                case "e_auth_element_se":
                    DEElementSE deSE = node as DEElementSE;
                    OEElementSE oeSE = new OEElementSE();

                    oeSE.ElementKind = Reflection.GetElementIDByName("e_auth_element_sound", oeGame);
                    if (deSE.SoundIndex != 0)
                        oeSE.Sound = (ushort)(deSE.SoundIndex - 1);
                    else
                        oeSE.Sound = 0;

                    oeSE.CustomDecayNearDist = deSE.CustomDecayNearDist;
                    oeSE.CustomDecayNearVol = deSE.CustomDecayNearVol;
                    oeSE.CustomDecayFarDist = deSE.CustomDecayFarDist;
                    oeSE.CustomDecayFarVol = deSE.CustomDecayFarVol;

                    oeSE.Volume = deSE.Volume;

                    //GV Fighter
                    if (deSE.CueSheet == 36 || deSE.CueSheet == 49)
                    {
                        if (CMN.LastHActDEGame >= Game.Ishin)
                            oeSE.Cuesheet = 32828;
                        else
                            oeSE.Cuesheet = 32788;

                        switch(deSE.SoundIndex)
                        {
                            case 5:
                                oeSE.Sound = 8;
                                break;
                        }

                        switch(deSE.Name.ToLowerInvariant())
                        {
                            case "gv_fighter_throw":
                                oeSE.Sound = 4;
                                break;
                            case "gv_fighter_attack_xs":
                                oeSE.Sound = 3;
                                break;
                            case "gv_fighter_damage_head_s":
                                oeSE.Sound = 5;
                                break;
                            case "gv_fighter_damage_body_s":
                                oeSE.Sound = 6;
                                break;
                            case "gv_fighter_damage_head_l":
                                oeSE.Sound = 7;
                                break;
                        }
                    }
                    else
                        oeSE.Cuesheet = 0; //invalid/unknown

                    return oeSE;
            }

            return null;
        }

        public static string GetReplaceName(HActReplaceID replaceID)
        {
            if (replaceID == HActReplaceID.hu_player || replaceID == HActReplaceID.hu_player1)
                return "ZA_HUPLAYER";

            string[] names = Enum.GetNames(typeof(HActReplaceID));
            string deName = names[(int)replaceID];
            string[] deNameSplit = deName.Split('_');

            if (deName.Contains("_enemy"))
                return "ZA_HUENEMY" + int.Parse(deNameSplit[2]);


            return null;
        }


        //ELEMENTS

        private static Node ConvertHActSE(OEElementSE oeSE, Game game)
        {
            DEFighterSound GetFighterSoundCategory(ushort category)
            {
                
                switch((OEFighterSound)category)
                {
                    default:
                        return DEFighterSound.damage_head_s;

                    case OEFighterSound.Death:
                        return DEFighterSound.dead_body;

                    case OEFighterSound.Light:
                        return DEFighterSound.attack_s;

                    case OEFighterSound.Heavy:
                        return DEFighterSound.attack_l;

                    case OEFighterSound.LightHeavy:
                        return DEFighterSound.attack_s;

                    case OEFighterSound.LightHit2:
                        return DEFighterSound.damage_head_s;

                    case OEFighterSound.HeavyHit:
                        return DEFighterSound.damage_xl;
                    case OEFighterSound.LargeHeadHit:
                        return DEFighterSound.damage_head_l;
                    case OEFighterSound.TiredHeavyHit:
                        return DEFighterSound.damage_xl;
         
                }
            }


            if (!oeSE.IsFighterSound())
                return null;


            DEElementSE se = new DEElementSE();
            se.ElementKind = Reflection.GetElementIDByName("e_auth_element_se", game);
            se.SEVer = 1;
            se.SoundIndex = (byte)GetFighterSoundCategory(oeSE.Sound);
            se.Unk = 128;
            se.CueSheet = 49;
            se.Flags = oeSE.Flags;
            se.CustomDecayNearDist = oeSE.CustomDecayNearDist;
            se.CustomDecayFarDist = oeSE.CustomDecayFarDist;
            se.CustomDecayFarVol = oeSE.CustomDecayFarVol;
            se.CustomDecayNearVol = oeSE.CustomDecayNearVol;

            Console.WriteLine("converted se");
           

            return se;
        }

        private static Node ConvertHActInput(OEHActInput oeInput, Game game)
        {
            DEHActInput input = new DEHActInput();

            input.DecideTick = oeInput.Timing;
            //  input.CondFlagNo = 3;  //idk
            input.InputID = 1; //random input
            input.ElementKind = Reflection.GetElementIDByName("e_auth_element_hact_input", game);

            return input;
        }

        private static Node ConvertHActBarrage(OEHActInputBarrage oeBarrage, Game game)
        {
            DEHActInputBarrage barrage = new DEHActInputBarrage();

            barrage.DecideTick = new GameTick(oeBarrage.End).Tick - new GameTick(oeBarrage.Start).Tick;
            barrage.BarrageCount = oeBarrage.Presses;
            // barrage.CondFlagNo = 3;  //heuteristic
            barrage.InputID = 3; //random input
            barrage.ElementKind = Reflection.GetElementIDByName("e_auth_element_hact_input_barrage", game);

            return barrage;
        }
    }
}