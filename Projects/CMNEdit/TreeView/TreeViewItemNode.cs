using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HActLib;
using HActLib.Internal;

namespace CMNEdit
{
    public class TreeViewItemNode : TreeNode
    {
        public Node HActNode;

        public override object Clone()
        {
            TreeViewItemNode cloned =  (TreeViewItemNode)base.Clone();
            cloned.HActNode = HActNode.Copy();
            cloned.HActNode.Guid = Guid.NewGuid();

            return cloned;
        }

        public TreeViewItemNode() : base()
        {

        }

        public void Update()
        {
            Text = TranslateName(HActNode);
        }

        public TreeViewItemNode(Node node) : base()
        {
            HActNode = node;

            if(Form1.TranslateNames || Form1.IsBep)
            {
                Text = TranslateName(node);
            }
            else
                Text = node.Name;

            foreach (Node children in node.Children)
            {
                Nodes.Add(new TreeViewItemNode(children));
            }

            node.Children.Clear();

            ContextMenuStrip = Form1.Instance.nodeContext;

            bool isOE = Form1.IsOE;

            SetIcon();
          
            if (HActNode.Category == AuthNodeCategory.Element)
                SpecialDraw();
        }


        public void SetIcon()
        {
            int icon = SetIcon(HActNode.Category,  (HActNode.Category != AuthNodeCategory.Element ? 0 : (HActNode as NodeElement).ElementKind));
            
            ImageIndex = icon;
            SelectedImageIndex = icon;
        }

        public static int SetIcon(AuthNodeCategory cat, uint elementID)
        {
            bool isOE = Form1.IsOE;

            switch (cat)
            {
                case AuthNodeCategory.Asset:
                    return 8;
                case AuthNodeCategory.Character:
                    return 6;

                case AuthNodeCategory.CharacterMotion:
                    return 5;

                case AuthNodeCategory.Camera:
                    return 4;
                case AuthNodeCategory.CameraMotion:
                    return 5;
                case AuthNodeCategory.ModelMotion:
                    return 5;
                case AuthNodeCategory.Model_node:
                    return 10;
                case AuthNodeCategory.FolderCondition:
                    return 13;
            }


            if (isOE)
            {
                switch (cat)
                {
                    case AuthNodeCategory.Element:
                        switch (HActLib.Internal.Reflection.GetElementNameByID(elementID, Form1.curGame))
                        {
                            case "e_auth_element_particle":
                                return 7;

                            case "e_auth_element_face_expression":
                                return 17;

                            case "e_auth_element_draw_off":
                                return 20;

                            case "e_auth_element_damage":
                                return 3;

                            case "e_auth_element_sound":
                                return 2;

                            case "e_auth_element_hact_branching":
                                return 12;

                            case "e_auth_element_heat_change":
                                return 16;

                            case "e_auth_element_controller_vibration":
                                return 15;
                        }
                        break;
                }
            }
            else
            {

                if (cat == AuthNodeCategory.Element)
                    System.Diagnostics.Debug.Print(HActLib.Internal.Reflection.GetElementNameByID(elementID, Form1.curGame) + "  " + elementID);

                switch (cat)
                {
                    case AuthNodeCategory.ModelMotion:
                        return 5;

                    case AuthNodeCategory.ModelCustom:
                        return 8;


                    //TODO save chosen hact game instead of accessing
                    //combobox raw
                    case AuthNodeCategory.Element:
                        switch (HActLib.Internal.Reflection.GetElementNameByID(elementID, Form1.curGame))
                        {
                            case "e_auth_element_battle_damage":
                                return 3;
                            case "e_auth_element_battle_attack":
                                return 3;
                            case "e_auth_element_battle_control_window":
                                return 14;
                            case "e_auth_element_battle_followup_window":
                                return 14;
                            case "e_auth_element_particle":
                                return 7;
                            case "e_auth_element_se":
                                return 2;
                            case "e_auth_element_connect_camera":
                                return 9;
                            case "e_auth_element_spot_light":
                                return 11;

                            case "e_auth_element_battle_shift":
                                return 14;
                            case "e_auth_element_controller_rumble":
                                return 15;
                            case "e_auth_element_battle_heat":
                                return 16;
                            case "e_auth_element_face_anim":
                                return 17;
                            case "e_auth_element_expression_target":
                                return 17;
                            case "e_auth_element_connect_chara_out":
                                return 18;

                            case "e_auth_element_asset_break_range":
                                return 19;
                            case "e_auth_element_play_draw_off":
                                return 20;
                            case "e_auth_element_battle_muteki":
                                return 21;
                        }
                        break;
                }
            }

            return 1;
        }


