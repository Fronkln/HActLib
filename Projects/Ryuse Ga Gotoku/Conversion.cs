using System;
using System.Linq;
using System.Collections.Generic;
using HActLib;


namespace RyuseGaGotoku
{
    internal static class Conversion
    {
        public static HashSet<string> ConvertedTypes = new HashSet<string>();
        public static HashSet<string> FailedTypes = new HashSet<string>();
        public static HashSet<uint> FailedIDs = new HashSet<uint>();
        public static HashSet<uint> BlackListedIDs = new HashSet<uint>();
        public static HashSet<string> BlackListedTypes = new HashSet<string>();

        private static List<NodeCamera> NodeCameras = new List<NodeCamera>();


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




        public static List<Node> Convert(List<Node> nodes, Type inputDEGameEnum, Type outputDEGameEnum)
        {
            /*
            if (HAct.Root != null)
                foreach (NodeElement element in HAct.AllElements)
                    SwapNodeID(element, inputDEGameEnum, outputDEGameEnum);
            */

            if(Program.Bep == null)
                SwapNodeID(nodes[0], inputDEGameEnum, outputDEGameEnum);
            else
                foreach(Node node in nodes)
                    SwapNodeID(node, inputDEGameEnum, outputDEGameEnum);

            return nodes;
        }

        static void SwapNodeID(Node node, Type inputDEGame, Type outputDEGame)
        {

            NodeElement element = (node as NodeElement);
            bool convertFail = false;


            if (element != null)
            {
                string elementName = Enum.GetName(inputDEGame, element.ElementKind);

                if (string.IsNullOrEmpty(elementName))
                {
                    Console.WriteLine("Element kind does not exist in input game element list. Wrong input game setting? Element kind: " + element.ElementKind);
                    return;
                }

                bool sameVer = CMN.GetVersionForGame(CMN.GetGameFromString(Program.GetNameFromEnum(inputDEGame))) == CMN.GetVersionForGame(CMN.GetGameFromString(Program.GetNameFromEnum(outputDEGame)));

                if ((!sameVer && blackListedBetweenEngines.Contains(elementName)) || blackListedBetweenGames.Contains(elementName))
                {

                    if (!BlackListedTypes.Contains(elementName))
                    {
                        BlackListedTypes.Add(elementName);
                        BlackListedIDs.Add(element.ElementKind);
                    }

                    element.ElementKind = 0;
                    element.Name += " (invalid)";

                    convertFail = true;
                }
                else
                {
                    if (Enum.IsDefined(outputDEGame, elementName))
                    {
                        uint originalID = (uint)Enum.Parse(inputDEGame, elementName);
                        uint outputID = (uint)Enum.Parse(outputDEGame, elementName);

                        if (originalID != outputID)
                            if (!ConvertedTypes.Contains(elementName))
                                ConvertedTypes.Add(elementName);

                        element.ElementKind = outputID;
                    }
                    else
                    {
                        element.ElementKind = 0;
                        element.Name += " (invalid)";

                        if (!FailedTypes.Contains(elementName))
                        {
                            FailedTypes.Add(elementName);
                            FailedIDs.Add(element.ElementKind);
                        }

                        convertFail = true;
                    }
                }
            }

            List<Node> filteredList = new List<Node>(node.Children);

            if (!convertFail)
            {

                if (Program.Bep == null)
                {
                    foreach (Node child in node.Children)
                    {
                        if (child.Category == AuthNodeCategory.CharacterBehavior)
                        {
                            Console.WriteLine("Character Behavior is incompatible between engines. Removing");
                            filteredList.Remove(child);
                        }

                    }

                    node.Children = filteredList;

                    foreach (Node child in node.Children)
                        SwapNodeID(child, inputDEGame, outputDEGame);
                }
            }
        }

        public static GameVersion VersionForDEGame(string game)
        {
            switch (game)
            {
                default:
                    return GameVersion.DE2;

                case "y6demo":
                    return GameVersion.Yakuza6Demo;
                case "yakuza6demo":
                    return GameVersion.Yakuza6Demo;

                case "y6":
                    return GameVersion.Yakuza6;
                case "yakuza6":
                    return GameVersion.Yakuza6;

                case "yk2":
                    return GameVersion.DE1;
                case "yakuzakiwami2":
                    return GameVersion.DE1;

                case "judgeeyes":
                    return GameVersion.DE1;
                case "judgment":
                    return GameVersion.DE1;
                case "je":
                    return GameVersion.DE1;
            }
        }

