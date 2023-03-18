using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
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
            cloned.HActNode = HActNode.Clone();
            cloned.HActNode.Guid = Guid.NewGuid();

            return cloned;
        }

        public TreeViewItemNode() : base()
        {

        }
        public TreeViewItemNode(Node node) : base()
        {
            HActNode = node;

            if(Form1.TranslateNames && !Form1.IsBep)
            {
                switch(node.Category)
                {
                    default:
                        Text = node.Name;
                        break;
                    case AuthNodeCategory.Path:
                        Text = "Path";
                        break;
                    case AuthNodeCategory.CameraMotion:
                        Text = "Camera Animation";
                        break;
                    case AuthNodeCategory.CharacterMotion:
                        Text = "Character Animation";
                        break;
                    case AuthNodeCategory.Motion_model:
                        Text = "Model Animation";
                        break;
                    case AuthNodeCategory.FolderCondition:
                        Text = "Condition Folder";
                        break;
                    case AuthNodeCategory.ModelCustom:
                        Text = "Model";
                        break;
                    case AuthNodeCategory.Element:
                        Text = Reflection.GetElementNameByID((node as NodeElement).ElementKind, Form1.curGame).Replace("e_auth_element_", "").Replace("_", " ").ToTitleCase();
                        break;
                }
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
          
            if (!isOE && HActNode.Category == AuthNodeCategory.Element)
                SpecialDraw();
        }

        public void SetIcon()
        {
            bool isOE = Form1.IsOE;

            ImageIndex = 1;
            SelectedImageIndex = 1;

            switch (HActNode.Category)
            {
                case AuthNodeCategory.Asset:
                    ImageIndex = 8;
                    SelectedImageIndex = 8;
                    break;

                case AuthNodeCategory.Character:
                    ImageIndex = 6;
                    SelectedImageIndex = 6;
                    break;

                case AuthNodeCategory.CharacterMotion:
                    ImageIndex = 5;
                    SelectedImageIndex = 5;
                    break;

                case AuthNodeCategory.Camera:
                    ImageIndex = 4;
                    SelectedImageIndex = 4;
                    break;
                case AuthNodeCategory.CameraMotion:
                    ImageIndex = 5;
                    SelectedImageIndex = 5;
                    break;

                case AuthNodeCategory.Motion_model:
                    ImageIndex = 5;
                    SelectedImageIndex = 5;
                    break;

                case AuthNodeCategory.Model_node:
                    ImageIndex = 10;
                    SelectedImageIndex = 10;
                    break;
                case AuthNodeCategory.FolderCondition:
                    ImageIndex = 13;
                    SelectedImageIndex = 13;
                    break;
            }


            if (isOE)
            {
                switch (HActNode.Category)
                {
                    case AuthNodeCategory.Element:
                        switch (HActLib.Internal.Reflection.GetElementNameByID((HActNode as NodeElement).ElementKind, Form1.curGame))
                        {

                            case "e_auth_element_particle":
                                ImageIndex = 7;
                                SelectedImageIndex = 7;
                                break;

                            case "e_auth_element_face_expression":
                                ImageIndex = 17;
                                SelectedImageIndex = 17;
                                break;

                            case "e_auth_element_draw_off":
                                ImageIndex = 20;
                                SelectedImageIndex = 20;
                                break;

                            case "e_auth_element_damage":
                                ImageIndex = 3;
                                SelectedImageIndex = 3;
                                break;

                            case "e_auth_element_sound":
                                ImageIndex = 2;
                                SelectedImageIndex = 2;
                                break;

                            case "e_auth_element_hact_branching":
                                ImageIndex = 12;
                                SelectedImageIndex = 12;
                                break;

                            case "e_auth_element_heat_change":
                                ImageIndex = 16;
                                SelectedImageIndex = 16;
                                break;

                            case "e_auth_element_controller_vibration":
                                ImageIndex = 15;
                                SelectedImageIndex = 15;
                                break;
                        }
                        break;
                }
            }
            else
            {

                if (HActNode.Category == AuthNodeCategory.Element)
                    System.Diagnostics.Debug.Print(HActLib.Internal.Reflection.GetElementNameByID((HActNode as NodeElement).ElementKind, Form1.curGame) + "  " + (HActNode as NodeElement).ElementKind);

                switch (HActNode.Category)
                {
                    case AuthNodeCategory.Motion_model:
                        ImageIndex = 5;
                        SelectedImageIndex = 5;
                        break;

                    case AuthNodeCategory.ModelCustom:
                        ImageIndex = 8;
                        SelectedImageIndex = 8;
                        break;


                    //TODO save chosen hact game instead of accessing
                    //combobox raw
                    case AuthNodeCategory.Element:
                        switch (HActLib.Internal.Reflection.GetElementNameByID((HActNode as NodeElement).ElementKind, Form1.curGame))
                        {
                            case "e_auth_element_battle_damage":
                                ImageIndex = 3;
                                SelectedImageIndex = 3;
                                break;

                            case "e_auth_element_battle_attack":
                                ImageIndex = 3;
                                SelectedImageIndex = 3;
                                break;

                            case "e_auth_element_battle_control_window":
                                ImageIndex = 14;
                                SelectedImageIndex = 14;
                                break;

                            case "e_auth_element_battle_followup_window":
                                ImageIndex = 14;
                                SelectedImageIndex = 14;
                                break;

                            case "e_auth_element_particle":
                                ImageIndex = 7;
                                SelectedImageIndex = 7;
                                break;

                            case "e_auth_element_se":
                                ImageIndex = 2;
                                SelectedImageIndex = 2;
                                break;
                            case "e_auth_element_connect_camera":
                                ImageIndex = 9;
                                SelectedImageIndex = 9;
                                break;
                            case "e_auth_element_spot_light":
                                ImageIndex = 11;
                                SelectedImageIndex = 11;
                                break;

                            case "e_auth_element_battle_shift":
                                ImageIndex = 14;
                                SelectedImageIndex = 14;
                                break;

                            case "e_auth_element_controller_rumble":
                                ImageIndex = 15;
                                SelectedImageIndex = 15;
                                break;

                            case "e_auth_element_battle_heat":
                                ImageIndex = 16;
                                SelectedImageIndex = 16;
                                break;

                            case "e_auth_element_face_anim":
                                ImageIndex = 17;
                                SelectedImageIndex = 17;
                                break;

                            case "e_auth_element_expression_target":
                                ImageIndex = 17;
                                SelectedImageIndex = 17;
                                break;

                            case "e_auth_element_connect_chara_out":
                                ImageIndex = 18;
                                SelectedImageIndex = 18;
                                break;

                            case "e_auth_element_asset_break_range":
                                ImageIndex = 19;
                                SelectedImageIndex = 19;
                                break;
                            case "e_auth_element_play_draw_off":
                                ImageIndex = 20;
                                SelectedImageIndex = 20;
                                break;
                            case "e_auth_element_battle_muteki":
                                ImageIndex = 21;
                                SelectedImageIndex = 21;
                                break;
                        }
                        break;
                }
            }
        }

        public void SpecialDraw()
        {
            string name = HActLib.Internal.Reflection.GetElementNameByID((HActNode as NodeElement).ElementKind, Form1.curGame);

            switch(name)
            {
                case "e_auth_element_expression_target":
                    DEElementExpressionTarget expTarget = HActNode as DEElementExpressionTarget;

                    TreeNode root = new TreeNode("Animation Curves");
                    root.ImageIndex = 5;
                    root.SelectedImageIndex = 5;
                    Nodes.Add(root);

                    foreach(ExpressionTargetData expDat in expTarget.Data)
                        root.Nodes.Add(new TreeViewItemExpressionTargetData(expDat));

                    break;
            }
        }
    }
}
