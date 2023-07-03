using System;
using Yarhl.IO;

namespace HActLib
{
    public enum BattleCommandSpecialID
    {
        invalid = 0,
        Common = 1,
        ChangeCommandSet = 2,
        RobWeapon = 3,
        CancelInvisibleFighter = 4,
        ChangeHActRoot = 5,
        CollisionSafePoint = 6,
        MagazineDrop = 7,
        RecoverHP = 8,
        Dorobou = 9,
        DispHPGauge = 10,
        BattleTransform = 11,
        PlayBattleStart = 12,
        TrademarkPose = 13,
        PlayBattleDamage = 14,
        PlayCardUI = 15,
        PlayBattleStartNoJoint = 16,
        ForceBattleFinish = 17,
        PlayDiceUI = 18
    }
    [ElementID(Game.Y6, 0xBF)]
    [ElementID(Game.YK2, 0xBF)]
    [ElementID(Game.JE, 0xBF)]
    [ElementID(Game.YLAD, 0xBB)]
    [ElementID(Game.LJ, 0xBB)]
    [ElementID(Game.LAD7Gaiden, 0xBB)]
    [ElementID(Game.LADIW, 0xBB)]
    public class DEElementBattleCommandSpecial : NodeElement
    {
        public BattleCommandSpecialID Type;
        public int Param1;
        public int Param2;
        public int Param3;
        public string ParamString;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Type = (BattleCommandSpecialID)reader.ReadUInt32();
            Param1 = reader.ReadInt32();
            Param2 = reader.ReadInt32();
            Param3 = reader.ReadInt32();
            ParamString = reader.ReadString(32);
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            writer.Write((uint)Type);
            writer.Write(Param1);
            writer.Write(Param2);
            writer.Write(Param3);
            writer.Write(ParamString.ToLength(32), false);
        }
    }
}
