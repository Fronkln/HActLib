using System;
using Yarhl.IO;

namespace HActLib
{

    [ElementID(Game.Y6, 0x6B)]
    [ElementID(Game.YK2, 0x6B)]
    [ElementID(Game.JE, 0x6B)]
    [ElementID(Game.YLAD, 0x68)]
    [ElementID(Game.LJ, 0x68)]
    [ElementID(Game.LAD7Gaiden, 0x68)]
    [ElementID(Game.LADIW, 0x68)]
    [ElementID(Game.LADPYIH, 0x68)]
    public class DEElementCharaOut : NodeElement
    {
        public AuthReturnType ReturnType;
        public GameTick PlayRange;
        public uint TickLength;
        public bool RagdollInfoExists;

        public PXDHash Hash;

        public TimingInfoRagdoll RagdollInfo;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            ReturnType = (AuthReturnType)reader.ReadUInt32();
            PlayRange = new GameTick(reader.ReadUInt32());
            TickLength = reader.ReadUInt32();
            RagdollInfoExists = reader.ReadInt32() == 1;
            Hash = reader.Read<PXDHash>();
            RagdollInfo = new TimingInfoRagdoll();
            
           // if (RagdollInfoExists)
               // RagdollInfo.Read(reader);
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write((uint)ReturnType);
            writer.Write(PlayRange.Tick);
            writer.Write(TickLength);
            writer.Write(RagdollInfoExists == true ? 1 : 0);
            writer.WriteOfType(Hash);

           // if(RagdollInfoExists)
                //RagdollInfo.Write(writer);
        }
    }
}
