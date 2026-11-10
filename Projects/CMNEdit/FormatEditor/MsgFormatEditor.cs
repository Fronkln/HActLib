using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YakuzaDataTypes.MSG;

namespace CMNEdit
{
    internal class MsgFormatEditor : FormatEditorBase
    {
        public override bool HasHierarchy()
        {
            return true;
        }

        public override void Save(string filePath)
        {
            base.Save(filePath);

            if (FileContext.Msg != null)
            {
                List<TreeNodeMsgGroup> groups = Form1.Instance.nodesTree.Nodes[0].Nodes.Cast<TreeNodeMsgGroup>().ToList();
                List<TreeNodeMsgCoordinate> coords = Form1.Instance.nodesTree.Nodes[1].Nodes.Cast<TreeNodeMsgCoordinate>().ToList();

                foreach (var gNode in groups)
                {
                    gNode.Group.Events.Clear();

                    List<TreeNodeMsgEvent> events = gNode.Nodes.Cast<TreeNodeMsgEvent>().ToList();

                    foreach (var eNode in events)
                    {
                        eNode.Event.Properties.Clear();
                        List<TreeNodeMsgProperty> properties = eNode.Nodes.Cast<TreeNodeMsgProperty>().ToList();

                        foreach (var prop in properties)
                            eNode.Event.Properties.Add(prop.Property);

                        gNode.Group.Events.Add(eNode.Event);
                    }
                }

                FileContext.Msg.Groups = groups.Select(x => x.Group).ToList();
                FileContext.Msg.Positions = coords.Select(x => x.Position).ToList();
                Msg.Write(FileContext.Msg, filePath);
            }
            else
            {
                List<TreeNodeMsgGroupOOE> groups = Form1.Instance.nodesTree.Nodes[0].Nodes.Cast<TreeNodeMsgGroupOOE>().ToList();
                List<TreeNodeMsgCoordinate> coords = Form1.Instance.nodesTree.Nodes[1].Nodes.Cast<TreeNodeMsgCoordinate>().ToList();

                foreach (var gNode in groups)
                {
                    gNode.Group.Subgroups.Clear();

                    List<TreeNodeMsgSubGroupOOE> subGroups = gNode.Nodes.Cast<TreeNodeMsgSubGroupOOE>().ToList();

                    foreach (var sNode in subGroups)
                    {
                        sNode.SubGroup.Events.Clear();

                        List<TreeNodeMsgEventOOE> events = sNode.Nodes.Cast<TreeNodeMsgEventOOE>().ToList();

                        foreach (var eNode in events)
                        {
                            eNode.Event.Properties.Clear();

                            List<TreeNodeMsgPropertyOOE> properties = eNode.Nodes.Cast<TreeNodeMsgPropertyOOE>().ToList();

                            foreach (var prop in properties)
                                eNode.Event.Properties.Add(prop.Property);
                        }

                        foreach (var _event in events)
                            sNode.SubGroup.Events.Add(_event.Event);

                        gNode.Group.Subgroups.Add(sNode.SubGroup);
                    }
                }

                FileContext.MsgOOE.Groups = groups.Select(x => x.Group).ToList();
                FileContext.MsgOOE.Coordinates = coords.Select(x => x.Position).ToList();

                MsgOOE.Write(FileContext.MsgOOE, filePath);
            }
        }
    }
}
