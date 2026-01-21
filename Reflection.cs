using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HActLib.Internal
{
    public class Reflection
    {
        public static bool Done = false;
        public static Dictionary<Game, Dictionary<uint, Type>> ElementNodes = new Dictionary<Game, Dictionary<uint, Type>>();
        public static Dictionary<Game, Dictionary<uint, UserElementData>> UserNodes = new Dictionary<Game, Dictionary<uint, UserElementData>>();
        public static Dictionary<Game, List<string>> GamePrefixes = new Dictionary<Game, List<string>>();

        public static Type GetElementEnumFromGame(Game game)
        {
            switch(game)
            {
                default:
                    return typeof(LADIWNodeIDs);
                case Game.YK3:
                    return typeof(YK3NodeIDs);
                case Game.LADPYIH:
                    return typeof(LADPYIHNodeIDs);
                case Game.LADIW:
                    return typeof(LADIWNodeIDs);
                case Game.LAD7Gaiden:
                    return typeof(GaidenNodeIDs);
                case Game.LJ:
                    return typeof(LJNodeIDs);
                case Game.YLAD:
                    return typeof(YLADNodeIDs);
                case Game.YK2:
                    return typeof(YK2NodeIDs);
                case Game.JE:
                    return typeof(JENodeIDs);
                case Game.Y6:
                    return typeof(Y6NodeIDs);
                case Game.Y6Demo:
                    return typeof(Y6NodeIDs);
                case Game.Y5:
                    return typeof(Y5NodeIDs);
                case Game.Ishin:
                    return typeof(IshinNodeIDs);
                case Game.Y0:
                    return typeof(Y0NodeIDs);
                case Game.YK1:
                    return typeof(Y0NodeIDs);
                case Game.FOTNS:
                    return typeof(FOTNSNodeIDs);
            }
        }

        public static void RegisterUserNode(Game game, UserElementData dat)
        {
            if (!Done)
                Process();

            UserNodes[game][dat.ElementID] = dat;
        }

        public static string[] GetGamePrefixes(Game game)
        {
            if (!Done)
                Process();

            return GamePrefixes[game].ToArray();
        }

        public static uint GetElementIDByName(string name, Game game)
        {
            if (!Done)
                Process();

            Type elementEnum = GetElementEnumFromGame(game);

            uint value;

            if (Enum.TryParse(name, true, out value))
                return value;
            else
                return 0;
        }

        public static string GetElementNameByID(uint id, Game game)
        {
            if(id >= 1337)
            {   
                switch(id)
                {
                    default:
                        return "UNKNOWN_EX_AUTH_NODE_" + id;
                    case 1337:
                        return "System Speed (EX Auth)";
                    case 60010:
                        return "Transit HAct (Like a Brawler)";
                }
            }

            Type elementEnum = GetElementEnumFromGame(game);

            try
            {
                string name = Enum.GetName(elementEnum, id);

                if (string.IsNullOrEmpty(name))
                    return "UNKNOWN_NODE_" + id;
                else
                    return name;
            }
            catch
            {
                return "UNKNOWN_NODE_" + id;
            }
        }

        public static Game GetGameFromString(string str)
        {
            if (!Done)
                Process();

            str = str.ToLowerInvariant();

            foreach (var kv in GamePrefixes)
                foreach (string prefix in kv.Value)
                    if (prefix == str)
                        return kv.Key;

            return (Game)9999;
        }

        public static void Process()
        {
            if (Done)
                return;

            ElementNodes.Clear();

            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            Type baseType = typeof(NodeElement);

            for (int i = 0; i < Enum.GetValues(typeof(Game)).Length; i++)
            {
                ElementNodes.Add((Game)i, new Dictionary<uint, Type>());
                UserNodes.Add((Game)i, new Dictionary<uint, UserElementData>());
                GamePrefixes.Add((Game)i, new List<string>());
            }

            foreach(Type type in types)
            {
                if (!type.IsSubclassOf(baseType))
                {
                    if(type.IsEnum)
                    {
                        GamePrefixAttribute[] prefixes = type.GetCustomAttributes<GamePrefixAttribute>().ToArray();

                        foreach (GamePrefixAttribute attrib in prefixes)
                            GamePrefixes[attrib.Game].AddRange(attrib.Prefixes);
                    }

                    continue;
                }

                ElementIDAttribute[] elementIDs = type.GetCustomAttributes<ElementIDAttribute>().ToArray();

                foreach(ElementIDAttribute attrib in elementIDs)
                    ElementNodes[attrib.Game][attrib.ID] = type;
            }

            ElementNodes[Game.Y6Demo] = ElementNodes[Game.Y6];
            ElementNodes[Game.YK1] = ElementNodes[Game.Y0];

            Done = true;
        }
    }
}
