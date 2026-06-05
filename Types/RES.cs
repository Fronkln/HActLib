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
        public string MainResource { get { return Resources[0]; } set { if (Resources.Count <= 0) Resources.Add(value); else Resources[0] = value; } } //192 bytes long
        public List<string> Resources = new List<string>();
        public float StartFrame;
        public float EndFrame;

        public byte[] UnkData = new byte[0];

        public Resource()
        {

        }

        public Resource(Guid target, ResourceType type, string resource, float start, float end)
        {
            NodeGUID = target;
            Type = type;
            MainResource = resource;
            StartFrame = start;
            EndFrame = end;
        }

        public Resource Clone()
        {
            Resource newRes = new Resource();
            newRes.NodeGUID = NodeGUID;
            newRes.Unk1 = Unk1;
            newRes.StartFrame = StartFrame;
            newRes.EndFrame = EndFrame;
            newRes.Type = Type;
            newRes.UnkData = UnkData;

            newRes.Resources.AddRange(Resources);

            return newRes;
        }

        public override string ToString()
        {
            return MainResource;
        }
    }

    public enum ResourceType
    {
        Unknown1 = 0x0,
        Unknown2 = 0x1,
        Character = 0x2,
        CustomModel = 0x3,
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

        //0 is pre pirate game, 1 is after. not in header calculated by us
        public int Version = 0;

        public List<Resource> Resources = new List<Resource>();

        public Resource FindByGUID(Guid guid)
        {
            return Resources.FirstOrDefault(x => x.NodeGUID == guid);
        }

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

            int detectedVersion = 0;

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
                int ResourceCount = reader.ReadInt32();

                long end = reader.Stream.Position + 192;

                for (int k = 0; k < ResourceCount; k++)
                    resInf.Resources.Add(reader.ReadString(32).Split(new[] { '\0' }, 2)[0]);

                if (reader.Stream.Position < end)
                    resInf.UnkData = reader.ReadBytes((int)(end - reader.Stream.Position));

                if (detectedVersion == 0)
                {
                    resInf.StartFrame = reader.ReadSingle();
                    resInf.EndFrame = reader.ReadSingle();

                    //Too precise: Pirates format 
                    bool tooPrecise = Math.Abs(resInf.EndFrame) < 1e-10 || resInf.EndFrame.ToString("G17").Length > 10;

                    if (tooPrecise)
                    {
                        //Auto detection of pirate game RES which uses a new timing format
                        reader.Stream.Position -= 8;
                        resInf.StartFrame = new GameTick2(reader.ReadUInt32());
                        resInf.EndFrame = new GameTick2(reader.ReadUInt32());
                        detectedVersion = 1;
                    }
                }
                else
                {
                    resInf.StartFrame = new GameTick2(reader.ReadUInt32()).Frame;
                    resInf.EndFrame = new GameTick2(reader.ReadUInt32()).Frame;
                }

                reader.ReadBytes(12);

                res.Resources.Add(resInf);
            }

            res.Version = detectedVersion;
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
                writer.Write(resInf.Resources.Count);

                long end = writer.Stream.Position + 192;

                for(int i = 0; i < resInf.Resources.Count; i++)
                    writer.Write(resInf.Resources[i].ToLength(32), fixedSize: 32, nullTerminator: false);

                writer.Write(resInf.UnkData);

                if (writer.Stream.Position < end)
                    writer.WriteTimes(0, (int)(end - writer.Stream.Position));

                if (res.Version <= 0)
                {
                    writer.Write(resInf.StartFrame);
                    writer.Write(resInf.EndFrame);
                }
                else
                {
                    writer.Write(new GameTick2(resInf.StartFrame).Tick);
                    writer.Write(new GameTick2(resInf.EndFrame).Tick);
                }
                writer.WriteTimes(0, 12);
            }

            writer.Stream.WriteTo(path);
        }
    }
}
