using HActLib;
using System;
using System.Collections.Generic;
using System.IO;

namespace CMNEdit
{
    public class UserElementFile
    {
        public Game TargetGame;
        public UserElementData Data = new UserElementData();

        public static UserElementFile Read(string path)
        {
            if (!File.Exists(path))
                return null;

            string[] buf = File.ReadAllLines(path);

            if(buf.Length <= 0) 
                return null;

            string[] split = buf[0].Split(' ');

            if (split.Length < 4)
                return null;

            object gameVal = null;

            if (!Enum.TryParse(typeof(Game), split[0], out gameVal))
                return null;

            UserElementFile file = new UserElementFile();
            file.TargetGame = (Game)gameVal;
            file.Data.DeveloperName = split[1];
            file.Data.NodeName = split[2];
            file.Data.ElementID = uint.Parse(split[3]);

            if (buf.Length <= 1)
                return file;

            for (int i = 1; i < buf.Length; i++)
            {
                if (string.IsNullOrEmpty(buf[i]))
                    continue;

                string[] fieldDat = buf[i].Split(new char[] { ' ' });

                if (fieldDat.Length < 2)
                    continue;

                string name = fieldDat[1];
                string type = fieldDat[0]; //rest is args

                object typeObject = null;

                if (!Enum.TryParse(typeof(UserElementFieldType), type, true, out typeObject))
                    continue;

                UserElementField field = new UserElementField();
                field.FieldType = (UserElementFieldType)typeObject;
                field.Name = name;

                if(fieldDat.Length > 2)
                    for(int k = 2; k <  fieldDat.Length; k++)
                        field.Args.Add(fieldDat[k]);

                 file.Data.Fields.Add(field);
            }

            return file;
        }
    }
}
