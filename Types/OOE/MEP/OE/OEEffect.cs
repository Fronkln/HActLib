using System;
using System.Linq;
using Yarhl.IO;

namespace HActLib
{
    public class OEEffect
    {
        public Guid GUID;

        public uint ElementID;
        public int BoneID; //Y5
        public PXDHash BoneName = new PXDHash(); //Y0, 32 bytes

        private int Unknown1;
        public float Start;
        public float End;

        public byte[] UnreadBytes;

        internal virtual void Read(DataReader reader, MEPVersion ver)
        {
            GUID = new Guid(reader.ReadBytes(16));

            if (ver == MEPVersion.Y0)
                BoneName = reader.Read<PXDHash>();

            ElementID = reader.ReadUInt32();
            reader.ReadBytes(4);
            BoneID = reader.ReadInt32(); //I'm having doubts that this is actually bone ID or is used in Y0

            Unknown1 = reader.ReadInt32();

            //Element Header
            reader.ReadInt32();
            Start = reader.ReadSingle();
            End = reader.ReadSingle();
            reader.ReadInt32();
        }

        internal virtual void ReadEffectData(DataReader reader, MEPVersion ver)
        {

        }

        internal virtual void Write(DataWriter writer, MEPVersion ver)
        {
            writer.Write(GUID.ToByteArray());

            if (ver == MEPVersion.Y0)
                writer.WriteOfType(BoneName);

            writer.Write(ElementID);

            long sizeOffset = writer.Stream.Position;

            writer.Write(0xDEADBEEF); //Size, return later after write
            writer.Write(BoneID);

            writer.Write(Unknown1);

            long elementStart = writer.Stream.Position;
            writer.Write(ElementID);
            writer.Write(Start);
            writer.Write(End);
            writer.Write(0);

            WriteEffectData(writer, ver);

            if (UnreadBytes != null)
                writer.Write(UnreadBytes);

            long size = writer.Stream.Position - sizeOffset - 12;
            writer.Stream.RunInPosition(delegate { writer.Write((uint)size); }, sizeOffset, SeekMode.Start);
        }

        internal virtual void WriteEffectData(DataWriter writer, MEPVersion ver)
        {
        }


        /// <summary>
        /// Convert Y0 data to be functional in Y5. Writing to stream is not done here.
        /// </summary>
        public virtual void ConvertToY5()
        {
        }

        /// <summary>
        /// Convert Y5 data to be functional in Y0. Writing to stream is not done here.
        /// </summary>
        public virtual void ConvertToY0()
        {
            BoneName.Set(ConvertY5BoneIDToY0Name(BoneID));
            BoneID = ConvertY5BoneIDToY0ID(BoneID);
        }


        public static string ConvertY5BoneNameToY0Name(string boneName)
        {
            if (string.IsNullOrEmpty(boneName))
                return null;

            if (boneName == "face")
                return MEPDict.OEBoneID.FirstOrDefault(x => x.Key == "face_c_n").Key;


            int nIdx = boneName.IndexOf("_n");

            if (nIdx >= 0)
                if (!boneName.Contains("_l") && !boneName.Contains("_r"))
                    boneName = boneName.Insert(nIdx, "_c");

            return boneName;
        }

        //Y5 bone ID goes in, Y0 bone name comes out
        public static string ConvertY5BoneIDToY0Name(int boneID)
        {
            if (boneID == -1)
                return "";

            string bone = MEPDict.OOEBoneID.ElementAt(boneID).Key;

            if (bone == "face")
                return MEPDict.OEBoneID.FirstOrDefault(x => x.Key == "face_c_n").Key;
            else
            {

                int nIdx = bone.IndexOf("_n");

                if (nIdx >= 0)
                    if (!bone.Contains("_l") && !bone.Contains("_r"))
                        bone = bone.Insert(nIdx, "_c");

                return MEPDict.OEBoneID.FirstOrDefault(x => x.Key == bone).Key;
            }
        }

        //Y5 bone ID goes in, Y0 bone name comes out
        public static int ConvertY5BoneIDToY0ID(int boneID)
        {
            if (boneID == -1)
                return -1;

            string bone = MEPDict.OOEBoneID.ElementAt(boneID).Key;

            if (bone == "face")
                return MEPDict.OEBoneID.FirstOrDefault(x => x.Key == "face_c_n").Value;
            else
            {
                int nIdx = bone.IndexOf("_n");

                if (nIdx >= 0)
                    if (!bone.Contains("_l") && !bone.Contains("_r"))
                        bone = bone.Insert(nIdx, "_c");

                return MEPDict.OEBoneID.FirstOrDefault(x => x.Key == bone).Value;
            }
        }
    }
}