        public static void ProcessSpecificConversion(CMN HAct, string inputDEGame, string outputDEGame)
        {
            NodeCamera[] allCams = HAct.AllCameras;

            GameVersion inputVersion = VersionForDEGame(inputDEGame);
            GameVersion outputVersion = VersionForDEGame(outputDEGame);

            Game inputGame = CMN.GetGameFromString(inputDEGame);
            Game outputGame = CMN.GetGameFromString(outputDEGame);

            if (allCams.Length > 0)
            {
                NodeCamera cam = allCams[0];

                //Fix: add frame progression to HAct with none
                if (cam.FrameProgression.Length <= 0 && HAct.CutInfo.Length > 0)
                {
                    float target = HAct.CutInfo[HAct.CutInfo.Length - 1] - 1;

                    Console.WriteLine("No frame progression in HAct, generating based on cut info...");

                    cam.FrameProgression = new float[(int)target];
                    cam.FrameProgressionSpeed = new float[(int)target];

                    int cur = 1;
                    for (int i = 0; i < cam.FrameProgression.Length; i++)
                    {
                        cam.FrameProgression[i] = cur;
                        cam.FrameProgressionSpeed[i] = 1f;
                        cur++;
                    }

                }

            }



            //Limitation:
            //Auth page format has not been reversed yet for Y6
            if (CMN.VersionEqualsLess(inputVersion, GameVersion.Yakuza6) || CMN.VersionEqualsLess(outputVersion, GameVersion.Yakuza6))
                HAct.AuthPages = new List<AuthPage>();


            //Limitation:
            //Don't know how to use model nodes for Y6 and YK2
            if (CMN.VersionEqualsLess(inputVersion, GameVersion.DE1))
            {
                foreach (Node node in HAct.AllNodes.Where(x => x.Category == AuthNodeCategory.ModelCustom))
                {
                    node.Parent.Children.Remove(node);
                }

                foreach (Node node in HAct.AllElements.Where(x => x.Start > x.End ||
                x.ElementKind == 0x8)) //bad dof can crash y6
                {
                    node.Parent.Children.Remove(node);
                }

                foreach (Node node in HAct.AllNodes.Where(x => x.Category == AuthNodeCategory.CameraMotion || x.Category == AuthNodeCategory.CharacterMotion))
                {
                    NodeCameraMotion camMot = node as NodeCameraMotion;
                    DENodeCharacterMotion charMot = node as DENodeCharacterMotion;

                    if (camMot != null)
                    {
                        if (camMot.Start > camMot.End)
                            camMot.Parent.Children.Remove(camMot);
                        else if (camMot.End.Frame > HAct.Header.End)
                            camMot.End.Frame = HAct.Header.End;
                    }
                    else
                    {
                        if (charMot.Start.Frame > charMot.End.Frame)
                            charMot.Parent.Children.Remove(charMot);
                        else if (charMot.End.Frame > HAct.Header.End)
                            charMot.End.Frame = HAct.Header.End;
                    }

                }
            }

            if (CMN.VersionEqualsLess(outputVersion, GameVersion.Yakuza6))
            {
                bool assetsWasAltered = false;

                foreach (Node node in HAct.AllNodes)
                {
                    if (node.Category == AuthNodeCategory.Asset)
                    {
                        NodeAsset asset = node as NodeAsset;
                        asset.AssetID = 6163;

                        assetsWasAltered = true;
                    }
                }

                //speech
                NodeElement[] speech = HAct.AllElements.Where(x => x.ElementKind == 26).ToArray();

                if (speech.Length > 0)
                {
                  //  foreach (NodeElement speechEl in speech)
                      //  speechEl.Parent.Children.Remove(speechEl);

                    Console.WriteLine("WARNING: Yakuza 6 can get stuck loading when invalid sound effects are present. Don't forget to add them!");
                }

                if (assetsWasAltered)
                    Console.WriteLine("WARNING: Yakuza 6 can crash with invalid asset ids. They were set to 6163 (dummy)");

                Node[] cond = HAct.AllNodes.Where(x => x.Category == AuthNodeCategory.FolderCondition).ToArray();

                if(cond.Length > 0)
                {
                    //Console.WriteLine("VERY IMPORTANT WARNING: Yakuza 6 can crash with condition folders from other games. They will be invalidated and will need to be manually removed/cleared with AuthEdit");

                    foreach(Node node in cond)
                    {
                       // node.Category = AuthNodeCategory.DummyNode;
                    }
                }
            }

            //DE 1.0 -> DE 2.0 CMN
            if (CMN.VersionEqualsGreater(outputVersion,GameVersion.DE2))
            {
                if (inputVersion == GameVersion.DE1 || CMN.VersionEqualsLess(inputVersion, GameVersion.Yakuza6))
                {
                    for (int i = 0; i < HAct.AuthPages.Count; i++)
                    {
                        AuthPage page = HAct.AuthPages[i];



                        //Fix 1:
                        //Convert auth pages to new format
                        if (page.IsTalkPage() && (page.Flag & 0x40) == 0)
                            page.Flag |= 0x40;

                        page.IsOldDE = false;
                        page.PageTitleText = "page" + (i + 1) + " (converted by Ryuse)";

                        //Improvement 1:
                        //Add transition for pages that dont have any to finish
                        if (page.Transitions.Count == 0)
                            page.Transitions = new List<Transition>(new Transition[]
                            {
                                new Transition()
                                {
                                    DestinationPageIndex = -1,
                                    ConditionSize = 0,
                                    Conditions =  new List<Condition> (new Condition[]{new ConditionPageEnd()})
    
                                }
                            });
                    }

                    //Fix 2:
                    //set HAct length to the last value in first camera frame progression minus 1 to prevent double playback
                    //still dont know what causes this but chopping off 1 frame seems to do the trick and is unnoticeable enough
                    if (allCams.Length > 0)
                    {
                        NodeCamera cam = allCams[0];
                        if (cam.FrameProgression.Length > 0)
                        {
                            float frameProgressionEnd = cam.FrameProgression[cam.FrameProgression.Length - 1];

                            if (HAct.Header.End > frameProgressionEnd)
                                HAct.Header.End = frameProgressionEnd - 1;
                        }
                    }
                }
            }


        }
    }
}
