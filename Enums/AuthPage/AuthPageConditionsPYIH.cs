using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HActLib
{
    public enum AuthPageConditionsPYIH : uint
    {
        dmy = 0x0,
        random = 0x1,
        scenario_timeline_results = 0x2,
        play_count = 0x3,
        program_param = 0x4,
        distance = 0x5,
        pad_button = 0x6,
        shop_trade_item = 0x7,
        loop_count = 0x8,
        page_end = 0x9,
        select_menu = 0xA,
        battle = 0xB,
        ai_finish = 0xC,
        clock_pkg = 0xD,
        hact_condition_flag = 0xE,
        player_point = 0xF,
        ui_finish = 0x10,
        no_speech = 0x11,
        scenario_timeline_clock_range = 0x12,
        item_giving = 0x13,
        skip = 0x14,
        mg_caba_rank = 0x15,
        item_have = 0x16,
        page_play_count = 0x17,
        finish_reward = 0x18,
        special_favor = 0x19,
        tgs = 0x1A,
        not_download_chunk = 0x1B,
        taxi_select = 0x1C,
        stage_warp = 0x1D,
        playing_drama_talk = 0xE,
        drunk_level = 0xF,
        talk_text_end = 0x20,
    }
}
