

namespace HActLib
{
    public enum AuthNodeCategory : uint
    {
        DummyNode = 0x0,
        Path = 0x1, 
        PathMotion = 0x2,
        Camera = 0x3,
        CameraMotion = 0x4,
        Character = 0x5,
        CharacterMotion = 0x6,
        CharacterBehavior = 0x7,
        ModelCustom = 0x8,
        Asset = 0x9,
        Motion_model = 0xA,
        Model_node = 0xB,
        Element = 0xC,
        Stage = 0xD,
        StageScenarioFlag = 0xE,
        InstanceMotion = 0xF,
        InstanceMotionData = 0x10,
        FolderCondition = 0x11,
        CharacterBehaviorSimpleTalk = 0x12,
        InvalidNode = 0xFFFFFFFF,
    }
}
