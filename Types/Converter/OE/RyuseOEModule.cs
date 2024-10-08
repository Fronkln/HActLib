﻿using HActLib.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HActLib
{
    public static class RyuseOEModule
    {
        public enum ConversionResult
        {
            Success,
            Fail,
            NoChange,
            Removed,
            BlacklistedGame,
            BlacklistedEngine
        }

        public struct ConversionInformation
        {
            public HashSet<string> ConvertedTypes;
            public HashSet<string> FailedTypes;
            public HashSet<uint> FailedIDs;
            public HashSet<uint> BlackListedIDs;
            public HashSet<string> BlackListedTypes;

            public Node[] OutputNodes;
        }

        private static List<string> blackListedBetweenEngines = new List<string>()
        {
        };

        private static List<string> blackListedBetweenGames = new List<string>()
        {
            "e_auth_element_body_flash"
        };

        public static ConversionInformation ConvertNodes(Node[] nodes, Game inputGame, Game outputGame)
        {
            string GetNodeName(uint id, Game game)
            {
                return Reflection.GetElementNameByID(id, game);
            }

            ConversionInformation inf = new ConversionInformation();
            inf.ConvertedTypes = new HashSet<string>();
            inf.FailedTypes = new HashSet<string>();
            inf.FailedIDs = new HashSet<uint>();
            inf.BlackListedIDs = new HashSet<uint>();
            inf.BlackListedTypes = new HashSet<string>();

            List<Node> nodesList = nodes.ToList();
            List<Node> removedNodes = new List<Node>();

            foreach (Node node in nodesList)
            {
                if (node == null)
                    continue;

                ConversionResult result = ConvertNode(node, null, inputGame, outputGame);

                switch (result)
                {
                    case ConversionResult.Removed:
                        removedNodes.Add(node);
                        break;

                    case ConversionResult.Success:
                        NodeElement convertedElement = node as NodeElement;

                        if(convertedElement != null)
                            inf.ConvertedTypes.Add(GetNodeName(convertedElement.ElementKind, outputGame));
                        break;

                    case ConversionResult.Fail:
                        NodeElement failedElement = node as NodeElement;
                        inf.FailedTypes.Add(GetNodeName(failedElement.ElementKind, inputGame));
                        inf.FailedIDs.Add(failedElement.ElementKind);
                        break;

                    case ConversionResult.BlacklistedGame:
                        NodeElement blacklistedGameElement = node as NodeElement;
                        inf.BlackListedIDs.Add(blacklistedGameElement.ElementKind);
                        inf.BlackListedTypes.Add(GetNodeName(blacklistedGameElement.ElementKind, inputGame));
                        break;

                    case ConversionResult.BlacklistedEngine:
                        NodeElement blacklistedEngineElement = node as NodeElement;
                        inf.BlackListedIDs.Add(blacklistedEngineElement.ElementKind);
                        inf.BlackListedTypes.Add(GetNodeName(blacklistedEngineElement.ElementKind, inputGame));
                        break;

                    case ConversionResult.NoChange:
                        NodeElement unchangedElement = node as NodeElement;

                        if(unchangedElement != null)
                            inf.ConvertedTypes.Add(GetNodeName(unchangedElement.ElementKind, outputGame));
                        break;

                }
            }




            foreach (Node node in removedNodes)
                nodesList.Remove(node);

            foreach(Node node in nodesList)
            {
                node.Children = node.Children.Where(x => !removedNodes.Contains(x)).ToList();
            }

            inf.OutputNodes = nodesList.ToArray();

            return inf;
        }

        public static ConversionResult ConvertNode(Node node, Node parent, Game inputGame, Game outputGame)
        {
            if (node == null)
                return ConversionResult.Fail;

            NodeElement element = (node as NodeElement);

            if (element == null)
                return ConvertNormalNode(node,parent, inputGame, outputGame);

            Type inputOEGame = Reflection.GetElementEnumFromGame(inputGame);
            Type outputOEGame = Reflection.GetElementEnumFromGame(outputGame);

            string elementName = Reflection.GetElementNameByID(element.ElementKind, inputGame);

            if (inputGame != outputGame && blackListedBetweenGames.Contains(elementName))
            {
                element.ElementKind = 0;
                element.Name += " (invalid)";

                return ConversionResult.BlacklistedGame;
            }

            if (Enum.IsDefined(outputOEGame, elementName))
            {
                uint originalID = (uint)Enum.Parse(inputOEGame, elementName);
                uint outputID = (uint)Enum.Parse(outputOEGame, elementName);

                if (inputGame == Game.Y5 && outputGame == Game.Y0)
                    element.OE_ConvertToY0();

                if (inputGame == Game.Y0 && outputGame == Game.Y5)
                    element.OE_ConvertToY5();

                    if (originalID == outputID)
                    return ConversionResult.NoChange;
                else
                {
                    /*
                    if(!element.TryConvert(inputGame, outputGame))
                    {
                        element.ElementKind = 0;
                        element.Name += " (invalid)";

                        return ConversionResult.Fail;
                    }
                    */

                    element.ElementKind = outputID;
                    element.BEPDat.PropertyType = (ushort)outputID;
                    return ConversionResult.Success;
                }
            }
            else
            {
                element.ElementKind = 0;
                element.Name += " (invalid)";

                return ConversionResult.Fail;
            }
        }


        public static ConversionResult ConvertNormalNode(Node node, Node parent, Game inputGame, Game outputGame)
        {
            if (inputGame >= Game.Y0 && outputGame == Game.Y5)
            {
                if (node.Category == AuthNodeCategory.FolderCondition)
                {
                    node.Category = AuthNodeCategory.DummyNode;
                    return ConversionResult.Removed;
                }
            }

            if (node.Category == AuthNodeCategory.Model_node)
            {
                NodeModel model = node as NodeModel;

                if(inputGame <= Game.Y5)
                {
                    model.BoneName.Set(OOEToOEBone(model.BoneName.Text));

                    if (MEPDict.OEBoneID.ContainsKey(model.BoneName.Text))
                        model.BoneID = MEPDict.OEBoneID[model.BoneName.Text];
                }
                else
                {
                    model.BoneName.Set(OEToOOEBone(model.BoneName.Text));

                    if (MEPDict.OOEBoneID.ContainsKey(model.BoneName.Text))
                        model.BoneID = MEPDict.OOEBoneID[model.BoneName.Text];
                }
            }

            if(node.Category == AuthNodeCategory.Character)
            {
                OENodeCharacter character = node as OENodeCharacter;

                if (inputGame <= Game.Y5)
                {
                    if((character.Flag & (1 << 0)) != 0)
                    {
                        character.Flag |= 1 << 1;
                    }
                }
            }

            return ConversionResult.NoChange;
        }

        public static string OOEToOEBone(string bonename)
        {
            string boneName = "";

            switch (bonename)
            {
                default:
                    return bonename;
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

            return boneName;
        }

        public static string OEToOOEBone(string bonename)
        {
            string boneName = "";

            switch (bonename)
            {
                default:
                    return bonename;
                case "center_c_n":
                    boneName = "center_n";
                    break;
                case "mune_c_n":
                    boneName = "mune_n";
                    break;
                case "kubi_c_n":
                    boneName = "kubi_n";
                    break;
                case "ketu_c_n":
                    boneName = "ketu_n";
                    break;
                case "face_c_n":
                    boneName = "face";
                    break;
                case "kosi_c_n":
                    boneName = "kosi_n";
                    break;
                case "_lip_top1_c_n":
                    boneName = "_lip_top";
                    break;

            }

            return boneName;
        }
    }
}
