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
    };

    [ElementID(Game.Y6, 0x4D)]
    [ElementID(Game.YK2, 0x4D)]
    [ElementID(Game.JE, 0x4D)]
    [ElementID(Game.YLAD, 0x4A)]
    [ElementID(Game.LJ, 0x4A)]
    [ElementID(Game.LAD7Gaiden, 0x4A)]
    [ElementID(Game.LADIW, 0x4A)]
    public class DETimingInfoAttack : NodeElement
    {
        public TimingInfoAttack Data = new TimingInfoAttack();

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            bool isNewDE = version == GameVersion.DE2;

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

    }

}
