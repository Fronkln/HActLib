using System;
using HActLib;

namespace CMNEdit.Windows
{
    internal static class DEElementScenarioTimelineWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementScenarioTimeline timeline = node as DEElementScenarioTimeline;

            form.CreateHeader("Scenario Timeline");
            form.CreateInput("Category", timeline.TimelineCategory, delegate (string val) { timeline.TimelineCategory = val; });
            form.CreateInput("Timeline", timeline.Timeline, delegate (string val) { timeline.Timeline = val; });
            form.CreateInput("Clock", timeline.Clock, delegate (string val) { timeline.Clock = val; });
        }
    }
}