        public static string TranslateName(Node node)
        {
            string TranslateElement()
            {
                bool isOE = Form1.IsOE;
                string nodeName = Reflection.GetElementNameByID((node as NodeElement).ElementKind, Form1.curGame);


                if (!isOE)
                {
                    if (node is NodeElementUser)
                        return (node as NodeElementUser).UserData.NodeName.Replace("_", " ");

                    switch (nodeName)
                    {
                        case "e_auth_element_particle":
                            DEElementParticle particleElem = node as DEElementParticle;
                            if (Form1.curGame > Game.Y6)
                                return "PIB " + particleElem.ParticleID + $" ({particleElem.ParticleName})";
                            else
                                return "PIB " + particleElem.ParticleID;

                        case "e_auth_element_se":
                            DEElementSE seElem = node as DEElementSE;
                            return $"Sound Cue {seElem.CueSheet} ID {seElem.SoundIndex}" + $"{(seElem.Unk > 0 ? $"Unk {seElem.Unk}" : "")}";
                        case "e_auth_element_battle_damage":
                            NodeBattleDamage damageElem = node as NodeBattleDamage;
                            return $"{damageElem.Damage} Damage {(damageElem.NoDead ? "(Non-Lethal)" : "(Lethal)")}";
                        case "e_auth_element_battle_heat":
                            DEElementBattleHeat heatElem = node as DEElementBattleHeat;
                            return $"{heatElem.HeatChange} Heat Change";
                        case "e_auth_element_rim_flash":
                            DEElementRimflash rimflashElem = node as DEElementRimflash;
                            if (rimflashElem.ParamID > 0)
                                return "Rimflash Ver." + rimflashElem.RimflashVersion + " (DB Ref)";
                            else
                                return "Rimflash Ver." + rimflashElem.RimflashVersion + "." + rimflashElem.ParamVersion;
                        case "e_auth_element_flow_dust_gen":
                            DEElementFlowdust flowdustElem = node as DEElementFlowdust;
                            if (flowdustElem.ParameterFlowdust)
                                return "Flowdust Ver." + flowdustElem.FlowVersion + " (DB Ref)";
                            else
                                return "Flowdust Ver." + flowdustElem.FlowVersion + "." + flowdustElem.SetParamVersion;
                            break;

                        case "e_auth_element_battle_command_special":
                            DEElementBattleCommandSpecial battleSpecialElem = node as DEElementBattleCommandSpecial;
                            return $"Battle Special ({battleSpecialElem.Type})";
                    }
                }
                else
                {
                    switch (nodeName)
                    {
                        case "e_auth_element_particle":
                            OEParticle particleElem = node as OEParticle;
                            return "PIB " + particleElem.ParticleID;
                        case "e_auth_element_damage":
                            OEDamage damageElem = node as OEDamage;
                            return $"{damageElem.Damage} Damage {(damageElem.NoDead ? "(Non-Lethal)" : "(Lethal)")}";
                        case "e_auth_element_heat_change":
                            OEHeat heatElem = node as OEHeat;
                            return $"{heatElem.HeatChange} Heat Change";
                        case "e_auth_element_sound":
                            OEElementSE seElem = node as OEElementSE;
                            return $"Sound Cue {seElem.Cuesheet.ToString("x")} ID {seElem.Sound}";
                    }

                    if (node is NodeElementUser)
                        return (node as NodeElementUser).UserData.NodeName;
                }

                return nodeName.Replace("e_auth_element_", "").Replace("_", " ").ToTitleCase();
            }

            switch (node.Category)
            {
                default:
                    return node.Name;
                case AuthNodeCategory.Path:
                    return "Path";
                case AuthNodeCategory.CameraMotion:
                    return "Camera Animation";
                case AuthNodeCategory.CharacterMotion:
                    return "Character Animation";
                case AuthNodeCategory.ModelMotion:
                    return "Model Animation";
                case AuthNodeCategory.FolderCondition:
                    if (!Form1.IsOE)
                        return $"Condition ({(node as DENodeConditionFolder).Condition})";
                    else
                        return "Condition Folder";
                case AuthNodeCategory.ModelCustom:
                    return  "Model";
                case AuthNodeCategory.Element:
                    return TranslateElement();
            }
        }

        public void SpecialDraw()
        {
            string name = HActLib.Internal.Reflection.GetElementNameByID((HActNode as NodeElement).ElementKind, Form1.curGame);

            if (!Form1.IsOE)
            {
                switch (name)
                {
                    case "e_auth_element_expression_target":
                        DEElementExpressionTarget expTarget = HActNode as DEElementExpressionTarget;

                        TreeNode root = new TreeNode("Animation Curves");
                        root.ImageIndex = 5;
                        root.SelectedImageIndex = 5;
                        Nodes.Add(root);

                        foreach (ExpressionTargetData expDat in expTarget.Data)
                            root.Nodes.Add(new TreeViewItemExpressionTargetData(expDat));

                        break;
                }
            }
            else
            {
                switch (name)
                {
                    case "e_auth_element_expression_target":
                        OEExpressionTarget expTarget = HActNode as OEExpressionTarget;

                        TreeNode root = new TreeNode("Animation Curves");
                        root.ImageIndex = 5;
                        root.SelectedImageIndex = 5;
                        Nodes.Add(root);

                        for(int i = 0; i < expTarget.Datas.Length; i++)
                        {
                            OEExpressionTarget.ExpressionData expDat = expTarget.Datas[i];

                            TreeViewItemExpressionTargetDataOE nod = new TreeViewItemExpressionTargetDataOE(expDat);
                            nod.Text = ((OEExpressionTarget.Type)i).ToString();
                            root.Nodes.Add(nod);
                        }

                        break;
                }
            }
        }
    }
}
