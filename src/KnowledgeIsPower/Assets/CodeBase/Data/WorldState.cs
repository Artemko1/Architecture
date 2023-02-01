using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
    [Serializable]
    public class WorldState
    {
        public KillData KillData;

        public WorldState()
        {
            KillData = new KillData();
        }

        public WorldState(KillData killData)
        {
            KillData = killData;
        }
    }

    [Serializable]
    public class KillData
    {
        public List<string> ClearedSpawnersIds = new List<string>(); // do not make readonly as it breaks serialization
    }
}