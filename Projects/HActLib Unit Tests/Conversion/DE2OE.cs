using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;

namespace HActLib.UnitTests.Conversion
{
    internal class DE2OE
    {
        [Test]
        public void DE_TO_OE_1()
        {
            string path = System.IO.Directory.GetCurrentDirectory();
            Assert.IsTrue(HActFactory.ConvertDEToOE(@"D:\Program Files (x86)\Steam\steamapps\common\Lost Judgment\runtime\media\data\hact_coyote\jh27960_kai_tower_bridge.par.unpack", "out_16", Game.YLAD, 16));
        }

        [Test]
        public void DE_TO_OE_2()
        {
            Assert.IsTrue(HActFactory.ConvertDEToOE(@"C:\Users\orhan\Downloads\hact_judge\hact_judge\jc10020_tukamari_police_w.par.unpack", "out_16", Game.YK2, 16));
        }
    }
}
