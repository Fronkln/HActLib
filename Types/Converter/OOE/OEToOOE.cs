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
            tev.CuesheetID = 0;

            tev.Root = (ObjectBase)Convert(inf.Cmn, inf.Cmn.AllNodes[0])[0];


            return tev;
        }
       
        public List<ITEVObject> Convert(OECMN cmn, Node node)
        {
            List<ITEVObject> createdNodes = new List<ITEVObject>();

            ITEVObject createdNode = null;

            switch(node.Category)
            {
                case AuthNodeCategory.Path:
                    ObjectPath ooePath = new ObjectPath();

                    Set2ElementMotion pathMotion = new Set2ElementMotion();
                    pathMotion.Type = Set2NodeCategory.PathMotion;
                    pathMotion.Start = 0;
                    pathMotion.End = 120;
                    pathMotion.Resource = "global_center.gmt";

                    ooePath.Children.Add(pathMotion);

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
                    charaMot.Resource = res.FindByGUID(oeCharaMot.Guid).Name += ".gmt";

                    createdNode = charaMot;

                    break;

                case AuthNodeCategory.CameraMotion:
                    NodeMotionBase oeCamMot = node as NodeMotionBase;
                    Set2ElementMotion camMot = new Set2ElementMotion();
                    camMot.Type = Set2NodeCategory.CameraMotion;

                    camMot.Start = oeCamMot.Start;
                    camMot.End = oeCamMot.End;
                    camMot.Resource = res.FindByGUID(oeCamMot.Guid).Name += ".cmt";

                    createdNode = camMot;
                    break;

                case AuthNodeCategory.Element:
                    EffectBase effect = GenerateEffect(node as NodeElement);

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
                    List<ITEVObject> convertedChilds = Convert(cmn, child);

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

                boneName = OEEffect.ConvertY0BoneNameToY5Name(bone.BoneName);

                if (boneName == null)
                    boneName = bone.BoneName;

                bone.BoneName = boneName;

                if (MEPDict.OOEBoneID.ContainsKey(boneName))
                    bone.BoneID = MEPDict.OOEBoneID[boneName];
            }
            else
            {
                bone.BoneName = oeBone.BoneName.Text;
                bone.BoneID = oeBone.BoneID;
            }


            return bone;
        }

        private EffectBase GenerateEffect(NodeElement element)
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
                    effect = GenerateParticle(element as OEParticle);
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
                }

                effect.Guid = Guid.NewGuid();
            }


            return effect;
        }

        private EffectParticle GenerateParticle(OEParticle particle)
        {
            EffectParticle ptc = new EffectParticle();
            ptc.ParticleID = particle.ParticleID;
            ptc.Matrix = particle.Matrix;

            return ptc;
        }

        private EffectSound GenerateSound(OEElementSE oeSound)
        {
            EffectSound sound = new EffectSound();

            return null;
        }
    }
}
