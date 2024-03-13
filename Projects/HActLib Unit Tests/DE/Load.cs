using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit;
using NUnit.Framework;

namespace HActLib.UnitTests.DE
{
    public static class Load
    {
        [Test]
        public static void LoadYLAD1()
        {
            CMN cmn = CMN.Read(@"G:\Yakuza Like a Dragon\runtime\media\data\hact_yazawa\jh23760_buki_n.par.unpack\cmn.par.unpack\cmn_vo.bin", Game.YLAD);
        }
    }
}