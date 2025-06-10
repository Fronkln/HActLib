using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HActLib
{
    [Flags]
    public enum BattleColliball : long
    {
        invalid = 0,
        face = 1 << 1,
        ude3_r = 1 << 2,
        ude3_l = 1 << 3,
        asi3_r = 1 << 4,
        asi3_l = 1 << 5,
        kosi = 1 << 6,
        mune = 1 << 7,
        sync = 1 << 8,
        ude1_r = 1 << 9,
        ude1_l = 1 << 10,
        ude2_r = 1 << 11,
        ude2_l = 1 << 12,
        asi1_r = 1 << 13,
        asi1_l = 1 << 14,
        asi2_r = 1 << 15,
        asi2_l = 1 << 16,
        ketu = 1 << 17,
        buki_r = 1 << 18,
        buki_l = 1 << 19,
    }
}
