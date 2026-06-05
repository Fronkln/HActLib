using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;
using Yarhl.FileFormat;
using HActLib.OOE;
using HActLib.Internal;
using System.Runtime;
using System.Reflection.Metadata;



namespace HActLib
{
    public class OEToOOEConversionInfo
    {
        public OECMN Cmn;
        public CSVHAct CsvData;

        public string CMNPath;
    }

    public class OEToOOE : IConverter<OEToOOEConversionInfo, TEV>
    {
        private bool m_cameraSpecialNodesProcessed = false;

        public OEToOOEConversionInfo inf;
        CSVHAct csvData;

        RES res;

        public TEV Convert(OEToOOEConversionInfo inf)
        {
            this.inf = inf;
            csvData = inf.CsvData;

            CMN.LastHActDEGame = Game.Y3;

            HActDir dir = new HActDir();
            dir.Open(new FileInfo(inf.CMNPath).Directory.Parent.FullName);

            HActDir[] ress = dir.GetResources();

            if (ress.Length > 0)
                res = RES.Read(ress[0].FindFile("res.bin").Read(), false);

            TEV tev = new TEV();
            
            
            //not yet.
            //tev.CuesheetIDs = inf.Cmn.SoundInfo.Select(x => (ushort)((x >> 16) & 0xFFFF)).Distinct().Where(x => x < 0x8000).Select(x => (uint)x).ToList(); // new List<uint>();

            tev.Root = (ObjectBase)Convert(inf.Cmn, inf.Cmn.AllNodes[0], null, null)[0];


            return tev;
        }
       
        public List<ITEVObject> Convert(OECMN cmn, Node node, Node parent, ObjectBase tevParent)
        {
            List<ITEVObject> createdNodes = new List<ITEVObject>();

            ITEVObject createdNode = null;

            switch(node.Category)
            {
                case AuthNodeCategory.Path:
                    ObjectPath ooePath = new ObjectPath();
                    createdNode = ooePath;

                    break;

                case AuthNodeCategory.Camera:
                    createdNode = new ObjectCamera();
                    break;

                case AuthNodeCategory.Character:
                    createdNode = GenerateHuman(node as OENodeCharacter);
                    break;

                case AuthNodeCategory.Model_node:
                    createdNode = GenerateBone(node as NodeModel);
                    break;

                case AuthNodeCategory.CharacterMotion:
                    NodeMotionBase oeCharaMot = node as NodeMotionBase;
                    Set2ElementMotion charaMot = new Set2ElementMotion();
                    charaMot.Type = Set2NodeCategory.ModelMotion;

                    charaMot.Start = oeCharaMot.Start;
                    charaMot.End = oeCharaMot.End;
                    charaMot.Resource = res.FindByGUID(oeCharaMot.Guid).MainResource += ".gmt";

                    createdNode = charaMot;

                    break;

                case AuthNodeCategory.CameraMotion:
                    NodeMotionBase oeCamMot = node as NodeMotionBase;
                    Set2ElementMotion camMot = new Set2ElementMotion();
                    camMot.Type = Set2NodeCategory.CameraMotion;

                    camMot.Start = oeCamMot.Start;
                    camMot.End = oeCamMot.End;
                    camMot.Resource = res.FindByGUID(oeCamMot.Guid).MainResource += ".cmt";

                    createdNode = camMot;
                    break;

                case AuthNodeCategory.Element:
                    EffectBase effect = GenerateEffect(node as NodeElement, parent, tevParent);

                    if (effect != null)
                        createdNode = effect;
                    break;

            }

            if (createdNode != null)
                createdNodes.Add(createdNode);


            if(createdNode is ObjectBase)
            {
                ObjectBase obj = createdNode as ObjectBase;

                foreach(Node child in node.Children)
                {
                    List<ITEVObject> convertedChilds = Convert(cmn, child, node, obj);

                    foreach(ITEVObject childies in convertedChilds)
                    {
                        ObjectBase childObject = childies as ObjectBase;

                        if (childObject == null)
                            continue;

                        childObject.Parent = obj;
                    }

                    obj.Children.AddRange(convertedChilds);
                }
            }

            return createdNodes;
        }

