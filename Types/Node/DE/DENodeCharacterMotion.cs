using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;
using Yarhl.FileFormat;


namespace HActLib
{
    public class DENodeCharacterMotion : NodeMotionBase
    {
        public GameTick MotionTick = new GameTick();
        public Matrix4x4 PreviousMotionMatrix = Matrix4x4.Default;

        public DENodeCharacterMotion()
        {
            Category = AuthNodeCategory.CharacterMotion;
        }

        internal override void ReadNodeData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Flags = reader.ReadUInt32();

            if (inf.version < 19)
            {
                Start.Tick = reader.ReadUInt32();
                End.Tick = reader.ReadUInt32();
                MotionTick.Tick = reader.ReadUInt32();
            }
            else
            {
                Start = new GameTick(new GameTick2(reader.ReadUInt32()).Frame);
                End = new GameTick(new GameTick2(reader.ReadUInt32()).Frame);
                MotionTick = new GameTick(new GameTick2(reader.ReadUInt32()).Frame);
            }
            PreviousMotionMatrix =  (Matrix4x4)ConvertFormat.With<Matrix4x4Convert>(
                new ConversionInf(new BinaryFormat(reader.Stream), CMN.IsDE(version) ? EndiannessMode.LittleEndian : EndiannessMode.BigEndian));
        }

        internal override void WriteNodeData(DataWriter writer, GameVersion version, uint hactVer)
        {
            WriteCoreData(writer, version, hactVer);

            writer.Write(Flags);
            if (hactVer < 19)
            {
                writer.Write(Start.Tick);
                writer.Write(End.Tick);
                writer.Write(MotionTick.Tick);
            }
            else
            {
                writer.Write(new GameTick2(Start.Frame).Tick);
                writer.Write(new GameTick2(End.Frame).Tick);
                writer.Write(new GameTick2(MotionTick.Frame).Tick);
            }
            writer.Write(PreviousMotionMatrix);
        }

        internal override int GetSize(GameVersion version, uint hactVer)
        {
            return 80 / 4;
        }
    }
}
