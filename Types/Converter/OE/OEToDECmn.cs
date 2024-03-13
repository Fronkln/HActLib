using System;
using System.Linq;
using System.Collections.Generic;
using Yarhl.FileFormat;
using Yarhl.IO;
using HActLib.Internal;
using System.Reflection.Metadata;
using Yarhl.FileSystem;
using HActLib.OOE;



namespace HActLib
{
    public class OE2DECmn : IConverter<OECMN, CMN>
    {
        public CMN Convert(OECMN oeCMN)
        {
            CMN converted = new CMN();
            converted.GameVersion = CMN.LastGameVersion;

            Game oeGame;

            switch(oeCMN.CMNHeader.Version)
            {
                default:
                    oeGame = Game.Y0;
                    break;
                case 10:
                    oeGame = Game.Y5;
                    break;
                case 16:
                    oeGame = Game.Y0;
                    break;
            }

            ConvertHeader(oeCMN, converted);
            converted.Root = Convert(oeCMN.Root, oeCMN.CMNHeader.Version, CMN.LastHActDEGame, oeGame);
            converted.AuthPages = GeneratePages(oeCMN);

            return converted;
        }


        private static void ConvertHeader(OECMN oeCMN, CMN converted)
        {
            converted.Header = new CMNHeader();

            converted.Header.Version = 18;
            converted.Header.Type = 1;
            converted.Header.Flags = 2;
            converted.Header.NodeDrawNum = oeCMN.CMNHeader.NodeDrawNum;
            converted.Header.Start = oeCMN.CMNHeader.Start;
            converted.Header.End = oeCMN.CMNHeader.End;

            converted.CutInfo = oeCMN.CutInfo;
            converted.ResourceCutInfo = oeCMN.ResourceCutInfo;
            converted.DisableFrameInfo = oeCMN.DisableFrameInfo;

            converted.Header.ChainCameraIn = -1;
            converted.Header.ChainCameraOut = -1;

        }

        public static Node Convert(Node oeNode, uint version, Game game, Game oeGame)
        {
            //Come, my child. Let's venture to Dragon Engine!
            List<Node> convertedChild = new List<Node>();
            Node deNode = null;

            switch (oeNode.Category)
            {
                case AuthNodeCategory.Path:
                    deNode = new DENodePath() { Matrix = (oeNode as NodePathBase).Matrix, PathFlags = 0 };
                    break;

                case AuthNodeCategory.Model_node:
                    deNode = oeNode; //No conversion needed
                    break;

                case AuthNodeCategory.Camera:
                    deNode = oeNode; //No conversion needed
                    break;

                case AuthNodeCategory.Character:
                    OENodeCharacter oeCharacter = (OENodeCharacter)oeNode;
                    DENodeCharacter deCharacter = new DENodeCharacter();

                    //try to find the height in the scale list
                    string scaleLookup = "height_" + oeCharacter.Height.ToString();
                    DECharacterScaleID scale;
                    try
                    {
                        scale = (DECharacterScaleID)Enum.Parse(typeof(DECharacterScaleID), Enum.GetNames<DECharacterScaleID>().FirstOrDefault(x => x == scaleLookup));
                    }
                    catch
                    {
                        scale = DECharacterScaleID.invalid;
                    }
                    deCharacter.CharacterID = 1; //dummy if not replaced
                    deCharacter.ScaleID = (uint)scale;
                    deCharacter.ReplaceID = GetReplaceIDFromName(oeNode.Name);

                    deNode = deCharacter;


                    break;

                case AuthNodeCategory.CameraMotion:
                    deNode = oeNode; //No conversion needed
                    break;

                case AuthNodeCategory.CharacterMotion:
                    NodeMotionBase characterMot = (NodeMotionBase)oeNode;

                    DENodeCharacterMotion deNodeCharacterMotion = new DENodeCharacterMotion();

                    //i get the idea this might cause funky stuff
                    deNodeCharacterMotion.Flags = characterMot.Flags;
                    deNodeCharacterMotion.Start = new GameTick(characterMot.Start);
                    deNodeCharacterMotion.End = new GameTick(characterMot.End);
                    deNodeCharacterMotion.MotionTick = new GameTick(0);
                    deNodeCharacterMotion.PreviousMotionMatrix = Matrix4x4.Default;

                    deNode = deNodeCharacterMotion;

                    break;

                case AuthNodeCategory.ModelMotion:
                    deNode = oeNode;
                    break;

                case AuthNodeCategory.Element:
                    deNode = TryConvertElement(oeNode as NodeElement, version, game, oeGame);
                    break;
                case AuthNodeCategory.Asset:
                    NodeAsset deAsset = new NodeAsset();
                    oeNode.Category = AuthNodeCategory.Asset;
                    deAsset.AssetID = 1;
                    deNode = deAsset;
                    break;

                case AuthNodeCategory.ModelCustom:
                    goto case AuthNodeCategory.Asset;
            }

            if (deNode != null)
            {
                Node.CopyBaseInfo(oeNode, deNode);

                if (deNode is NodeElement)
                {
                    NodeElement elem = deNode as NodeElement;
                    Node.CopyBaseElementInfo(oeNode as NodeElement, elem);

                   // elem.PlayType = ElementPlayType.Always;
                    elem.ElementFlag = 0;
                   // elem.UpdateTimingMode = 2;
                }

                if(oeNode.Children.Count > 0)
                    foreach (Node child in oeNode.Children)
                        convertedChild.Add(Convert(child, version, game, oeGame));

                deNode.Children = convertedChild.Where(x => x != null).ToList();
            }

            return deNode;
        }


