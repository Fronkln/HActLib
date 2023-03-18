using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;


namespace HActLib.UnitTests.Conversion
{
    public class OOE2DE
    {
        [Test]
        public void OOETODE_1()
        {
            string hactDir = @"D:\Program Files (x86)\Steam\steamapps\common\Yakuza 3\data\hact\hact_all.par.unpack\7035.par.unpack";
            string csvPath = @"D:\Program Files (x86)\Steam\steamapps\common\Yakuza 3\data\hact\hact_csv.par.unpack\hact_csv\hact_csv.bin";

            Assert.IsTrue(HActFactory.ConvertOOEToDE(@"D:\Program Files (x86)\Steam\steamapps\common\Yakuza 3\data\hact\hact_all_SRC\3270_hires", "out_ooe", Game.YLAD, csvPath));
        }

        [Test]
        public void OOETODE_2()
        {
            string hactDir = @"D:\Program Files (x86)\Steam\steamapps\common\Yakuza 3\data\hact\hact_all.par.unpack\7035.par.unpack";
            string csvPath = @"D:\Program Files (x86)\Steam\steamapps\common\Yakuza 3\data\hact\hact_csv.par.unpack\hact_csv\hact_csv.bin";

            Assert.IsTrue(HActFactory.ConvertOOEToDE(@"D:\Program Files (x86)\Steam\steamapps\common\Yakuza 3\data\hact\hact_all.par.unpack\1750.par.unpack", "out_ooe_kanda", Game.YLAD, csvPath));
        }
    }
}
