using CodeBase.Data;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "PlayerProgress", menuName = "StaticData/PlayerProgress", order = 0)]
    public class PlayerProgressStaticData : ScriptableObject
    {
        public PositionOnLevel PositionOnLevel;
    }
}