using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    //[ElementID(Game.Y6, 0x35)]
   // [ElementID(Game.YK2, 0x35)]
   // [ElementID(Game.JE, 0x35)]
    [ElementID(Game.YLAD, 0x33)]
    [ElementID(Game.LJ, 0x33)]
    [ElementID(Game.LAD7Gaiden, 0x33)]
    [ElementID(Game.LADIW, 0x33)]
    [ElementID(Game.LADPYIH, 0x33)]
    public class DEElementUITexture : NodeElement
    {
        public uint UITexFlags = 0x1;
        public uint InFrame = 0;
        public uint OutFrame = 0;
        public Vector2 BeforeCenter = new Vector2(0.5f, 0.5f);
        public Vector2 BeforeScale = new Vector2(1, 1);
        public float BeforeAlpha = 1;
        public Vector2 AfterCenter = new Vector2(0, 0);
        public Vector2 AfterScale = new Vector2(1, 1);
        public float AfterAlpha = 1;

        public uint TextureID = 0;

        public string TextureName = "";
        public float[] Animation = new float[32];

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            if (version < GameVersion.DE2)
                return;

            UITexFlags = reader.ReadUInt32();
            InFrame = reader.ReadUInt32();
            OutFrame = reader.ReadUInt32();
            BeforeCenter = new Vector2(reader.ReadSingle(), reader.ReadSingle());
            BeforeScale = new Vector2(reader.ReadSingle(), reader.ReadSingle());
            BeforeAlpha = reader.ReadSingle();
            AfterCenter = new Vector2(reader.ReadSingle(), reader.ReadSingle());
            AfterScale = new Vector2(reader.ReadSingle(), reader.ReadSingle());
            AfterAlpha = reader.ReadSingle();
            TextureID = reader.ReadUInt32();
            TextureName = reader.ReadString(32).Split(new[] { '\0' }, 2)[0];

            reader.ReadBytes(8);

            for (int i = 0; i < 32; i++)
                Animation[i] = reader.ReadSingle();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            if (version < GameVersion.DE2)
                return;

            writer.Write(UITexFlags);
            writer.Write(InFrame);
            writer.Write(OutFrame);

            writer.Write(BeforeCenter.X);
            writer.Write(BeforeCenter.Y);
           
            writer.Write(BeforeScale.X);
            writer.Write(BeforeScale.Y);
            writer.Write(BeforeAlpha);
            
            writer.Write(AfterCenter.X);
            writer.Write(AfterCenter.Y);
            
            writer.Write(AfterScale.X);
            writer.Write(AfterScale.Y);
            writer.Write(AfterAlpha);
            
            writer.Write(TextureID);
            writer.Write(TextureName.ToLength(32), false, maxSize: 32);
            
            writer.WriteTimes(0, 8);

            for(int i = 0; i < 32; i++)
                writer.Write(Animation[i]);
        }
    }
}