        public List<AuthPage> GeneratePages(OECMN cmn)
        {
            try
            {
                //We are gonna treat every button sequence as the start of a new page.
                IEnumerable<NodeElement> buttonNodes = cmn.AllElements.Where(x => x.ElementKind == 0x22 || x.ElementKind == 0x2C).Cast<NodeElement>().OrderBy(x => x.Start);
                IEnumerable<NodeElement> endNodes = cmn.AllElements.Where(x => x.ElementKind == 37).Cast<NodeElement>().OrderBy(x => x.Start);
                IEnumerable<NodeElement> branchNodes = cmn.AllElements.Where(x => x.ElementKind == 38).Cast<NodeElement>().OrderBy(x => x.Start);

                if (branchNodes.Count() <= 0 || buttonNodes.Count() <= 0)
                    return new List<AuthPage>();

                List<AuthPage> pages = new List<AuthPage>();
                AuthPage startPage = new AuthPage("START", 0, buttonNodes.ElementAt(0).Start - 1);

                pages.Add(startPage);

                int pageIdx = 2;

                NodeElement GetNearestEndNodeInRange(float start)
                {
                    NodeElement endNode = endNodes.FirstOrDefault(x => x.Start >= start);
                    return endNode;
                }

                for (int i = 0; i < branchNodes.Count(); i++)
                {
                    NodeElement branch = branchNodes.ElementAt(i);

                    AuthPage successPage = new AuthPage("SUCCESS " + i);
                    AuthPage failPage = new AuthPage("FAIL " + i);

                    successPage.Start.Frame = branch.Start;
                    successPage.PageIndex = pageIdx;

                    failPage.Start.Frame = branch.End;
                    failPage.PageIndex = pageIdx + 1;

                    NodeElement failEnd = GetNearestEndNodeInRange(branch.End);

                    if (i == branchNodes.Count() - 1)
                    {
                        successPage.End.Frame = GetNearestEndNodeInRange(successPage.Start).Start;
                    }
                    else
                    {
                        successPage.End.Frame = branchNodes.ElementAt(i + 1).Start - 1;
                        successPage.Transitions.Add(new Transition(pageIdx + 2, new ConditionHActFlag(1, 0), new ConditionPageEnd()));
                        successPage.Transitions.Add(new Transition(pageIdx + 3, new ConditionHActFlag(0, 1), new ConditionPageEnd()));
                    }

                    if (failEnd != null)
                        failPage.End.Frame = failEnd.Start;
                    else
                    {
                        if (i == branchNodes.Count() - 1)
                            failPage.End.Frame = cmn.CMNHeader.End;
                        else
                            failPage.End.Frame = branchNodes.ElementAt(i + 1).Start - 1;
                        // throw new Exception("Don't know how to calculate page end");
                    }

                    pages.Add(successPage);
                    pages.Add(failPage);

                    pageIdx += 2;
                }


                startPage.Transitions.Add(new Transition(1, new ConditionPageEnd()));

                AuthPage promptPage = new AuthPage("PROMPT START", buttonNodes.ElementAt(0).Start, branchNodes.ElementAt(0).Start - 1);
                promptPage.PageIndex = 1;
                promptPage.Transitions.Add(new Transition(2, new ConditionHActFlag(1, 0), new ConditionPageEnd()));
                promptPage.Transitions.Add(new Transition(3, new ConditionHActFlag(0, 1), new ConditionPageEnd()));

                pages.Insert(1, promptPage);

                //Fix potentially broken pages that have no transitions
                foreach (AuthPage page in pages)
                {
                    if (page.Transitions.Count <= 0)
                    {
                        page.Transitions.Add(new Transition(-1, new ConditionPageEnd()));
                    }
                }

                return pages;
            }
            catch
            {
                return new List<AuthPage>();
            }
        }

