using System;
using Yarhl.IO;

namespace HActLib
{
    [Yarhl.IO.Serialization.Attributes.Serializable]
    public class RimflashParams
    {
        public uint Version { get; set; }

        internal virtual void Read(DataReader reader)
        {

        }
        
        internal virtual void Write(DataWriter writer)
        {

        }
    }
}
