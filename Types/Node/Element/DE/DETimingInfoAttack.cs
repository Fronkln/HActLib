using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Yarhl.IO;

namespace HActLib
{
    public struct TimingInfoAttack
    {
        public uint Damage;
        public ushort Power;
        public ushort Flag;
        public uint Parts;
        public ulong Attributes; // battle_reaction_attr.bin
        public int AttackID;
        public int Unknown; //Introduced in Gaiden
        public bool Unknown2; //Introduced in Pirate Gaiden
        public bool Unknown3; //Introduced in Pirate Gaiden
        public float Unknown4; //Introduced in Kiwami 3
        public int Unknown5; //Introduced in Kiwami 3
        public int Unknown6; //Introduced in Kiwami 3
    };

    [ElementID(Game.Y6, 0x4D)]
    [ElementID(Game.YK2, 0x4D)]
    [ElementID(Game.JE, 0x4D)]
    [ElementID(Game.YLAD, 0x4A)]
    [ElementID(Game.LJ, 0x4A)]
    [ElementID(Game.LAD7Gaiden, 0x4A)]
    [ElementID(Game.LADIW, 0x4A)]
    [ElementID(Game.LADPYIH, 0x4A)]
    [ElementID(Game.YK3, 0x4A)]
    public class DETimingInfoAttack : NodeElement
    {
        public TimingInfoAttack Data = new TimingInfoAttack();

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            bool isNewDE = version >= GameVersion.DE2;

            Data.Damage = reader.ReadUInt32();
            Data.Power = reader.ReadUInt16();
            Data.Flag = reader.ReadUInt16();
            Data.Parts = reader.ReadUInt32();


            if (version >= GameVersion.DE2)
                Data.Attributes = reader.ReadUInt64();
            else
                Data.Attributes = reader.ReadUInt32();

            if (isNewDE)
                Data.AttackID = reader.ReadInt32();

            if (CMN.LastHActDEGame == Game.LAD7Gaiden || CMN.LastHActDEGame >= Game.LADPYIH)
                Data.Unknown = reader.ReadInt32();


            if (CMN.LastHActDEGame >= Game.LADPYIH)
            {
                Data.Unknown2 = reader.ReadByte() > 0;
                Data.Unknown3 = reader.ReadByte() > 0;
                
                if(CMN.LastHActDEGame < Game.YK3)
                    reader.Stream.Position += 2;
            }

            if (CMN.LastHActDEGame >= Game.YK3)
            {
                Data.Unknown4 = reader.ReadSingle();
                Data.Unknown5 = reader.ReadInt32();
                Data.Unknown6 = reader.ReadInt32();
            }

        }


        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(Data.Damage);
            writer.Write(Data.Power);
            writer.Write(Data.Flag);
            writer.Write(Data.Parts);

            if (version >= GameVersion.DE2)
                writer.Write(Data.Attributes);
            else
                writer.Write((int)Data.Attributes);

            if (version >= GameVersion.DE2)
                writer.Write(Data.AttackID);

            if (CMN.LastHActDEGame == Game.LAD7Gaiden || CMN.LastHActDEGame >= Game.LADPYIH)
                writer.Write(Data.Unknown);

            if (CMN.LastHActDEGame >= Game.LADPYIH)
            {
                writer.Write(Convert.ToByte(Data.Unknown2));
                writer.Write(Convert.ToByte(Data.Unknown3));

                if (CMN.LastHActDEGame < Game.YK3)
                    writer.WriteTimes(0, 2);
            }
            if (CMN.LastHActDEGame >= Game.YK3)
            {
                writer.Write(Data.Unknown4);
                writer.Write(Data.Unknown5);
                writer.Write(Data.Unknown6);
            }
        }

        public override Node TryConvert(Game input, Game output)
        {
            GameVersion inputGameVer = CMN.GetVersionForGame(input);
            GameVersion outputGameVer = CMN.GetVersionForGame(output);

            /*
            if(inputGameVer <= GameVersion.DE1 && outputGameVer == GameVersion.DE2)
            {
                byte[] newAttribs = new byte[6];

                Array.Copy(Data.Attributes, newAttribs, 4);
                Data.Attributes = newAttribs;
            }
            else if(inputGameVer == GameVersion.DE2 && outputGameVer <= GameVersion.DE1)
            {
                byte[] newAttribs = new byte[4];

                Array.Copy(Data.Attributes, newAttribs, 4);
                Data.Attributes = newAttribs;
            }
            */

            return this;
        }

        public static Type GetAttributesForGame(Game game)
        {
            switch (game)
            {
                default:
                    return typeof(BattleAttributeLJ);
                case Game.Y6Demo:
                    return typeof(BattleAttributeYK2);
                case Game.Y6:
                    return typeof(BattleAttributeYK2);
                case Game.YK2:
                    return typeof(BattleAttributeYK2);
                case Game.JE:
                    return typeof(BattleAttributeJE);
                case Game.YLAD:
                    return typeof(BattleAttributeYLAD);
                case Game.LJ:
                    return typeof(BattleAttributeLJ);
                case Game.LAD7Gaiden:
                    return typeof(BattleAttributeGaiden);
                case Game.LADIW:
                    return typeof(BattleAttributeIW);
                case Game.LADPYIH:
                    return typeof(BattleAttributeYK3);
                case Game.YK3:
                    return typeof(BattleAttributeYK3);
            }
        }
    }

}
