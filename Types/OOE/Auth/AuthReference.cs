using System;

namespace HActLib
{
    public enum AuthReferenceType
    {
        Character = 4
    }

    public struct AuthReference
    {
        /// <summary>
        /// Determines how the resource is loaded/accessed
        /// </summary>
        public AuthReferenceType Type;
        public Guid Guid;
    }
}
