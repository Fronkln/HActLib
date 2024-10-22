using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    
    [ElementID(Game.Y6, 0x1F)]
    [ElementID(Game.YK2, 0x1F)]
    [ElementID(Game.JE, 0x1F)]
    [ElementID(Game.YLAD, 0x1D)]
    [ElementID(Game.LJ, 0x1D)]
    [ElementID(Game.LAD7Gaiden, 0x1D)]
    [ElementID(Game.LADIW, 0x1D)]
    [ElementID(Game.LADPYIH, 0x1D)]
    public class DEElementRimflash : NodeElement
    {
        public uint RimflashVersion;
        public float FadeOutTime;
        public float RootValue;
        public uint ParamID;
        public AttachmentSlot AttachmentSlot;

        public uint ParamVersion { get; private set; }
        public RimflashParams RimflashParams;

        uint parametersOffset;
        uint curveOffset;
        uint commandOffset;
        uint pulseOffset;

        public Parameter Params;

        private int m_curveUnk1;
        public List<float> Curve = new List<float>();

        public byte[] CommandData;

        private byte[] Unk1 = new byte[8];

        private byte[] unreadSections;

        public struct Parameter
        {
            public uint Version;

            public float Color1Intensity;
            public float Color2Intensity;
        }

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {

            //Header position + offset * 4 = structure
            long headerStart = reader.Stream.Position;
            long nodeEnd = reader.Stream.Position + inf.expectedSize;

            //HEADER START - SAME BETWEEN V5 AND V4
            RimflashVersion = reader.ReadUInt32();

            parametersOffset = reader.ReadUInt32() * 4;
            curveOffset = reader.ReadUInt32() * 4;
            commandOffset = reader.ReadUInt32() * 4;

            long parameterAddr = headerStart + parametersOffset;
            long curveAddr = headerStart + curveOffset;
            long commandAddr = headerStart + commandOffset;

            FadeOutTime = reader.ReadSingle();
            RootValue = reader.ReadSingle();

            Unk1 = reader.ReadBytes(8);

            pulseOffset = reader.ReadUInt32();

            ParamID = reader.ReadUInt32();
            AttachmentSlot = (AttachmentSlot)reader.ReadUInt32();

            if(curveOffset > 0 && RimflashVersion >= 5)
            {
                reader.Stream.Seek(curveAddr);
                ReadCurve(reader, inf, version);
            }

            if(commandOffset > 0 && RimflashVersion >= 5)
            {
                reader.Stream.Seek(commandAddr);

                int numBytes = 0;

                if (parameterAddr > commandAddr)
                    numBytes = (int)(parameterAddr - commandAddr);
                else
                    numBytes = (int)(nodeEnd - commandAddr);

                CommandData = reader.ReadBytes(numBytes);
            }

            //idk if this would work on 4, not taking risks
            if (parametersOffset > 0 && RimflashVersion > 4)
            {
                unreadSections = reader.ReadBytes((int)(parameterAddr - reader.Stream.Position));
                reader.Stream.Seek(parameterAddr);
                unkBytes = null;
               // reader.Stream.Seek(headerStart + parametersOffset);
                ReadParams(reader, inf, version);
            }
        }

        internal void ReadCurve(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            m_curveUnk1 = reader.ReadInt32();
            int curveLength = reader.ReadInt32();

            Curve = new List<float>();

            for(int i = 0; i < curveLength; i++)
                Curve.Add(reader.ReadSingle());
        }

        internal void ReadParams(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            uint paramsVer = 0;
            reader.Stream.RunInPosition(() => paramsVer = reader.ReadUInt32(), 0, SeekMode.Current);
            ParamVersion = paramsVer;

            switch(paramsVer)
            {
                //DE 1.0
                case 0:
                    break;
                case 3:
                    RimflashParamsV3 paramsV3 = new RimflashParamsV3();
                    paramsV3.Read(reader);
                    RimflashParams = paramsV3;
                    break;
                //LJ and above
                case 4:
                    RimflashParamsV4 paramsV4 = new RimflashParamsV4();
                    paramsV4.Read(reader);
                    RimflashParams = paramsV4;
                    break;
            }
        }

        public void ImportParams(string path)
        {
            byte[] dat = File.ReadAllBytes(path);
            var buf = DataStreamFactory.FromArray(dat, 0, dat.Length);
            var reader = new DataReader(buf);

            ReadParams(reader, new NodeConvInf(), CMN.LastGameVersion);
        }

        public void ExportParams(string path)
        {
            var buf = DataStreamFactory.FromMemory();
            DataWriter writer = new DataWriter(buf);
            RimflashParams.Write(writer);

            File.WriteAllBytes(path, buf.ToArray());

            buf.Dispose();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            long headerStart = writer.Stream.Position;
            writer.Write(RimflashVersion);

            long offsets = writer.Stream.Position;
            //writer.Write(parametersOffset /4);
            //writer.Write(curveOffset / 4);
            //writer.Write(commandOffset / 4);

            writer.Write(0);
            writer.Write(0);
            writer.Write(0);

            writer.Write(FadeOutTime);
            writer.Write(RootValue);

            writer.Write(Unk1);

            writer.Write(pulseOffset);

            writer.Write(ParamID);
            writer.Write((uint)AttachmentSlot);

            if(Curve != null)
            {
                curveOffset = (uint)(writer.Stream.Position - headerStart);
                writer.Write(m_curveUnk1);
                writer.Write(Curve.Count);

                foreach (float f in Curve)
                    writer.Write(f);
            }

            if(CommandData != null)
            {
                uint commandAddr = (uint)writer.Stream.Position;
                commandOffset = commandAddr - (uint)headerStart;
                writer.Write(CommandData);
            }

            if (unreadSections != null && unreadSections.Length > 0)
                writer.Write(unreadSections);

            if (RimflashParams != null)
            {
                parametersOffset = (uint)(writer.Stream.Position - headerStart);
                RimflashParams.Write(writer);
            }

            long end = writer.Stream.Position;

            writer.Stream.Seek(offsets);
            writer.Write(parametersOffset / 4);
            writer.Write(curveOffset / 4);
            writer.Write(commandOffset / 4);

            writer.Stream.Position = end;
        }
    }
}
