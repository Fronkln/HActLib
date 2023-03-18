using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HActLib
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    internal class ElementIDAttribute : Attribute
    {
        public Game Game;
        public uint ID;

        public ElementIDAttribute(Game game, uint id)
        {
            Game = game;
            ID = id;
        }
    }
}
