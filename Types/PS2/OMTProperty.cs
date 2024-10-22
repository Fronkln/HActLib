using System;
using System.IO;
using System.Collections.Generic;
using Yarhl.IO;
using System.Text;

namespace HActLib
{
    public class OMTProperty
    {
        public List<OMTMoveProperty> MoveProperties = new List<OMTMoveProperty>();
        public List<OMTEffectProperty> Effects = new List<OMTEffectProperty>();

        public static OMTProperty Read(string path)
        {
            if (!File.Exists(path))
                return null;

            return Read(File.ReadAllBytes(path));
        }

        public static OMTProperty Read(byte[] buffer)
        {
            DataStream readStream = DataStreamFactory.FromArray(buffer, 0, buffer.Length);
            DataReader reader = new DataReader(readStream) { Endianness = EndiannessMode.LittleEndian, DefaultEncoding = Encoding.GetEncoding(932) };

            OMTProperty prop = new OMTProperty();

            int data1Ptr = reader.ReadInt32();
            int data1Count = reader.ReadInt32();

            int data2Ptr = reader.ReadInt32();
            int data2Count = reader.ReadInt32();

            reader.Stream.Seek(data1Ptr);

            for(int i = 0; i < data1Count; i++)
            {
                long endAddress = reader.Stream.Position + 32;
                int type = reader.ReadInt32();

                OMTMoveProperty moveProperty = new OMTMoveProperty();

                switch(type)
                {
                    default:
                        moveProperty = new OMTMoveProperty();
                        break;
                }

                moveProperty.Read(reader);

                if (reader.Stream.Position < endAddress)
                    moveProperty.UnknownData = reader.ReadBytes((int)(endAddress - reader.Stream.Position));

                prop.MoveProperties.Add(moveProperty);
            }

            reader.Stream.Seek(data2Ptr);

            for (int i = 0; i < data2Count; i++)
            {
                long end = reader.Stream.Position + 96;

                OMTEffectProperty pibProp = new OMTEffectProperty();
                pibProp.Type = reader.ReadInt32();
                pibProp.Read(reader);

                if (reader.Stream.Position < end)
                    pibProp.UnknownData = reader.ReadBytes((int)(end - reader.Stream.Position));

                prop.Effects.Add(pibProp);
            }

            return prop;
        }

        public static void Write(OMTProperty property, string path)
        {
            DataWriter writer = new DataWriter(new DataStream()) { Endianness = EndiannessMode.LittleEndian };
            writer.WriteTimes(0, 32);

            uint data1Start = (uint)writer.Stream.Position;

            foreach (var moveProp in property.MoveProperties)
                moveProp.Write(writer);

            uint data2Start = (uint)writer.Stream.Position;

            foreach (var pibProp in property.Effects)
                pibProp.Write(writer);

            writer.Stream.Seek(0);

            writer.Write(data1Start);
            writer.Write(property.MoveProperties.Count);
            writer.Write(data2Start);
            writer.Write(property.Effects.Count);
            writer.Write(writer.Stream.Length);

            File.WriteAllBytes(path, writer.Stream.ToArray());
        }
    }
}
