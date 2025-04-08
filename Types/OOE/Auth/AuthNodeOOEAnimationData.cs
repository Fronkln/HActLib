using System;

namespace HActLib
{
    public class AuthNodeOOEAnimationData
    {
        public int Type;
        public int Unknown2;
        public Guid Guid;

        public float StartFrame;
        public float EndFrame;

        public byte[] UnknownData = new byte[96];

        public static int GetTypeForNodeCategory(AuthNodeTypeOOE type)
        {
            switch(type)
            {
                default:
                    return 0;
                case AuthNodeTypeOOE.Path:
                    return 0;
                case AuthNodeTypeOOE.Character:
                    return 2;
                case AuthNodeTypeOOE.Model:
                    return 3;
                case AuthNodeTypeOOE.Camera:
                    return 0;
            }
        }
    }
}
