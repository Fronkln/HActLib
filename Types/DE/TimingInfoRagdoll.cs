using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HActLib
{

    [Yarhl.IO.Serialization.Attributes.Serializable]
    public class TimingInfoRagdoll
    {
        public uint Param { get; set; }
        
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }
        public float VelocityZ { get; set; }
      
        public int Flag { get; set; }
       
        public float AngularX { get; set; }
        public float AngularY { get; set; }
        public float AngularZ { get; set; }

        public uint Unk_LJ1;
    }
}
