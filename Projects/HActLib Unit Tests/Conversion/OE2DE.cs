using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;


namespace HActLib.UnitTests.Conversion
{
    public class OE2DE
    {
        [Test]
        public void OETODE_1()
        {
            Assert.IsTrue(HActFactory.ConvertOEToDE(@"G:\Program Dosyaları (x86)\SteamLibrary\steamapps\common\Yakuza 0\media\data\hact.par.unpack\h3090_pullout_tooth.par.unpack", "out", Game.YLAD));
        }

        [Test]
        public void OETODE_2()
        {
            Assert.IsTrue(HActFactory.ConvertOEToDE(@"H:\steamapps2\steamapps\common\Yakuza 5\main\data\hact\A31100_chase_tsukamari.par.unpack\a31100_chase_tsukamari", "out2", Game.YLAD));
        }

        [Test]
        public void OETODE_3()
        {
            Assert.IsTrue(HActFactory.ConvertOEToDE(@"H:\steamapps2\steamapps\common\Yakuza Kiwami\media\data\hact.par.unpack\h6195_nishiki_fight_02.par.unpack", "out3", Game.YLAD));
        }
    }
}
