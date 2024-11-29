using System;
using System.Collections.Generic;


namespace HActLib
{
    public class UserElementData
    {
        public string NodeName; //Fog
        public string DeveloperName; //e_auth_element_fog etc...
        public uint ElementID;
        public List<UserElementField> Fields = new List<UserElementField>();
    }
}
