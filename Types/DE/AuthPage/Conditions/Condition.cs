using Yarhl.IO;
using Yarhl.IO.Serialization.Attributes;
using Yarhl.FileFormat;
using System;

namespace HActLib
{
    public class ConditionConvert : IConverter<BinaryFormat, Condition>
    {

        public static string GetName(uint cond, Game game)
        {
            Type enumType;

            if (game <= Game.Y6)
                enumType = typeof(AuthPageConditionsY6);
            else
                enumType = typeof(AuthPageConditionsGeneric);

            return Enum.GetName(enumType, cond);
        }

        public static uint GetID(string name, Game game)
        {
            Type enumType;

            if (game == Game.Y6)
                enumType = typeof(AuthPageConditionsY6);
            else
                enumType = typeof(AuthPageConditionsGeneric);

            return (uint)Enum.Parse(enumType, name);
        }

        internal Condition GetCondFromType(uint cond, Game game)
        {
            switch(GetName(cond, game))
            {
                default:
                    return new Condition();
                case "page_play_count":
                    return new ConditionPagePlayCount();
                case "play_count":
                    return new ConditionPlayCount();
                case "hact_condition_flag":
                    return new ConditionHActFlag(game);
                case "enemy_num":
                    return new ConditionEnemyNum();
                case "program_param":
                    return new ConditionProgramParam();
            }
        }


        public Condition Convert(BinaryFormat format)
        {
            DataReader reader = new DataReader(format.Stream) { Endianness = EndiannessMode.LittleEndian, DefaultEncoding = System.Text.Encoding.GetEncoding(932) };
          
            uint condType = reader.ReadUInt32();
            uint parameterSize = reader.ReadUInt32();
           
            reader.ReadBytes(8);
            
            Condition cond = GetCondFromType(condType, CMN.LastHActDEGame);
            cond.ConditionID = condType;

            long paramStart = reader.Stream.Position;
            long paramTargetPos = reader.Stream.Position + parameterSize;

            cond.Read(reader, parameterSize);

            long paramPostRead = reader.Stream.Position;
            long readSize = paramPostRead - paramStart;
            long targetSize = paramTargetPos - paramStart;

            if (paramTargetPos != paramPostRead)
            {
                if (paramTargetPos < paramPostRead)
                    System.Diagnostics.Debug.WriteLine("Read " + (readSize - targetSize) + " more than the intended " + targetSize + "! Condition: " + condType);
                else
                    System.Diagnostics.Debug.WriteLine("Read " + (targetSize - readSize) + " less than the intended " + targetSize + "! Condition: " + condType);

                reader.Stream.Position = paramTargetPos;
            }
          

            return cond;
        }
    }

    [Yarhl.IO.Serialization.Attributes.Serializable]
    public class Condition
    {
        public uint ConditionID { get; set; }
        public byte[] CondBytes;

        public Condition Clone()
        {
            return (Condition)MemberwiseClone();
        }

        internal virtual void Read(DataReader reader, uint parameterSize)
        {
            CondBytes = reader.ReadBytes((int)parameterSize);
        }

        internal virtual void Write(DataWriter writer)
        {
            if(CondBytes != null)
                writer.Write(CondBytes);
        }

        internal virtual int Size()
        {
            return CondBytes.Length;
        }
    }
}
