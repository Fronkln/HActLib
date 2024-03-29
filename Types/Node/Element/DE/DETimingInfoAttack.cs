﻿using System;
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
        public byte[] Attributes; // 8
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
          
            Data.Attributes = version == GameVersion.DE2 ? reader.ReadBytes(6) : reader.ReadBytes(4);

            if (isNewDE)
            {
                reader.ReadBytes(2);
                Data.AttackID = reader.ReadInt32();
            }
        }


        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            writer.Write(Data.Damage);
            writer.Write(Data.Power);
            writer.Write(Data.Flag);
            writer.Write(Data.Parts);

            foreach (byte b in Data.Attributes)
                writer.Write(b);

            if (version == GameVersion.DE2)
            {
                writer.WriteTimes(0, 2);
                writer.Write(Data.AttackID);
            }
        }

        public override bool TryConvert(Game input, Game output)
        {
            GameVersion inputGameVer = CMN.GetVersionForGame(input);
            GameVersion outputGameVer = CMN.GetVersionForGame(output);

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

            return true;
        }

    }

}
