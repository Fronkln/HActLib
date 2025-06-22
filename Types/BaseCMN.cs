using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HActLib
{
    public abstract class BaseCMN
    {
        public virtual uint Version { get; set; }

        public virtual float HActStart { get; set; }
        public virtual float HActEnd { get; set; }

        public Node Root = null;

        public virtual Node[] GetNodes() { throw new NotImplementedException(); }

        public List<DisableFrameInfo> DisableFrameInfo = new List<DisableFrameInfo>();
        public float[] CutInfo = new float[0];
        public float[] ResourceCutInfo = new float[0];

        public virtual float GetChainCameraIn() { throw new NotImplementedException(); }
        public virtual void SetChainCameraIn(float val) { throw new NotImplementedException(); }

        public virtual float GetChainCameraOut() { throw new NotImplementedException(); }
        public virtual void SetChainCameraOut(float val) { throw new NotImplementedException(); }

        public virtual uint GetFlags() { throw new NotImplementedException(); }
        public virtual void SetFlags(uint val) { throw new NotImplementedException(); }

        public virtual int GetNodeDrawNum() { throw new NotImplementedException(); }
        public virtual void SetNodeDrawNum(int val) { throw new NotImplementedException(); }

        public Node FindNodeByGUID(Guid guid) { return GetNodes().FirstOrDefault(x => x.Guid == guid); }

        public NodeElement[] AllElements
        {
            get
            {
                List<NodeElement> elements = new List<NodeElement>();

                void Process(Node node)
                {
                    if (node.Category == AuthNodeCategory.Element)
                        elements.Add(node as NodeElement);

                    foreach (Node child in node.Children)
                        Process(child);
                }

                Process(Root);

                return elements.ToArray();
            }
        }

        public NodeCamera[] AllCameras
        {
            get
            {
                List<NodeCamera> cameras = new List<NodeCamera>();

                void Process(Node node)
                {
                    if (node.Category == AuthNodeCategory.Camera)
                        cameras.Add(node as NodeCamera);

                    foreach (Node child in node.Children)
                        Process(child);
                }

                Process(Root);

                return cameras.ToArray();
            }
        }


        public static BaseCMN ReadGeneric(string cmnPath, Game game)
        {
            if(CMN.IsDEGame(game))
                return CMN.Read(cmnPath, game);
            else
                return OECMN.Read(cmnPath);
        }

        public static void WriteGeneric(string cmnPath, BaseCMN cmn)
        {
            if (cmn is CMN)
                CMN.Write(cmn as CMN, cmnPath);
            else
                OECMN.Write(cmn as OECMN, cmnPath);
        }
    }
}
