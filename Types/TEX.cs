using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Yarhl.IO;

namespace HActLib
{

    public class TEX
    {
        public struct Texture
        {
            /// <summary>
            /// Only in DE
            /// </summary>
            public Guid GUID;
            public string TextureName;
        }

        public List<Texture> Textures = new List<Texture>();


        public static TEX Read(string path, bool isDE)
        {
            if (!File.Exists(path))
                return null;

            return Read(File.ReadAllBytes(path), isDE);
        }


        public static TEX Read(byte[] buffer, bool isDE)
        {
            DataStream readStream = DataStreamFactory.FromArray(buffer, 0, buffer.Length);
            DataReader reader = new DataReader(readStream) { Endianness = EndiannessMode.BigEndian, DefaultEncoding = Encoding.GetEncoding(932) };

            return Read(buffer, reader, isDE ? EndiannessMode.LittleEndian : EndiannessMode.BigEndian);
        }

        public static TEX Read(byte[] buffer)
        {
            DataStream readStream = DataStreamFactory.FromArray(buffer, 0, buffer.Length);
            DataReader reader = new DataReader(readStream) { Endianness = EndiannessMode.BigEndian, DefaultEncoding = Encoding.GetEncoding(932) };

            uint endianCheck = 0;
            EndiannessMode endian = EndiannessMode.BigEndian;

            reader.Stream.RunInPosition(delegate { endianCheck = reader.ReadUInt32(); }, 0, SeekMode.Start);

            //DE
            if (endianCheck > 49152)
                endian = EndiannessMode.LittleEndian;

            return Read(buffer, reader, endian);
        }

        private static TEX Read(byte[] buffer, DataReader reader, EndiannessMode endian)
        {
            reader.Endianness = endian;

            TEX tex = new TEX();

            uint count = reader.ReadUInt32();
            reader.ReadBytes(12);

            bool isDE = endian == EndiannessMode.LittleEndian;

            for (int i = 0; i < count; i++)
            {
                Texture texture = new Texture();

                if (isDE)
                    texture.GUID = new Guid(reader.ReadBytes(16));

                texture.TextureName = reader.ReadString(32).Split(new[] { '\0' }, 2)[0]; ;

                tex.Textures.Add(texture);
            }

            return tex;
        }

        public static void Write(TEX tex, string path, bool isDE)
        {
            DataWriter writer = new DataWriter(new DataStream()) { Endianness = (isDE ? EndiannessMode.LittleEndian : EndiannessMode.BigEndian) };
            writer.Write(tex.Textures.Count);

            writer.WriteTimes(0, 12);

            foreach (Texture texture in tex.Textures)
            {
                if (isDE)
                    writer.Write(texture.GUID.ToByteArray());

                string texName = texture.TextureName;

                if (isDE)
                {
                    if (texName.EndsWith(".dds"))
                        texName = texName.Replace(".dds", "");
                }

                writer.Write(texName.ToLength(32), false, maxSize:32);
            }

            writer.Stream.WriteTo(path);
        }
    }
}
