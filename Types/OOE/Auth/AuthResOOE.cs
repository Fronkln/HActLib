using System;
using System.IO;
using System.Collections.Generic;
using HActLib.Internal;
using System.Text;
using Yarhl.IO;

namespace HActLib
{
    public enum AuthResourceOOEType : int
    {
        Invalid = 0,
        CameraMotion = 1,
        Model = 2,
        ObjectMotion = 3,
        Character = 4
    }

    public struct AuthResourceOOE
    {
        public AuthResourceOOEType Type;
        public int Unknown;
        public Guid GUID;
        public string Resource;
        public string Resource2;  //character node
        public byte[] UnknownData;

        public AuthResourceOOE()
        {
            Type = AuthResourceOOEType.Invalid;
            Unknown = 0;
            GUID = Guid.Empty;
            Resource = "";
            Resource2 = null; 
            UnknownData = new byte[96];
        }

        public override string ToString()
        {
            return Resource.ToString();
        }
    }

    public class AuthResOOE
    {
        public List<AuthResourceOOE> Resources = new List<AuthResourceOOE>();

        public static AuthResOOE Read(string path)
        {
            if (!File.Exists(path))
                return null;

            return Read(File.ReadAllBytes(path));
        }

        public static AuthResOOE Read(byte[] buffer)
        {
            Reflection.Process();

            AuthResOOE auth = new AuthResOOE();

            DataStream readStream = DataStreamFactory.FromArray(buffer, 0, buffer.Length);
            DataReader authReader = new DataReader(readStream) { Endianness = EndiannessMode.BigEndian, DefaultEncoding = Encoding.GetEncoding(932) };

            authReader.Stream.Position = 20;

            uint resourcesPointer = authReader.ReadUInt32();
            uint resourcesCount = authReader.ReadUInt32();

            authReader.Stream.Position = resourcesPointer;

            for(int i = 0; i < resourcesCount; i++)
            {
                AuthResourceOOE resource = new AuthResourceOOE();
                resource.Type = (AuthResourceOOEType)authReader.ReadInt32();
                resource.Unknown = authReader.ReadInt32();
                resource.GUID = new Guid(authReader.ReadBytes(16));
                
                int resourcePtr = authReader.ReadInt32();
                int resourcePtr2 = authReader.ReadInt32();

                resource.Resource = authReader.ReadStringPointer(resourcePtr);

                if(resourcePtr2 != 0)
                {
                    resource.Resource2 = authReader.ReadStringPointer(resourcePtr2);
                }
                else
                {
                    resource.Resource2 = null;
                }

                resource.UnknownData = authReader.ReadBytes(96);

                auth.Resources.Add(resource);
            }

            return auth;
        }

        public static void Write(AuthResOOE file, string path)
        {
            Dictionary<AuthResourceOOE, long> resourceLocations = new Dictionary<AuthResourceOOE, long>();
            Dictionary<string, int> stringTableValues = new Dictionary<string, int>();

            var binary = new BinaryFormat();
            var writer = new DataWriter(new DataStream());
            writer.DefaultEncoding = Encoding.GetEncoding(932);

            writer.Endianness = EndiannessMode.BigEndian;

            writer.Write(1096111176);
            writer.Write(33619968);
            writer.Write(16777216);
            writer.Write(0);

            long headerStart = writer.Stream.Position;

            writer.WriteTimes(0, 32);

            long resourcesStart = writer.Stream.Position;
            writer.WriteTimes(0, file.Resources.Count * 128);

            long tableStart = writer.Stream.Position;
            StringTable table = new StringTable(writer, tableStart);

            writer.Stream.Position = resourcesStart;

            foreach (var resource in file.Resources)
            {
                resourceLocations[resource] = writer.Stream.Position;

                writer.Write((int)resource.Type);
                writer.Write(resource.Unknown);
                writer.Write(resource.GUID.ToByteArray());

                long stringLocation = table.Write(resource.Resource, true);
                long stringLocation2 = table.Write(resource.Resource2, true);
                writer.Write((uint)stringLocation);
                writer.Write((uint)stringLocation2);

                writer.Write(resource.UnknownData);
            };

            writer.Stream.Position = headerStart;
            writer.Write((uint)table.StartPos - 16);
            writer.Write((uint)resourcesStart);
            writer.Write(file.Resources.Count);

            File.WriteAllBytes(path, writer.Stream.ToArray());
        }
    }
}
