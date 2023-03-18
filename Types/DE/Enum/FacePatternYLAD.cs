using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HActLib
{
    public enum FacePatternYLAD : uint
    {
        invalid,         // constant 0x0
        hit_down1,       // constant 0x1
        hit_down2,       // constant 0x2
        hit_down3,       // constant 0x3
        hit_left1,       // constant 0x4
        hit_left2,       // constant 0x5
        hit_left3,       // constant 0x6
        hit_right1,      // constant 0x7
        hit_right2,      // constant 0x8
        hit_right3,      // constant 0x9
        hit_up1,         // constant 0xA
        hit_up2,         // constant 0xB
        hit_up3,         // constant 0xC
        hit_vomit1,      // constant 0xD
        hit_vomit2,      // constant 0xE
        oneshot_attack1_b,       // constant 0xF
        oneshot_attack2_b,       // constant 0x10
        oneshot_attack3_b,       // constant 0x11
        oneshot_catch_st_b,      // constant 0x12
        oneshot_dmg_d1_b,        // constant 0x13
        oneshot_dmg_f1_b,        // constant 0x14
        oneshot_dmg_l1_b,        // constant 0x15
        oneshot_dmg_r1_b,        // constant 0x16
        oneshot_guard1_b,        // constant 0x17
        oneshot_shout1_a,        // constant 0x18
        oneshot_sway1_b,         // constant 0x19
        pat_anger_s_ab,      // constant 0x1A
        pat_anger1_ab,       // constant 0x1B
        pat_anger2_ab,       // constant 0x1C
        pat_catch_lp_b,      // constant 0x1D
        pat_dead1_ab,        // constant 0x1E
        pat_drink1_ab,       // constant 0x1F
        pat_joy1_a,      // constant 0x20
        pat_joy3_a,      // constant 0x21
        pat_non1_ab,         // constant 0x22
        pat_non2_ab,         // constant 0x23
        pat_pain1_ab,        // constant 0x24
        pat_sad1_a,      // constant 0x25
        pat_scared1_a,       // constant 0x26
        pat_surp1_a,         // constant 0x27
        pat_tension1_a,      // constant 0x28
        pat_think1_a,        // constant 0x29
        pat_zidori_anger2,       // constant 0x2A
        pat_zidori_joy1,         // constant 0x2B
        pat_zidori_joy2,         // constant 0x2C
        pat_zidori_non2,         // constant 0x2D
        pat_zidori_sad1,         // constant 0x2E
        pat_zidori_sad2,         // constant 0x2F
        pat_sleep,       // constant 0x30
        pat_serious_ab,      // constant 0x31
    }
}
