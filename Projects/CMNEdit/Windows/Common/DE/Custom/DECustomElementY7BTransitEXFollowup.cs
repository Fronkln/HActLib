using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class DECustomElementY7BTransitEXFollowupWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DECustomY7BTransitEXFollowup transitHact = node as DECustomY7BTransitEXFollowup;

            form.CreateHeader("Transit HAct");

            form.CreateInput("YHC Name", transitHact.YHCName, delegate (string val) { transitHact.YHCName = val; });
            form.CreateInput("YHC Attack Name", transitHact.YHCAttackName, delegate (string val) { transitHact.YHCAttackName = val; });

            form.CreateInput("Job ID", transitHact.JobRestriction.ToString(), delegate (string val) { transitHact.JobRestriction = uint.Parse(val); }, NumberBox.NumberMode.UInt);
        }
    }
}
