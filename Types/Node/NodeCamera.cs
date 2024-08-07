﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    public class NodeCamera : Node
    {
        public uint CameraFlags;

        public float[] FrameProgression = new float[0];
        public float ProgressionEnd;

        public float[] FrameProgressionSpeed = new float[0];

        private float[] Caption = new float[0];

        public NodeCamera()
        {
            Category = AuthNodeCategory.Camera;
        }

        internal override void ReadNodeData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            CameraFlags = reader.ReadUInt32();

            int FrameProgressionCount = reader.ReadInt32();
            int CaptionCount = 0;

            if (inf.version > 10)
                CaptionCount = reader.ReadInt32();
            else
            {
                reader.Stream.Position += 8;
            }

            FrameProgression = new float[FrameProgressionCount];
            FrameProgressionSpeed = new float[FrameProgressionCount];

            for (int i = 0; i < FrameProgression.Length; i++)
                FrameProgression[i] = reader.ReadSingle();

            if(inf.version > 10)
                ProgressionEnd = reader.ReadSingle();

            for (int i = 0; i < FrameProgressionSpeed.Length; i++)
                FrameProgressionSpeed[i] = reader.ReadSingle();

            if (inf.version > 10)
            {
                Caption = new float[CaptionCount];

                for (int i = 0; i < Caption.Length; i++)
                    Caption[i] = reader.ReadSingle();
            }
        }

        internal override void WriteNodeData(DataWriter writer, GameVersion version, uint hactVer)
        {
            base.WriteNodeData(writer, version, hactVer);

            writer.Write(CameraFlags);
            writer.Write(FrameProgression.Length);

            if(hactVer > 10)
                writer.Write(Caption.Length);
            else
            {
                writer.Write(0f);
                writer.Write(0f);
            }

            foreach (float f in FrameProgression)
                writer.Write(f);

            if (hactVer > 10)
                writer.Write(ProgressionEnd);

            foreach (float f in FrameProgressionSpeed)
                writer.Write(f);

            foreach (float f in Caption)
                writer.Write(f);
            
        }

        internal override int GetSize(GameVersion version, uint hactVer)
        {
            return (FrameProgression.Length * 2) + 4 + (Caption.Length);
        }
    }
}
