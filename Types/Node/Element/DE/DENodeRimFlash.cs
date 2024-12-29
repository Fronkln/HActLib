using System;
using System.Collections.Generic;
using System.IO;
using Yarhl.IO;

namespace HActLib
{
    //Version 6: Y6
    //Version 4: YK2 and above
    //Version 5: YLAD and above
    
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
        public List<float> Curve = null;

        public byte[] CommandData = null;
        public byte[] PulseData = null;

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
            long nodeEnd = reader.Stream.Position + inf.expectedSize - 32;

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

            long pulseAddr = headerStart + (pulseOffset * 4);

            ParamID = reader.ReadUInt32();
            AttachmentSlot = (AttachmentSlot)reader.ReadUInt32();

            long preReadPos = reader.Stream.Position;
            bool parameterResult = false;

            //idk if this would work on 4, not taking risks
            if (parametersOffset > 0 && RimflashVersion >= 4)
            {
                //unreadSections = reader.ReadBytes((int)(parameterAddr - reader.Stream.Position));
                reader.Stream.Seek(parameterAddr);
                unkBytes = null;
                // reader.Stream.Seek(headerStart + parametersOffset);
                parameterResult = ReadParams(reader, inf, version);
            }

            //F R E A K Y  Kiwami 2 Rimflash
            if (!parameterResult && ParamVersion >= 5 && CMN.LastHActDEGame <= Game.YK2)
            {
                reader.Stream.Position = preReadPos;
                ParamVersion = 1337;
            }
            else
            {
                if (curveOffset > 0 && RimflashVersion >= 4)
                {
                    reader.Stream.Seek(curveAddr);
                    ReadCurve(reader, inf, version);
                }

                if (commandOffset > 0 && RimflashVersion >= 4)
                {
                    reader.Stream.Seek(commandAddr);

                    int numBytes = 0;

                    if (parameterAddr > commandAddr)
                        numBytes = (int)(parameterAddr - commandAddr);
                    else
                        numBytes = (int)(nodeEnd - commandAddr);

                    CommandData = reader.ReadBytes(numBytes);
                }

                if (pulseOffset > 0)
                {
                    reader.Stream.Seek(pulseAddr);
                    PulseData = reader.ReadBytes(80);
                }

                if (RimflashVersion >= 4)
                    reader.Stream.Seek(inf.endAddress);
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

        internal bool ReadParams(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            uint paramsVer = 0;
            reader.Stream.RunInPosition(() => paramsVer = reader.ReadUInt32(), 0, SeekMode.Current);
            ParamVersion = paramsVer;

            switch(paramsVer)
            {
                //Y6/YK2
                default:
                    return false;
                case 0:
                    if(RimflashVersion < 5)
                    {
                        RimflashParams paramy = new RimflashParams();
                        paramy.Read(reader);
                        RimflashParams = paramy;
                    }
                    return false;
                //Judgment
                case 2:
                    RimflashParamsV2 paramsV2 = new RimflashParamsV2();
                    paramsV2.Read(reader);
                    RimflashParams = paramsV2;
                    break;
                //YLAD
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

            return true;
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            long headerStart = writer.Stream.Position;
            writer.Write(RimflashVersion);

            long offsets = writer.Stream.Position;

            if (RimflashVersion < 5)
            {
                writer.Write(parametersOffset / 4);
                writer.Write(curveOffset / 4);
                writer.Write(commandOffset / 4);
            }
            else
            {
                writer.Write(0);
                writer.Write(0);
                writer.Write(0);
            }

            writer.Write(FadeOutTime);
            writer.Write(RootValue);

            writer.Write(Unk1);

            long pulseAddrOffset = writer.Stream.Position;

            writer.Write(pulseOffset);

            writer.Write(ParamID);
            writer.Write((uint)AttachmentSlot);

            if (Curve != null && Curve.Count > 0)
            {
                curveOffset = (uint)(writer.Stream.Position - headerStart);
                writer.Write(m_curveUnk1);
                writer.Write(Curve.Count);

                foreach (float f in Curve)
                    writer.Write(f);
            }

            if (CommandData != null)
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

            if(PulseData != null && PulseData.Length > 0)
            {
                pulseOffset = (uint)(writer.Stream.Position - headerStart);
                writer.Write(PulseData);
            }

            long end = writer.Stream.Position;


            if (RimflashVersion >= 4)
            {
                writer.Stream.Seek(offsets);
                writer.Write(parametersOffset / 4);
                writer.Write(curveOffset / 4);
                writer.Write(commandOffset / 4);

                writer.Stream.Seek(pulseAddrOffset);
                writer.Write(pulseOffset / 4);

                writer.Stream.Position = end;
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
    }
}
