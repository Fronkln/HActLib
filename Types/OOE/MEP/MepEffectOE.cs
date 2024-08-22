using HActLib.Internal;


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Yarhl.FileSystem;
using Yarhl.IO;

namespace HActLib
{
    public class MepEffectOE : MepEffect
    {
        public PXDHash BoneName = new PXDHash();
        public int BoneID = -1;
        public int Unknown1;

        public NodeElement Effect;

        internal override void Read(DataReader reader, MEPVersion version)
        {
            uint propertyType = 0;
            uint propertySize = 0;

            reader.Stream.RunInPosition(
                delegate
                {
                    propertyType = reader.ReadUInt32();
                    propertySize = reader.ReadUInt32();
                }, version == MEPVersion.Y5 ? 16 : 48, SeekMode.Current);

            if (Reflection.ElementNodes[CMN.LastHActDEGame].ContainsKey(propertyType))
                Effect = (NodeElement)Activator.CreateInstance(Reflection.ElementNodes[CMN.LastHActDEGame][propertyType]);
            else
                Effect = new NodeElement();// new NodeElement();

            //We will be reading and writing the *CORE* node data for the element
            Effect.Guid = new Guid(reader.ReadBytes(16));

            if (version == MEPVersion.Y0)
                BoneName = reader.Read<PXDHash>();

            Effect.ElementKind = reader.ReadUInt32();
            Effect.NodeSize = reader.ReadInt32();
            BoneID = reader.ReadInt32();
            Unknown1 = reader.ReadInt32();
            reader.ReadBytes(4);
            Effect.Start = reader.ReadSingle();
            Effect.End = reader.ReadSingle();
            reader.ReadBytes(4);

            long target = reader.Stream.Position + (propertySize - 16);

            Game game = version == MEPVersion.Y5 ? Game.Y5 : Game.Y0;

            NodeConvInf inf = new NodeConvInf()
            {
                format = new BinaryFormat(reader.Stream),
                version = OECMN.GetCMNVersionForGame(game),
                gameVersion = GameVersion.Y0_K1,
                file = AuthFile.MEP
            };

            Effect.ReadElementData(reader, inf, GameVersion.Y0_K1);

            long current = reader.Stream.Position;

            if (current > target) //Overread
                reader.Stream.Position = target;
            else if (current < target)
                Effect.unkBytes = reader.ReadBytes((int)(target -  current));

        }

        internal override void Write(DataWriter writer, MEPVersion version)
        {
            writer.Write(Effect.Guid.ToByteArray());

            if (version == MEPVersion.Y0)
                writer.WriteOfType(BoneName);

            writer.Write(Effect.ElementKind);

            long sizeOffset = writer.Stream.Position;

            writer.Write(0xDEADBEEF); //Size, return later after write
                writer.Write(BoneID);

            writer.Write(Unknown1);

            long elementStart = writer.Stream.Position;
            writer.Write(Effect.ElementKind);
            writer.Write(Effect.Start);
            writer.Write(Effect.End);
            writer.Write(0);

            Effect.WriteElementData(writer, GameVersion.Y0_K1, version == MEPVersion.Y0 ? 16 : 10);

            if (Effect.unkBytes != null)
                writer.Write(Effect.unkBytes);

            long size = writer.Stream.Position - sizeOffset - 12;
            writer.Stream.RunInPosition(delegate { writer.Write((uint)size); }, sizeOffset, SeekMode.Start);
        }

        public void OE_ConvertToY0()
        {
            BoneName.Set(OEEffect.ConvertY5BoneIDToY0Name(BoneID));
            BoneID = OEEffect.ConvertY5BoneIDToY0ID(BoneID);
        }

        public void OE_ConvertToY5()
        {

        }
    }
}
