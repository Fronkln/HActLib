using System.Collections.Generic;
using System.Linq;
using Yarhl.IO.Serialization.Attributes;

namespace HActLib
{
    [Serializable]
    public class Transition
    {
        public int DestinationPageIndex { get; set; }
        public int ConditionCount { get; set; }
        public int ConditionSize { get; set; }

        [BinaryString(MaxSize = 4, FixedSize = 4)]
        public string Padding { get; set; }

        public List<Condition> Conditions = new List<Condition>();

        public Transition()
        {

        }

        public Transition(int destinationPageIndex, params Condition[] conds)
        {
            DestinationPageIndex = destinationPageIndex;
            Conditions = conds.ToList();
        }

        public Transition Clone()
        {
            return (Transition)MemberwiseClone();
        }

        //Get the size of the conditions. Used for writing
        public int GetConditionSize()
        {
            int size = 0;

            foreach (Condition cond in Conditions)
            {
                size += 16;
                size += cond.Size();
            }

            return size;
        }
    }
}
