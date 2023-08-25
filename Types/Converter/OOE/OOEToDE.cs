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
    public class OOEToDEConversionInfo
    {
        public TEV Tev;
        public CSVHAct CsvData;
    }

    public class OOEToDE : IConverter<OOEToDEConversionInfo, CMN>
    {
        //Set2 pointer is unreliable, some stuff has to be processed by order
        private static List<Set2> m_processedSets = new List<Set2>();

        private static ObjectBase m_lastChar;
        private static HActReplaceID m_curEnemyID;

        CSVHAct csvData;

        public CMN Convert(OOEToDEConversionInfo inf)
        {
            csvData = inf.CsvData;

            TEV tev = inf.Tev;

            m_processedSets.Clear();
            m_lastChar = null;
            m_curEnemyID = HActReplaceID.hu_enemy_00;

            CMN cmn = new CMN();

            cmn.Header.Start = 0;
            cmn.Header.End = DetermineHactLength(tev);
            cmn.Header.Version = 18;
            cmn.AuthPages = GeneratePages(tev, cmn.Header.End);
            cmn.Header.Flags = 162;
            cmn.Header.Type = 1;

            cmn.ResourceCutInfo = new float[] { cmn.Header.End };

            if (tev.Root.Type != ObjectNodeCategory.Path)
                throw new Exception("Rewrite this part of conversion where you assume 0 is root path");


            ObjectBase[] objects = tev.AllObjects;


            for (int i = 1; i < objects.Length; i++)
                if (objects[i].Parent == null && objects[i].Category != 2)
                    tev.Root.Children.Add(objects[i]);

            cmn.Root = Convert(tev, tev.Root, cmn.Header.End);


            IEnumerable<Set2Element1019> buttons = tev.AllSet2.Where(x => x.Type == Set2NodeCategory.Element && x._InternalInfo.elementType == 1019).Cast<Set2Element1019>().Where(x => x.Type1019.StartsWith("HE_BUTTON")).Where(x => x.Type1019.Split('_').Length == 3);

            int buttonCount = buttons.Count();

            foreach(Set2Element1019 button in buttons)
            {
                cmn.Root.Children.Add(GenerateInput(tev, button));
            }

            /*
            foreach (Set2 set in tev.Set2)
                if (set.Type == Set2NodeCategory.Element && set._InternalInfo.elementType == 1019)
                {
                    Set2Element1019 element1019 = set as Set2Element1019;

                    if (element1019.Type1019.ToLower().StartsWith("he_button") && element1019.Type1019.)
                    {
                        cmn.Root.Children.Add(GenerateInput(tev, element1019));
                    }
                }
            */

            FixConflicts(cmn);


            //Branch nodes are in the camera in OE
            foreach (Node branch in cmn.AllElements.Where(x => x.ElementKind ==  Reflection.GetElementIDByName("e_auth_element_hact_input", CMN.LastHActDEGame)))
            {
                if(branch.Parent != null)
                    branch.Parent.Children.Remove(branch);
                cmn.Root.Children.Add(branch);
            }

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
        public List<AuthPage> GeneratePages(TEV tev, float hactLen)
        {
            try
            {
                ObjectCamera cam = (ObjectCamera)tev.AllObjects.FirstOrDefault(x => x is ObjectCamera);

                //We are gonna treat every button sequence as the start of a new page.
                IEnumerable<Set2Element1019> buttonNodes = cam.Children.Where(x => x is Set2Element1019).Cast<Set2Element1019>().Where(x => x.Type1019.StartsWith("HE_BUTTON_")).Where(x => x.Type1019.Split('_').Length == 3).OrderBy(x => x.Start);
                IEnumerable<Set2Element1019> endNodes = cam.Children.Where(x => x is Set2Element1019).Cast<Set2Element1019>().Where(x => x.Type1019.StartsWith("HE_END_")).OrderBy(x => x.Start);
                IEnumerable<Set2Element1019> branchNodes = cam.Children.Where(x => x is Set2Element1019).Cast<Set2Element1019>().Where(x => x.Type1019.StartsWith("HE_BRANCH_")).OrderBy(x => x.Start);

                if (branchNodes.Count() <= 0)
                    return new List<AuthPage>();

                List<AuthPage> pages = new List<AuthPage>();
                AuthPage startPage = new AuthPage("START", 0, buttonNodes.ElementAt(0).Start - 1);

                pages.Add(startPage);

                int pageIdx = 2;

                Set2Element1019 GetNearestEndNodeInRange(float start)
                {
                    Set2Element1019 endNode = endNodes.FirstOrDefault(x => x.Start >= start);
                    return endNode;
                }

                for (int i = 0; i < branchNodes.Count(); i++)
                {
                    Set2Element1019 branch = branchNodes.ElementAt(i);

                    AuthPage successPage = new AuthPage("SUCCESS " + i);
                    AuthPage failPage = new AuthPage("FAIL " + i);

                    successPage.Start.Frame = branch.Start;
                    successPage.PageIndex = pageIdx;

                    failPage.Start.Frame = branch.End;
                    failPage.PageIndex = pageIdx + 1;

                    Set2Element1019 failEnd = GetNearestEndNodeInRange(branch.End);

                    if (i == branchNodes.Count() - 1)
                    {
                        successPage.End.Frame = GetNearestEndNodeInRange(successPage.Start).Start;
                    }
                    else
                    {
                        successPage.End.Frame = branchNodes.ElementAt(i + 1).Start - 1;
                        successPage.Transitions.Add(new Transition(pageIdx + 2, new ConditionHActFlag(1, 0), new ConditionPageEnd()));
                        successPage.Transitions.Add(new Transition(pageIdx + 3, new ConditionHActFlag(0, 1), new ConditionPageEnd()));
                    }

                    if (failEnd != null)
                        failPage.End.Frame = failEnd.Start;
                    else
                    {
                        if (i == branchNodes.Count() - 1)
                            failPage.End.Frame = hactLen;
                        else
                            failPage.End.Frame = branchNodes.ElementAt(i + 1).Start - 1;
                        // throw new Exception("Don't know how to calculate page end");
                    }

                    pages.Add(successPage);
                    pages.Add(failPage);

                    pageIdx += 2;
                }


                startPage.Transitions.Add(new Transition(1, new ConditionPageEnd()));

                AuthPage promptPage = new AuthPage("PROMPT START", buttonNodes.ElementAt(0).Start, branchNodes.ElementAt(0).Start - 1);
                promptPage.PageIndex = 1;
                promptPage.Transitions.Add(new Transition(2, new ConditionHActFlag(1, 0), new ConditionPageEnd()));
                promptPage.Transitions.Add(new Transition(3, new ConditionHActFlag(0, 1), new ConditionPageEnd()));

                pages.Insert(1, promptPage);

                return pages;
            }
            catch
            {
                return new List<AuthPage>();
            }
        }

        void FixConflicts(CMN cmn)
        {
            uint buttonID = HActLib.Internal.Reflection.GetElementIDByName("e_auth_element_hact_input", CMN.LastHActDEGame);
            List<NodeElement> inputs = cmn.AllElements.Where(x => x.ElementKind == buttonID).ToList();
        }

        public DEHActInput GenerateInput(TEV tev, Set2Element1019 button)
        {
            DEHActInput input = new DEHActInput();
            input.Guid = Guid.NewGuid();
            input.Name = button.Type1019;
            input.Start = button.Start;
            input.End = button.End;
            input.InputID = 1;
            input.Category = AuthNodeCategory.Element;
            input.ElementKind = Reflection.GetElementIDByName("e_auth_element_hact_input", CMN.LastHActDEGame);
            input.PlayType = ElementPlayType.Always;
            input.UpdateTimingMode = 2;

            input.DecideTick = new GameTick((input.End - input.Start) - 1).Tick;

            return input;
        }
        public DENodePath GeneratePath()
        {
            DENodePath root = new DENodePath();
            root.Category = AuthNodeCategory.Path;
            root.Guid = Guid.NewGuid();
            root.Matrix = Matrix4x4.Default;
            root.Name = "Path";

            return root;
        }

        public DENodeCharacter GenerateCharacter(TEV tev, ObjectHuman character, float length)
        {
            DENodeCharacter chara = new DENodeCharacter();
            chara.Guid = Guid.NewGuid();
            chara.Name = character.Replace;

            chara.CharacterID = 1;
            chara.ScaleID = 22;

            string name = character.Replace.ToLowerInvariant();

            if (m_lastChar == null)
                chara.ReplaceID = HActReplaceID.hu_player;
            else
            {
                chara.ReplaceID = m_curEnemyID;
                m_curEnemyID = (HActReplaceID)((uint)chara.ReplaceID + 1);
            }

            m_lastChar = character;

            DENodeCharacterMotion motion = new DENodeCharacterMotion();
            motion.Guid = Guid.NewGuid();
            motion.End.Tick = new GameTick(length).Tick;

            foreach (Set2 set in tev.AllSet2)
            {
                if (set.Type == Set2NodeCategory.ModelMotion && !m_processedSets.Contains(set))
                {
                    motion.Name = set.Resource;
                    m_processedSets.Add(set);
                    break;
                }
            }

            chara.Children.Add(motion);

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

            Set2ElementMotion set2Mot = cam.GetChildOfType<Set2ElementMotion>();

            NodeCameraMotion motion = new NodeCameraMotion();
            motion.Guid = Guid.NewGuid();
            motion.Name = set2Mot.Resource;
            motion.Start.Tick = 0;
            motion.End.Tick = (uint)length;

            foreach (Set2 set in tev.AllSet2)
            {
                if (set.Type == Set2NodeCategory.CameraMotion && !m_processedSets.Contains(set))
                {
                    motion.End.Frame = set.End;
                    motion.Name = set.Resource;
                    m_processedSets.Add(set);
                    break;
                }
            }

            camera.Children.Add(motion);

            return camera;
        }

        public NodeModel GenerateBone(TEV tev, ObjectBone bone)
        {
            NodeModel model = new NodeModel();
            model.Category = AuthNodeCategory.Model_node;
            model.Guid = new Guid();

            string boneName;

            switch(bone.BoneName)
            {
                default:
                    boneName = bone.BoneName;
                    break;
                case "center_n":
                    boneName = "center_c_n";
                    break;
                case "mune_n":
                    boneName = "mune_c_n";
                    break;
                case "kubi_n":
                    boneName = "kubi_c_n";
                    break;
                case "ketu_n":
                    boneName = "ketu_c_n";
                    break;
                case "face":
                    boneName = "face_c_n";
                    break;
                case "kosi_n":
                    boneName = "kosi_c_n";
                    break;
                case "_lip_top":
                    boneName = "_lip_top1_c_n";
                    break;

            }

            model.Name = boneName;
            model.BoneName.Set(boneName);

            return model;
        }

        public Node Convert(TEV tev, ObjectBase set, float hactLen)
        {

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
                foreach (ITEVObject setChild in set.Children)
                {
                    if (setChild is ObjectBase)
                    {
                        Node childConv = Convert(tev, setChild as ObjectBase, hactLen);

                        if (childConv != null)
                            deNode.Children.Add(childConv);
                    }
                    else if(setChild is Set2)
                    {
                        Node childConv = ConvertSet2(tev, setChild as Set2);

                        if (childConv != null)
                            deNode.Children.Add(childConv);
                    }

                    else if (setChild is EffectBase)
                    {
                        Node childConv = ConvertElementEffect(tev, setChild as EffectBase);

                        if (childConv != null)
                            deNode.Children.Add(childConv);
                    }
                }

            return deNode;
        }

        void GenerateFrameProgression(TEV tev, CMN hact)
        {
            ObjectCamera cam = tev.Root.GetChildOfType<ObjectCamera>();
            List<Set2> slowMoAreas = cam.Children.Where(x => x is Set2).Cast<Set2>().Where(x => x.Type == Set2NodeCategory.Slowmo).ToList();
            List<DEHActInput> inputs = hact.AllElements.Where(x => x.ElementKind == Reflection.GetElementIDByName("e_auth_element_hact_input", CMN.LastHActDEGame)).Cast<DEHActInput>().ToList();

            if (slowMoAreas.Count <= 0)
                return;

            float current = 0;

            List<float> progression = new List<float>() { 0, 0 };
            List<float> progressionSpeed = new List<float>() { 1, 1 };

            Set2 GetSlowMoArea(float frame)
            {
                return slowMoAreas.FirstOrDefault(x => frame >= x.Start && frame < x.End);
            }

            do
            {
                if (GetSlowMoArea(current) != null)
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

            foreach (NodeCamera camera in hact.AllCameras)
            {
                camera.FrameProgression = progression.ToArray();
                camera.FrameProgressionSpeed = progressionSpeed.ToArray();
                camera.CameraFlags = 1;
            }
        }


        public Node ConvertSet2(TEV tev, Set2 set2Obj)
        {
            Node convertedNode = null;

            switch (set2Obj.EffectID)
            {
                case EffectID.Special:
                    Set2Element1019 set2Special = set2Obj as Set2Element1019;
                    CSVHActEvent specialData = csvData.TryGetHActEventData(set2Special.Type1019);

                    string[] typeSplit = set2Special.Type1019.Split('_');

                    switch (typeSplit[1])
                    {
                        case "DAMAGE":
                            if (typeSplit[2] != "99")
                            {
                                CSVHActEventDamage specialDataDamage = specialData as CSVHActEventDamage;

                                NodeBattleDamage damage = new NodeBattleDamage();
                                damage.Name = set2Special.Type1019;
                                damage.Guid = Guid.NewGuid();
                                damage.Start = set2Special.Start;
                                damage.End = set2Special.End;
                                damage.ElementKind = Reflection.GetElementIDByName("e_auth_element_battle_damage", CMN.LastHActDEGame);

                                damage.Damage = (uint)specialDataDamage.Damage;

                                convertedNode = damage;
                            }
                            break;

                        case "GAUGE":
                            CSVHActEventHeatGauge specialDataGauge = specialData as CSVHActEventHeatGauge;

                            DEElementBattleHeat heat = new DEElementBattleHeat();
                            heat.Name = set2Special.Type1019;
                            heat.Guid = Guid.NewGuid();
                            heat.Start = set2Special.Start;
                            heat.End = set2Special.End;
                            heat.ElementKind = Reflection.GetElementIDByName("e_auth_element_battle_heat", CMN.LastHActDEGame);

                            //Try to adjust heat change to be more in line with OE standards
                            //OE has things like -150 while OOE has -8000 on waking wrath which consumes 75% of gauge.
                            heat.HeatChange = (int)(specialDataGauge.Change * 0.015f);

                            convertedNode = heat;
                            break;

                        case "BUTTON":
                            if (typeSplit[2] != "99")
                            {
                                DEHActInput button = new DEHActInput();
                                button.Name = set2Special.Type1019;
                                button.Guid = Guid.NewGuid();
                                button.Start = set2Special.Start;
                                button.End = set2Special.End;
                                button.ElementKind = Reflection.GetElementIDByName("e_auth_element_battle_heat", CMN.LastHActDEGame);
                                button.DecideTick = new GameTick(button.End - button.Start).Tick;

                                convertedNode = button;
                            }
                            break;
                    }
                    break;
            }

            return convertedNode;
        }

        public NodeElement ConvertElementEffect(TEV tev, EffectBase effect)
        {
            NodeElement createdNode = null;

            switch (effect.ElementKind)
            {
                case EffectID.Sound:
                    DEElementSE soundNode = new DEElementSE();
                    EffectSound ooeSound = effect as EffectSound;

                    soundNode.Category = AuthNodeCategory.Element;
                    soundNode.ElementKind = Reflection.GetElementIDByName("e_auth_element_se", CMN.LastHActDEGame);
                    soundNode.Start = ooeSound.Start;
                    soundNode.End = ooeSound.End;
                    soundNode.CueSheet = ooeSound.CuesheetID;
                    soundNode.SoundIndex = (byte)(ooeSound.SoundID + 1);

                    //gv fighter sound
                    if (ooeSound.CuesheetID == 0)
                    {
                        soundNode.Name = "GV_FIGHTER";

                        if (CMN.GetVersionForGame(CMN.LastHActDEGame) >= GameVersion.DE2)
                            soundNode.CueSheet = 49;
                        else
                            soundNode.CueSheet = 36;

                        switch(ooeSound.SoundID)
                        {
                            case 4:
                                soundNode.SoundIndex = 3;
                                break;

                            case 5:
                                soundNode.SoundIndex = 3;
                                break;
                            case 6:
                                soundNode.SoundIndex = 4;
                                break;
                            case 8:
                                soundNode.SoundIndex = 5;
                                break;

                        }

                        soundNode.Unk = 128;
                    }
                    else
                        soundNode.Name = "Sound";

                    createdNode = soundNode;
                    break;
            }

            if (createdNode != null)
            {
                createdNode.Guid = Guid.NewGuid();
                createdNode.UpdateTimingMode = 2;
                createdNode.PlayType = ElementPlayType.Normal;

                if (effect is EffectElement)
                    CopyEffectElementData(effect as EffectElement, createdNode);
            }

            return createdNode;
        }

        public void CopyEffectElementData(EffectElement effect, NodeElement target)
        {
            target.Start = effect.Start;
            target.End = effect.End;
        }
    }
}
