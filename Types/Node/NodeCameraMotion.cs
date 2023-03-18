using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    public class NodeCameraMotion : NodeMotionBase
    {
        public NodeCameraMotion()
        {
            Category = AuthNodeCategory.CameraMotion;
        }
    }
}
