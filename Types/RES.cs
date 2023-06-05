using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;


namespace HActLib
{
    public class Resource
    {
        public Guid NodeGUID;
        public ResourceType Type;
        public int Unk1; // 1 sometimes in OE always 0 in DE
        public int Unk2; // seems to be always 1
        public string Name; //192 bytes long
        public float StartFrame;
        public float EndFrame;

        public Resource()
        {

        }

        public Resource(Guid target, ResourceType type, string resource, float start, float end)
        {
            NodeGUID = target;
            Type = type;
            Name = resource;
            StartFrame = start;
            EndFrame = end;
        }

        public Resource Clone()
        {
            return (Resource)MemberwiseClone();
        }
    }

    public enum ResourceType
    {
        Unknown1 = 0x0,
        Unknown2 = 0x1,
        Character = 0x2,
        Unknown3 = 0x3,
        CameraMotion = 0x4,
        PathMotion = 0x5,
        AssetMotion = 0x6,
        CharacterMotion = 0x7,
        Unknown5 = 0x8,
        Unknown6 = 0x9,
        Model = 0xA
    }

    public class RES
    {
        public List<Resource> Resources = new List<Resource>();

        public static RES Read(string path, bool isDE)
        {
            if (!File.Exists(path))
                return null;

            return Read(File.ReadAllBytes(path), isDE);
        }


        public static RES Read(byte[] buffer, bool isDE)
        {
            DataStream readStream = DataStreamFactory.FromArray(buffer, 0, buffer.Length);
            DataReader reader = new DataReader(readStream) { Endianness = EndiannessMode.BigEndian, DefaultEncoding = Encoding.GetEncoding(932) };

            return Read(buffer, reader, isDE ? EndiannessMode.LittleEndian : EndiannessMode.BigEndian);
        }

        public static RES Read(byte[] buffer)
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

        private static RES Read(byte[] buffer, DataReader reader, EndiannessMode endian)
        {
            reader.Endianness = endian;

            //empty res = little endian (DE) assumed       
            RES res = new RES();

            uint count = reader.ReadUInt32();
            reader.ReadBytes(12);

            for (int i = 0; i < count; i++)
            {
                Resource resInf = new Resource();
                resInf.NodeGUID = new Guid(reader.ReadBytes(16));
                resInf.Type = (ResourceType)reader.ReadUInt32();
                resInf.Unk1 = reader.ReadInt32();
                resInf.Unk2 = reader.ReadInt32();
                resInf.Name = reader.ReadString(192).Split(new[] { '\0' }, 2)[0];
                resInf.StartFrame = reader.ReadSingle();
                resInf.EndFrame = reader.ReadSingle();

                reader.ReadBytes(12);

                res.Resources.Add(resInf);
            }


            return res;
        }

        public static void Write(RES res, string path, bool isDE)
        {
            DataWriter writer = new DataWriter(new DataStream()) { Endianness = (isDE ? EndiannessMode.LittleEndian : EndiannessMode.BigEndian) };
            writer.Write(res.Resources.Count);
            
            writer.WriteTimes(0, 12);

            foreach (Resource resInf in res.Resources)
            {
                writer.Write(resInf.NodeGUID.ToByteArray());
                writer.Write((uint)resInf.Type);
                writer.Write(resInf.Unk1);
                writer.Write(resInf.Unk2);
                writer.Write(resInf.Name.ToLength(192), fixedSize: 192, nullTerminator: false);
                writer.Write(resInf.StartFrame);
                writer.Write(resInf.EndFrame);
                writer.WriteTimes(0, 12);
            }

            writer.Stream.WriteTo(path);
        }
    }
}
