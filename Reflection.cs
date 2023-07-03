using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib.Types.DE.Enum;

namespace HActLib.Internal
{
    public class Reflection
    {
        public static bool Done = false;
        public static Dictionary<Game, Dictionary<uint, Type>> ElementNodes = new Dictionary<Game, Dictionary<uint, Type>>();

        public static Type GetElementEnumFromGame(Game game)
        {
            switch(game)
            {
                default:
                    return typeof(LJNodeIDs);

                case Game.LADIW:
                    return typeof(LJNodeIDs);
                case Game.LAD7Gaiden:
                    return typeof(LJNodeIDs);
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
            }
        }

        public static uint GetElementIDByName(string name, Game game)
        {
            Type elementEnum = GetElementEnumFromGame(game);

            string[] names = Enum.GetNames(elementEnum);
            Array values = Enum.GetValues(elementEnum);

            int idx = Array.IndexOf(names, name);

            if (idx > -1)
                return (uint)values.GetValue(idx);
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

        public static void Process()
        {
            ElementNodes.Clear();

            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            Type baseType = typeof(NodeElement);
            
            for(int i = 0; i < Enum.GetValues(typeof(Game)).Length; i++)
                ElementNodes.Add((Game)i, new Dictionary<uint, Type>());

            foreach(Type type in types)
            {
                //Class must derive from NodeElement
                if (!type.IsSubclassOf(baseType))
                    continue;

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
