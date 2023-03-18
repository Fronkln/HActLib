using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;

namespace HActLib.UnitTests.DE
{
    public class ReadAndWrite
    {
        [Test]
        public void ReadWrite1()
        {
            CMN cmn = CMN.Read(@"G:\Yakuza Like a Dragon\runtime\media\mods\Brawler\hact_src\test_hact\cmn\cmn.bin", Game.YK2);
            CMN.Write(cmn, @"G:\Yakuza Like a Dragon\runtime\media\mods\Brawler\hact_src\test_hact\cmn\cmn_test1.bin");
        }
    }
}
