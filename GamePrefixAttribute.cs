using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HActLib
{
    [AttributeUsage(AttributeTargets.Enum, AllowMultiple = true)]
    internal class GamePrefixAttribute : Attribute
    {
        public Game Game;
        public string[] Prefixes;

        public GamePrefixAttribute(Game game, params string[] prefixes)
        {
            Game = game;
            Prefixes = prefixes;
        }
    }
}