        private static HActReplaceID GetReplaceIDFromName(string name)
        {

            name = name.Split(new[] { '\0' }, 2)[0].ToLower().Replace("za_", "hu_").Replace("_hu", "_");

            if (name.ToLower() == "kiryu")
                return HActReplaceID.hu_player;

            if (name.StartsWith("hu_npc"))
            {
                int idx = name.IndexOf("npc") + 3;
                int npcID = int.Parse(name.Substring(idx, name.Length - idx));

                name = string.Format("hu_npc_{0:00}", npcID);
            }
            else
            if (name.StartsWith("hu_enemy"))
            {
                int idx = name.IndexOf("enemy") + 5;
                int enemyID = int.Parse(name.Substring(idx, name.Length - idx));

                name = string.Format("hu_enemy_{0:00}", enemyID);
            }

            if (Enum.IsDefined(typeof(HActReplaceID), name))
                return (HActReplaceID)Enum.Parse(typeof(HActReplaceID), name);

            return HActReplaceID.invalid;
        }
        private static Node TryConvertElement(NodeElement node, uint version, Game game, Game oeGame)
        {

            string elemName = Reflection.GetElementNameByID(node.ElementKind, oeGame);

            switch (elemName)
            {
                case "e_auth_element_fade":
                    return ConvertFade(node as OEFade, game);
                case "e_auth_element_damage":
                    return ConvertDamage(node as OEDamage, game);
                case "e_auth_element_noise":
                    return ConvertNoise(node as OENoise, game);
                case "e_auth_element_sound":
                    return ConvertHActSE(node as OEElementSE, game);
                case "e_auth_element_draw_off":
                    return ConvertDrawOff(node, game);
                //Input
                case "e_auth_element_hact_input":
                    return ConvertHActInput(node as OEHActInput, game);
                //Input barrage
                case "e_auth_element_hact_input_barrage":
                    return ConvertHActBarrage(node as OEHActInputBarrage, game);

                case "e_auth_element_path_offset":
                    return ConvertPathAdjustment(node as OEPathAdjustment, game);
                case "e_auth_element_face_expression":
                    return ConvertFaceExpression(node as OEFaceExpression, game);

                case "e_auth_element_picture":
                    return ConvertScreenPicture(node as OENodePicture, game);

                case "e_auth_element_particle":
                    return ConvertParticle(node as OEParticle, game);
            }

            return null;
        }


        private static Node ConvertParticle(OEParticle ptc, Game game)
        {
            DEElementParticle dePtc = new DEElementParticle();
            dePtc.ElementKind = Reflection.GetElementIDByName("e_auth_element_particle", game);

            dePtc.Animation = ptc.Animation;
            dePtc.Color = ptc.Color;
            dePtc.Matrix = ptc.Matrix;
            dePtc.Scale = ptc.Scale;
            dePtc.ParticleID = ptc.ParticleID;
            dePtc.Name = "PTC " + dePtc.ParticleID;
            dePtc.ParticleName = "AAa0000";

            return dePtc;
        }

        //ELEMENTS

        private static NodeBattleDamage ConvertDamage(OEDamage oeDamage, Game game)
        {
            NodeBattleDamage damage = new NodeBattleDamage();
            damage.Damage = (uint)oeDamage.Damage;
            damage.NoDead = oeDamage.NoDead;
            damage.ElementKind = Reflection.GetElementIDByName("e_auth_element_battle_damage", game);

            return damage;
        }


