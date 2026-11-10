using System;
using HActLib;

namespace CMNEdit.Windows
{
    internal static class DEElementScenarioTimelineWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementScenarioTimeline timeline = node as DEElementScenarioTimeline;

            if (timeline == null)
                return;

            PropertyEditorBuilder.CreateHeader("Scenario Timeline");
            PropertyEditorBuilder.CreateInput("Category", timeline.TimelineCategory, delegate (string val) { timeline.TimelineCategory = val; });
            PropertyEditorBuilder.CreateInput("Timeline", timeline.Timeline, delegate (string val) { timeline.Timeline = val; });
            PropertyEditorBuilder.CreateInput("Clock", timeline.Clock, delegate (string val) { timeline.Clock = val; });
        }
    }
}
