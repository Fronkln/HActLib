using HActLib.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HActLib
{
    public static class RyuseModule
    {
        public enum ConversionResult
        {
            Success,
            Fail,
            NoChange,
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
            "e_auth_element_texture_change",
            "e_auth_element_eyes_highlight",
            "e_auth_element_character_light_influence",
            "e_auth_element_speech",
            "e_auth_element_talk_text"
        };

        private static List<string> blackListedBetweenGames = new List<string>()
        {
            "e_auth_element_caption",
            "e_auth_element_ai",
            "e_auth_element_div_play",
            "e_auth_element_div_play_async",
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

            foreach (Node node in nodes)
            {
                if (node == null)
                    continue;

                ConversionResult result = ConvertNode(node, inputGame, outputGame);

                switch (result)
                {
                    case ConversionResult.Success:
                        NodeElement convertedElement = node as NodeElement;
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

                        if (unchangedElement != null)
                            inf.ConvertedTypes.Add(GetNodeName(unchangedElement.ElementKind, outputGame));
                        break;

                }
            }

            inf.OutputNodes = nodes;

            return inf;
        }

        public static ConversionResult ConvertNode(Node node, Game inputGame, Game outputGame)
        {
            if (node == null)
                return ConversionResult.Fail;

            NodeElement element = (node as NodeElement);

            if (element == null)
                return ConversionResult.NoChange;

            Type inputDEGame = Reflection.GetElementEnumFromGame(inputGame);
            Type outputDEGame = Reflection.GetElementEnumFromGame(outputGame);

            string elementName = Reflection.GetElementNameByID(element.ElementKind, inputGame);

            bool sameVer = CMN.GetVersionForGame(inputGame) == CMN.GetVersionForGame(outputGame);

            if ((!sameVer && blackListedBetweenEngines.Contains(elementName)))
            {
                element.ElementKind = 0;
                element.Name += " (invalid)";

                return ConversionResult.BlacklistedEngine;
            }

            if (inputGame != outputGame && blackListedBetweenGames.Contains(elementName))
            {
                element.ElementKind = 0;
                element.Name += " (invalid)";

                return ConversionResult.BlacklistedGame;
            }

            if (Enum.IsDefined(outputDEGame, elementName))
            {
                uint originalID = (uint)Enum.Parse(inputDEGame, elementName);
                uint outputID = (uint)Enum.Parse(outputDEGame, elementName);

                if (originalID == outputID)
                {
                    ProcessSpecialNode(element, inputGame, outputGame);
                    return ConversionResult.NoChange;
                }
                else
                {
                    element.ElementKind = outputID;
                    element.BEPDat.PropertyType = (ushort)outputID;

                    ProcessSpecialNode(element, inputGame, outputGame);

                    return ConversionResult.Success;
                }
            }
            else
            {
                NodeElement convertedNode = node.TryConvert(inputGame, outputGame) as NodeElement;

                if (convertedNode != null)
                {
                    Node.CopyBaseInfo(node, convertedNode);
                    Node.CopyBaseElementInfo(element, convertedNode);

                    if (node.Parent != null)
                    {
                        int origIdx = node.Parent.Children.IndexOf(node);
                        node.Parent.Children.Remove(node);
                        node.Parent.Children.Insert(origIdx, convertedNode);
                        convertedNode.Parent = node.Parent;
                        node.Parent = null;
                    }
                    return ConversionResult.Success;
                }

                element.ElementKind = 0;
                element.Name += " (invalid)";

                return ConversionResult.Fail;
            }
        }


        private static void ProcessSpecialNode(NodeElement element, Game input, Game output)
        {
            switch (Reflection.GetElementNameByID(element.ElementKind, output))
            {
                case "e_auth_element_se":
                    DEElementSE seElem = element as DEElementSE;

                    Type specialSoundsInput = DEElementSE.GetSpecialSoundTypeForGame(input);

                    if(Enum.IsDefined(specialSoundsInput, seElem.CueSheet))
                    {
                        Type specialSoundsOutput = DEElementSE.GetSpecialSoundTypeForGame(output);

                        string value = Enum.ToObject(specialSoundsInput, seElem.CueSheet).ToString();
                        object outVal;

                        if (Enum.TryParse(specialSoundsOutput, value, out outVal))
                            seElem.CueSheet = (ushort)outVal;

                    }
                    else
                    {
                        Type cuesheetsInput = DEElementSE.GetSoundCuesheetTypeForGame(input);

                        if(cuesheetsInput != null && Enum.IsDefined(cuesheetsInput, seElem.CueSheet))
                        {
                            Type cuesheetsOutput = DEElementSE.GetSoundCuesheetTypeForGame(output);

                            if (cuesheetsOutput != null)
                            {
                                string value = Enum.ToObject(cuesheetsInput, seElem.CueSheet).ToString();
                                object outVal;

                                if (Enum.TryParse(cuesheetsOutput, value, out outVal))
                                    seElem.CueSheet = (ushort)outVal;
                            }
                        }
                    }

                    break;
                case "e_auth_element_post_effect_dof2":
                    DEElementDOF2 dof2Elem = element as DEElementDOF2;

                    if (input <= Game.LJ && output >= Game.LAD7Gaiden)
                        dof2Elem.Unknown2 = 2;
                        break;
                       
            }
        }

        public static ushort GetGVFighterIDForGame(Game game)
        {
            switch (game)
            {
                default:
                    return 54;
                case Game.Y6Demo:
                    return 36;
                case Game.Y6:
                    return 36;
                case Game.YK2:
                    return 36;
                case Game.JE:
                    return 36;
                case Game.YLAD:
                    return 49;
                case Game.LJ:
                    return 49;
                case Game.LAD7Gaiden:
                    return 55;
                case Game.LADIW:
                    return 54;
            }

        }
    }
}
