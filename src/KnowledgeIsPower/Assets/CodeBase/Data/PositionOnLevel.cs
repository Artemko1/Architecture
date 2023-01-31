using System;

namespace CodeBase.Data
{
    [Serializable]
    public class PositionOnLevel
    {
        public string LevelName;
        public Vector3Data Position;

        public PositionOnLevel(string level, Vector3Data position)
        {
            LevelName = level;
            Position = position;
        }

        public PositionOnLevel(string initialLevel)
        {
            LevelName = initialLevel;
        }
    }
}