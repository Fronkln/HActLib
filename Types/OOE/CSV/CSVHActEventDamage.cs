using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    //Unrelated to DAMAGE_99, does 99 scale damage?
    //Seems to be a float in what normally would be the int
    internal class CSVHActEventDamage : CSVHActEvent
    {
        public int Damage;

        internal override void ReadData(DataReader reader)
        {
            Damage = reader.ReadInt32();
        }

        internal override void WriteData(DataWriter writer)
        {
            base.WriteData(writer);

            writer.Write(Damage);
        }
    }
}
