using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    public enum ConditionTable : uint
    {
        invalid = 0,
        program_flag = 1,
        dragon_boost = 2,
        attack_hit = 3,
        player = 4,
        not_player = 5,
        dragon_boost_damage = 6,
        human_hit = 7,
        game_damage = 8,
        human_purge = 9,
        language_japanese = 10,
        language_chinese_t = 11,
        shop = 12,
        eating = 13,
        shop_range = 14,
        stage = 15,
        dispose_range = 16,
        reminiscence = 17,
        main_chara = 18,
        stage_daynight = 19,
        hold_baby = 20,
        hold_baby_include_stalker = 21,
        btl_style_speed = 22,
        btl_style_power = 23,
        atk_inside = 24,
        atk_outside = 25,
        project = 26,
        battle_skill = 27,
        player_skill = 28,
        haruka_onedari = 29,
        item_special = 30,
        player_type = 31,
        language_korean = 32,
        player_hact = 33,
        is_drama_talk = 34,
        is_boss = 35,
        camera = 36,
        chara_body_type = 37,
        game_variable = 38,
        timeline_result = 39,
        photo_mission_result = 40,
        talk_flag = 41,
        play_talk_count = 42,
        sync_chara_body_type = 43,
        chara_height = 44,
        sync_chara_height = 45,
        out_doors = 46,
        in_doors = 47,
        only_human = 48,
        only_animal = 49,
        only_machine = 50,
        cabaret_yoasobi_chara_emotion_lv = 51,
        character_man = 52,
        character_woman = 53,
        only_demi_human = 54,
        unknown = 55,
        elevator_type = 56,
        timeline_clock_range = 57,
        character_class = 58
    }

    public class DENodeConditionFolder : Node
    {
        public uint Unknown1;
        public ConditionTable Condition;
        public int Index;
        public int TagValue;
        public uint DataFlag;
        public string ConditionTag; //64
        public Guid TagGUID;
        public ulong TagUID;
        public uint PuidID;
        public uint Unknown2;

        
        public byte[] test;

        internal override void ReadNodeData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Condition = (ConditionTable)reader.ReadUInt32();
            Index = reader.ReadInt32();
            TagValue = reader.ReadInt32();
            DataFlag = reader.ReadUInt32();
            ConditionTag = reader.ReadString(64);
            TagGUID = new Guid(reader.ReadBytes(16));
            TagUID = reader.ReadUInt64();
            PuidID = reader.ReadUInt32();
            Unknown2 = reader.ReadUInt32();

            /*
            if (version >= GameVersion.DE2)
                reader.Stream.Position += 32;
            else
                reader.Stream.Position += 28;
            */
        }

        internal override void WriteNodeData(DataWriter writer, GameVersion version, uint hactVer)
        {
            base.WriteNodeData(writer, version, hactVer);

            writer.Write((uint)Condition);
            writer.Write(Index);
            writer.Write(TagValue);
            writer.Write(DataFlag);
            writer.Write(ConditionTag.ToLength(64));
            writer.Write(TagGUID.ToByteArray());
            writer.Write(TagUID);
            writer.Write(PuidID);
            writer.Write(Unknown2);

            /*
            if(version >= GameVersion.DE2)
                writer.WriteTimes(0, 32);
            else
                writer.WriteTimes(0, 28);
           */
        }
    }
}
