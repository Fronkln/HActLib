using System;
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
    public class OOEToOEConversionInfo
    {
        public TEV Tev;
        public CSVHAct CsvData;
        public uint TargetVer;
    }

    public class OOEToOE : IConverter<OOEToOEConversionInfo, OECMN>
    {
        private bool m_cameraSpecialNodesProcessed = false;

        public OOEToOEConversionInfo inf;
        CSVHAct csvData;

        public OECMN Convert(OOEToOEConversionInfo inf)
        {
            this.inf = inf;
            csvData = inf.CsvData;

            if (inf.TargetVer == 16)
                CMN.LastHActDEGame = Game.Y0;
            else if (inf.TargetVer == 15)
                CMN.LastHActDEGame = Game.Ishin;
            else
                CMN.LastHActDEGame = Game.Y5;

            OECMN cmn = new OECMN();

            cmn.CMNHeader.Start = 0;
            cmn.CMNHeader.End = DetermineHactLength(inf.Tev);
            cmn.CMNHeader.Version = inf.TargetVer;
            cmn.CMNHeader.ChainCameraIn = -1;
            cmn.CMNHeader.ChainCameraOut = -1;
            cmn.CMNHeader.NodeDrawNum = 2;
            cmn.CMNHeader.Flags = 1;

            cmn.CutInfo = new float[] { cmn.CMNHeader.End }; 
            cmn.ResourceCutInfo = new float[] { cmn.CMNHeader.End };

            TEV tev = inf.Tev;

            ObjectBase[] objects = tev.AllObjects;

            for (int i = 1; i < objects.Length; i++)
                if (objects[i].Parent == null && objects[i].Category != 2)
                    tev.Root.Children.Add(objects[i]);

            cmn.Root = Convert(tev, tev.Root, cmn.CMNHeader.End)[0];


            //Branch nodes are in the camera in OE
            foreach(Node branch in cmn.AllElements.Where(x => x.ElementKind == 38 || x.ElementKind == 34 || x.ElementKind == 37 || x.ElementKind == Reflection.GetElementIDByName("e_auth_element_hact_stop_end", CMN.LastHActDEGame)))
            {
                branch.Parent.Children.Remove(branch);
                cmn.Root.Children.Add(branch);
            }

            cmn.Root.Children =  cmn.Root.Children.OrderByDescending(x => x.Category == AuthNodeCategory.Camera).ToList();

            GenerateFrameProgression(tev, cmn);

            return cmn;
        }


        public float DetermineHactLength(TEV tev)
        {
            ObjectCamera cam = (ObjectCamera)tev.AllObjects.FirstOrDefault(x => x is ObjectCamera);

            IEnumerable<Set2Element1019> endNodes = cam.Children.Where(x => x is Set2Element1019).Cast<Set2Element1019>().Where(x => x.Type1019.StartsWith("HE_END_"));
            IEnumerable<Set2Element1019> branchNodes = cam.Children.Where(x => x is Set2Element1019).Cast<Set2Element1019>().Where(x => x.Type1019.StartsWith("HE_BRANCH_"));

            if(branchNodes.Count() <= 0)
            {
                if (endNodes.Count() <= 0)
                    return tev.AllSet2.Where(x => x.Type == Set2NodeCategory.CameraMotion).Max(x => x.End);
                else
                    return endNodes.ElementAt(0).Start;
            }
            else
            {
                return tev.AllSet2.Where(x => x.Type == Set2NodeCategory.CameraMotion).Max(x => x.End);
            }
        }

        public NodePathBase GeneratePath()
        {
            NodePathBase root = new NodePathBase();
            root.Category = AuthNodeCategory.Path;
            root.Guid = Guid.NewGuid();
            root.Matrix = Matrix4x4.Default;
            root.Name = "Path";

            return root;
        }

        public OENodeCharacter GenerateCharacter(TEV tev, ObjectHuman character, float length)
        {
            OENodeCharacter chara = new OENodeCharacter();
            chara.Flag = 3;
            chara.Guid = Guid.NewGuid();

            string name = character.Replace.ToLowerInvariant();

            if (character.Replace == "KIRYU")
                chara.Name = "ZA_HUPLAYER";
            else
            {
                chara.Name = "ZA_HU" + character.Replace;
                chara.Height = 185;
            }

            Set2[] charMotions = character.GetSet2ChildsOfCategory(Set2NodeCategory.ModelMotion);

            foreach(Set2 charMotion in charMotions)
            {
                NodeMotionBase motion = new NodeMotionBase();
                motion.Category = AuthNodeCategory.CharacterMotion;
                motion.Guid = Guid.NewGuid();
                motion.Name = charMotion.Resource;
                motion.Start.Frame = charMotion.Start;
                motion.End.Frame = charMotion.End;
                motion.Priority = 1;

                chara.Children.Add(motion);
            }

            return chara;
        }

        public NodeCamera GenerateCamera(TEV tev, ObjectCamera cam, int length)
        {
            NodeCamera camera = new NodeCamera();

            camera.Name = cam.Name;
            camera.Guid = Guid.NewGuid();
            camera.FrameProgression = new float[length];
            camera.FrameProgressionSpeed = new float[length];
            camera.ProgressionEnd = length + 1;

            for(int i = 0; i < camera.FrameProgression.Length; i++)
            {
                camera.FrameProgression[i] = i;
                camera.FrameProgressionSpeed[i] = 1;
            }

            Set2[] cameraMotions = cam.GetSet2ChildsOfCategory(Set2NodeCategory.CameraMotion);

            foreach(Set2 cameraMotion in cameraMotions)
            {
                NodeCameraMotion motion = new NodeCameraMotion();
                motion.Guid = Guid.NewGuid();
                motion.Name = cameraMotion.Resource;
                motion.Start.Frame = cameraMotion.Start;
                motion.End.Frame = cameraMotion.End;
                motion.Parent = camera;
                motion.Priority = 1;

                camera.Children.Add(motion);
            }

            return camera;
        }

        void GenerateFrameProgression(TEV tev, OECMN hact)
        {
            ObjectCamera cam = tev.Root.GetChildOfType<ObjectCamera>();
            List<Set2> slowMoAreas = cam.Children.Where(x => x is Set2).Cast<Set2>().Where(x => x.Type == Set2NodeCategory.Slowmo).ToList();
            List<OEHActInput> inputs = hact.AllElements.Where(x => x.ElementKind == 34).Cast<OEHActInput>().ToList();

            if (slowMoAreas.Count <= 0)
                return;

            float current = 0;

            List<float> progression = new List<float>() {0, 0};
            List<float> progressionSpeed = new List<float>() {1, 1};

            Set2 GetSlowMoArea(float frame)
            {
                return slowMoAreas.FirstOrDefault(x => frame >= x.Start && frame < x.End);
            }

            do
            {
                if(GetSlowMoArea(current) != null)
                {
                    current += 0.45f;
                    progression.Add(current);
                    progressionSpeed.Add(0.45f);
                }
               else
               {
                    current += 1f;
                    progression.Add(current);
                    progressionSpeed.Add(1);
               }
            } while (current < hact.HActEnd);

            foreach(NodeCamera camera in hact.AllCameras)
            {
                camera.FrameProgression = progression.ToArray();
                camera.FrameProgressionSpeed = progressionSpeed.ToArray();
                camera.CameraFlags = 1;
            }
        }

        public NodeModel GenerateBone(TEV tev, ObjectBone bone)
        {
            NodeModel model = new NodeModel();
            model.Category = AuthNodeCategory.Model_node;
            model.Guid = Guid.NewGuid();

            string boneName = "";

            if (inf.TargetVer >= 15)
            {
                boneName = OEEffect.ConvertY5BoneNameToY0Name(bone.BoneName);

                if (boneName == null)
                    boneName = bone.BoneName;

                if (MEPDict.OEBoneID.ContainsKey(boneName))
                    model.BoneID = MEPDict.OEBoneID[boneName];
            }
            else
            {
                boneName = bone.BoneName;

                if(MEPDict.OOEBoneID.ContainsKey(boneName))
                    model.BoneID = MEPDict.OOEBoneID[boneName];
            }

            model.Name = boneName;
            model.BoneName.Set(boneName);

            return model;
        }

        public List<Node> Convert(TEV tev, ObjectBase set, float hactLen)
        {
            List<Node> createdNodes = new List<Node>();

            Node deNode = null;

            switch (set.Type)
            {
                case ObjectNodeCategory.Camera:
                    deNode = GenerateCamera(tev, set as ObjectCamera, (int)set.GetChildOfType<Set2ElementMotion>().End);
                    break;

                case ObjectNodeCategory.Path:
                    ObjectPath ooePath = set as ObjectPath;

                    deNode = GeneratePath();
                    deNode.Name = ooePath.Description;
                    break;
                case ObjectNodeCategory.HumanOrWeapon:
                    if (set.Category >= 0 && set.Category <= 1)
                        deNode = GenerateCharacter(tev, set as ObjectHuman, hactLen);
                    break;
                case ObjectNodeCategory.Bone:
                    deNode = GenerateBone(tev, set as ObjectBone);
                    break;


            }

            if (deNode != null)
            {
                foreach (ITEVObject setChild in set.Children)
                {
                    List<Node> childConv = null;

                    if (setChild is ObjectBase)
                        childConv = Convert(tev, setChild as ObjectBase, hactLen);
                    else if (setChild is Set2)
                    {
                        Set2 set2Child = setChild as Set2;

                        if (set.Type == ObjectNodeCategory.Camera && set2Child.EffectID == EffectID.Special)
                            if (m_cameraSpecialNodesProcessed)
                                continue;

                        childConv = ConvertSet2(tev, setChild as Set2);
                    }
                    else if (setChild is EffectBase)
                        childConv = ConvertElementEffect(tev, setChild as EffectBase);

                    foreach(Node node in childConv)
                    {
                        node.Parent = deNode;
                        deNode.Children.Add(node);
                    }
                }


                if (set.Type == ObjectNodeCategory.Camera)
                    m_cameraSpecialNodesProcessed = true;
            }

            if (deNode != null)
                createdNodes.Add(deNode);

            return createdNodes;
        }


        public List<Node> ConvertSet2(TEV tev, Set2 set2Obj)
        {
            List<Node> createdNodes = new List<Node>();

            Node convertedNode = null;
        
            switch(set2Obj.EffectID)
            {
                case EffectID.Blood:
                    NodeElement blood = new NodeElement();
                    blood.Name = "Blood";
                    blood.Guid = Guid.NewGuid();
                    blood.Start = set2Obj.Start;
                    blood.End = set2Obj.End;
                    blood.ElementKind = 31;
                    blood.unkBytes = set2Obj.Unk2;

                    convertedNode = blood;
                    break;

                case EffectID.Special:
                    Set2Element1019 set2Special = set2Obj as Set2Element1019;
                    CSVHActEvent specialData = csvData.TryGetHActEventData(set2Special.Type1019);

                    string[] typeSplit = set2Special.Type1019.Split('_');

                    switch(typeSplit[1])
                    {
                        case "DAMAGE":
                            if (typeSplit[2] != "99")
                            {
                                CSVHActEventDamage specialDataDamage = specialData as CSVHActEventDamage;

                                OEDamage damage = new OEDamage();
                                damage.Name = set2Special.Type1019;
                                damage.Guid = Guid.NewGuid();
                                damage.Start = set2Special.Start;
                                damage.End = set2Special.End;
                                damage.ElementKind = 32;

                                damage.Damage = specialDataDamage.Damage;

                                convertedNode = damage;
                            }
                            break;

                        case "GAUGE":
                            CSVHActEventHeatGauge specialDataGauge = specialData as CSVHActEventHeatGauge;

                            OEHeat heat = new OEHeat();
                            heat.Name = set2Special.Type1019;
                            heat.Guid = Guid.NewGuid();
                            heat.Start = set2Special.Start;
                            heat.End = set2Special.End;
                            heat.ElementKind = Reflection.GetElementIDByName("e_auth_element_heat_change", CMN.LastHActDEGame);

                            //Try to adjust heat change to be more in line with OE standards
                            //OE has things like -150 while OOE has -8000 on waking wrath which consumes 75% of gauge.
                            heat.HeatChange = (int)(specialDataGauge.Change * 0.015f);

                            convertedNode = heat;
                            break;

                        case "BRANCH":
                            OEHActBranch branch = new OEHActBranch();
                            branch.Name = set2Special.Type1019;
                            branch.Guid = Guid.NewGuid();
                            branch.Start = set2Special.Start;
                            branch.End = set2Special.End;
                            branch.ElementKind = 38;

                            convertedNode = branch;
                            break;
                        case "BUTTON":
                            if (typeSplit[2] != "99")
                            {
                                OEHActInput button = new OEHActInput();
                                button.Name = set2Special.Type1019;
                                button.Guid = Guid.NewGuid();
                                button.Start = set2Special.Start;
                                button.End = set2Special.End;
                                button.ElementKind = 34;
                                button.Timing = new GameTick(button.End - button.Start).Tick;

                                convertedNode = button;
                            }
                            break;
                        case "END":
                            OEHActEnd end = new OEHActEnd();
                            end.Name = set2Special.Type1019;
                            end.Guid = Guid.NewGuid();
                            end.Start = set2Special.Start;
                            end.End = set2Special.Start;
                            end.ElementKind = 37;
                            end.Unknown = 1;
                            end.Priority = 3;

                            convertedNode = end;
                            break;
                    }
                    break;
            }

            if (convertedNode != null)
                createdNodes.Add(convertedNode);

            return createdNodes;
        }

        public List<Node> ConvertElementEffect(TEV tev, EffectBase effect)
        {
            List<Node> createdElements = new List<Node>();

            NodeElement createdNode = null;

            switch (effect.ElementKind)
            {
                case EffectID.Particle:
                    OEParticle oeParticle = new OEParticle();
                    EffectParticle ooeParticle = effect as EffectParticle;

                    oeParticle.Category = AuthNodeCategory.Element;
                    oeParticle.ElementKind = 2;
                    oeParticle.Start = ooeParticle.Start;
                    oeParticle.End = ooeParticle.End;
                    oeParticle.ParticleID = ooeParticle.ParticleID;
                    oeParticle.Flags = (int)ooeParticle.Flag;
                    oeParticle.Matrix = ooeParticle.Matrix;
                    oeParticle.Name = "PTC: " + oeParticle.ParticleID;
                    oeParticle.UpdateTimingMode = 0;

                    if (oeParticle.Flags > 20)
                        oeParticle.Flags = 15;

                    createdNode = oeParticle;

                    break;

                case EffectID.Sound:
                    EffectSound ooeSound = effect as EffectSound;

                    //gv fighter sound
                    if (ooeSound.CuesheetID == 0)
                    {

                        OEElementSE soundNode = new OEElementSE();

                        soundNode.Category = AuthNodeCategory.Element;
                        soundNode.ElementKind = 33;
                        soundNode.Start = ooeSound.Start;
                        soundNode.End = ooeSound.End;
                        soundNode.Cuesheet = ooeSound.CuesheetID;
                        soundNode.Sound = ooeSound.SoundID;

                        soundNode.Name = "GV_FIGHTER";

                        if (CMN.LastHActDEGame >= Game.Ishin)
                            soundNode.Cuesheet = 32828;
                        else
                            soundNode.Cuesheet = 32788;

                        switch(ooeSound.SoundID)
                        {
                            case 4:
                                soundNode.Sound = 5;
                                break;
                        }

                        createdNode = soundNode;
                    }

                    break;
                
            }

            if (createdNode != null)
            {
                createdNode.Guid = Guid.NewGuid();
                createdNode.PlayType = ElementPlayType.Normal;

                if (effect is EffectElement)
                    CopyEffectElementData(effect as EffectElement, createdNode);
            }


            if (createdNode != null)
                createdElements.Add(createdNode);

            return createdElements;
        }

        public void CopyEffectElementData(EffectElement effect, NodeElement target)
        {
            target.Start = effect.Start;
            target.End = effect.End;
        }
    }
}
