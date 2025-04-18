﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Yarhl.IO;

namespace HActLib
{
    public class NodeAsset : Node
    {
        public uint AssetID;
        public uint OffsetType;
        public ulong UID;
        public int IsKeepPose;
        public uint ReplaceID;
        public int IsUseTrans;
        public int IsUnUseTrans;
        public int DisableShadow;
        public int InitCalcMatrix;
        public int EnableAssetPositionUse;
        public int IsReplaceDisposeOnly;
        public float ReplacePosX;
        public float ReplacePosY;
        public float ReplacePosZ;
        public float ReplaceOrientX;
        public float ReplaceOrientY;
        public float ReplaceOrientZ;
        public float ReplaceOrientW;


        private float UnkGD1;
        private float UnkGD2;
        private float UnkGD3;
        private float UnkGD4;
        private float UnkGD5;


        //44
        private byte[] unk_LJ; //LJ ONLY

        internal override void ReadNodeData(DataReader reader, NodeConvInf inf, GameVersion version)
        {

            int nodeSize = NodeSize * 4;
            bool isNewDE = nodeSize > 32;

            AssetID = reader.ReadUInt32();
            OffsetType = reader.ReadUInt32();

            if (isNewDE)
                UID = reader.ReadUInt64();

            IsKeepPose = reader.ReadInt32();
            ReplaceID = reader.ReadUInt32();

            IsUseTrans = reader.ReadInt32();
            IsUnUseTrans = reader.ReadInt32();
            DisableShadow = reader.ReadInt32();
            InitCalcMatrix = reader.ReadInt32();

            if(isNewDE)
            {
                EnableAssetPositionUse = reader.ReadInt32();
                IsReplaceDisposeOnly = reader.ReadInt32();


                if (nodeSize > 48)
                {
                    ReplacePosX = reader.ReadSingle();
                    ReplacePosY = reader.ReadSingle();
                    ReplacePosZ = reader.ReadSingle();

                    ReplaceOrientX = reader.ReadSingle();
                    ReplaceOrientY = reader.ReadSingle();
                    ReplaceOrientZ = reader.ReadSingle();
                    ReplaceOrientX = reader.ReadSingle();

                    if (CMN.LastHActDEGame == Game.LJ)
                        unk_LJ = reader.ReadBytes(44);
                    if (CMN.LastHActDEGame >= Game.LAD7Gaiden) //Shrink node, no more unk LJ
                    {
                        UnkGD1 = reader.ReadSingle();
                        UnkGD2 = reader.ReadSingle();
                        UnkGD3 = reader.ReadSingle();
                        UnkGD4 = reader.ReadSingle();
                        UnkGD5 = reader.ReadSingle();
                    }
                }
            }
        }

        internal override void WriteNodeData(DataWriter writer, GameVersion version, uint hactVer)
        {
            base.WriteNodeData(writer, version, hactVer);
            bool isNewDE = version == GameVersion.DE2 || version == GameVersion.DE1;

            
            if (CMN.IsDE(version))
            {
                writer.Write(AssetID);
                writer.Write(OffsetType);

                if (isNewDE)
                    writer.Write(UID);

                if (isNewDE)
                {
                    writer.Write(IsKeepPose);
                    writer.Write(ReplaceID);

                    writer.Write(IsUseTrans);
                    writer.Write(IsUnUseTrans);
                    writer.Write(DisableShadow);
                    writer.Write(InitCalcMatrix);

                    writer.Write(EnableAssetPositionUse);
                    writer.Write(IsReplaceDisposeOnly);
                    if (version >= GameVersion.DE2 || unk_LJ != null)
                    {
                        writer.Write(ReplacePosX);
                        writer.Write(ReplacePosY);
                        writer.Write(ReplacePosZ);

                        writer.Write(ReplaceOrientX);
                        writer.Write(ReplaceOrientY);
                        writer.Write(ReplaceOrientZ);
                        writer.Write(ReplaceOrientW);
                    }
                }
                else
                {
                    //its am ess lol
                    writer.Write(IsKeepPose);
                    writer.WriteTimes(0, 4); //unk
                    writer.Write(ReplaceID);
                    writer.Write(IsUnUseTrans);
                    writer.Write(DisableShadow);
                    writer.Write(InitCalcMatrix);

                }

                if (CMN.LastHActDEGame == Game.LJ)
                {
                    if (unk_LJ == null)
                        unk_LJ = new byte[44];

                    writer.Write(unk_LJ);
                }

                if(CMN.LastHActDEGame >= Game.LAD7Gaiden)
                {
                    writer.Write(UnkGD1);
                    writer.Write(UnkGD2);
                    writer.Write(UnkGD3);
                    writer.Write(UnkGD4);
                    writer.Write(UnkGD5);
                }
            }
            else
            {

            }
            
        }

        internal override int GetSize(GameVersion version, uint hactVer)
        {
            if (version >= GameVersion.DE2)
            {
                if (unk_LJ != null)
                    return 120 / 4;
                return 76 / 4;
            }
            else if (version == GameVersion.DE1)
                return 48 / 4;
            else if (CMN.VersionEqualsLess(version, GameVersion.Yakuza6))
                return 32 / 4;

            return 0xdead;
        }
    }
}
