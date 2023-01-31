using System;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public WorldState WorldState;
        public PlayerState PlayerState;

        public PlayerProgress(PlayerState playerState)
        {
            WorldState = new WorldState();
            PlayerState = playerState;
        }
    }
}