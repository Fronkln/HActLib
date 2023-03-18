using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib.OOE
{
    public class Set2ElementMotion : Set2Element
    {
        public override string GetName()
        {
            switch(Type)
            {
                default:
                    return "Motion (Unknown)";
                case Set2NodeCategory.PathMotion:
                    return "Path Motion";
                case Set2NodeCategory.ModelMotion:
                    return "Model Motion";
                case Set2NodeCategory.CameraMotion:
                    return "Camera Motion";
            }         
           
        }

    }
}
