using System;
using Yarhl.IO;

namespace HActLib
{
    public class CSVCondition
    {
        public CSVConditionType Type;
        public string[] Resources = new string[3];

        public byte[] UnreadData;

        /// <summary>
        /// Does not include resources and type, only direct data
        /// </summary>
        public virtual void Read(DataReader reader)
        {

        }

        public virtual void Write(DataWriter writer) 
        {
        }
    }
}
