using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y5, 0x22)]
    [ElementID(Game.Ishin, 0x22)]
    [ElementID(Game.Y0, 0x22)]
    [ElementID(Game.YK1, 0x22)]
    public class OEHActInput : NodeElement
    {
        public ushort Unk1 = 1; //1 between 10 and 16
        public ushort Unk2 = 1; // 1 between 10 and 16
        public uint Unk3 = 0; // 0 between 10 and 16

        public uint Timing = 15;

        public int Unk4;

        //Ishin and above
        public byte[] Unknown = new byte[20];

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {

            Unk1 = reader.ReadUInt16();
            Unk2 = reader.ReadUInt16();
            Unk3 = reader.ReadUInt32();

            if (inf.version > 10)
            {
                //Timing but in frames but as unsigned int? funny if true
                Unk4 = reader.ReadInt32();
                Timing = reader.ReadUInt32();
            }
            else
            {
                //what is this??? its 1 on sera qte
                Unk4 = reader.ReadInt32();

                //assumed to be the qte length in frames. if it isnt, we can use node start end
                Timing = new GameTick((float)reader.ReadUInt32()).Tick;
            }

            if (CMN.LastHActDEGame >= Game.Ishin)
                Unknown = reader.ReadBytes(20);
            else
                Unknown = reader.ReadBytes(4);            
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            writer.Write(Unk1);
            writer.Write(Unk2);
            writer.Write(Unk3);
            writer.Write(Unk4);

            if (CMN.LastHActDEGame != Game.Y5)
                writer.Write(Timing);
            else
                writer.Write(new GameTick(Timing).Frame);

            if (CMN.LastHActDEGame >= Game.Ishin)
                writer.Write(Unknown);
            else
                writer.Write(0);
        }
    }
}
