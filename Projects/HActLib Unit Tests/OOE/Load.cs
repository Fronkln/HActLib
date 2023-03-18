using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;

namespace HActLib.UnitTests.OOE
{
    public static class Load
    {
        [Test]
        public static void LoadY3_1()
        {
            TEV tev = TEV.Read(@"D:\Program Files (x86)\Steam\steamapps\common\Yakuza 3\data\hact\hact_all_SRC\3270_hires\tmp\hact_tev.bin");
        }

        [Test]
        public static void LoadY3CSV()
        {
            CSV csv = CSV.Read(@"D:\Program Files (x86)\Steam\steamapps\common\Yakuza 3\data\hact\hact_csv.par.unpack\hact_csv\hact_csv.bin");
        }
    }
}
