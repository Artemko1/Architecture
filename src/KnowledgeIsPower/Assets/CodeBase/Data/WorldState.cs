using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
    [Serializable]
    public class WorldState
    {
        public KillData KillData = new KillData();
    }

    [Serializable]
    public class KillData
    {
        public List<string> ClearedSpawnersIds = new List<string>(); // do not make readonly as it breaks serialization
    }
}