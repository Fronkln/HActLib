using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    public enum InvulnerabilityParam
    {
        Unhittable,
        Unflinchable,
        //Unknown,
       //Unknown2
    }

    [ElementID(Game.Y6, 0x80)]
    [ElementID(Game.YK2, 0x80)]
    [ElementID(Game.JE, 0x80)]
    [ElementID(Game.YLAD, 0x7D)]
    [ElementID(Game.LJ, 0x7D)]
    [ElementID(Game.Gaiden, 0x7D)]
    [ElementID(Game.Y8, 0x7D)]
    public class DETimingInfoMuteki : DETimingInfo
    {
    }
}
