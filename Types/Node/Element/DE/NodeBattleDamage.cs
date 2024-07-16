using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0x61)]
    [ElementID(Game.YK2, 0x61)]
    [ElementID(Game.JE, 0x61)]
    [ElementID(Game.YLAD, 0x5E)]
    [ElementID(Game.LJ, 0x5E)]
    [ElementID(Game.LAD7Gaiden, 0x5E)]
    [ElementID(Game.LADIW, 0x5E)]
    public class NodeBattleDamage : NodeElement
    {

        public uint Damage;
        public int ForceDead;
        public bool NoDead;
        public int Vanish;
        public int Fatal;

        public int Recover;
        public int TargetSync;
        public uint Attacker;
        public int DirectDamage;
        public int DirectDamageIsHpRatio;
        public int AttackId;


        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Damage = reader.ReadUInt32();
            ForceDead = reader.ReadInt32();
            NoDead = reader.ReadInt32() > 0;
            Vanish = reader.ReadInt32();

            int nodeSize = NodeSize * 4;


            if (Version == 1 || nodeSize == 64)
            {
                Fatal = reader.ReadInt32();
                Recover = reader.ReadInt32();
                TargetSync = reader.ReadInt32();
                Attacker = reader.ReadUInt32();
            }
            if (Version == 1)
            {
                DirectDamage = reader.ReadInt32();
                DirectDamageIsHpRatio = reader.ReadInt32();
                AttackId = reader.ReadInt32();
            }

        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            if (version >= GameVersion.DE2)
                Version = 1;
            else
                Version = 0;

            writer.Write(Damage);
            writer.Write(ForceDead);
            writer.Write(Convert.ToInt32(NoDead));
            writer.Write(Vanish);

            if (version == GameVersion.DE1 || Version == 1)
            {
                writer.Write(Fatal);
                writer.Write(Recover);
                writer.Write(TargetSync);
                writer.Write(Attacker);
            }
            if (Version == 1)
            {
                writer.Write(DirectDamage);
                writer.Write(DirectDamageIsHpRatio);
                writer.Write(AttackId);
            }
        }

        internal override int GetSize(GameVersion version, uint hactVer)
        {
            switch (Version)
            {
                default:
                    throw new Exception("Unknown version " + Version);

                case 0:
                    if (version == GameVersion.DE1)
                        return 16;
                    return 12;
                case 1:
                    return (48 + 28) / 4;
            }
        }

    }
}