        private static Node ConvertNoise(OENoise oeNoise, Game game)
        {
            if (CMN.GetVersionForGame(game) < GameVersion.DE2)
                return null;

            DEElementGrainNoise noise = new DEElementGrainNoise();
            noise.ElementKind = Reflection.GetElementIDByName("e_auth_element_grain_noise", game);

            //There isnt a proper way to programmatically convert OE values to DE
            noise.Power = 3;
            noise.Size = 0.1f;
            noise.Intensity = 1;
          //  noise.Power = oeNoise.Power;
           // noise.Intensity = oeNoise.Intensity;
           // noise.Size = oeNoise.Size;
           // noise.Animation = oeNoise.Animation;

            return noise;
        }

        private static DEElementUIFade ConvertFade(OEFade oeFade, Game game)
        {
            DEElementUIFade deFade = new DEElementUIFade();
            deFade.ElementKind = Reflection.GetElementIDByName("e_auth_element_ui_fade", game);
            deFade.Color = oeFade.Color;

            for (int i = 0; i < 32; i++)
                deFade.Animation[i] = oeFade.Animation[i] / 255f;

            return deFade;
        }

        private static Node ConvertDrawOff(Node drawOff, Game game)
        {
            DEElementDrawOff deDrawOff = new DEElementDrawOff();
            deDrawOff.ElementKind = Reflection.GetElementIDByName("e_auth_element_play_draw_off", game);

            return deDrawOff;
        }

        private static Node ConvertScreenPicture(OENodePicture pic, Game game)
        {
            DEElementUITexture dePic = new DEElementUITexture();
            dePic.ElementKind = Reflection.GetElementIDByName("e_auth_element_ui_texture", game);
            dePic.TextureName = pic.PictureName;
            dePic.BeforeCenter.X = pic.BeforeCenterX;
            dePic.BeforeCenter.Y = pic.BeforeCenterY;
            dePic.AfterCenter.X = pic.AfterCenterX;
            dePic.AfterCenter.Y = pic.AfterCenterY;
           // dePic.BeforeScale.X = pic.BeforeSizeX;
           // dePic.BeforeScale.Y = pic.BeforeSizeY;
           // dePic.AfterScale.X = pic.AfterSizeX;
           // dePic.AfterScale.Y = pic.AfterSizeY;

            for (int i = 0; i < 32; i++)
                dePic.Animation[i] = pic.Animation[i] / 255f;

            return dePic;
        }

        private static Node ConvertPathAdjustment(OEPathAdjustment path, Game game)
        {
            DEElementPathOffset offs = new DEElementPathOffset();
            offs.ElementKind = Reflection.GetElementIDByName("e_auth_element_path_offset", game);
            offs.Matrix = path.Adjustment;

            return offs;
        }

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

            DEElementSE se = new DEElementSE();
            se.ElementKind = Reflection.GetElementIDByName("e_auth_element_se", game);
            se.SEVer = 1;
            se.SoundIndex = (byte)oeSE.Sound;
            se.CueSheet = oeSE.Cuesheet;
            se.Flags = oeSE.Flags;
            se.CustomDecayNearDist = oeSE.CustomDecayNearDist;
            se.CustomDecayFarDist = oeSE.CustomDecayFarDist;
            se.CustomDecayFarVol = oeSE.CustomDecayFarVol;
            se.CustomDecayNearVol = oeSE.CustomDecayNearVol;

            if (oeSE.IsFighterSound())
            {
                se.SoundIndex = (byte)GetFighterSoundCategory(oeSE.Sound);
                se.Unk = 128;

                se.CueSheet = (ushort)(CMN.GetVersionForGame(game) >= GameVersion.DE2 ? 49 : 36);
            }
            else
            {
                se.CueSheet = 0;
                se.SoundIndex += 1;
            }

            Console.WriteLine("converted se");
           

            return se;
        }

        private static void PostConversion(NodeElement element)
        {

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

        private static Node ConvertFaceExpression(OEFaceExpression exp, Game game)
        {
            DEElementFaceAnim anim = new DEElementFaceAnim();
            anim.ElementKind = Reflection.GetElementIDByName("e_auth_element_face_anim", game);

            switch (exp.ExpressionID)
            {
                case 0:
                    anim.PatternID = (uint)FacePatternY6.pat_anger1_ab;
                    break;
                case 1:
                    anim.PatternID = (uint)FacePatternY6.pat_pain1_ab;
                    break;
                case 3:
                    anim.PatternID = (uint)FacePatternY6.pat_surp1_a;
                    break;
            }

            return anim;
        }
    }
}