using HActLib.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                        if(unchangedElement != null)
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

            string elementName = Enum.GetName(inputDEGame, element.ElementKind);

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
                    return ConversionResult.NoChange;
                else
                {
                    if(!element.TryConvert(inputGame, outputGame))
                    {
                        element.ElementKind = 0;
                        element.Name += " (invalid)";

                        return ConversionResult.Fail;
                    }

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
    }
}