        private ObjectHuman GenerateHuman(OENodeCharacter chara)
        {
            ObjectHuman human = new ObjectHuman();
            human.Height = (int)chara.Height;
            human.Replace = chara.Name.Replace("ZA_HU", "");

            switch(human.Replace)
            {
                case "PLAYER":
                    human.Replace = "KIRYU";
                    break;
            }


            return human;
        }

        private ObjectBone GenerateBone(NodeModel oeBone)
        {
            ObjectBone bone = new ObjectBone();

            if (inf.Cmn.Version >= 15)
            {
                string boneName = "";

                int oeBoneID = oeBone.BoneID;
                var oeKv = MEPDict.OEBoneID.FirstOrDefault(x => x.Value == oeBoneID);

                if (!string.IsNullOrEmpty(oeKv.Key))
                    boneName = OEEffect.ConvertY0BoneNameToY3Name(oeKv.Key);

                if (boneName == null)
                    boneName = bone.BoneName;

                bone.BoneName = boneName;

                if (MEPDict.Y3BoneID.ContainsKey(bone.BoneName))
                    bone.BoneID = MEPDict.Y3BoneID[bone.BoneName];
            }
            else
            {
                bone.BoneName = oeBone.BoneName.Text;
                bone.BoneID = oeBone.BoneID;

                var y5Kv = MEPDict.OOEBoneID.FirstOrDefault(x => x.Value == bone.BoneID);

                if(!string.IsNullOrEmpty(y5Kv.Key))
                {
                    bone.BoneID = MEPDict.Y3BoneID[y5Kv.Key];
                }
            }


            return bone;
        }

        private EffectBase GenerateEffect(NodeElement element, Node parent, ObjectBase tevParent)
        {
            Game game;

            EffectBase effect = null;
            
            switch(inf.Cmn.Version)
            {
                default:
                    game = Game.YK1;
                    break;
                case 10:
                    game = Game.Y5;
                    break;
                case 15:
                    game = Game.Ishin;
                    break;
                case 16:
                    game = Game.Y0;
                    break;
            }

            switch(Reflection.GetElementNameByID(element.ElementKind, game))
            {
                case "e_auth_element_particle":
                    effect = GenerateParticle(element as OEParticle, parent);
                    break;
                case "e_auth_element_sound":
                    effect = GenerateSound(element as OEElementSE);
                    break;
            }

            if(effect != null)
            {
                EffectElement ooeElement = effect as EffectElement;

                if(ooeElement != null)
                {
                    ooeElement.Start = element.Start;
                    ooeElement.End = element.End;
                    ooeElement.ElementFlags = 256;

                    if (tevParent != null)
                        if (tevParent.Type == ObjectNodeCategory.Bone)
                            ooeElement.BoneID = (tevParent as ObjectBone).BoneID;
                }

                effect.Guid = Guid.NewGuid();
            }


            return effect;
        }

        private EffectParticle GenerateParticle(OEParticle particle, Node parent)
        {
            EffectParticle ptc = new EffectParticle();
            ptc.ParticleID = particle.ParticleID;
            ptc.Matrix = particle.Matrix;
            ptc.Flag = (EffectParticleFlags)particle.Flags;
            ptc.Scale = particle.Scale;

            if(parent != null)
            {
                if (parent.Category == AuthNodeCategory.Camera)
                    ptc.Flag |= EffectParticleFlags.Screen;
                else
                    ptc.Flag &= ~EffectParticleFlags.Screen;
            }

            return ptc;
        }

        private EffectSound GenerateSound(OEElementSE oeSound)
        {
            EffectSound sound = new EffectSound();

            if (oeSound.IsGVSound())
                sound.CuesheetID = 0;
            else
                sound.CuesheetID = oeSound.Cuesheet;

            sound.SoundID = oeSound.Sound;

            sound.Start = (int)oeSound.Start;
            sound.End = (int)oeSound.End;

            return sound;
        }
    }
}
