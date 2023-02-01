using CodeBase.Data;
using UnityEngine;

namespace CodeBase.StaticData.Hero
{
    [CreateAssetMenu(fileName = "PlayerProgress", menuName = "StaticData/PlayerProgress", order = 0)]
    public class PlayerProgressStaticData : ScriptableObject
    {
        public PositionOnLevel PositionOnLevel;
    }
}