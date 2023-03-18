using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;

namespace HActLib.UnitTests.Common
{
    
    internal class ResLoad
    {
        [Test]
        public void LoadRes1()
        {
            RES res = RES.Read(@"G:\Yakuza Like a Dragon\runtime\media\mods\Brawler\hact_src\test_hact\000\res.bin", true);
        }

        [Test]
        public void LoadWrite()
        {
            RES res = RES.Read(@"G:\Yakuza Like a Dragon\runtime\media\mods\Brawler\hact_src\test_hact\000\res.bin", true);
            RES.Write(res, @"G:\Yakuza Like a Dragon\runtime\media\mods\Brawler\hact_src\test_hact\000\res_out.bin", true);
        }
        [Test]
        public void ConvertToDE()
        {
            RES res = RES.Read(@"G:\Program Dosyaları (x86)\SteamLibrary\steamapps\common\Yakuza 0\media\data\hact.par.unpack\a8310_appear_rifle_5men.par.unpack\000.par.unpack\res.bin", false);
            RES.Write(res, @"G:\Yakuza Like a Dragon\runtime\media\mods\Brawler\hact_src\test_hact\000\res.bin", true);
        }
    }
}
