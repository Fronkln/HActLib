using HActLib.Internal;
using MotionLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HActLib
{
    public class OEToDEProperty
    {
        public static BEP ConvertPropertyEntryToDE(string propertyBinPath, string propertyName, Game oeGame, Game game)
        {
            OldEngineFormat oeProperty = OldEngineFormat.Read(propertyBinPath);
            BEP bep = new BEP();
            bep.Game = game;

            foreach (var entry in oeProperty.Moves)
            {
                if(entry.Name == propertyName)
                {
                    foreach (OEAnimProperty property in entry.Properties)
                    {
                        Node convertedNode = ConvertPropertyToNode(property, oeGame, Game.LADIW);

                        if (convertedNode != null)
                            bep.Nodes.Add(convertedNode);
                    }

                    return bep;
                }
            }

            return null;
        }

        public static Node ConvertPropertyToNode(OEAnimProperty property, Game oeGame, Game game)
        {
            NodeElement deNode = null;

            bool overrideTiming = false;

            switch (property.Type)
            {
                case OEPropertyType.VoiceAudio:
                    ushort voiceCuesheet = 0;

                    voiceCuesheet = RyuseModule.GetGVFighterIDForGame(game);

                    OEAnimPropertyVoiceSE oePropertyVoiceSE = property as OEAnimPropertyVoiceSE;
                    DEElementSE voiceSE = new DEElementSE();

                    voiceSE.ElementKind = Reflection.GetElementIDByName("e_auth_element_se", game);
                    voiceSE.CueSheet = voiceCuesheet;
                    voiceSE.SoundIndex = (byte)oePropertyVoiceSE.ID;
                    voiceSE.Unk = 128;

                    deNode = voiceSE;
                    break;

                case OEPropertyType.SEAudio:
                    OEAnimPropertySE oePropertySE = property as OEAnimPropertySE;
                    DEElementSE seNode = new DEElementSE();

                    seNode.ElementKind = Reflection.GetElementIDByName("e_auth_element_se", game);
                    seNode.CueSheet = oePropertySE.Cuesheet;
                    seNode.SoundIndex = (byte)(oePropertySE.ID + 1);

                    deNode = seNode;
                    break;

                case OEPropertyType.FollowupWindow:
                    DEElementFollowupWindow deFollowup = new DEElementFollowupWindow();
                    deFollowup.ElementKind = Reflection.GetElementIDByName("e_auth_element_battle_followup_window", game);

                    deNode = deFollowup;
                    break;

                case OEPropertyType.ControlLock:
                    DEElementControlLock deControl = new DEElementControlLock();
                    deControl.ElementKind = Reflection.GetElementIDByName("e_auth_element_battle_control_window", game);

                    deNode = deControl;
                    break;

                case OEPropertyType.Hitbox:
                    OEAnimPropertyHitbox oeHitbox = property as OEAnimPropertyHitbox;
                    DETimingInfoAttack attack = new DETimingInfoAttack();

                    attack.ElementKind = Reflection.GetElementIDByName("e_auth_element_battle_attack", game);

                    attack.Data.Damage = (uint)(oeHitbox.Damage * 8);



                    if (oeHitbox.HitboxLocation1.HasFlag(HitboxLocation1Flag.LeftElbow))
                        attack.Data.Parts |= ((int)BattleColliball.ude2_l);

                    if (oeHitbox.HitboxLocation1.HasFlag(HitboxLocation1Flag.LeftHand))
                        attack.Data.Parts |= ((int)BattleColliball.ude3_l);

                    if (oeHitbox.HitboxLocation1.HasFlag(HitboxLocation1Flag.RightArm))
                        attack.Data.Parts |= ((int)BattleColliball.ude1_r);

                    if (oeHitbox.HitboxLocation1.HasFlag(HitboxLocation1Flag.RightElbow))
                        attack.Data.Parts |= ((int)BattleColliball.ude2_r);

                    if (oeHitbox.HitboxLocation1.HasFlag(HitboxLocation1Flag.RightHand))
                        attack.Data.Parts |= ((int)BattleColliball.ude3_r);

                    if (oeHitbox.HitboxLocation1.HasFlag(HitboxLocation1Flag.RightThigh))
                        attack.Data.Parts |= ((int)BattleColliball.asi1_r);

                    if (oeHitbox.HitboxLocation1.HasFlag(HitboxLocation1Flag.RightKnee))
                        attack.Data.Parts |= ((int)BattleColliball.asi2_r);

                    if (oeHitbox.HitboxLocation1.HasFlag(HitboxLocation1Flag.RightFoot))
                        attack.Data.Parts |= ((int)BattleColliball.asi3_r);

                    if (oeHitbox.HitboxLocation1.HasFlag(HitboxLocation1Flag.LeftThigh))
                        attack.Data.Parts |= ((int)BattleColliball.asi1_l);

                    if (oeHitbox.HitboxLocation1.HasFlag(HitboxLocation1Flag.LeftKnee))
                        attack.Data.Parts |= ((int)BattleColliball.asi2_l);

                    if (oeHitbox.HitboxLocation1.HasFlag(HitboxLocation1Flag.LeftFoot))
                        attack.Data.Parts |= ((int)BattleColliball.asi3_l);

                    if (oeHitbox.HitboxLocation1.HasFlag(HitboxLocation1Flag.Head))
                        attack.Data.Parts |= ((int)BattleColliball.face);

                    //not too sure how to detect these but these have been consistent-ish enough
                    if (oeHitbox.HitboxLocation2 == 32768 || oeHitbox.Flags == 255)
                    {
                        HitboxLocation1Flag flag = oeHitbox.HitboxLocation1;
                        bool isLeftBuki = flag.HasFlag(HitboxLocation1Flag.LeftArm) || flag.HasFlag(HitboxLocation1Flag.LeftElbow) || flag.HasFlag(HitboxLocation1Flag.LeftHand);
                        bool isRightBuki = flag.HasFlag(HitboxLocation1Flag.RightArm) || flag.HasFlag(HitboxLocation1Flag.RightElbow) || flag.HasFlag(HitboxLocation1Flag.RightHand);

                      //  if(isLeftBuki)
                           // attack.Data.Parts |= (1 << (int)BattleColliball.buki_l);

                       // if(isRightBuki)
                            attack.Data.Parts |= (1 << (int)BattleColliball.buki_r);
                    }

                    //Placeholder
                    attack.Data.Attributes = 1;
                    attack.Data.Power = 1;

                    if (oeHitbox.MoveEffect2 == 16777216)
                        attack.Data.Attributes |= (1 << (int)YLADReactionAttributes.sync);

                    deNode = attack;

                    break;

                case OEPropertyType.Muteki:
                    DETimingInfoMuteki invincibility = new DETimingInfoMuteki();
                    invincibility.ElementKind = Reflection.GetElementIDByName("e_auth_element_battle_muteki", game);

                    deNode = invincibility;

                    break;

                case OEPropertyType.ThrowWeapon:
                    DEElementBattleThrow throwWep = new DEElementBattleThrow();
                    throwWep.ElementKind = Reflection.GetElementIDByName("e_auth_element_battle_throw", game);

                    deNode = throwWep;
                    break;

                case OEPropertyType.Bullet:
                    DEElementBattleShoot shoot = new DEElementBattleShoot();
                    shoot.ElementKind = Reflection.GetElementIDByName("e_auth_element_battle_shoot", game);
                    deNode = shoot;
                    break;

                case OEPropertyType.SyncDamage:
                    NodeBattleDamage dam = new NodeBattleDamage();
                    dam.Damage = (property as OEAnimPropertySyncDamage).Damage;
                    dam.Damage *= 10; //otherwise mediocre damage in DE
                    dam.ElementKind = Reflection.GetElementIDByName("e_auth_element_battle_damage", game);
                    dam.PlayType = ElementPlayType.Oneshot;
                    dam.UpdateTimingMode = 2;
                    overrideTiming = true;

                    deNode = dam;
                    break;

            }

            if (deNode != null)
            {
                deNode.Start = property.Start;
                deNode.End = property.End;
                deNode.BEPDat.Guid2 = Guid.NewGuid();

                if (!overrideTiming)
                {
                    deNode.PlayType = ElementPlayType.Normal;
                    deNode.UpdateTimingMode = 1;
                }
            }

            return deNode;
        }
    }
}
